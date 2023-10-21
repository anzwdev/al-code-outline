using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.ALTools.Workspace;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    internal class GetRawSyntaxTreeRequestHandler : BaseALRequestHandler<GetSyntaxTreeRequest, GetSyntaxTreeResponse>
    {

        public GetRawSyntaxTreeRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getrawsyntaxtree")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetSyntaxTreeResponse> HandleMessage(GetSyntaxTreeRequest parameters, RequestContext<GetSyntaxTreeResponse> context)
        {
            ALProject project = Server.Workspace.FindProject(parameters.path, parameters.projectPath, true);
            ALSyntaxTree syntaxTree = this.Server.RawSyntaxTrees.FindOrCreate(parameters.path, parameters.open);
            syntaxTree.Load(parameters.source, project);
            if (syntaxTree.RootSymbol != null)
                syntaxTree.RootSymbol.Sort(new ALSymbolPositionComparer());

            GetSyntaxTreeResponse response = new GetSyntaxTreeResponse
            {
                root = syntaxTree.RootSymbol
            };

            return response;
        }
#pragma warning restore 1998

    }
}
