using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Server.Contracts;
using System;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbolReferences;
using StreamJsonRpc;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class AppPackageSymbolsRequestHandler : RequestHandler
    {

        public AppPackageSymbolsRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/packagesymbols", UseSingleObjectParameterDeserialization = true)]
        public AppPackageSymbolsResponse GetPackageSymbols(AppPackageSymbolsRequest parameters)
        {
            RebuildModifiedSymbols();

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

    }
}
