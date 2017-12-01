using System;
using System.Diagnostics;

namespace Fasetto.Word.Core
{
    /// <summary>
    /// Logs the messages to the Debug log
    /// </summary>
    public class DebugLogger : ILogger
    {
        /// <summary>
        /// Logs the given message to the system Console
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="level">The level of the message</param>
        public void Log(string message, LogLevel level)
        {
            // The default category
            var category = default(string);

            // NOTE: The native Debug output has no color
            //       However if you install the VS extension VSColorOutput
            //       then this style will color the outputs nicely
            //
            //       https://github.com/mike-ward/VSColorOutput
            //

            // Color console based on level
            switch (level)
            {
                // Debug
                case LogLevel.Debug:
                    category = "information";
                    break;

                // Verbose
                case LogLevel.Verbose:
                    category = "verbose";
                    break;

                // Warning
                case LogLevel.Warning:
                    category = "warning";
                    break;

                // Error
                case LogLevel.Error:
                    category = "error";
                    break;

                // Success
                case LogLevel.Success:
                    category = "-----";
                    break;
            }

            // Write message to console
            Debug.WriteLine(message, category);
        }
    }
}
