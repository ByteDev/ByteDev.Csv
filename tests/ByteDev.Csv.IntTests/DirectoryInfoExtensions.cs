using System.IO;

namespace ByteDev.Csv.IntTests
{
    internal static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Delete all files and directories
        /// </summary>
        public static void Empty(this DirectoryInfo source)
        {
            DeleteFiles(source);
            DeleteDirectories(source);
        }

        /// <summary>
        /// Delete all directories
        /// </summary>
        public static void DeleteDirectories(this DirectoryInfo source)
        {
            foreach (var dir in source.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        /// <summary>
        /// Delete all files
        /// </summary>
        public static void DeleteFiles(this DirectoryInfo source)
        {
            foreach (var file in source.GetFiles())
            {
                file.Delete();
            }
        }
    }
}