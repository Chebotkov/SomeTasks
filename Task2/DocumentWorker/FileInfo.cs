using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DocumentWorker
{
    public static class FileInfo
    {
        public static string[] AvailableExcelExtensions = { ".xls", ".xlsx", ".xlsm" };

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

        public static bool CanBeAdded(string filename)
        {
            if (IsFileSupported(filename))
            {
                if (!File.Exists())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
