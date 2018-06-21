using System.ComponentModel;
using System.IO;
using FileManager.Core.Entities;

namespace FileManager.Core.Interfaces
{
    /// <summary>
    /// Сервис для работы с файлами
    /// </summary>
    [Description("Сервис для работы с файлами")]
    public interface IFileManager
    {
        /// <summary>
        /// Получить информацию о файле
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Информация о файле</returns>
        [Description("Получить информацию о файле")]
        AttachedFileInfo Get(long id);

        /// <summary>
        /// Получить данные файла
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Данные файла</returns>
        [Description("Получить данные файла")]
        AttachedFileData GetData(long id);
        
        /// <summary>
        /// Получить данные файла
        /// </summary>
        /// <param name="fileInfo">Информация о файле</param>
        /// <returns>Данные файла</returns>
        [Description("Получить данные файла")]
        AttachedFileData GetData(AttachedFileInfo fileInfo);
        
        /// <summary>
        /// Проверить наличие файла
        /// </summary>
        /// <param name="id">Идентификатор файла</param>
        /// <returns>Реузультат проверки</returns>
        [Description("Проверить информацию о файле")]
        bool CheckFile(long id);
        
        /// <summary>
        /// Проверить наличие файла
        /// </summary>
        /// <param name="fileInfo">Информация о файле</param>
        /// <returns>Реузультат проверки</returns>
        [Description("Проверить информацию о файле")]
        bool CheckFile(AttachedFileInfo fileInfo);

        /// <summary>
        /// Создать файл
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="data">Данные</param>
        /// <returns>Информация о файле</returns>
        [Description("Создать файл")]
        AttachedFileInfo Create(string fileName, byte[] data);


        /// <summary>
        /// Создать файл
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="data">Данные</param>
        /// <returns>Информация о файле</returns>
        [Description("Создать файл")]
        AttachedFileInfo Create(string fileName, Stream data);

        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="id">Идентификатор</param>
        [Description("Удалить файл")]
        void Delete(long id);
        
        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="fileInfo">Информация о файле</param>
        [Description("Удалить файл")]
        void Delete(AttachedFileInfo fileInfo);
    }
}
