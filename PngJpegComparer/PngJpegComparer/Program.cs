using System;
using System.Collections.Generic;
using System.IO;

namespace PngJpegComparer
{
    public class Program
    {
        protected Program()
        {

        }

        /// <summary>
        /// Main method of the console application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            string boolQuestionStart = "Would you like to compare the sizes of images in the ";
            string boolQuestionEnd = " dataset?";

            if (AskUserBool(boolQuestionStart + Settings.RaiseFile + boolQuestionEnd))
            {
                // RAISE
                RaisePicturesDownloader.Download(Settings.RaiseFile);
                FormatConverter.CompareFormats(
                    Settings.GetOutputDirrectory(Settings.RaiseFile), Settings.GetResultsFile(Settings.RaiseFile),
                    ImageType.TIFF, new List<ImageType> { ImageType.PNG, ImageType.JPEG },
                    SearchOption.TopDirectoryOnly
                    );
            }

            if (AskUserBool("\n" + boolQuestionStart + Settings.LegoFile + boolQuestionEnd))
            {
                // LEGO brick images
                FormatConverter.CompareFormats(
                    Settings.LegoDirectory, Settings.GetResultsFile(Settings.LegoFile),
                    ImageType.PNG, new List<ImageType> { ImageType.JPEG },
                    SearchOption.AllDirectories
                    );
            }

            boolQuestionStart = "\nWould you like to save some number of examples of images from ";
            boolQuestionEnd = " dataset in the original format, PNG format and JPEG format?";
            string intQuestion = "How many examples would you like?";
            if (AskUserBool(boolQuestionStart + Settings.RaiseFile + boolQuestionEnd))
            {
                // Save some number of examples of RAISE images in png and in jpeg
                FormatConverter.SaveExamples(
                    Settings.GetOutputDirrectory(Settings.RaiseFile),
                    Settings.GetOutputDirrectory(Settings.RaiseFile) + "_examples",
                    AskUserPositiveInt(intQuestion),
                    ImageType.TIFF, new List<ImageType> { ImageType.PNG, ImageType.JPEG },
                    SearchOption.TopDirectoryOnly
                    );
            }

            if (AskUserBool(boolQuestionStart + Settings.LegoFile + boolQuestionEnd))
            {
                // Save some number of examples of Lego images in jpeg
                FormatConverter.SaveExamples(
                    Settings.GetOutputDirrectory(Settings.LegoDirectory),
                    Settings.GetOutputDirrectory(Settings.LegoDirectory) + "_examples",
                    AskUserPositiveInt(intQuestion),
                    ImageType.PNG, new List<ImageType> { ImageType.JPEG },
                    SearchOption.AllDirectories
                    );
            }
        }

        /// <summary>
        /// Returns true if the user responds yes to the question and ne if the user responds no;
        /// </summary>
        /// <param name="message">Question to be posed to the user.</param>
        /// <returns>The user's response.</returns>
        private static bool AskUserBool(string message)
        {
            try
            {
                Console.WriteLine(message);
                Console.WriteLine("Y/N");
                while (true)
                {
                    string userInput = Console.ReadLine().ToLower();
                    if (userInput == "y" || userInput == "yes")
                    {
                        return true;
                    }
                    else if (userInput == "n" || userInput == "no")
                    {
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("Invalid response, please respond Y/N.");
                        Console.WriteLine(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error in Program.AskUserBool", ex);
                return false;
            }
        }

        /// <summary>
        /// Returns the positive integer that the user writes as a response to the question.
        /// </summary>
        /// <param name="message">Question to be posed to the user.</param>
        /// <returns>The user's response.</returns>
        private static int AskUserPositiveInt(string message)
        {
            try
            {
                Console.WriteLine(message);
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out int number) && number >= 0)
                    {
                        return number;
                    }
                    else
                    {
                        Console.WriteLine("Invalid response, please write a positive integer.");
                        Console.WriteLine(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error in Program.AskUserInt", ex);
                return 0;
            }
        }
    }
}
