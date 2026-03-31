using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts;
using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Workspaces;
using AnZwDev.LanguageServer;
using AnZwDev.System.Logging;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Handlers
{
    internal class GetObjectsListRequestHandler : RequestHandler
    {

        public GetObjectsListRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/projectinformation/getobjectslist", UseSingleObjectParameterDeserialization = true)]
        public GetObjectsListResponse GetObjectsList(GetObjectsListRequest parameters)
        {
            var response = new GetObjectsListResponse();

            if (parameters.Path != null)
            {
                try
                {

                    var project = this.Services
                        .GetService<Workspace>()?
                        .Projects.FindByPath(parameters.Path);

                    if (project != null)
                    {
                        HashSet<string>? appIdFilter = null;
                        if ((parameters.Filter?.AppIdFilter != null) && (parameters.Filter.AppIdFilter.Length > 0))
                            appIdFilter = new HashSet<string>(parameters.Filter.AppIdFilter);
                        
                        HashSet<ObjectKind>? kindFilter = null;
                        if ((parameters.Filter != null) && (parameters.Filter.Kind != ObjectKind.Unknown))
                            kindFilter = new HashSet<ObjectKind>() { parameters.Filter.Kind };

                        var skipDependencies = (parameters.Filter != null) && (parameters.Filter.SkipDependencies);
                        var excludeFullInherentPermissions = (parameters.Filter != null) && (parameters.Filter.ExcludeFullInherentPermissions);

                        var objectUid = 0;

                        IEnumerable<ObjectSymbol>? objectsEnumerable;                       
                        if (skipDependencies)
                            objectsEnumerable = project.SymbolsProvider.ProjectCodeSymbolsProvider.GetSymbols()?.AllObjects.Filter(kindFilter);
                        else
                            objectsEnumerable = project.Symbols.AllObjects.Filter(kindFilter, appIdFilter);

                        if (objectsEnumerable != null)
                            foreach (var objectHeader in objectsEnumerable)
                            {
                                if ((!excludeFullInherentPermissions) || (!objectHeader.HasFullInherentPermissions()))
                                {
                                    objectUid++;

                                    response.Objects.Add(new PIObjectListItem()
                                    {
                                        Uid = objectUid,
                                        Id = objectHeader.Identifier.Id,
                                        Kind = objectHeader.Identifier.ObjectKind,
                                        Name = objectHeader.Identifier.FullyQualifiedName.Name,
                                        Namespace = objectHeader.Identifier.FullyQualifiedName.Namespace,
                                        InherentPermissions = objectHeader.Properties.InherentPermissions,
                                        FullInherentPermissions = objectHeader.HasFullInherentPermissions()
                                    });
                                }
                            }
                    }
                }
                catch (Exception e)
                {
                    Services.GetService<ILogger>()?.Log(e);
                }
            }

            return response;
        }


    }
}
