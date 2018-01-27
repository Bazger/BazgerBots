using System.Configuration;

namespace Bazger.Bots.VkBot_v2.Config
{
    public class SearchResultsSettings : ConfigurationElement
    {

        [ConfigurationProperty("resultsFileName", IsRequired = true)]
        public string ResultsOutputFileName
        {
            get
            {
                return (string)this["resultsFileName"];
            }
            set
            {
                this["resultsFileName"] = value;
            }
        }


        [ConfigurationProperty("linksFileName", IsRequired = true)]
        public string LinksInputFileName
        {
            get
            {
                return (string)this["linksFileName"];
            }
            set
            {
                this["linksFileName"] = value;
            }
        }

        [ConfigurationProperty("toRun", IsRequired = false, DefaultValue = true)]
        public bool ToRun
        {
            get
            {
                return (bool)this["toRun"];
            }
            set
            {
                this["toRun"] = value;
            }
        }
    }
}
