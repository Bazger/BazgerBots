using System.Collections.Generic;
using Bazger.Bots.Core;
using Bazger.Bots.VkBot_v2.Components;
using Bazger.Bots.VkBot_v2.Config;
using log4net.Config;
using OpenQA.Selenium.Chrome;

namespace Bazger.Bots.VkBot_v2
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            var botComponents = new List<BotComponent>()
            {
                new ConnectorComponent(Configs.Login, Configs.Password),
                new SearchResultsScannerComponent(Configs.BaseDir, Configs.LinksFileName,  Configs.ResultsFileName, Configs.GetConfig().SearchLinks.ToRun),
                new UserProfileScannerComponent(Configs.BaseDir, Configs.ProfilePhotosCount, Configs.GalleryPhotosCount, Configs.PostsCount, Configs.ResultsFileName, Configs.LoadFromSavedResults)
            };
            BotLoader loader = new BotLoader(botComponents, typeof(ChromeDriver), Configs.BaseDir);
            loader.Start();
        }
    }
}
