using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Server.Contracts;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetLibrarySymbolLocationRequestHandler : RequestHandler
    {

        public GetLibrarySymbolLocationRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/librarysymbollocation", UseSingleObjectParameterDeserialization = true)]
        public GetLibrarySymbolLocationResponse GetLibrarySymbolLocation(GetLibrarySymbolLocationRequest parameters)
        {
            RebuildModifiedSymbols();

            GetLibrarySymbolLocationResponse response = new GetLibrarySymbolLocationResponse();
            try
            {
                ALSymbolsLibrary library = this.Server.SymbolsLibraries.GetLibrary(parameters.libraryId);
                if (library != null)
                {
                    ALSymbol symbol = library.GetSymbolByPath(parameters.path);
                    if (symbol != null)
                        response.location = library.GetSymbolSourceLocation(symbol);
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
