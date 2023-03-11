using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbols;
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
    public class GetSyntaxTreeSymbolRequestHandler : BaseALRequestHandler<GetSyntaxTreeSymbolRequest, GetSyntaxTreeSymbolResponse>
    {

        public GetSyntaxTreeSymbolRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getsyntaxtreesymbol")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetSyntaxTreeSymbolResponse> HandleMessage(GetSyntaxTreeSymbolRequest parameters, RequestContext<GetSyntaxTreeSymbolResponse> context)
        {
            ALSyntaxTree syntaxTree = this.Server.SyntaxTrees.FindOrCreate(parameters.path, false);
            ALSyntaxTreeSymbol symbol = syntaxTree.GetSyntaxTreeSymbolByPath(parameters.symbolPath);
            if (symbol != null)
                symbol = symbol.CreateSerializableCopy();

            GetSyntaxTreeSymbolResponse response = new GetSyntaxTreeSymbolResponse
            {
                symbol = symbol
            };

            return response;
        }
#pragma warning restore 1998
    }

}
