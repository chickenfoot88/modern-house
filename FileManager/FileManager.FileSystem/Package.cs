using FileManager.Core.Interfaces;
using FileManager.FileSystem.Services;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace FileManager.FileSystem
{
    public class Package : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<IFileManager, FileSystemFileManager>(Lifestyle.Transient);
        }

        public static void Bootstrap()
        {

        }
    }
}
