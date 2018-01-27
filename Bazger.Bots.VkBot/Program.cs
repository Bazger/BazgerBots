using System;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Bazger.Bots.VkBot.Config;

namespace Bazger.Bots.VkBot
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            RemoteWebDriver driver = new ChromeDriver();
            //VkConnector
            var vkConnector = new VkConnector();
            if (!vkConnector.Connect(driver))
            {
                System.Console.WriteLine("Can't Connect to VK");
                return;
            }

            //VkSearchResultsScanner
            var vkSearchScanner = new VkSearchResultsScanner();
            Dictionary<string, List<string>> searchResults;

            if (!VkConfigs.GetConfig().ProfileScanner.LoadFromSavedResults)
            {
                searchResults = vkSearchScanner.Search(driver);
            }
            else
            {
                try
                {
                    searchResults =
                        SerDeUtils.DeserializeJsonFile<Dictionary<string, List<string>>>(
                            VkSearchResultsScanner.ResultsOutputPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Can't load results from file. " + e);
                    return;
                }
            }

            //VkProfileScanner
            var vkProfileScanner = new VkUserProfileScanner();
            vkProfileScanner.Scan(driver, searchResults);
        }
    }
}
