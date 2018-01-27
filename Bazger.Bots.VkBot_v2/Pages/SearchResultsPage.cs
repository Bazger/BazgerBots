using System.Collections.Generic;
using System.Linq;
using Bazger.Bots.Infrastructure;
using OpenQA.Selenium.Remote;

namespace Bazger.Bots.VkBot_v2.Pages
{
    public class SearchResultsPage : WebPage, IChangableUrlPage
    {
        private string _url;

        public SearchResultsPage(RemoteWebDriver driver) : base(driver)
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

        public List<string> GetAllProfilesUrls()
        {
            var results = _driver.FindElementsByXPath("//*[@class='labeled name']/a");
            return results.Select(user => user.GetAttribute("href")).ToList();
        }
    }
}
