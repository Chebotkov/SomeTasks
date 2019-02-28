using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    /// <summary>
    /// Works with files.
    /// </summary>
    public class FileWorker
    {
        public int FilesNumber { get; set; } = 100;
        public int RowsNumber { get; set; } = 100000;
        public string FileExtension { get; set; } = ".txt";
        public IDataGenerator Generator { get; set; }
        public event Action<int, int> ProcessInfo;

        /// <summary>
        /// Initializes instance of the FileWorker.
        /// </summary>
        /// <param name="generator">Data generator</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="generator"/> is null.</exception>
        public FileWorker(IDataGenerator generator)
        {
            if (generator is null)
            {
                throw new ArgumentNullException(String.Format("{0} can't be null.", generator));
            }

            Generator = generator;
        }

        /// <summary>
        /// Creates specified amount of files in the specified directory. 
        /// </summary>
        /// <param name="directory">Specified directory.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="directory"/> is null.</exception>
        /// <exception cref="Exception">Throws when some exeptions occurs.</exception>
        public void GenerateFiles(string directory)
        {
            if (directory is null)
            {
                throw new ArgumentNullException(String.Format("{0} can't be null.", directory));
            }

            if (!Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            string currentFileName;
            for (int i = 0; i < FilesNumber; i++)
            {
                currentFileName = String.Format("{0}\\{1}{2}", directory, i + 1, FileExtension);

                FillTheFile(currentFileName);
            }
        }

        /// <summary>
        /// Creates and fills specified file.
        /// </summary>
        /// <param name="filePath">Path to file.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="filePath"/> is null.</exception>
        /// <exception cref="IOException">Throws when exeptions occurs while working with file.</exception>
        /// <exception cref="Exception">Throws when some exeptions occurs.</exception>
        private void FillTheFile(string filePath)
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(String.Format("{0} can't be null.", filePath));
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    for (int i = 0; i < RowsNumber; i++)
                    {
                        writer.WriteLine(Generator.GetRow().ToString());
                    }

                    writer.Close();
                }
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("{0}\nPlease, change the directory path.", ex.Message));
            }
        }

        /// <summary>
        /// Merges all files from <paramref name="directoryWithFiles"/> into one <paramref name="newFileName"/> and deletes all entrance of <paramref name="deletedSequence"/>.
        /// </summary>
        /// <param name="directoryWithFiles">Specified directory.</param>
        /// <param name="newFileName">New file name.</param>
        /// <param name="deletedSequence">Specified sequence.</param>
        /// <returns>Amount of deleted lines.</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="directoryWithFiles"/> or <paramref name="deletedSequence"/> is null.</exception>
        /// <exception cref="DirectoryNotFoundException">Throws when directory wasn't found.</exception>
        /// <exception cref="ArgumentException">Throws when some exeptions with file occurs.</exception>
        /// <exception cref="IOException">Throws when exeptions occurs while working with file.</exception>
        /// <exception cref="Exception">Throws when some exeptions occurs.</exception>
        public int MergeFiles(string directoryWithFiles, string newFileName, string deletedSequence)
        {
            if (directoryWithFiles is null)
            {
                throw new ArgumentNullException(String.Format("{0} can't be null.", directoryWithFiles));
            }

            if (deletedSequence is null)
            {
                throw new ArgumentNullException(String.Format("{0} can't be null.", deletedSequence));
            }

            if (!Directory.Exists(directoryWithFiles))
            {
                throw new DirectoryNotFoundException("Wrong directory.");
            }

            if (newFileName is null)
            {
                newFileName = String.Format("{0}\\{1}{2}", directoryWithFiles, "Result", FileExtension);
            }

            if (File.Exists(newFileName))
            {
                try
                {
                    File.Delete(newFileName);
                }
                catch (Exception)
                {
                    throw new ArgumentException("Change file name.");
                }
            }

            string currentLine = null;
            string tempFileName = String.Format("{0}\\{1}{2}", directoryWithFiles, "Temp", FileExtension);

            int deletedlines = 0;

            try
            {
                foreach (string fileName in Directory.GetFiles(directoryWithFiles))
                {
                    if (Path.GetExtension(fileName) == FileExtension)
                    {
                        try
                        {
                            using (StreamWriter currentFileWriter = new StreamWriter(tempFileName, false, Encoding.Default))
                            {
                                using (StreamReader reader = new StreamReader(fileName))
                                {
                                    using (StreamWriter newFileWriter = new StreamWriter(newFileName, true, Encoding.Default))
                                    {
                                        while ((currentLine = reader.ReadLine()) != null)
                                        {
                                            if (!currentLine.Contains(deletedSequence))
                                            {
                                                newFileWriter.WriteLine(currentLine);
                                                currentFileWriter.WriteLine(currentLine);
                                            }
                                            else
                                            {
                                                deletedlines++;
                                            }
                                        }

                                        newFileWriter.Close();
                                    }

                                    reader.Close();
                                }

                                currentFileWriter.Close();
                            }
                        }
                        catch (Exception) { }

                        File.Delete(fileName);
                        File.Move(tempFileName, fileName);
                    }
                }
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("{0}\nPlease, change the directory path.", ex.Message));
            }

            return deletedlines;
        }

        /// <summary>
        /// Saves data to specified place using <paramref name="saver"/> interface.
        /// </summary>
        /// <param name="saveFromFile">Specified file with data.</param>
        /// <param name="saver">Interaface for saving.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="saveFromFile"/>, <paramref name="saver"/> or ProccesInfo event is null.</exception>
        /// <exception cref="Exception">Throws when some exeptions occurs.</exception>
        public void SaveData(string saveFromFile, IDataSaver saver)
        {
            if (saveFromFile is null)
            {
                throw new ArgumentNullException(String.Format("{0} can't be null.", saveFromFile));
            }

            if (saver is null)
            {
                throw new ArgumentNullException(String.Format("{0} can't be null.", saver));
            }

            if (ProcessInfo is null)
            {
                throw new ArgumentNullException(String.Format("You must be subscribed on event {0}.", ProcessInfo));
            }

            int currentLineNumber = 0;
            int totalLinesCount = 0;
            try
            {
                using (StreamReader reader = new StreamReader(saveFromFile))
                {
                    while (reader.ReadLine() != null)
                    {
                        totalLinesCount++;
                    }
                }
                
                using (StreamReader reader = new StreamReader(saveFromFile, Encoding.Default))
                {
                    string currentLine = null;
                    while ((currentLine = reader.ReadLine()) != null)
                    {
                        currentLineNumber++;
                        saver.SaveRow(RowRepresentation.ParseToRowRepresentation(currentLine));
                        ProcessInfo?.Invoke(currentLineNumber, totalLinesCount - currentLineNumber);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
