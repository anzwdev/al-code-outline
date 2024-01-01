using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Server.Contracts;
using System.Threading.Tasks;
using AnZwDev.ALTools.Workspace;
using StreamJsonRpc;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetSyntaxTreeRequestHandler : RequestHandler
    {

        public GetSyntaxTreeRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getsyntaxtree", UseSingleObjectParameterDeserialization = true)]
        public GetSyntaxTreeResponse GetSyntaxTree(GetSyntaxTreeRequest parameters)
        {
            ALProject project = Server.Workspace.FindProject(parameters.path, parameters.projectPath, true);
            ALSyntaxTree syntaxTree = this.Server.SyntaxTrees.FindOrCreate(parameters.path, parameters.open);
            syntaxTree.Load(parameters.source, project);

            GetSyntaxTreeResponse response = new GetSyntaxTreeResponse
            {
                root = syntaxTree.RootSymbol
            };

            return response;
        }

    }
}
