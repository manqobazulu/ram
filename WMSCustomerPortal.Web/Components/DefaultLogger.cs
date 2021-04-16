using System;

using RAM.Logging;

namespace WMSCustomerPortal.Web.Components {
    public class DefaultLogger {

        private static ILogger logger = LoggerFactory.GetLogger();

        /// <summary>
        /// Logs the event.
        /// </summary>
        /// <param name="eventSource">The event source</param>
        /// <param name="message">The message</param>
        /// <param name="level">The log level</param>
        /// <param name="ex">The exception that has occured, if any</param>
        public static void LogEvent(string eventSource, string message, RAM.Logging.LogLevel level = LogLevel.INFO, Exception ex = null, string context = "") {

            logger.WriteMessage(eventSource, level, message, ex, context);
        }
    }
}