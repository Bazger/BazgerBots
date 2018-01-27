using System.Collections.Generic;
using System.IO;
using Bazger.Bots.Core;
using Bazger.Bots.Core.Utils;
using Bazger.Bots.VkBot_v2.Config;
using Bazger.Bots.VkBot_v2.Pages;

namespace Bazger.Bots.VkBot_v2.Components
{
    public class SearchResultsScannerComponent : BotComponent
    {
        private readonly string _baseDir;
        private readonly string _linksInputFileName;
        private readonly string ResultsOutputFileName;

        private SearchResultsPage _searchResultsPage;
        private Dictionary<string, string> _searchLinks;
        private Dictionary<string, List<string>> _searchResults;
        public string ResultsOutputPath => Path.Combine(_baseDir, ResultsOutputFileName);
        public string LinksInputPath => Path.Combine(_baseDir, _linksInputFileName);

        public SearchResultsScannerComponent(string baseDir, string linksInputFileName, string resultsOutputFileName, bool toRun = true) : base(toRun)
        {
            _baseDir = baseDir;
            _linksInputFileName = linksInputFileName;
            ResultsOutputFileName = resultsOutputFileName;
        }

        public override void Prepare(BotStateBuilder botState)
        {
            _searchLinks = SerDeUtils.DeserializeJsonFile<Dictionary<string, string>>(LinksInputPath);
            _searchResults = new Dictionary<string, List<string>>();
            _searchResultsPage = new SearchResultsPage(botState.Driver);
        }

        public override void Process(BotStateBuilder botState)
        {
            botState.ComponentsDataHandler.Add(this.GetType().Name, _searchResults);
            foreach (var link in _searchLinks)
            {
                _searchResultsPage.SetPageUrl(link.Value);
                _searchResultsPage.GoToPageUrl();
                _searchResultsPage.GoToPageUrl();
                _searchResultsPage.ScrollDownToPageEnd();
                _searchResults.Add(link.Key, _searchResultsPage.GetAllProfilesUrls());
                Configs.Logger.Info($"Resuls for link: {link.Key} was added to list");
            }
            SerDeUtils.SerializeToJsonFile(_searchResults, ResultsOutputPath);
        }
    }
}
