using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.VSCodeLangServer.Utility;
using AnZwDev.VSCodeLangServer.Protocol.Server;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class AppPackageSymbolsRequestHandler : BaseALRequestHandler<AppPackageSymbolsRequest, AppPackageSymbolsResponse>
    {

        public AppPackageSymbolsRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/packagesymbols")
        {
        }

#pragma warning disable 1998
        protected override async Task<AppPackageSymbolsResponse> HandleMessage(AppPackageSymbolsRequest parameters, RequestContext<AppPackageSymbolsResponse> context)
        {
            AppPackageSymbolsResponse response = new AppPackageSymbolsResponse();
            try
            {
                ALAppSymbolReference symbolReference = this.Server.Workspace.SymbolReferencesCache.GetSymbolReference(parameters.path);
                ALSymbolsLibrary library;
                if (symbolReference != null)
                    library = symbolReference.ToALSymbolsLibrary();
                else
                {
                    library = new ALSymbolsLibrary();
                    response.SetError("File does not contain any symbols.");
                }

                //ALPackageSymbolsLibrary library = this.Server.AppPackagesCache.GetSymbols(parameters.path, false);
                if (library != null)
                {
                    response.libraryId = this.Server.SymbolsLibraries.AddLibrary(library);
                    response.root = library.GetObjectsTree(); // .Root;
                }
            }
            catch (Exception e)
            {
                response.root = new ALSymbol
                {
                    fullName = e.Message
                };
                response.SetError(e.Message);
                this.LogError(e);
            }
            return response;
        }
#pragma warning restore 1998

    }
}
