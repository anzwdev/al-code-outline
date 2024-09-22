using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Server.Contracts;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class LibrarySymbolsDetailsRequestHandler : RequestHandler
    {

        public LibrarySymbolsDetailsRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/librarysymbolsdetails", UseSingleObjectParameterDeserialization = true)]
        public LibrarySymbolsDetailsResponse LibrarySymbolsDetails(LibrarySymbolsDetailsRequest parameters)
        {
            RebuildModifiedSymbols();

            LibrarySymbolsDetailsResponse response = new LibrarySymbolsDetailsResponse();
            try
            {
                ALSymbolsLibrary library = this.Server.SymbolsLibraries.GetLibrary(parameters.libraryId);
                if (library != null)
                {
                    response.symbols = library.GetSymbolsListByPath(parameters.paths, parameters.kind);
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
