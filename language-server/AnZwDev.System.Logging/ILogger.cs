using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.Logging
{
    public interface ILogger
    {

        LogLevel LogLevel { get; set; }
        void Log(string message, LogLevel level);
        void Log(Exception e, string? messagePrefix = null, LogLevel level = LogLevel.Error);

    }
}
