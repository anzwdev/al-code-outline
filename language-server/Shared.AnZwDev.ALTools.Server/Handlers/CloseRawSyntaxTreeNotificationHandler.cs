using AnZwDev.ALTools.Server.Contracts;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class CloseRawSyntaxTreeNotificationHandler : RequestHandler
    {

        public CloseRawSyntaxTreeNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/closerawsyntaxtree", UseSingleObjectParameterDeserialization = true)]
        public void CloseRawSyntaxTree(CloseSyntaxTreeRequest parameters)
        {
            this.Server.RawSyntaxTrees.Close(parameters.path);
        }

    }
}
