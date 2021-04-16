using System.ComponentModel;

namespace RAM.Logging {
    /// <summary>
    /// Enum defining log levels to use in the common logging interface
    /// </summary>
    public enum LogLevel {

        [Description("FATAL")]
        FATAL = 0,

        [Description("ERROR")]
        ERROR = 1,

        [Description("WARN")]
        WARN = 2,

        [Description("INFO")]
        INFO = 3,

        [Description("DEBUG")]
        DEBUG = 4

        //[Description("ALL")]
        //ALL = 5,

        //[Description("OFF")]
        //OFF = -1
    }
}
