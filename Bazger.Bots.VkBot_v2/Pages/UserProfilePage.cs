using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bazger.Bots.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Bazger.Bots.VkBot_v2.Pages
{
    public class UserProfilePage : WebPage, IChangableUrlPage
    {
        public static string PostAuthorUrlField = "Author_Url";
        public static string PostAuthorNameField = "Author_Name";
        public static string PostAuthorPostDateField = "Author_Post_Date";
        public static string PostAuthorWallPostTextField = "Author_Wall_Post_Text";
        public static string PostCopyAuthorUrlField = "Copy_Author_Url";
        public static string PostCopyAuthorNameField = "Copy_Author_Name";
        public static string PostCopyAuthorPostDateField = "Copy_Author_Post_Date";
        public static string PostCopyAuthorWallPostTextField = "Copy_Author_Wall_Post_Text";

        private string _url;

        public UserProfilePage(RemoteWebDriver driver) : base(driver)
        {
        }

        public override string GetPageUrl()
        {
            return _url;
        }


        public void SetPageUrl(string url)
        {
            _url = url;
        }

        public string GetFullName()
        {
            return _driver.FindElementByClassName("page_name").Text;
        }

        public string GetId()
        {
            return Path.GetFileNameWithoutExtension(_driver.Url);
        }

        public void ClickOnShowMoreProfileInfo()
        {
            _driver.FindElementByClassName("profile_more_info_link").Click();
        }

        public bool IsMoreProfileInfoExists()
        {
            return _driver.PageSource.Contains("profile_more_info_link");
        }

        public void ClickOnTheFirstGalleryPhoto()
        {
            _driver.FindElementByClassName("page_square_photo").Click();
        }

        public bool IsGalleryViewExists()
        {
            return _driver.TryFindElement(By.ClassName("page_square_photo")) != null;
        }

        public void ClickOnProfilePhoto()
        {
            _driver.FindElementByClassName("page_avatar_img").Click();
        }

        public void GoRightInPhotosView()
        {
            _driver.ExecuteScript("cur.pvClicked = true; Photoview.show(false, cur.pvIndex + 1, event);");
        }

        public bool IsRightButtonExistsInPhotosView()
        {
            if (_driver.FindElementById("pv_nav_btn_right").Displayed)
            {
                return true;
            }
            return false;
        }

        public void ClickOnShowAllAlbums()
        {
            var allCouners = _driver.FindElementsByClassName("page_counter");
            foreach (var couner in allCouners)
            {
                var counterLink = couner.GetAttribute("href");
                if (counterLink.Contains("albums"))
                {
                    couner.Click();
                    return;
                }
            }
        }

        public void ClickOnAlbumByAlbumNumber(int pos)
        {
            var allAlbums = _driver.FindElementsByClassName("photos_album_thumb");
            if (pos < allAlbums.Count)
            {
                allAlbums[pos].Click();
            }
        }

        //USING TRYFIND
        public int GetPhotosCountInPhotoView()
        {
            var count = _driver.TryFindElement(By.ClassName("pv_counter"))?.Text;
            if (String.IsNullOrEmpty(count))
            {
                return IsPhotosViewOpened() ? 1 : 0;
            }
            return Convert.ToInt32(count.Split(' ')[2]);
        }

        public string GetPhotoUrlInPhotosView()
        {
            return _driver.FindElementByXPath("//*[@id='pv_photo']/img").GetAttribute("src");
        }

        public void ClosePhotosView()
        {
            _driver.TryExecuteScript("Photoview.hide(0)");
        }

        //TODO: REMOVE TRY
        public bool IsPhotosViewOpened()
        {
            var inView = _driver.TryFindElement(By.ClassName("pv_photo_wrap"))?.Displayed;
            return inView != null && (bool)inView;
        }

        //TODO: REMOVE TRY
        public bool IsAlbumViewOpened()
        {
            var inView = _driver.TryFindElement(By.Id("pv_albums_wrap"))?.Displayed;
            return inView != null && (bool)inView;
        }

        //USING TRYFIND
        public Dictionary<string, object> GetProfileInfoFields()
        {
            var fieldsDiv = _driver.FindElementsByClassName("profile_info_row");
            var fields = new Dictionary<string, object>();
            if (IsMoreProfileInfoExists())
            {
                ClickOnShowMoreProfileInfo();
            }
            if (fieldsDiv == null || fieldsDiv.Count == 0)
            {
                return null;
            }
            foreach (var fieldDiv in fieldsDiv)
            {
                var key = fieldDiv.FindElement(By.ClassName("label")).Text.Replace(":", "");
                var value = fieldDiv.TryFindElement(By.ClassName("labeled"))?.Text;
                if (value == null)
                {
                    continue;
                }
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
            return fields;
        }

        public int GetPostsCount()
        {
            return (_driver.FindElementByXPath("//*[@id='page_wall_posts']")).FindElements(By.ClassName("post")).Count;
        }
        /// <summary>
        /// Enter -1 to get all loaded posts
        /// </summary>
        /// <param name="maxCount">Ammout of posts for returning</param>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetPostsByCount(int maxCount)
        {
            var postsElements =
                (_driver.FindElementByXPath("//*[@id='page_wall_posts']")).FindElements(By.ClassName("post")).Select(post => post.GetAttribute("id")).ToList();

            if (maxCount == -1)
            {
                maxCount = postsElements.Count;
            }

            var posts = new List<Dictionary<string, string>>();
            for (int i = 0; i < maxCount && i < postsElements.Count; i++)
            {
                posts.Add(GetPost(postsElements[i]));
            }
            return posts.Count == 0 ? null : posts;
        }

        /// <summary>
        /// Get post by it's place nuber
        /// </summary>
        /// <param name="pos">Post number</param>
        /// <returns></returns>
        public Dictionary<string, string> GetPostByPosition(int pos)
        {
            var postsElements =
                (_driver.FindElementByXPath("//*[@id='page_wall_posts']")).FindElements(By.ClassName("post")).Select(post => post.GetAttribute("id")).ToList();
            if (pos < postsElements.Count)
            {
                return GetPost(postsElements[pos]);
            }
            return null;
        }

        //USING TRYFIND
        private Dictionary<string, string> GetPost(string postId)
        {
            var postFields = new Dictionary<string, string>();
            var postElement = _driver.FindElementById(postId);
            if (IsExpandWallPostTextExist(postElement) || IsExpandCopyQuotePostTextExist(postElement))
            {
                ScrollTo(postElement.Location.X, postElement.Location.Y);
            }

            var authorUrl = postElement.TryFindElement(By.ClassName("author"))?.GetAttribute("href");
            var authorName = postElement.TryFindElement(By.ClassName("author"))?.Text;
            var authorPostDate = postElement.TryFindElement(By.ClassName("rel_date"))?.Text;

            ExpandWallPostText(postElement);
            var authorWallPostText = postElement.TryFindElement(By.ClassName("_wall_post_cont"))?.Text;

            var copyAuthorUrl = postElement.TryFindElement(By.ClassName("copy_author"))?.GetAttribute("href");
            var copyAuthorName = postElement.TryFindElement(By.ClassName("copy_author"))?.Text;
            var copyAuthorPostDate = postElement.TryFindElement(By.ClassName("rel_date"))?.Text;


            ExpandCopyQuotePostText(postElement);
            var copyAuthorWallPostText = postElement.TryFindElement(By.ClassName("copy_quote"))?.TryFindElement((By.ClassName("wall_post_text")))?.Text;

            AddPostFieldTo(postFields, PostAuthorUrlField, authorUrl);
            AddPostFieldTo(postFields, PostAuthorNameField, authorName);
            AddPostFieldTo(postFields, PostAuthorPostDateField, authorPostDate);
            AddPostFieldTo(postFields, PostAuthorWallPostTextField, authorWallPostText);

            AddPostFieldTo(postFields, PostCopyAuthorUrlField, copyAuthorUrl);
            AddPostFieldTo(postFields, PostCopyAuthorNameField, copyAuthorName);
            AddPostFieldTo(postFields, PostCopyAuthorPostDateField, copyAuthorPostDate);
            AddPostFieldTo(postFields, PostCopyAuthorWallPostTextField, copyAuthorWallPostText);

            return postFields;
        }

        private void AddPostFieldTo(Dictionary<string, string> postFields, string fieldKey, string fieldValue)
        {
            if (fieldValue == null)
            {
                return;
            }
            postFields.Add(fieldKey, fieldValue);
        }

        //USING TRYFIND
        private void ExpandWallPostText(IWebElement postElement)
        {
            postElement.TryFindElement(By.ClassName("_wall_post_cont"))?
                .TryFindElement(By.ClassName("wall_post_more"))?
                .Click();
        }

        //USING TRYFIND
        private bool IsExpandWallPostTextExist(IWebElement postElement)
        {
            var expandElement = postElement.TryFindElement(By.ClassName("_wall_post_cont"))?
                .TryFindElement(By.ClassName("wall_post_more"));
            if (expandElement != null)
            {
                return true;
            }
            return false;
        }

        //USING TRYFIND
        private void ExpandCopyQuotePostText(IWebElement postElement)
        {
            postElement.TryFindElement(By.ClassName("copy_quote"))?
                .TryFindElement(By.ClassName("wall_post_more"))?
                .Click();
        }

        //USING TRYFIND
        private bool IsExpandCopyQuotePostTextExist(IWebElement postElement)
        {
            var expandElement = postElement.TryFindElement(By.ClassName("copy_quote"))?
                .TryFindElement(By.ClassName("wall_post_more"));
            if (expandElement != null)
            {
                return true;
            }
            return false;
        }

        public void WaitToPhotosView()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.ClassName("pv_photo_wrap")).Displayed);
        }

        public void WaitToPhotosViewCounter()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.ClassName("pv_counter")).Displayed);
        }

        public void WaitUntilImageLoads()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.XPath("//*[@id='pv_photo']/img")).Displayed);
        }

        public void WaitToAlbumView()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.ClassName("photos_album_thumb")).Displayed);
        }

        public void WaitUntilImageChanges(string oldUrl)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => GetPhotoUrlInPhotosView() != oldUrl);
        }
    }
}
