using AnZwDev.LanguageServer;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.LanguageServer.MessageHandlers
{
    public class ExitNotificationHandler : RequestHandler
    {

        private readonly LanguageServerHost _languageServerHost;

        public ExitNotificationHandler(IServiceProvider service, LanguageServerHost languageServerHost) : base(service)
        {
            _languageServerHost = languageServerHost;
        }

        [JsonRpcMethod("exit", UseSingleObjectParameterDeserialization = true)]
        public void Exit(object parameters)
        {
            _languageServerHost.Stop();
        }

    }
}
