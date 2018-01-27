using System.IO;
using Newtonsoft.Json;

namespace Bazger.Bots.Core.Utils
{
    public static class SerDeUtils
    {
        public static TObject DeserializeJsonFile<TObject>(string path)
        {
            return JsonConvert.DeserializeObject<TObject>(File.ReadAllText(path));
        }

        public static void SerializeToJsonFile<TObject>(TObject results, string path)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(path))
            {
                using (JsonWriter writer = new JsonTextWriter(sw) { Formatting = Formatting.Indented })
                {
                    serializer.Serialize(writer, results);
                }
            }
        }
    }
}
