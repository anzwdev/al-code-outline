using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Server.Contracts;
using System.Threading.Tasks;
using AnZwDev.ALTools.Workspace;
using StreamJsonRpc;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetFullSyntaxTreeRequestHandler : RequestHandler
    {
        
        public GetFullSyntaxTreeRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getfullsyntaxtree", UseSingleObjectParameterDeserialization = true)]
        public GetFullSyntaxTreeResponse GetFullSyntaxTree(GetFullSyntaxTreeRequest parameters)
        {
            ALProject project = Server.Workspace.FindProject(parameters.path, parameters.projectPath, true);
            ALFullSyntaxTree syntaxTree = new ALFullSyntaxTree();
            syntaxTree.Load(parameters.source, parameters.path, project);

            GetFullSyntaxTreeResponse response = new GetFullSyntaxTreeResponse
            {
                root = syntaxTree.Root
            };

            return response;
        }

    }
}

