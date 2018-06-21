using System;
using System.IO;
using System.Linq;
using DAL.EF;
using DAL.EF.Implementations;
using FileManager.Core.Entities;
using FileManager.Core.Interfaces;
using FileManager.FileSystem.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileManager.FileSystem.Test
{
    [TestClass]
    public class FileSystemFileManagerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var fileName = GetFileName();
            var data = GetData();
            var fileManager = GetFileManager();


            var fi = TestCreate(fileManager, fileName, data);

            TestCheckFile(fileManager, fi);

            TestGetFileAndData(fileManager, fi);

            TestDeleteFile(fileManager, fi);
        }




        private void TestDeleteFile(IFileManager fileManager, AttachedFileInfo fi)
        {
            var checkResult = fileManager.CheckFile(fi.Id);
            Assert.IsTrue(checkResult);

            fileManager.Delete(fi);

            var checkDeletedResult = fileManager.CheckFile(fi.Id);
            Assert.IsFalse(checkDeletedResult);
        }

        private void TestGetFileAndData(IFileManager fileManager, AttachedFileInfo fi1)
        {
            Assert.IsNotNull(fi1);
            var fd1 = fileManager.GetData(fi1);
            Assert.IsNotNull(fd1);

            var fi2 = fileManager.Get(fi1.Id);
            Assert.IsNotNull(fi2);
            var fd2 = fileManager.GetData(fi2.Id);
            Assert.IsNotNull(fd2);

            var fiEqualsResult = CompareFi(fi1, fi2);
            Assert.IsTrue(fiEqualsResult);

            var fdEqualsResult = CompareFd(fd1, fd2);
            Assert.IsTrue(fdEqualsResult);
        }

        private void TestCheckFile(IFileManager fileManager, AttachedFileInfo fi)
        {
            var checkResult = fileManager.CheckFile(fi);
            Assert.IsTrue(checkResult);

            var checkResult2 = fileManager.CheckFile(fi.Id);
            Assert.IsTrue(checkResult2);

            try
            {
                fileManager.CheckFile(-1);
            }
            catch (Exception e)
            {
                var correctException = e is FileNotFoundException;
                Assert.IsTrue(correctException);
            }
        }

        private AttachedFileInfo TestCreate(IFileManager fileManager, string fileName, byte[] data)
        {
            var fi = fileManager.Create(fileName, data);
            Assert.IsNotNull(fi);
            Assert.AreNotEqual(fi.Id, 0);
            Assert.AreEqual(fi.FullName, fileName);

            return fi;
        }


        #region private

        private string GetFileName()
        {
            return "TestExcel.xlsx";
        }

        private byte[] GetData()
        {
            return TestResources.TestExcel;
        }

        private IFileManager GetFileManager()
        {
            var fileStorageDir = Path.Combine(Path.GetTempPath(), "FileManagerFileSystem");
            
            //delete existing data
            if (Directory.Exists(fileStorageDir))
            {
                Directory.Delete(fileStorageDir, true);
            }
            Directory.CreateDirectory(fileStorageDir);

            var dbContext = new ApplicationDbContext();
            var dataStore = new DataStore(dbContext);
            var fileManager = new FileSystemFileManager(dataStore)
            {
                DataStore = dataStore
            };

            //delete existing info from db
            foreach (var fileInfo in dataStore.GetAll<AttachedFileInfo>().ToList())
            {
                dataStore.Delete(fileInfo);
            }

            return fileManager;
        }

        private bool CompareFi(AttachedFileInfo fi1, AttachedFileInfo fi2)
        {
            var result = true;
            result &= fi1.Id == fi2.Id;
            result &= fi1.Deleted == fi2.Deleted;
            result &= fi1.DeletedDate == fi2.DeletedDate;
            result &= fi1.Extension == fi2.Extension;
            result &= fi1.FileName == fi2.FileName;

            return result;
        }

        private bool CompareFd(AttachedFileData fd1, AttachedFileData fd2)
        {
            var result = true;
            result &= fd1.FileData.Length == fd2.FileData.Length;
            if (result)
            {
                for (int i = 0; i < fd1.FileData.Length; i++)
                {
                    result &= fd1.FileData[i] == fd2.FileData[i];
                }
            }

            return result;
        }

        #endregion
    }
}
