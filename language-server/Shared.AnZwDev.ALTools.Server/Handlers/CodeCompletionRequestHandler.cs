using AnZwDev.ALTools.CodeCompletion;
using AnZwDev.ALTools.Server.Contracts;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class CodeCompletionRequestHandler : RequestHandler
    {

        private CodeCompletionProvidersCollection _codeCompletionProviders;

        public CodeCompletionRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
            _codeCompletionProviders = new CodeCompletionProvidersCollection(this.Server);
        }

        [JsonRpcMethod("al/codecompletion", UseSingleObjectParameterDeserialization = true)]
        public CodeCompletionResponse GetCodeCompletion(CodeCompletionRequest parameters)
        {
            RebuildModifiedSymbols();

            CodeCompletionResponse response = new CodeCompletionResponse();
            try
            {
                response.completionItems = new List<CodeCompletionItem>();
                _codeCompletionProviders.CollectCompletionItems(
                    Server.Workspace,
                    parameters.path,
                    parameters.position,
                    parameters.providers,
                    parameters.parameters,
                    response.completionItems);
            }
            catch (Exception e)
            {
                this.LogError(e);
            }
            return response;
        }

    }

}
