using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using FileHelpers;
using GProof.Alerta.ExcelHelper.Entities;

namespace GProof.Alerta.ExcelHelper
{
    public static class FileHelper
    {
        private static readonly List<string> FileExtension = new List<string> { ".xlsx", ".xls" };

        public static void OpenFile(string fullFileName)
        {
            Process.Start(new ProcessStartInfo(fullFileName) { UseShellExecute = true });
        }
        
        public static DataFile CreateDataFile(string fileName, byte[] content)
        {
            return new DataFile
            {
                StreamFactory = () => new MemoryStream(content),
                Filename = fileName
            };
        }
        
        public static string CreateFileAtTmpFolder(string fileName, byte[] content)
        {
            string filePath = Path.Combine(GenerateTempDirectory("Optiways"), fileName);

            File.WriteAllBytes(filePath, content);

            return filePath;
        }

        public static string CreateFileAtTmpFolder(string fileName, string content)
        {
            string filePath = Path.Combine(GenerateTempDirectory("Optiways"), fileName);

            File.WriteAllText(filePath, content);

            return filePath;
        }

        public static DataFile CreateDataFile(string filePath)
            => new DataFile
            {
                StreamFactory = () => File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
                Filename = filePath
            };

        public static DateTime GetDateFromFileName(string fileName)
        {
            List<Regex> stringDatePatterns = new List<Regex>
            {
                new Regex("(?:.*)?([0-9][0-9][.][0-9][0-9])(?:.*)?"), //bla bla 25.08 bla bla
                new Regex("(?:.*)?([0-9][0-9][.][0-9])(?:.*)?"), //bla bla 25.8 bla bla
                new Regex("(?:.*)?([0-9][.][0-9][0-9])(?:.*)?"), //bla bla 5.08 bla bla
                new Regex("(?:.*)?([0-9][.][0-9])(?:.*)?") //bla bla 5.8 bla bla
            };

            foreach (Regex stringDatePattern in stringDatePatterns)
            {
                if (stringDatePattern.IsMatch(fileName))
                {
                    GroupCollection groupCollection = stringDatePattern.Match(fileName).Groups;
                    if (groupCollection.Count != 2)
                        continue;

                    string[] dateStrings = groupCollection[1].Value.Split(".");
                    if (dateStrings.Length == 2)
                    {
                        int day = int.Parse(dateStrings[0]);
                        int month = int.Parse(dateStrings[1]);

                        return new DateTime(DateTime.Now.Year, month, day).Date;
                    }

                }
            }


            return DateTime.MinValue;
        }

        public static List<string> GetFilesNames(string fullDirectoryPath)
        {
            return Directory.GetFiles(fullDirectoryPath).Where(file => FileExtension.Contains(Path.GetExtension(file))).ToList();
        }

        public static List<string> GetFilesNames(string fullDirectoryPath, List<string> fileExtension)
        {
            return Directory.GetFiles(fullDirectoryPath).Where(file => fileExtension.Contains(Path.GetExtension(file))).ToList();
        }

        /*
        public static string GetFullDirctoryPath(string dataRootFolder, string subDirctoryName)
        {
            //string rootPath = ConfigurationHelper.RetriveAppSettingsValue(DataFilesRootPathConfiguration);
            string rootPath = ConfigurationHelper.RetriveAppSettingsValue(dataRootFolder);

            if (rootPath != null && rootPath.Trim() != "")
            {
                string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Path.Combine(executableLocation, rootPath, subDirctoryName);
            }


            return Path.Combine(dataRootFolder, subDirctoryName);
        }
        */

        public static void MoveFileWithDateTimeStamp(string sourceFile, string targetFolder)
        {
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            string targetFullPath = Path.Combine(targetFolder, Path.GetFileNameWithoutExtension(sourceFile) + " - " + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") +
                                                               Path.GetExtension(sourceFile));
            File.Move(sourceFile, targetFullPath);
        }

        public static List<T> ReadFileData<T>(string filePath) where T : class
        {
            return new FileHelperEngine<T>(Encoding.UTF8).ReadFile(filePath).ToList();
        }

        #region Serialization
        public static void Serialize<T>(T obj, string dir, string preFileName = null)
        {
            string typeName = obj is IList && obj.GetType().IsGenericType
                ? obj.GetType().GetGenericArguments()[0].Name + "s"
                : typeof(T).Name;

            var filePath = Path.Combine(dir, (preFileName ?? "") + typeName + ".dat");

            if (File.Exists(filePath))
            {
                //TODO: use connetigate's file helper
                File.Delete(filePath);
            }

            try
            {
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    // Construct a BinaryFormatter and use it to serialize the data to the stream.
                    var formatter = new BinaryFormatter();
                    try
                    {
                        formatter.Serialize(fs, obj);
                    }
                    catch (SerializationException e)
                    {
                        Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to open file:");
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public static T Deserialize<T>(string dir, string preFileName = null)
        {
            string typeName = typeof(T).IsGenericType ? typeof(T).GetGenericArguments()[0].Name + "s" : typeof(T).Name;
            var filePath = Path.Combine(dir, preFileName + typeName + ".dat");
            // Open the file containing the data that you want to deserialize.
            FileStream fs = new FileStream(filePath, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // Deserialize the hashtable from the file and 
                // assign the reference to the local variable.

                return (T)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }
        #endregion

        #region Temp Directory
        /// <summary>
        /// Returns the path of current user's temporary directory
        /// </summary>
        public static string TempDirectory
        {
            get { return Path.GetTempPath(); }
        }

        public static string GenerateTempDirectory(string subDirectory, string subSubDirectory, string subSubSubDirectory, bool overrideIfExists = false)
        {
            string path = Path.Combine(TempDirectory, subDirectory, subSubDirectory, subSubSubDirectory);

            return InternalGenerateTempDirectory(path, overrideIfExists);
        }

        public static string GenerateTempDirectory(string subDirectory, string subSubDirectory, bool overrideIfExists = false)
        {
            string path = Path.Combine(TempDirectory, subDirectory, subSubDirectory);

            return InternalGenerateTempDirectory(path, overrideIfExists);
        }

        public static string GenerateTempDirectory(string subDirectory, bool overrideIfExists = false)
        {
            string path = Path.Combine(TempDirectory, subDirectory);

            return InternalGenerateTempDirectory(path, overrideIfExists);
        }

        private static string InternalGenerateTempDirectory(string path, bool overrideIfExists)
        {
            if (Directory.Exists(path))
            {
                if (!overrideIfExists)
                {
                    return path;
                }

                try
                {
                    Directory.Delete(path);
                    Directory.CreateDirectory(path);
                    return path;
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch //Do nothing
                      // ReSharper restore EmptyGeneralCatchClause
                {
                }

            }

            if (!Directory.Exists(Directory.GetParent(path).FullName))
            {
                Directory.CreateDirectory(Directory.GetParent(path).FullName);
            }

            List<int> directoryIndecies = Directory.GetParent(path).EnumerateDirectories()
                          .Where(dir => dir.FullName.Contains(path))
                          .Select(dir => dir.FullName.Substring(path.Length - 1))
                          .Select(dirName => dirName.Substring(dirName.IndexOf('_') + 1))
                          .Where(nameSuffix =>
                          {
                              int tempIndex;
                              return int.TryParse(nameSuffix, out tempIndex);
                          }).Select(int.Parse).ToList();

            int maxDirctoryIndex = directoryIndecies.Count == 0 ? 0 : directoryIndecies.Max();

            string newPath = path + "_" + (maxDirctoryIndex + 1);

            string lastErrMsg = string.Empty;

            try
            {
                Directory.CreateDirectory(newPath);
                return newPath;
            }
            catch (Exception e)
            {
                lastErrMsg = e.Message;
            }

            throw new IOException(string.Format("Fail to create temporary file for the base path: {0}{1}{2} ", path,
                                                Environment.NewLine, lastErrMsg));
        }

        #endregion Temp Directory
    }
}
