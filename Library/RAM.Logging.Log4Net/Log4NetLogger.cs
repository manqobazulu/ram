using log4net;

namespace RAM.Logging.Log4Net {
    /// <summary>
    /// Driver class to adapts calls from ILogger to work with a log4net backend
    /// </summary>
    internal class Log4NetLogger : ILogger {

        public Log4NetLogger() {
            // Configures log4net by using log4net's XMLConfigurator class
            log4net.Config.XmlConfigurator.Configure();
        }        

        /// <summary>
        /// Writes messages to the log4net backend.
        /// </summary>
        /// /// <remarks>
        /// This method is responsible for converting the WriteMessage call of the interface into something log4net can understand.
        /// It does this by doing a switch/case on the log level and then calling the appropriate log method
        /// </remarks>
        /// <param name="category">A string of the category to log to</param>
        /// <param name="level">A LogLevel value of the level of the log</param>
        /// <param name="message">A String of the message to write to the log</param>
        /// <param name="ex">The exception</param>        
        public void WriteMessage(string category, RAM.Logging.LogLevel level, string message, System.Exception ex, string context = "") {
            // Get the Log we are going to write this message to            
            ILog log = LogManager.GetLogger(category);

            log4net.ThreadContext.Properties["ApplicationContext"] = context;

            switch (level) {
                case LogLevel.DEBUG:
                    if (log.IsDebugEnabled) log.Debug(message);
                    break;
                case LogLevel.INFO:
                    if (log.IsInfoEnabled) log.Info(message);
                    break;
                case LogLevel.WARN:
                    if (log.IsWarnEnabled) log.Warn(message);
                    break;
                case LogLevel.ERROR:
                    if (log.IsErrorEnabled && ex != null) 
                        log.Error(message, ex);
                    else
                        log.Error(message);
                    break;
                case LogLevel.FATAL:
                    if (log.IsFatalEnabled && ex != null) 
                        log.Fatal(message, ex);
                    else
                        log.Fatal(message);
                    break;
            }
        }

        #region Original code

        /// <summary>
        /// Writes messages to the log4net backend.
        /// </summary>
        /// <remarks>
        /// This method is responsible for converting the WriteMessage call of the interface into something log4net can understand.  
        /// It does this by doing a switch/case on the log level and then calling the appropriate log method
        /// </remarks>
        /// <param name="category">A string of the category to log to</param>
        /// <param name="level">A LogLevel value of the level of the log</param>
        /// <param name="message">A String of the message to write to the log</param>
        //public void WriteMessage(string category, LogLevel level, string message) {
        //    // Get the Log we are going to write this message to            
        //    ILog log = LogManager.GetLogger(category);

        //    switch (level) {
        //        case LogLevel.DEBUG:
        //            if (log.IsDebugEnabled) log.Debug(message);
        //            break;
        //        case LogLevel.INFO:
        //            if (log.IsInfoEnabled) log.Info(message);
        //            break;
        //        case LogLevel.WARN:
        //            if (log.IsWarnEnabled) log.Warn(message);
        //            break;
        //        case LogLevel.ERROR:
        //            if (log.IsErrorEnabled) log.Error(message);
        //            break;
        //        case LogLevel.FATAL:
        //            if (log.IsFatalEnabled) log.Fatal(message);
        //            break;
        //    }
        //}

        #endregion
    }
}
