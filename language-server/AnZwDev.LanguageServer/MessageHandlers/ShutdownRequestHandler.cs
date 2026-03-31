using AnZwDev.LanguageServer;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.LanguageServer.MessageHandlers
{

    public class ShutdownRequestHandler : RequestHandler
    {

        public ShutdownRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("shutdown")]
        public object ShutDown()
        {
            return new object();
        }

    }

}
