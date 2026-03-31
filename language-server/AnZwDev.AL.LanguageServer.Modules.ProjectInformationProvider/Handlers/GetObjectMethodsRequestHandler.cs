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
using AnZwDev.AL.Symbols.Formatters;
using AnZwDev.AL.Workspaces.InformationProviders.Objects;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Handlers
{
    internal class GetObjectMethodsRequestHandler : RequestHandler
    {

        public GetObjectMethodsRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/projectinformation/getobjectmethods", UseSingleObjectParameterDeserialization = true)]
        public GetObjectMethodsResponse GetObjectMethods(GetObjectMethodsRequest parameters)
        {
            if ((parameters.Path != null) && (parameters.Identifier != null))
            {
                try
                {

                    var project = this.Services
                        .GetService<Workspace>()?
                        .Projects.FindByPath(parameters.Path);

                    if (project != null)
                    {
                        var objectIdentifier = parameters.Identifier.ToObjectIdentifier();
                        var methodsEnumerable = ObjectMethodsInformationProvider.GetObjectMethods(project, objectIdentifier, parameters.IncludePrivate);
                        if (methodsEnumerable != null)
                        {
                            var methodList = new List<PIMethodListItem>();
                            foreach (var method in methodsEnumerable)
                                methodList.Add(new PIMethodListItem()
                                {
                                    Name = method.Name,
                                    Header = DisplayStringFormatter.FormatMethodSymbol(method)
                                });

                            return new GetObjectMethodsResponse()
                            {
                                Methods = methodList
                            };
                        }
                    }

                }
                catch (Exception e)
                {
                    Services.GetService<ILogger>()?.Log(e);
                }
            }

            return new GetObjectMethodsResponse()
            {
                Methods = null
            };
        }

    }
}
