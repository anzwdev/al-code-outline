using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetWarningDirectivesRequestHandler : RequestHandler
    {

        public GetWarningDirectivesRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getwarningdirectives", UseSingleObjectParameterDeserialization = true)]
        public GetWarningDirectivesResponse GetWarningDirectives(GetWarningDirectivesRequest parameters)
        {
            RebuildModifiedSymbols();

            GetWarningDirectivesResponse response = new GetWarningDirectivesResponse();

            WarningDirectivesInformationProvider provider = new WarningDirectivesInformationProvider(this.Server);
            response.directives = provider.GetWarningDirectives(this.Server.Workspace);

            return response;
        }

    }
}
