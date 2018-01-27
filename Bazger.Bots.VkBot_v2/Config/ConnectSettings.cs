using System.Configuration;

namespace Bazger.Bots.VkBot_v2.Config
{
    public class ConnectSettings : ConfigurationElement
    {
        [ConfigurationProperty("login", IsRequired = true)]
        public string Login
        {
            get
            {
                return (string)this["login"];
            }
            set
            {
                this["login"] = value;
            }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }
    }
}
