using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{

    public class ShutdownRequestHandler : RequestHandler
    {

        public ShutdownRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("shutdown")]
        public object ShutDown()
        {
            return new object();
        }

    }

}
