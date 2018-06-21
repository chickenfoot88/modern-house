using System;
using System.Configuration;
using System.IO;

namespace FileManager.FileSystem
{
    /// <summary>
    /// Настройки модуля
    /// </summary>
    public static class Settings
    {
        private static readonly string SettingsPrefix = "FileManager.FileSystem";

        private static readonly string FileStorageDirKey = "FileStorageDir";

        private static readonly string SecondPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FileManagerData");

        /// <summary>
        /// Путь до папки в которой будут храниться файлы
        /// </summary>
        public static string FileStorageDir => GetValue(FileStorageDirKey);


        private static string GetValue(string key)
        {
            var fullKey = $"{SettingsPrefix}.{key}";
            
            return ConfigurationManager.AppSettings[fullKey] ?? SecondPath;
        }
    }
}
