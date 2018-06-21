using System;
using System.ComponentModel;
using Core.DataAccess;
using Core.DataAccess.Interfaces;

namespace FileManager.Core.Entities
{
    /// <summary>
    /// Файл
    /// </summary>
    [Description("Файл")]
    public class AttachedFileInfo : PersistentObject, ISoftDeletable
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        [Description("Имя файла")]
        public string FileName { get; set; }

        /// <summary>
        /// Расширение файла
        /// </summary>
        [Description("Расширение файла")]
        public string Extension { get; set; }

        public string FullName => $"{FileName}{Extension}";

        /// <summary>
        /// Признак удаления
        /// </summary>
        [Description("Признак удаления")]
        public bool Deleted { get; set; }

        /// <summary>
        /// Дата удаления
        /// </summary>
        [Description("Дата удаления")]
        public DateTime? DeletedDate { get; set; }
    }
}
