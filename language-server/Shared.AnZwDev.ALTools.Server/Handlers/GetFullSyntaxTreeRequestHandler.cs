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
using AnZwDev.ALTools.Workspace;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetFullSyntaxTreeRequestHandler : BaseALRequestHandler<GetFullSyntaxTreeRequest, GetFullSyntaxTreeResponse>
    {
        
        public GetFullSyntaxTreeRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getfullsyntaxtree")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetFullSyntaxTreeResponse> HandleMessage(GetFullSyntaxTreeRequest parameters, RequestContext<GetFullSyntaxTreeResponse> context)
        {
            ALProject project = Server.Workspace.FindProject(parameters.projectPath, true);
            ALFullSyntaxTree syntaxTree = new ALFullSyntaxTree();
            syntaxTree.Load(parameters.source, parameters.path, project);

            GetFullSyntaxTreeResponse response = new GetFullSyntaxTreeResponse
            {
                root = syntaxTree.Root
            };

            return response;
        }
#pragma warning restore 1998
    }
}

