using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace PngJpegComparer
{
    public static class RaisePicturesDownloader
    {
        private const string imageDownloadFormat = ".tif";
        private const string imageQuality = "Lossless Compressed RAW (14-bit)";

        public static void FilterCSV(string file, string outputFile)
        {
            try
            {
                if (File.Exists(outputFile))
                {
                    Console.WriteLine("Output file already exists.");
                    return;
                }

                string[] lines = File.ReadAllLines(file);
                if (lines.Length == 0)
                {
                    Logger.Log("Error in CSVParser: File is empty");
                    return;
                }

                List<string> firstLine = new List<string>(lines[0].Split(','));

                int fileIndex = firstLine.FindIndex(x => x == "File");
                if (fileIndex < 0)
                {
                    Logger.Log("Error in CSVParser: File name column is missing.");
                    return;
                }

                int destinationTIFFIndex = firstLine.FindIndex(x => x == "TIFF");
                if (destinationTIFFIndex < 0)
                {
                    Logger.Log("Error in CSVParser: TIFF destination column is missing.");
                    return;
                }

                int imageQualityIndex = firstLine.FindIndex(x => x == "Image Quality");
                if (fileIndex < 0)
                {
                    Logger.Log("Error in CSVParser: Image Quality column is missing.");
                    return;
                }

                // there is one column with a comma and that messes with the index
                imageQualityIndex++;

                int lastNecessaryColumn = Math.Max(Math.Max(fileIndex, destinationTIFFIndex), imageQualityIndex);
                List<string> filteredInputData = new List<string> { "File,TIFF" };

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] line = lines[i].Split(',');
                    if (lastNecessaryColumn < line.Length && line[imageQualityIndex] == imageQuality)
                    {
                        filteredInputData.Add(line[fileIndex] + ", " + line[destinationTIFFIndex]);
                    }
                }

                File.WriteAllLines(outputFile, filteredInputData.ToArray());

                Console.WriteLine("Filtered input data successfully saved: " + outputFile);
                Console.WriteLine("Lines: " + filteredInputData.Count);
            }
            catch (Exception ex)
            {
                Logger.Log("Error in CSVParser.Load", ex);
            }
        }
        
        public static void DownloadFiles(string file, string outputDirrectory)
        {
            try
            {
                string[] lines = File.ReadAllLines(file);

                List<string> firstLine = new List<string>(lines[0].Split(','));

                int fileIndex = firstLine.FindIndex(x => x == "File");
                int destinationIndex = firstLine.FindIndex(x => x == "TIFF");

                Directory.CreateDirectory(outputDirrectory);
                Console.WriteLine("Downloading files...");

                int totalFileNumber = lines.Length - 1;
                string totalFiles = totalFileNumber.ToString();
                int downloaded = 0;
                int skipped = 0;

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] line = lines[i].Split(',');
                    string name = line[fileIndex];
                    string location = line[destinationIndex];
                    string fileName = Path.Combine(outputDirrectory, name) + imageDownloadFormat;
                    bool fileAlreadyExists = File.Exists(fileName);

                    if (!fileAlreadyExists)
                    {
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(location, fileName);
                        }
                        downloaded++;
                    }
                    else
                    {
                        skipped++;
                    }

                    if ((i == 1) || (i % 10 == 0) || (i == totalFileNumber))  // first and then every 10th
                    {
                        Console.Write(i.ToString().PadLeft(totalFiles.Length, '0') + " / " + totalFiles + " ");
                    }
                    else
                    {
                        Console.Write(i.ToString().PadLeft(totalFiles.Length, '0').PadRight(2 * totalFiles.Length + 4));
                    }

                    Console.WriteLine(fileAlreadyExists ? "skipped" : "");
                }

                Console.WriteLine(downloaded + " files downloaded successfully to:");
                Console.WriteLine(outputDirrectory);
                Console.WriteLine(skipped + " files skipped.");
            }
            catch (Exception ex)
            {
                Logger.Log("Error in CSVParser.DownloadFiles", ex);
            }
        }

        public static void Download(string file)
        {
            FilterCSV(Settings.GetResourceFile(file), Settings.GetFilteredInputDataFile(file));
            DownloadFiles(Settings.GetFilteredInputDataFile(file), Settings.GetOutputDirrectory(file));
        }
    }
}
