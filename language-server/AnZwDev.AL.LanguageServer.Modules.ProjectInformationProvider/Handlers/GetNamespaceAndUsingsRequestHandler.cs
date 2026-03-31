using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts;
using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Platform;
using AnZwDev.AL.Syntax;
using AnZwDev.AL.Workspaces;
using AnZwDev.AL.Workspaces.InformationProviders.Namespaces;
using AnZwDev.LanguageServer;
using AnZwDev.System.ServiceModel;
using MessagePack.Resolvers;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Handlers
{
    internal class GetNamespaceAndUsingsRequestHandler : RequestHandler
    {

        public GetNamespaceAndUsingsRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/projectinformation/getnamespaceandusings", UseSingleObjectParameterDeserialization = true)]
        public GetNamespaceAndUsingsResponse SuggestNamespaceAndUsings(GetNamespaceAndUsingsRequest parameters)
        {
            //find project
            var project = this.Services
                .GetService<Workspace>()?
                .Projects.FindByPath(parameters.Path ?? String.Empty);

            if (project != null)
            {
                var namespaceInformation = NamespaceSuggestionProvider.SuggestNamespaceAndUsings(
                    project,
                    parameters.ObjectIdentifier?.ToObjectIdentifier(),
                    parameters.ReferencedObjectsIdentifiers.ToObjectIdentifierList(),
                    parameters.Path);

                return new GetNamespaceAndUsingsResponse()
                {
                    Namespace = namespaceInformation.Namespace,
                    Usings = namespaceInformation.Usings?.ToList()
                };
            }

            return new GetNamespaceAndUsingsResponse()
            {
                Namespace = null,
                Usings = null
            };
        }

    }
}
