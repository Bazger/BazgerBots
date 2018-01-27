using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Bazger.Bots.Core
{
    public class BotStateBuilder
    {
        [JsonIgnore]
        public RemoteWebDriver Driver { get; private set; }

        [JsonProperty]
        public ComponentsStages RunningStage { get; set; }

        [JsonProperty]
        public string RunningComponent { get; set; }

        [JsonProperty]
        public Dictionary<string, object> ComponentsDataHandler { get; private set; }


        public BotStateBuilder(Type driverType = null)
        {
            if (driverType == null || driverType == typeof(RemoteWebDriver) || !driverType.IsSubclassOf(typeof(RemoteWebDriver)))
            {
                Driver = new ChromeDriver();
            }
            else
            {
                Driver = (RemoteWebDriver)Activator.CreateInstance(driverType);
            }
            ComponentsDataHandler = new Dictionary<string, object>();
        }
    }
}
