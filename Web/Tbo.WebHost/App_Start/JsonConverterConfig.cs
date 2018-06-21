using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Tbo.WebHost
{
    public static class JsonConverterConfig
    {
        public static void Configure()
        {
            JsonConvert.DefaultSettings = GetSettings;
        }

        private static JsonSerializerSettings GetSettings()
        {
            var defaultSettings = new JsonSerializerSettings();
            defaultSettings.Converters.Add(new StringEnumConverter());
            return defaultSettings;
        }
    }
}