using System.ComponentModel;

namespace FileManager.Core.Entities
{
    /// <summary>
    /// Содержимое файла
    /// </summary>
    [Description("Содержимое файла")]
    public abstract class AttachedFileData
    {
        /// <summary>
        /// Информация о файле
        /// </summary>
        [Description("Информация о файле")]
        public AttachedFileInfo FileInfo { get; set; }

        /// <summary>
        /// Данные
        /// </summary>
        [Description("Данные")]
        public virtual byte[] FileData { get; set; }
    }
}
