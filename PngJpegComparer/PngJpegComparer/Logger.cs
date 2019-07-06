using System;
using System.Diagnostics;

namespace PngJpegComparer
{
    public static class Logger
    {
        public static void Log(string message)
        {
            Console.WriteLine(message);
            Debug.WriteLine(message);
        }

        public static void Log(string message, Exception ex)
        {
            Log(message + ": " + ex.Message);
        }
    }
}
