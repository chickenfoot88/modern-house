using System;
using System.IO;
using Core.DataAccess.Interfaces;
using FileManager.Core.Entities;
using FileManager.Core.Interfaces;
using FileManager.FileSystem.Entities;
using FileManager.FileSystem.Utils;

namespace FileManager.FileSystem.Services
{
    public class FileSystemFileManager : IFileManager
    {
        public IDataStore DataStore { get; set; }

        public string FileStorageDir { get; set; }

        public FileSystemFileManager(IDataStore dataStore)
        {
            FileStorageDir = Settings.FileStorageDir;
            DataStore = dataStore;
        }
        
        public AttachedFileInfo Get(long id)
        {
            var fi = DataStore.Get<AttachedFileInfo>(id);
            if (fi == null || fi.Deleted)
            {
                return null;
            }
            return fi;
        }

        public AttachedFileData GetData(long id)
        {
            var fi = Get(id);
            return GetData(fi);
        }

        public AttachedFileData GetData(AttachedFileInfo fileInfo)
        {
            var filePath = CheckAndGetFilePath(fileInfo);

            var data = new FileSystemAttachedFIleData
            {
                FileInfo = fileInfo,
                FileData = File.ReadAllBytes(filePath)
            };

            return data;
        }

        public bool CheckFile(long id)
        {
            var fi = Get(id);
            return CheckFile(fi);
        }

        public bool CheckFile(AttachedFileInfo fi)
        {
            try
            {
                var filePath = CheckAndGetFilePath(fi);
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            return true;
        }

        public AttachedFileInfo Create(string fileName, byte[] data)
        {
            var fi = new AttachedFileInfo
            {
                FileName = Path.GetFileNameWithoutExtension(fileName),
                Extension = Path.GetExtension(fileName)
            };

            var filePath = CheckAndGetFilePath(fi, false);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.Create(filePath).Close();

            File.WriteAllBytes(filePath, data);

            DataStore.Save(fi);

            return fi;
        }

        /// <summary>
        /// Создать файл
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="data">Данные</param>
        /// <returns>Информация о файле</returns>
        public AttachedFileInfo Create(string fileName, Stream data)
        {
            using (var memStream = new MemoryStream())
            {
                data.CopyTo(memStream);

                return Create(fileName, memStream.ToArray());
            }
        }

        public void Delete(long id)
        {
            var fi = Get(id);
            Delete(fi);
        }

        public void Delete(AttachedFileInfo fi)
        {
            if (CheckFile(fi))
            {
                var filePath = CheckAndGetFilePath(fi);
                File.Delete(filePath);

                DataStore.Delete(fi);
            }
        }



        private string CheckAndGetFilePath(AttachedFileInfo fi, bool checkFilePath = true)
        {
            if (fi == null)
            {
                throw new FileNotFoundException();
            }

            var hash = Hasher.GetHash(fi.FullName);
            var fileName = $"{hash}.bin";
            var fileStorageDir = FileStorageDir;

            if (!Directory.Exists(fileStorageDir))
            {
                Directory.CreateDirectory(fileStorageDir);
            }

            var filePath = Path.Combine(fileStorageDir, fileName);

            if (checkFilePath)
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException();
                }
            }

            return filePath;
        }
    }
}
