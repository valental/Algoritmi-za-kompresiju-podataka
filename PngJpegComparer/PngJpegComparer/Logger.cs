using System;
using System.Diagnostics;

namespace PngJpegComparer
{
    /// <summary>
    /// Static class with methods for logging errors.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        public static void Log(string message)
        {
            Console.WriteLine(message);
            Debug.WriteLine(message);
        }

        /// <summary>
        /// Logs the specified message and exception.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        /// <param name="ex">Exception that has to be logged along with the message.</param>
        public static void Log(string message, Exception ex)
        {
            Log(message + ": " + ex.Message);
        }
    }
}
