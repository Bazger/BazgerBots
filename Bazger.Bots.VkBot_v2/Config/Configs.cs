using System.Configuration;
using log4net;

namespace Bazger.Bots.VkBot_v2.Config
{
    public class Configs : ConfigurationSection
    {
        public static ILog Logger = LogManager.GetLogger("AllLoggers");

        public static readonly string Login = GetConfig().ConnectSettings.Login;
        public static readonly string Password = GetConfig().ConnectSettings.Password;

        public static readonly string BaseDir = GetConfig().BaseDirectory;
        public static readonly string ResultsFileName = GetConfig().SearchLinks.ResultsOutputFileName;
        public static readonly string LinksFileName = GetConfig().SearchLinks.LinksInputFileName;

        public static readonly int ProfilePhotosCount = GetConfig().ProfileScanner.ProfilePhotosCount;
        public static readonly int GalleryPhotosCount = GetConfig().ProfileScanner.GalleryPhotosCount;
        public static readonly int PostsCount = GetConfig().ProfileScanner.PostsCount;
        public static readonly bool LoadFromSavedResults = GetConfig().ProfileScanner.LoadFromSavedResults;


        public static Configs GetConfig()
        {
            return (Configs)ConfigurationManager.GetSection("BotConfigs") ?? new Configs();
        }

        [ConfigurationProperty("connectSettings", IsRequired = true)]
        public ConnectSettings ConnectSettings
        {
            get
            {
                return (ConnectSettings)this["connectSettings"];
            }
            set
            {
                this["connectSettings"] = value;
            }
        }

        [ConfigurationProperty("searchResultsScanner", IsRequired = true)]
        public SearchResultsSettings SearchLinks
        {
            get
            {
                return (SearchResultsSettings)this["searchResultsScanner"];
            }
            set
            {
                this["searchResultsScanner"] = value;
            }
        }

        [ConfigurationProperty("userProfileScanner", IsRequired = true)]
        public ProfileScannerSettings ProfileScanner
        {
            get
            {
                return (ProfileScannerSettings)this["userProfileScanner"];
            }
            set
            {
                this["userProfileScanner"] = value;
            }
        }

        [ConfigurationProperty("baseDir", IsRequired = true)]
        public string BaseDirectory
        {
            get
            {
                return (string)this["baseDir"];
            }
            set
            {
                this["baseDir"] = value;
            }
        }

    }
}
