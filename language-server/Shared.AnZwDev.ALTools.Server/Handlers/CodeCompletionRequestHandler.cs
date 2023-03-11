using AnZwDev.ALTools.CodeCompletion;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class CodeCompletionRequestHandler : BaseALRequestHandler<CodeCompletionRequest, CodeCompletionResponse>
    {

        private CodeCompletionProvidersCollection _codeCompletionProviders;

        public CodeCompletionRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/codecompletion")
        {
            _codeCompletionProviders = new CodeCompletionProvidersCollection(this.Server);
        }

#pragma warning disable 1998
        protected override async Task<CodeCompletionResponse> HandleMessage(CodeCompletionRequest parameters, RequestContext<CodeCompletionResponse> context)
        {
            CodeCompletionResponse response = new CodeCompletionResponse();
            try
            {
                response.completionItems = new List<CodeCompletionItem>();
                _codeCompletionProviders.CollectCompletionItems(
                    Server.Workspace,
                    parameters.path,
                    parameters.position,
                    parameters.providers,
                    response.completionItems);
            }
            catch (Exception e)
            {
                this.LogError(e);
            }
            return response;
        }
#pragma warning restore 1998

    }

}
