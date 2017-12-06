using System;

namespace Fasetto.Word.Core
{
    /// <summary>
    /// Logs to a specific file
    /// </summary>
    public class FileLogger : ILogger
    {
        #region Public Properties

        /// <summary>
        /// The path to write the log file to
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// If true, logs the current time with each message
        /// </summary>
        public bool LogTime { get; set; } = true;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="filePath">The path to log to</param>
        public FileLogger(string filePath)
        {
            // Set the file path property
            FilePath = filePath;
        }

        #endregion

        #region Logger Methods

        public void Log(string message, LogLevel level)
        {
            // Get current time
            var currentTime = DateTimeOffset.Now.ToString("yyyy-MM-dd hh:mm:ss");

            // Prepend the time to the log if desired
            var timeLogString = LogTime ? $"[{ currentTime}] " : "";

            // Write the message
            IoC.File.WriteTextToFileAsync($"{timeLogString}{message}{Environment.NewLine}", FilePath, append: true);
        }

        #endregion
    }
}
