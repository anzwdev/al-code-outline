using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
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
    public class GetSyntaxTreeRequestHandler : BaseALRequestHandler<GetSyntaxTreeRequest, GetSyntaxTreeResponse>
    {

        public GetSyntaxTreeRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getsyntaxtree")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetSyntaxTreeResponse> HandleMessage(GetSyntaxTreeRequest parameters, RequestContext<GetSyntaxTreeResponse> context)
        {
            ALSyntaxTree syntaxTree = this.Server.SyntaxTrees.FindOrCreate(parameters.path, parameters.open);
            syntaxTree.Load(parameters.source);

            GetSyntaxTreeResponse response = new GetSyntaxTreeResponse
            {
                root = syntaxTree.RootSymbol
            };

            return response;
        }
#pragma warning restore 1998

    }
}
