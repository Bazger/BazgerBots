using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazger.Bots.VkBot.Config
{
    public class VkConfigs : ConfigurationSection
    {
        public static readonly string Login = GetConfig().ConnectSettings.Login;
        public static readonly string Password = GetConfig().ConnectSettings.Password;

        public static readonly string BaseDir = GetConfig().BaseDirectory;
        public static readonly string ResultsFileName = GetConfig().SearchLinks.ResultsOutputFileName;
        public static readonly string LinksFileName = GetConfig().SearchLinks.LinksInputFileName;

        public static readonly int ProfilePhotosCount = GetConfig().ProfileScanner.ProfilePhotosCount;
        public static readonly int GalleryPhotosCount = GetConfig().ProfileScanner.GalleryPhotosCount;
        public static readonly int PostsCount = GetConfig().ProfileScanner.PostsCount;
        public static readonly bool LoadFromSavedResults = GetConfig().ProfileScanner.LoadFromSavedResults;


        public static VkConfigs GetConfig()
        {
            return (VkConfigs)ConfigurationManager.GetSection("vkConfigs") ?? new VkConfigs();
        }

        [ConfigurationProperty("connectSettings", IsRequired = true)]
        public VkConnectSettings ConnectSettings
        {
            get
            {
                return (VkConnectSettings)this["connectSettings"];
            }
            set
            {
                this["connectSettings"] = value;
            }
        }

        [ConfigurationProperty("searchResultsScanner", IsRequired = true)]
        public VkSearchResultsSettings SearchLinks
        {
            get
            {
                return (VkSearchResultsSettings)this["searchResultsScanner"];
            }
            set
            {
                this["searchResultsScanner"] = value;
            }
        }

        [ConfigurationProperty("userProfileScanner", IsRequired = true)]
        public VkProfileScannerSettings ProfileScanner
        {
            get
            {
                return (VkProfileScannerSettings)this["userProfileScanner"];
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
