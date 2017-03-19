using System.IO;

namespace sel4.Helpers
{
    class FileHelper
    {
        public static string GetSupportFilesLocation()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var path = Path.GetDirectoryName(location).Replace(@"\bin\Debug","");
            DirectoryInfo currentDirectory = new DirectoryInfo(path);

            while (currentDirectory != null)
            {
                var directories = currentDirectory.GetDirectories();
                foreach (DirectoryInfo directory in directories)
                {
                    if (directory.Name == "SupportFiles")
                        return directory.FullName;
                }

                currentDirectory = currentDirectory.Parent;
            }

            return "";
        }

       
    }
}
