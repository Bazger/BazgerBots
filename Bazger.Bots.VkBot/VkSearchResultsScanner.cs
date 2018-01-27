using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Bazger.Bots.VkBot.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Bazger.Bots.VkBot
{
    public class VkSearchResultsScanner
    {

        public static readonly string ResultsOutputPath = Path.Combine(VkConfigs.BaseDir, VkConfigs.ResultsFileName);
        public static readonly string LinksInputPath = Path.Combine(VkConfigs.BaseDir, VkConfigs.LinksFileName);


        public Dictionary<string, List<string>> Search(RemoteWebDriver driver)
        {
            var allOutputResults = new Dictionary<string, List<string>>();
            var vkSearchLinks = SerDeUtils.DeserializeJsonFile<Dictionary<string, string>>(LinksInputPath);

            foreach (var vkSearchKey in vkSearchLinks.Keys)
            {

                var results = GetAllSearchResults(driver, vkSearchLinks[vkSearchKey]);
                allOutputResults.Add(vkSearchKey, results.Select(user => user.GetAttribute("href")).ToList());
            }

            SerDeUtils.SerializeToJsonFile(allOutputResults, ResultsOutputPath);

            return allOutputResults;
        }

        private IEnumerable<IWebElement> GetAllSearchResults(RemoteWebDriver driver, string link)
        {
            string xPathCommand = "//*[@class='labeled name']/a";
            driver.Navigate().GoToUrl(link);
            driver.Navigate().GoToUrl(link);
            var results = driver.FindElementsByXPath(xPathCommand);
            int resultsCount = 0;
            int oldResultsCount;
            do
            {
                oldResultsCount = resultsCount;
                driver.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                results = driver.FindElementsByXPath(xPathCommand);
                resultsCount = results.Count;
                //TODO: DRIVER WAIT
                Thread.Sleep(1000);
            } while (resultsCount != oldResultsCount);

            return results;
        }
    }
}
