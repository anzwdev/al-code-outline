using AnZwDev.ALTools.Server.Contracts;
using StreamJsonRpc;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class CloseSyntaxTreeNotificationHandler : RequestHandler
    {

        public CloseSyntaxTreeNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/closesyntaxtree", UseSingleObjectParameterDeserialization = true)]
        public void CloseSyntaxTree(CloseSyntaxTreeRequest parameters)
        {
            this.Server.SyntaxTrees.Close(parameters.path);
        }

    }
}
