using System.Configuration;

namespace Bazger.Bots.VkBot_v2.Config
{
    public class ProfileScannerSettings : ConfigurationElement
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
