using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.VSCodeLangServer.Protocol.MessageProtocol
{
    /// <summary>
    /// Defines all possible message types.
    /// </summary>
    public enum MessageType
    {
        Unknown,
        Request,
        Response,
        Event
    }
}
