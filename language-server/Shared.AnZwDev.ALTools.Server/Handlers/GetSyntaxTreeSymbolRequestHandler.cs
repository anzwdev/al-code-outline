using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Server.Contracts;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetSyntaxTreeSymbolRequestHandler : RequestHandler
    {

        public GetSyntaxTreeSymbolRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getsyntaxtreesymbol", UseSingleObjectParameterDeserialization = true)]
        public GetSyntaxTreeSymbolResponse GetSyntaxTreeSymbol(GetSyntaxTreeSymbolRequest parameters)
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

    }

}
