using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace PngJpegComparer
{
    /// <summary>
    /// Class that contains methods that save files in different formats and evaluate their sizes.
    /// </summary>
    public static class FormatConverter
    {
        #region GetSize
        /// <summary>
        /// Returns the size of the specified file.
        /// </summary>
        /// <param name="file">File whose size has to be determained.</param>
        /// <returns>The size of the file.</returns>
        private static long GetSize(string file) => new FileInfo(file).Length;

        /// <summary>
        /// Returns the size of the specified image in a given image format.
        /// </summary>
        /// <param name="file">File whose size has to be determained.</param>
        /// <param name="imageType">Image type in which to eveluate the size of the file.</param>
        /// <returns>The size of the file in a given format.</returns>
        private static long GetSize(string file, ImageType imageType)
        {
            string name = Path.GetFileNameWithoutExtension(file);
            string path = Path.GetDirectoryName(file);
            Image originalImage = Image.FromFile(file);
            string newImageName = path + @"\" + name + "." + imageType.GetExtension();
            originalImage.Save(newImageName, imageType.GetImageFormat());
            originalImage.Dispose();
            long size = GetSize(newImageName);
            File.Delete(newImageName);
            return size;
        }
        #endregion

        /// <summary>
        /// The name of the file and it's subpath inside the folder without the file extension.
        /// </summary>
        /// <param name="folder">Absolute path of the folder containing the file (not necessarily dirrectly).</param>
        /// <param name="file">Absolute path of the file.</param>
        /// <returns>The name of the the file and it's subpath inside the folder without the file extension.</returns>
        private static string GetFileNameAndPartialPath(string folder, string file)
        {
            int pl = folder.Length;
            return file.Substring(pl + 1, file.Length - pl - 5);
        }

        #region SaveExamples
        /// <summary>
        /// Saves example files in specified file formats.
        /// </summary>
        /// <param name="exampleFiles">A list of files to be converted.</param>
        /// <param name="sourceFolder">Root folder of exampleFiles.</param>
        /// <param name="targetFolder">Folder where all of the example files and converted files should be saved.</param>
        /// <param name="fromType">Type of the exampleFiles that have to be converted.</param>
        /// <param name="toTypes">Types to which exampleFiles have to be converted.</param>
        public static void SaveExamples(
            List<string> exampleFiles, string sourceFolder, string targetFolder,
            ImageType fromType, List<ImageType> toTypes)
        {
            try
            {
                foreach (string file in exampleFiles)
                {
                    Image originalImage = Image.FromFile(file);
                    string name = sourceFolder != null
                        ? GetFileNameAndPartialPath(sourceFolder, file).Replace('\\', '#') + "."
                        : file + ".";
                    string newImageName = targetFolder + @"\" + name;

                    // Save in new location in old file type
                    try
                    {
                        originalImage.Save(newImageName + fromType.GetExtension(), fromType.GetImageFormat());
                        Console.WriteLine(name + fromType.GetExtension() + " created");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Error in FormatConverter.SaveExamples, file: " + file + ", type: " + fromType.ToString(), ex);
                    }

                    // Save in all requested file types
                    foreach (ImageType imageType in toTypes)
                    {
                        try
                        {
                            originalImage.Save(newImageName + imageType.GetExtension(), imageType.GetImageFormat());
                            Console.WriteLine(name + imageType.GetExtension() + " created");
                        }
                        catch (Exception ex)
                        {
                            Logger.Log("Error in FormatConverter.SaveExamples, file: " + file + ", type: " + imageType.ToString(), ex);
                        }
                    }
                    originalImage.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error in FormatConverter.SaveExamples", ex);
            }
        }

        /// <summary>
        /// Saves example files in specified file formats.
        /// </summary>
        /// <param name="exampleFiles">A list of files to be converted.</param>
        /// <param name="targetFolder">Folder where all of the example files and converted files should be saved.</param>
        /// <param name="fromType">Type of the exampleFiles that have to be converted.</param>
        /// <param name="toTypes">Types to which exampleFiles have to be converted.</param>
        public static void SaveExamples(
            List<string> exampleFiles, string targetFolder,
            ImageType fromType, List<ImageType> toTypes)
        {
            SaveExamples(exampleFiles, null, targetFolder, fromType, toTypes);
        }

        /// <summary>
        /// Saves example files in specified file formats.
        /// </summary>
        /// <param name="sourceFolder">Root folder of exampleFiles.</param>
        /// <param name="targetFolder">Folder where all of the example files and converted files should be saved.</param>
        /// <param name="numExamples">Required number of examples.</param>
        /// <param name="fromType">Type of the exampleFiles that have to be converted.</param>
        /// <param name="toTypes">Types to which exampleFiles have to be converted.</param>
        /// <param name="searchOption">Specifies if example files should be only from the top directory or subdirectories as well.</param>
        public static void SaveExamples(
            string sourceFolder, string targetFolder, int numExamples,
            ImageType fromType, List<ImageType> toTypes,
            SearchOption searchOption)
        {
            try
            {

                Console.WriteLine("\nCreating example images from folder " + sourceFolder + "...");
                Random random = new Random();
                string[] allFiles = Directory.GetFiles(sourceFolder, "*.*", searchOption);
                List<string> files = new List<string>();
                foreach (string file in allFiles)
                {
                    if (Path.GetExtension(file) == "." + fromType.GetExtension())
                    {
                        files.Add(file);
                    }
                }

                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                List<string> exampleFiles = files.OrderBy(x => random.Next()).Take(numExamples).ToList();

                SaveExamples(exampleFiles, sourceFolder, targetFolder, fromType, toTypes);
            }
            catch (Exception ex)
            {
                Logger.Log("Error in FormatConverter.SaveExamples", ex);
            }
        }
        #endregion

        /// <summary>
        /// Creates a csv file and saves the sizes of the files inside the folder in different file formats.
        /// </summary>
        /// <param name="folder">Folder inside of which are the files whose sizes have to be evaluated.</param>
        /// <param name="resultsFile">Absolute path of the csv file in which results of the evaluations will be saved.</param>
        /// <param name="fromType">Format of the files whose sizes has to be evaluated.</param>
        /// <param name="toTypes">List of file formats to which files have to be converted for size evaluation.</param>
        /// <param name="searchOption">Specifies if files for evaluation should be only from the top directory or subdirectories as well.</param>
        public static void CompareFormats(
            string folder, string resultsFile,
            ImageType fromType, List<ImageType> toTypes,
            SearchOption searchOption
            )
        {
            try
            {
                HashSet<ImageData> data = new HashSet<ImageData>();

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("File,");
                stringBuilder.Append(fromType.ToString());
                stringBuilder.Append(" Size");
                foreach (ImageType imageType in toTypes)
                {
                    stringBuilder.Append(",");
                    stringBuilder.Append(imageType.ToString());
                    stringBuilder.Append(" Size");
                }
                string firstLine = stringBuilder.ToString();

                List<string> output = new List<string>() { firstLine };
                long[] totalSizes = new long[toTypes.Count + 1];
                int i = 1;

                // Reload data that already exists
                if (File.Exists(resultsFile))
                {
                    List<string> existingResults = new List<string>(File.ReadAllLines(resultsFile));
                    if (existingResults != null &&
                        existingResults.Count > 0 &&
                        existingResults[0] == firstLine
                        )
                    {
                        long[] sizes = new long[toTypes.Count + 1];
                        for (int j = 1; j < existingResults.Count; j++)
                        {
                            string[] array = existingResults[j].Split(',');
                            if (array.Length == totalSizes.Length + 1)
                            {
                                bool success = true;
                                for (int k = 1; k < array.Length; k++)
                                {
                                    if (!long.TryParse(array[k], out sizes[k - 1]))
                                    {
                                        success = false;
                                    }
                                }
                                if (success)
                                {
                                    ImageData imageData = new ImageData()
                                    {
                                        File = array[0]
                                    };
                                    imageData.Sizes = new List<long>(sizes);
                                    data.Add(imageData);
                                    output.Add(imageData.ToString());

                                    for (int k = 0; k < totalSizes.Length; k++)
                                    {
                                        totalSizes[k] += sizes[k];
                                    }
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("\nExisting results found for " + data.Count + " files.");

                // Convert remaining files
                Console.WriteLine("\nConverting files...");
                foreach (string file in Directory.GetFiles(folder, "*.*", searchOption))
                {
                    try
                    {
                        string extension = Path.GetExtension(file);
                        if (extension == "." + fromType.GetExtension())
                        {
                            ImageData imageData = new ImageData()
                            {
                                File = GetFileNameAndPartialPath(folder, file),
                            };
                            if (data.Contains(imageData))
                            {
                                Console.WriteLine((i++).ToString() + ". File " + imageData.File + " skipped.");
                            }
                            else
                            {
                                imageData.Sizes.Add(GetSize(file));
                                for (int j = 0; j < toTypes.Count; j++)
                                {
                                    imageData.Sizes.Add(GetSize(file, toTypes[j]));
                                }
                                data.Add(imageData);
                                output.Add(imageData.ToString());
                                for (int j = 0; j < totalSizes.Length; j++)
                                {
                                    totalSizes[j] += imageData.Sizes[j];
                                }

                                StringBuilder sb = new StringBuilder((i++).ToString());
                                sb.Append(". ");
                                sb.Append(fromType.ToString());
                                sb.Append(": ");
                                sb.Append(imageData.Sizes[0].ToString());

                                for (int j = 1; j < imageData.Sizes.Count; j++)
                                {
                                    sb.Append(", ");
                                    sb.Append(toTypes[j - 1].ToString());
                                    sb.Append(": ");
                                    sb.Append(imageData.Sizes[j].ToString());
                                }

                                Console.WriteLine(sb.ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Error in FormatConverter.CompareFormats, file " + Path.GetFileNameWithoutExtension(file), ex);
                    }
                }

                StringBuilder averagesSb = new StringBuilder();
                averagesSb.Append(",Average ");
                averagesSb.Append(fromType.ToString());
                averagesSb.Append(" Size");
                foreach (ImageType imageType in toTypes)
                {
                    averagesSb.Append(",Average ");
                    averagesSb.Append(imageType.ToString());
                    averagesSb.Append(" Size");
                }
                output.Add(averagesSb.ToString());

                // Calculate averages
                double[] averages = new double[toTypes.Count + 1];
                averagesSb = new StringBuilder();
                for (int j = 0; j < averages.Length; j++)
                {
                    averages[j] = (double)totalSizes[j] / data.Count;
                    averagesSb.Append(",");
                    averagesSb.Append(averages[j].ToString());

                    string imageType = (j == 0 ? fromType : toTypes[j - 1]).ToString();
                    Console.WriteLine("Average " + imageType + "size: " + averages[j].ToString());
                }

                output.Add(averagesSb.ToString());
                File.WriteAllLines(resultsFile, output);
            }
            catch (Exception ex)
            {
                Logger.Log("Error in FormatConverter.CompareFormats", ex);
            }
        }
    }
}
