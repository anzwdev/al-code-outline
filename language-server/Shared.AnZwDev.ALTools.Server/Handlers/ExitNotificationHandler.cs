using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class ExitNotificationHandler : RequestHandler
    {

        public ExitNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("exit", UseSingleObjectParameterDeserialization = true)]
        public void Exit(object parameters)
        {
            this.LanguageServerHost.Stop();
        }

    }
}
