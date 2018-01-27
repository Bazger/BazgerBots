using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazger.Bots.VkBot.Config
{
    public class VkProfileScannerSettings : ConfigurationElement
    {

        [ConfigurationProperty("galleryPhotosCount", IsRequired = true, DefaultValue = 0)]
        public int GalleryPhotosCount
        {
            get
            {
                return (int)this["galleryPhotosCount"];
            }
            set
            {
                this["galleryPhotosCount"] = value;
            }
        }

        [ConfigurationProperty("profilePhotosCount", IsRequired = true, DefaultValue = 0)]
        public int ProfilePhotosCount
        {
            get
            {
                return (int)this["profilePhotosCount"];
            }
            set
            {
                this["profilePhotosCount"] = value;
            }
        }

        [ConfigurationProperty("postsCount", IsRequired = true, DefaultValue = 0)]
        public int PostsCount
        {
            get
            {
                return (int)this["postsCount"];
            }
            set
            {
                this["postsCount"] = value;
            }
        }

        [ConfigurationProperty("loadFromBackup", DefaultValue = false)]
        public bool LoadFromSavedResults
        {
            get
            {
                return (bool)this["loadFromBackup"];
            }
            set
            {
                this["loadFromBackup"] = value;
            }
        }
    }
}
