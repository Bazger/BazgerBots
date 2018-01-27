using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Bazger.Bots.Core;
using Bazger.Bots.Core.Utils;
using Bazger.Bots.Infrastructure;
using Bazger.Bots.VkBot_v2.Config;
using Bazger.Bots.VkBot_v2.Pages;

namespace Bazger.Bots.VkBot_v2.Components
{
    public class UserProfileScannerComponent : BotComponent
    {
        private readonly string _baseDir;
        private readonly int _profilePhotosCount;
        private readonly int _galleryPhotosCount;
        private readonly int _postsCount;
        private readonly string _searchResultsFileName;
        private readonly bool _loadResultsFromFile;
        private UserProfilePage _userProfilePage;
        private Dictionary<string, List<string>> _searchResults;


        public UserProfileScannerComponent(string baseDir, int profilePhotosCount, int galleryPhotosCount, int postsCount, string searchResultsFileName = null, bool loadResultsFromFile = false, bool toRun = true) : base(toRun)
        {
            _baseDir = Path.Combine(baseDir);
            _postsCount = postsCount;
            _searchResultsFileName = searchResultsFileName;
            _loadResultsFromFile = loadResultsFromFile;
            _galleryPhotosCount = galleryPhotosCount;
            _profilePhotosCount = profilePhotosCount;
        }

        public override void Prepare(BotStateBuilder botState)
        {
            _userProfilePage = new UserProfilePage(botState.Driver);
        }

        public override void Process(BotStateBuilder botState)
        {
            if (!_loadResultsFromFile)
            {
                //Gets values from another components
                _searchResults =
                    (Dictionary<string, List<string>>)
                        botState.ComponentsDataHandler[typeof(SearchResultsScannerComponent).Name];
            }
            else
            {
                _searchResults = SerDeUtils.DeserializeJsonFile<Dictionary<string, List<string>>>(Path.Combine(_baseDir, _searchResultsFileName));
            }
            Wait wait = new Wait(botState.Driver);
            foreach (var searchResult in _searchResults)
            {
                foreach (var userProfile in searchResult.Value)
                {
                    _userProfilePage.SetPageUrl(userProfile);
                    Thread.Sleep(400);
                    _userProfilePage.GoToPageUrl();

                    var currentProfileDataPath = Path.Combine(_baseDir, "AllProfiles", searchResult.Key, Path.GetFileName(userProfile));
                    Directory.CreateDirectory(currentProfileDataPath);

                    //Get Profile info
                    GetProfileInfo(Path.Combine(currentProfileDataPath, "ProfileInfo.json"));
                    Configs.Logger.Debug($"Profile info was gathered | Profile: {userProfile}");

                    if (_postsCount != 0)
                    {
                        //Get Profile posts
                        GetProfilePosts(Path.Combine(currentProfileDataPath, "Posts.json"));
                        Configs.Logger.Debug($"Posts was gathered | Profile: {userProfile}");
                    }

                    if (_profilePhotosCount != 0 || _galleryPhotosCount != 0)
                    {
                        //Get All kind of photos
                        OpenAlbumViewForGettingPhotos(currentProfileDataPath);
                    }

                    Configs.Logger.Info($"Profile inteligence succed! Profile: {userProfile} | Search marker: {searchResult.Key}");
                }
            }
        }

        private void OpenAlbumViewForGettingPhotos(string currentProfileDataPath)
        {
            _userProfilePage.ClickOnShowAllAlbums();
            //Thread.Sleep(700);

            if (_profilePhotosCount != 0)
            {
                _userProfilePage.WaitToAlbumView();
                //Get Profile Photos
                string profilePhotosPath = Path.Combine(currentProfileDataPath, "ProfilePhoto.jpg");
                GetPhotos(profilePhotosPath, () =>
                {
                    _userProfilePage.ClickOnAlbumByAlbumNumber(0);
                }, _profilePhotosCount);
            }

            if (_galleryPhotosCount != 0)
            {
                _userProfilePage.WaitToAlbumView();
                //Get Gallery Photos
                string galleryPhotosPath = Path.Combine(currentProfileDataPath, "GalleryPhoto.jpg");
                GetPhotos(galleryPhotosPath, () =>
                {
                    _userProfilePage.ClickOnAlbumByAlbumNumber(1);
                }, _galleryPhotosCount);
            }

            if (_userProfilePage.IsAlbumViewOpened())
            {
                _userProfilePage.ClosePhotosView();
            }
        }

        private void GetPhotos(string path, Action openPhotosView, int downloadCount)
        {
            openPhotosView();
            _userProfilePage.WaitToPhotosViewCounter();
            //Thread.Sleep(450);
            //if (!_userProfilePage.IsPhotosViewOpened())
            //{
            //    return;
            //}

            for (int i = 1; i <= downloadCount && i <= _userProfilePage.GetPhotosCountInPhotoView(); i++)
            {
                var photoName = $"{Path.GetFileNameWithoutExtension(path)}_{i}.{Path.GetExtension(path)}";
                var dirPath = Path.GetDirectoryName(path);

                var url = _userProfilePage.GetPhotoUrlInPhotosView();

                //Downloading photo
                Thread photoThread = new Thread(() => DownloadAndSavePhoto(url, Path.Combine(dirPath, photoName)));
                photoThread.Start();

                if (i != _userProfilePage.GetPhotosCountInPhotoView())
                {
                    //Going to the next picture in the galery
                    _userProfilePage.WaitUntilImageLoads();
                    _userProfilePage.GoRightInPhotosView();
                    //_userProfilePage.WaitUntilImageChanges(url);
                    //Thread.Sleep(500);
                }
            }
            if (_userProfilePage.IsPhotosViewOpened())
            {
                _userProfilePage.ClosePhotosView();
            }
            Configs.Logger.Debug($"Photos from one of albums was gathered | Profile: {_userProfilePage.GetId()}");
        }

        private void DownloadAndSavePhoto(string url, string path)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, path);
            }
        }

        private void GetProfileInfo(string path)
        {
            var fields = new Dictionary<string, object>
            {
                {"Item_ID",Guid.NewGuid().ToString() },
                {"Profile_Url", _userProfilePage.GetPageUrl()},
                {"Data_Type", "ProfileInfo"},
                {"Full_Name", _userProfilePage.GetFullName()}
            };
            fields.AddRange(_userProfilePage.GetProfileInfoFields());

            SerDeUtils.SerializeToJsonFile(fields, path);
        }

        private void GetProfilePosts(string path)
        {
            _userProfilePage.ScrollDownUntil(() => _userProfilePage.GetPostsCount(), _postsCount);
            var allPosts = _userProfilePage.GetPostsByCount(_postsCount);
            allPosts.ForEach(post =>
            {
                post.Add("Item_ID", Guid.NewGuid().ToString());
                post.Add("Profile_Url", _userProfilePage.GetPageUrl());
                post.Add("Data_Type", "Post");
            });
            SerDeUtils.SerializeToJsonFile(allPosts, path);
        }
    }
}
