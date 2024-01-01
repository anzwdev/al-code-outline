using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Server.Contracts;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetCodeAnalyzersRulesRequestHandler : RequestHandler
    {

        public GetCodeAnalyzersRulesRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getcodeanalyzersrules", UseSingleObjectParameterDeserialization = true)]
        public GetCodeAnalyzersRulesResponse GetCodeAnalyzersRules(GetCodeAnalyzersRulesRequest parameters)
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

    }
}
