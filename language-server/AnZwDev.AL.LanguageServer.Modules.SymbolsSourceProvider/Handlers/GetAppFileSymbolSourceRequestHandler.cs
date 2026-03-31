using AnZwDev.AL.LanguageServer.Modules.SymbolsSourceProvider.Contracts;
using AnZwDev.AL.Symbols.Providers.AppPackages;
using AnZwDev.AL.Workspaces;
using AnZwDev.LanguageServer;
using AnZwDev.System.Logging;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsSourceProvider.Handlers
{
    internal class GetAppFileSymbolSourceRequestHandler : RequestHandler
    {

        public GetAppFileSymbolSourceRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/symbolssourceprovider/getappfilesymbolsource", UseSingleObjectParameterDeserialization = true)]
        public GetAppFileSymbolSourceResponse GetAppFileSymbolSource(GetAppFileSymbolSourceRequest parameters)
        {
            try
            {
                if ((!String.IsNullOrWhiteSpace(parameters.AppFilePath)) && (!String.IsNullOrWhiteSpace(parameters.SourceFilePath)))
                {
                    var appPackageContentProvider = new AppPackageContentProvider();
                    return new GetAppFileSymbolSourceResponse()
                    {
                        Source = appPackageContentProvider.GetAppPackageContent(parameters.AppFilePath, parameters.SourceFilePath)
                    };
                }
            }
            catch (Exception e)
            {
                Services.GetService<ILogger>()?.Log(e);
            }

            return new GetAppFileSymbolSourceResponse()
            {
                Source = null
            };
        }

    }
}
