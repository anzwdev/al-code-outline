using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetObjectsListRequestHandler : RequestHandler
    {

        public GetObjectsListRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getobjectslist", UseSingleObjectParameterDeserialization = true)]
        public GetObjectsListResponse GetObjectsList(GetObjectsListRequest parameters)
        {
            GetObjectsListResponse response = new GetObjectsListResponse();

            try
            {

                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    HashSet<ALSymbolKind> includeObjects = new HashSet<ALSymbolKind>();
                    includeObjects.AddObjectTypes(parameters.includeTables, parameters.includePages, parameters.includeReports, parameters.includeXmlPorts,
                        parameters.includeQueries, parameters.includeCodeunits, parameters.includeControlAddIns, parameters.includePageExtensions, parameters.includeTableExtensions, parameters.includePofiles,
                        parameters.includePageCustomizations, parameters.includeDotNetPackages, parameters.includeEnumTypes, parameters.includeEnumExtensionTypes, parameters.includeInterfaces,
                        parameters.includeReportExtensions, parameters.includePermissionSets, parameters.includePermissionSetExtensions);

                    ObjectInformationProvider objectIdInformationProvider = new ObjectInformationProvider();
                    response.symbols = objectIdInformationProvider.GetProjectObjects(project, includeObjects, parameters.includeDependencies, parameters.includeObsolete);
                }
            }
            catch (Exception e)
            {
                this.LogError(e);
            }

            return response;

        }

    }
}
