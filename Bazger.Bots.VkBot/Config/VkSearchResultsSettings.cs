using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazger.Bots.VkBot.Config
{
    public class VkSearchResultsSettings : ConfigurationElement
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
    }
}
