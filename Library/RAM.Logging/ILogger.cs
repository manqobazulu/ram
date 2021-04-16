
namespace RAM.Logging {

    /// <summary>
    /// Defines the common logging interface specification
    /// </summary>
    public interface ILogger {        

        /// <summary>
        /// Writes a message to the log
        /// </summary>
        /// <param name="category">A String of the category to write to</param>
        /// <param name="level">A LogLevel value of the level of this message</param>
        /// <param name="message">A String of the message to write to the log</param>
        /// <param name="ex">The exception</param>
        void WriteMessage(string category, RAM.Logging.LogLevel level, string message, System.Exception ex, string context = "Unknown");

        #region Original code
        /// <summary>
        /// Writes a message to the log
        /// </summary>
        /// <param name="category">A String of the category to write to</param>
        /// <param name="level">A LogLevel value of the level of this message</param>
        /// <param name="message">A String of the message to write to the log</param>
        //void WriteMessage(string category, LogLevel level, string message);
        #endregion
    }
}
