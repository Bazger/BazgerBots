using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazger.Bots.VkBot.Config
{
    public class VkConnectSettings : ConfigurationElement
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
