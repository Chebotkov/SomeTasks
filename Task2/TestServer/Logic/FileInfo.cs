using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestServer.Logic
{
    /// <summary>
    /// Gets Information about file.
    /// </summary>
    public static class FileInfo
    {
        public static string[] AvailableExcelExtensions = { ".xls", ".xlsx", ".xlsm" };

        /// <summary>
        /// Checks if file available for loading.
        /// </summary>
        /// <param name="fileName">Specified file.</param>
        /// <returns>True if file supported.</returns>
        private static bool IsFileSupported(string fileName)
        {
            string newFileExtension = Path.GetExtension(fileName);

            foreach (string extension in AvailableExcelExtensions)
            {
                if (newFileExtension == extension)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if file available for loading.
        /// </summary>
        /// <param name="filename">Specified file.</param>
        /// <returns>True if file supported.</returns>
        public static bool CanBeAdded(string filename)
        {
            if (IsFileSupported(filename))
            {
                if (!File.Exists(filename))
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Returns list of loaded files.
        /// </summary>
        /// <param name="filePath">Specified directory.</param>
        /// <returns>List of loaded files.</returns>
        public static IEnumerable<string> GetFiles(string filePath)
        {
            string[] allFiles = Directory.GetFiles(filePath);
            Queue<string> wantedFiles = new Queue<string>();

            foreach (string file in allFiles)
            {
                if (IsFileSupported(file))
                {
                    wantedFiles.Enqueue(Path.GetFileName(file));
                }
            }

            return wantedFiles;
        }

        /// <summary>
        /// Gets file name.
        /// </summary>
        /// <param name="file">Specified file.</param>
        /// <returns>File name.</returns>
        public static string GetFileName(string file)
        {
            return Path.GetFileName(file);
        }

        /// <summary>
        /// Deletes specified file.
        /// </summary>
        /// <param name="file">Specified file.</param>
        public static void DeleteFile(string file)
        {
            File.Delete(file);
        }
    }
}
