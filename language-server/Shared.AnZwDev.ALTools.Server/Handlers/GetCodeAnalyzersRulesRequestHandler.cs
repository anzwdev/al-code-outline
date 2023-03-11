using AnZwDev.ALTools;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetCodeAnalyzersRulesRequestHandler : BaseALRequestHandler<GetCodeAnalyzersRulesRequest, GetCodeAnalyzersRulesResponse>
    {

        public GetCodeAnalyzersRulesRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getcodeanalyzersrules")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetCodeAnalyzersRulesResponse> HandleMessage(GetCodeAnalyzersRulesRequest parameters, RequestContext<GetCodeAnalyzersRulesResponse> context)
        {
            CodeAnalyzersLibrary library = this.Server.CodeAnalyzersLibraries.GetCodeAnalyzersLibrary(parameters.name);

            GetCodeAnalyzersRulesResponse response = new GetCodeAnalyzersRulesResponse
            {
                name = parameters.name
            };
            if (library != null)
                response.rules = library.Rules;

            return response;
        }
#pragma warning restore 1998


    }
}
