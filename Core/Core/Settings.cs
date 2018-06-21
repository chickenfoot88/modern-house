namespace Core
{
    /// <summary>
    /// Настройки приложения
    /// </summary>
    public static class Settings
    {
        static Settings()
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;

            TestEnvironment = appSettings[TestEnvironmentKey]?.ToLower() != "false";
        }

        private const string TestEnvironmentKey = "TestEnvironment";
        /// <summary>
        /// Признак тестовой среды
        /// </summary>
        public static bool TestEnvironment { get; set; }
    }
}