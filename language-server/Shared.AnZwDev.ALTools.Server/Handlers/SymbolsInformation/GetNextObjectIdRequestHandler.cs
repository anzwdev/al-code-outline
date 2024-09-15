using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetNextObjectIdRequestHandler : RequestHandler
    {

        public GetNextObjectIdRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getnextobjectid", UseSingleObjectParameterDeserialization = true)]
        public GetNextObjectIdResponse GetNextObjectId(GetNextObjectIdRequest parameters)
        {
            RebuildModifiedSymbols();

            GetNextObjectIdResponse response = new GetNextObjectIdResponse
            {
                id = 0
            };

            try
            {

                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    ObjectIdInformationProvider objectIdInformationProvider = new ObjectIdInformationProvider();
                    response.id = objectIdInformationProvider.GetNextObjectId(project, parameters.objectType);
                }
            }
            catch (Exception e)
            {
                response.id = 0;
                this.LogError(e);
            }

            return response;

        }

    }
}
