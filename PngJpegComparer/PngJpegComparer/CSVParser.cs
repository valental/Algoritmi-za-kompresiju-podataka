using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace PngJpegComparer
{
    public class CSVParser
    {
        public static void Load() => Load(Settings.ResourceFile, Settings.FilteredInputDataFile);

        public static void Load(string file, string outputFile)
        {
            try
            {
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

                int destinationNEFIndex = firstLine.FindIndex(x => x == "NEF");
                if (destinationNEFIndex < 0)
                {
                    Logger.Log("Error in CSVParser: NEF destination column is missing.");
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

                int lastNecessaryColumn = Math.Max(Math.Max(fileIndex, destinationNEFIndex), Math.Max(destinationTIFFIndex, imageQualityIndex));
                List<string> filteredInputData = new List<string>();
                filteredInputData.Add("File,NEF,TIFF");

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] line = lines[i].Split(',');
                    if (lastNecessaryColumn < line.Length && line[imageQualityIndex] == Settings.ImageQuality)
                    {
                        filteredInputData.Add(line[fileIndex] + "," + line[destinationNEFIndex] +  ", " + line[destinationTIFFIndex]);
                    }
                }

                File.WriteAllLines(Settings.FilteredInputDataFile, filteredInputData.ToArray());

                Console.WriteLine("Filtered input data successfully saved:");
                Console.WriteLine(outputFile);
                Console.WriteLine("Lines: " + filteredInputData.Count);
            }
            catch (Exception ex)
            {
                Logger.Log("Error in CSVParser.Load", ex);
            }
        }

        public static void DownloadFiles() => DownloadFiles(Settings.FilteredInputDataFile, Settings.OutputDirrectory);

        async public static void DownloadFiles(string file, string outputDirrectory)
        {
            try
            {
                string[] lines = File.ReadAllLines(file);
                
                List<string> firstLine = new List<string>(lines[0].Split(','));

                int fileIndex = firstLine.FindIndex(x => x == "File");
                int destinationNEFIndex = firstLine.FindIndex(x => x == "NEF");
                //int destinationTIFFIndex = firstLine.FindIndex(x => x == "TIFF");

                Directory.CreateDirectory(outputDirrectory);
                Console.WriteLine("Downloaded:");

                string totalFiles = (lines.Length - 1).ToString();

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] line = lines[i].Split(',');
                    string name = line[fileIndex];
                    string location = line[destinationNEFIndex];
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(location, Path.Combine(outputDirrectory, name) + Settings.ImageDownloadFormat);
                    }
                    Console.WriteLine(i.ToString().PadLeft(totalFiles.Length, '0') + "/" + totalFiles);
                }
                
                Console.WriteLine((lines.Length - 1).ToString() + " files downloaded successfully to:");
                Console.WriteLine(outputDirrectory);
            }
            catch (Exception ex)
            {
                Logger.Log("Error in CSVParser.DownloadFiles", ex);
            }
        }
    }
}
