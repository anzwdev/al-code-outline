using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.ALTools.Workspace;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    internal class GetRawSyntaxTreeRequestHandler : RequestHandler
    {

        public GetRawSyntaxTreeRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getrawsyntaxtree", UseSingleObjectParameterDeserialization = true)]
        public GetSyntaxTreeResponse GetSyntaxTree(GetSyntaxTreeRequest parameters)
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

    }
}
