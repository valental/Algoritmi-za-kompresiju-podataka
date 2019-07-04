using System;

namespace PngJpegComparer
{
    public static class Logger
    {
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }

        public static void Log(string message, Exception ex)
        {
            Log(message + ": " + ex.Message);
        }
    }
}
