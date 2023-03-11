using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Logging
{
    public interface IMessageLogWriter
    {

        void WriteError(Exception e);
        void WriteError(Exception e, string messageStartPart);

    }
}
