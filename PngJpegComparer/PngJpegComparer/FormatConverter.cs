using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System;

namespace PngJpegComparer
{
    public static class FormatConverter
    {
        #region General
        private static long GetSize(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            return fileInfo.Length;
        }

        private static long ConvertAndGetSize(string file, string toExtension, ImageFormat toFormat)
        {
            string name = Path.GetFileNameWithoutExtension(file);
            string path = Path.GetDirectoryName(file);
            Image originalImage = Image.FromFile(file);
            string newImageName = path + @"\" + name + "." + toExtension;
            originalImage.Save(newImageName, toFormat);
            originalImage.Dispose();
            long size = GetSize(newImageName);
            File.Delete(newImageName);
            return size;
        }

        private static long GetPngSize(string file) => ConvertAndGetSize(file, "png", ImageFormat.Png);

        private static long GetJpegSize(string file) => ConvertAndGetSize(file, "jpg", ImageFormat.Jpeg);
        #endregion

        #region RAISE
        public static void CompareFormatsTiffToPngAndJpeg(string folder, string resultsFile)
        {
            try
            {
                HashSet<ImageData> data = new HashSet<ImageData>();
                List<string> output = new List<string>() { "File,TIFF Size,PNG Size,JPEG Size" };
                long totalTiffSize = 0;
                long totalPngSize = 0;
                long totalJpegSize = 0;
                int i = 1;

                // Reload data that already exists
                if (File.Exists(resultsFile))
                {
                    List<string> existingResults = new List<string>(File.ReadAllLines(resultsFile));
                    if (existingResults != null &&
                        existingResults.Count > 0 &&
                        existingResults[0] == "File,TIFF Size,PNG Size,JPEG Size"
                        )
                    {
                        for (int j = 1; j < existingResults.Count; j++)
                        {
                            string[] array = existingResults[j].Split(',');
                            if (array.Length == 4 &&
                                long.TryParse(array[1], out long sizeTiff) &&
                                long.TryParse(array[2], out long sizePng) &&
                                long.TryParse(array[3], out long sizeJpeg)
                                )
                            {
                                ImageData imageData = new ImageData()
                                {
                                    File = array[0],
                                    SizeTiff = sizeTiff,
                                    SizePng = sizePng,
                                    SizeJpeg = sizeJpeg
                                };
                                data.Add(imageData);
                                output.Add(imageData.ToString());
                                totalTiffSize += imageData.SizeTiff;
                                totalPngSize += imageData.SizePng;
                                totalJpegSize += imageData.SizeJpeg;
                            }
                        }
                    }
                }
                Console.WriteLine("\nExisting results found for " + data.Count + " files.");

                // Convert remaining files
                Console.WriteLine("\nConverting files...");
                foreach (string file in Directory.GetFiles(folder))
                {
                    try
                    {
                        string extension = Path.GetExtension(file);
                        if (extension == ".tif")
                        {
                            ImageData imageData = new ImageData()
                            {
                                File = Path.GetFileNameWithoutExtension(file),
                            };
                            if (data.Contains(imageData))
                            {
                                Console.WriteLine((i++).ToString() + ". File " + imageData.File + " skipped.");
                            }
                            else
                            {
                                imageData.SizeTiff = GetSize(file);
                                imageData.SizePng = GetPngSize(file);
                                imageData.SizeJpeg = GetJpegSize(file);
                                data.Add(imageData);
                                output.Add(imageData.ToString());
                                totalTiffSize += imageData.SizeTiff;
                                totalPngSize += imageData.SizePng;
                                totalJpegSize += imageData.SizeJpeg;

                                Console.Write((i++).ToString() + ". TIFF: " + imageData.SizeTiff);
                                Console.WriteLine(", PNG: " + imageData.SizePng + ", JPEG: " + imageData.SizeJpeg);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Error in FormatConverter.CompareFormats, file " + Path.GetFileNameWithoutExtension(file), ex);
                    }
                }

                // Calculate averages
                output.Add(",Average TIFF Size,Average PNG Size,Average JPEG Size");
                double averageTiffSize = (double)totalTiffSize / data.Count;
                double averagePngSize = (double)totalPngSize / data.Count;
                double averageJpegSize = (double)totalJpegSize / data.Count;
                output.Add("," + averageTiffSize + "," + averagePngSize + "," + averageJpegSize);
                Console.WriteLine("Average TIFF size: " + averageTiffSize);
                Console.WriteLine("Average PNG size: " + averagePngSize);
                Console.WriteLine("Average JPEG size: " + averageJpegSize);

                File.WriteAllLines(resultsFile, output);
            }
            catch (Exception ex)
            {
                Logger.Log("Error in FormatConverter.CompareFormats", ex);
            }
        }
        #endregion

        #region LEGO brick images
        private static string GetFileNameAndPartialPath(string file)
        {
            int pl = Settings.LegoDirectory.Length;
            return file.Substring(pl + 1, file.Length - pl - 5);
        }
        
        public static void CompareFormatsPngToJpegRecursively(string folder, string resultsFile)
        {
            try
            {
                HashSet<ImageData> data = new HashSet<ImageData>();
                List<string> output = new List<string>() { "File,PNG Size,JPEG Size" };
                long totalPngSize = 0;
                long totalJpegSize = 0;
                int i = 1;

                // Reload data that already exists
                if (File.Exists(resultsFile))
                {
                    List<string> existingResults = new List<string>(File.ReadAllLines(resultsFile));
                    if (existingResults != null &&
                        existingResults.Count > 0 &&
                        existingResults[0] == "File,PNG Size,JPEG Size"
                        )
                    {
                        for (int j = 1; j < existingResults.Count; j++)
                        {
                            string[] array = existingResults[j].Split(',');
                            if (array.Length == 3 &&
                                long.TryParse(array[1], out long sizePng) &&
                                long.TryParse(array[2], out long sizeJpeg)
                                )
                            {
                                ImageData imageData = new ImageData()
                                {
                                    File = array[0],
                                    SizePng = sizePng,
                                    SizeJpeg = sizeJpeg
                                };
                                data.Add(imageData);
                                output.Add(imageData.ToStringWithoutTiff());
                                totalPngSize += imageData.SizePng;
                                totalJpegSize += imageData.SizeJpeg;
                            }
                        }
                    }
                }
                Console.WriteLine("\nExisting results found for " + data.Count + " files.");

                // Convert remaining files
                Console.WriteLine("\nConverting files...");
                foreach (string file in Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories))
                {
                    try
                    {
                        string extension = Path.GetExtension(file);
                        if (extension == ".png")
                        {
                            ImageData imageData = new ImageData()
                            {
                                File = GetFileNameAndPartialPath(file),
                            };
                            if (data.Contains(imageData))
                            {
                                Console.WriteLine((i++).ToString() + ". File " + imageData.File + " skipped.");
                            }
                            else
                            {
                                imageData.SizePng = GetSize(file);
                                imageData.SizeJpeg = GetJpegSize(file);
                                data.Add(imageData);
                                output.Add(imageData.ToStringWithoutTiff());
                                totalPngSize += imageData.SizePng;
                                totalJpegSize += imageData.SizeJpeg;

                                Console.WriteLine((i++).ToString() + ". PNG: " + imageData.SizePng + ", JPEG: " + imageData.SizeJpeg);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Error in FormatConverter.CompareFormatsPngToJpegRecursively, file " + Path.GetFileNameWithoutExtension(file), ex);
                    }
                }

                // Calculate averages
                output.Add(",Average PNG Size,Average JPEG Size");
                double averagePngSize = (double)totalPngSize / data.Count;
                double averageJpegSize = (double)totalJpegSize / data.Count;
                output.Add("," + averagePngSize + "," + averageJpegSize);
                Console.WriteLine("Average PNG size: " + averagePngSize);
                Console.WriteLine("Average JPEG size: " + averageJpegSize);

                File.WriteAllLines(resultsFile, output);
            }
            catch (Exception ex)
            {
                Logger.Log("Error in FormatConverter.CompareFormatsPngToJpegRecursively", ex);
            }
        }
        #endregion
    }
}
