using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.Text;

namespace PngJpegComparer
{
    public static class FormatConverter
    {
        #region GetSize
        private static long GetSize(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            return fileInfo.Length;
        }

        private static long GetSize(string file, FileType fileType)
        {
            string name = Path.GetFileNameWithoutExtension(file);
            string path = Path.GetDirectoryName(file);
            Image originalImage = Image.FromFile(file);
            string newImageName = path + @"\" + name + "." + fileType.GetExtension();
            originalImage.Save(newImageName, fileType.GetImageFormat());
            originalImage.Dispose();
            long size = GetSize(newImageName);
            File.Delete(newImageName);
            return size;
        }
        #endregion
        
        private static string GetFileNameAndPartialPath(string folder, string file)
        {
            int pl = folder.Length;
            return file.Substring(pl + 1, file.Length - pl - 5);
        }
        
        public static void CompareFormats(
            string folder, string resultsFile, 
            FileType fromType, List<FileType> toTypes, 
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
                foreach (FileType fileType in toTypes)
                {
                    stringBuilder.Append(",");
                    stringBuilder.Append(fileType.ToString());
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
                foreach (FileType fileType in toTypes)
                {
                    averagesSb.Append(",Average ");
                    averagesSb.Append(fileType.ToString());
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

                    string fileType = (j == 0 ? fromType : toTypes[j - 1]).ToString();
                    Console.WriteLine("Average " + fileType + "size: " + averages[j].ToString());
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
