using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.SyntaxHelpers;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolReferences;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetNewFileRequiredInterfacesHandler : RequestHandler
    {

        public GetNewFileRequiredInterfacesHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getnewfilerequiredinterfaces", UseSingleObjectParameterDeserialization = true)]
        public GetNewFileRequiredInterfacesResponse GetNewFileRequiredInterfaces(GetNewFileRequiredInterfacesRequest parameters)
        {
            RebuildModifiedSymbols();

            GetNewFileRequiredInterfacesResponse response = new GetNewFileRequiredInterfacesResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if ((project != null) && (project.UsesNamespaces()))
            {
                response.usesNamespaces = true;
                response.namespaceName = NamespacesHelper.GetNamespaceName(
                    project.RootPath,
                    parameters.path,
                    parameters.rootNamespace,
                    parameters.useFolderStructure);

                if ((parameters.referencedObjects != null) && (parameters.referencedObjects.Count > 0))
                {
                    HashSet<string> usedNamespaces = new HashSet<string>();
                    if (!String.IsNullOrWhiteSpace(response.namespaceName))
                        usedNamespaces.Add(response.namespaceName);

                    for (int i = 0; i < parameters.referencedObjects.Count; i++)
                    {
                        var symbolReference = parameters.referencedObjects[i];
                        var objectType = ALObjectTypesInformationCollection.Get(symbolReference.typeName);
                        if (objectType != null)
                        {
                            var objectReference = symbolReference.ToALObjectReference();
                            var referencedObject = project
                                .SymbolsWithDependencies
                                .GetObjectsCollection(objectType.ALObjectType)
                                .FindFirst(objectReference);
                            if ((!String.IsNullOrWhiteSpace(referencedObject?.NamespaceName)) && (!usedNamespaces.Contains(referencedObject.NamespaceName)))
                            {
                                response.usings.Add(referencedObject.NamespaceName);
                                usedNamespaces.Add(referencedObject.NamespaceName);
                            }
                        }    
                    }
                }
            }

            return response;
        }

    }
}
