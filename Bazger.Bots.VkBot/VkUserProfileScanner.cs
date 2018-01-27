using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Bazger.Bots.VkBot.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Bazger.Bots.VkBot
{
    public class VkUserProfileScanner
    {
        public void Scan(RemoteWebDriver driver, Dictionary<string, List<string>> resultsPerEachSearch = null)
        {
            if (resultsPerEachSearch == null)
            {
                return;
            }
            foreach (var searchResult in resultsPerEachSearch)
            {
                foreach (var userProfile in searchResult.Value)
                {
                    Thread.Sleep(1000);
                    driver.Navigate().GoToUrl(userProfile);

                    var currentProfileDataPath = Path.Combine(VkConfigs.BaseDir, searchResult.Key, Path.GetFileName(userProfile));
                    Directory.CreateDirectory(currentProfileDataPath);

                    //NOT FINISHED
                    //GetProfilePosts(driver, currentProfileDataPath);
                    GetProfilePhotos(driver, currentProfileDataPath);
                    GetGaleryPhotos(driver, currentProfileDataPath);
                    GetProfileInfo(driver, currentProfileDataPath);
                }
            }
        }

        private void GetProfilePhotos(RemoteWebDriver driver, string path)
        {
            string profilePhotoPrefix = "ProfilePhoto";

            ClickOnProfilePhoto(driver);
            Thread.Sleep(1000);

            for (int i = 1; i <= VkConfigs.ProfilePhotosCount; i++)
            {

                var photoName = $"{profilePhotoPrefix}_{i}.jpg";
                var url = driver.FindElementByXPath("//*[@id='pv_photo']/img").GetAttribute("src");

                //Downloading photo
                Thread photoThread = new Thread(() => DownloadAndSavePhoto(url, Path.Combine(path, photoName)));
                photoThread.Start();

                //Going to the next picture in the galery
                driver.FindElementByXPath("//*[@id='pv_photo']/img").Click();
                Thread.Sleep(500);
            }
            ClosePhotosView(driver);
        }

        private void GetGaleryPhotos(RemoteWebDriver driver, string path)
        {
            string galeryPhotoPrefix = "GaleryPhoto";

            ClickOnTheFirstGalleryPhotoInUserProfile(driver);
            Thread.Sleep(1000);

            for (int i = 1; i <= VkConfigs.GalleryPhotosCount; i++)
            {

                var photoName = $"{galeryPhotoPrefix}_{i}.jpg";
                var url = GetPhotoUrlInPhotosView(driver);

                //Downloading photo
                Thread photoThread = new Thread(() => DownloadAndSavePhoto(url, Path.Combine(path, photoName)));
                photoThread.Start();

                //Going to the next picture in the galery
                GoRightInPhotosView(driver);
                Thread.Sleep(500);
            }
            ClosePhotosView(driver);
        }

        private void ClickOnProfilePhoto(RemoteWebDriver driver)
        {
            driver.FindElementByClassName("page_square_photo").Click();
        }

        private void ClickOnTheFirstGalleryPhotoInUserProfile(RemoteWebDriver driver)
        {
            driver.FindElementByClassName("page_avatar_img").Click();
        }

        private void GoRightInPhotosView(RemoteWebDriver driver)
        {
            driver.FindElementByXPath("//*[@id='pv_photo']/img").Click();
        }

        private string GetPhotoUrlInPhotosView(RemoteWebDriver driver)
        {
            return driver.FindElementByXPath("//*[@id='pv_photo']/img").GetAttribute("src");
        }

        private void ClosePhotosView(RemoteWebDriver driver)
        {
            driver.FindElementByClassName("pv_close_btn").Click();
        }


        private void DownloadAndSavePhoto(string url, string path)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, path);
            }
        }

        private void GetProfilePosts(RemoteWebDriver driver, string path)
        {
            var postsElements = (driver.FindElementByXPath("//*[@id='page_wall_posts']")).FindElements(By.ClassName("post"));
            while (postsElements.Count < VkConfigs.PostsCount)
            {
                driver.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                postsElements = (driver.FindElementByXPath("//*[@id='page_wall_posts']")).FindElements(By.ClassName("post"));
                //TODO: DRIVER WAIT
                Thread.Sleep(1000);
            }

            var posts = new List<KeyValuePair<string, string>>();
            foreach (var postElement in postsElements)
            {
                var authorUrl = postElement.FindElement(By.ClassName("author")).GetAttribute("href");
                var authorName = postElement.FindElement(By.ClassName("author")).Text;
                var authorPostDate = postElement.FindElement(By.ClassName("rel_date")).Text;
                var authorWallPostText = postElement.FindElement(By.ClassName("wall_post_text")).Text;

                //var copy_authorUrl = post.FindElement(By.ClassName("copy_author")).GetAttribute("href");
                //var copy_authorName = post.FindElement(By.ClassName("copy_author")).Text;
                //var copy_authorPostDate = post.FindElement(By.ClassName("rel_date")).Text;
            }
        }

        private void GetProfileInfo(RemoteWebDriver driver, string path)
        {
            var profileInfoPrefix = "ProfileInfo.txt";
            var fieldsDiv = driver.FindElementsByClassName("profile_info_row");
            var fields = new Dictionary<string, object>();
            fields.Add("ProfileID_s", GetProfileId(driver));
            fields.Add("Name_s", GetProfileFullName(driver));
            if (IsMoreProfileInfoExists(driver))
            {
                ClickOnShowMoreProfileInfo(driver);
            }
            foreach (var fieldDiv in fieldsDiv)
            {
                var key = fieldDiv.FindElement(By.ClassName("label")).Text.Replace(":", "");
                var value = fieldDiv.FindElement(By.ClassName("labeled")).Text;
                if (fields.ContainsKey(key))
                {
                    if (!(fields[key] is List<string>))
                    {
                        fields[key] = new List<string> { fields[key] as string };
                    }
                    ((List<string>)fields[key]).Add(value);
                }
                else
                {
                    fields.Add(key, value);
                }
            }

            SerDeUtils.SerializeToJsonFile(fields, Path.Combine(path, profileInfoPrefix));
        }

        private string GetProfileFullName(RemoteWebDriver driver)
        {
            return driver.FindElementByClassName("page_name").Text;
        }

        private string GetProfileId(RemoteWebDriver driver)
        {
            return Path.GetFileNameWithoutExtension(driver.Url);
        }

        private void ClickOnShowMoreProfileInfo(RemoteWebDriver driver)
        {
            driver.FindElementByClassName("profile_more_info_link").Click();
        }

        private bool IsMoreProfileInfoExists(RemoteWebDriver driver)
        {
            return driver.PageSource.Contains("profile_more_info_link");
        }

    }
}
