using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.VSCodeLangServer.Protocol.Server;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class LibrarySymbolsDetailsRequestHandler : BaseALRequestHandler<LibrarySymbolsDetailsRequest, LibrarySymbolsDetailsResponse>
    {

        public LibrarySymbolsDetailsRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/librarysymbolsdetails")
        {
        }

#pragma warning disable 1998
        protected override async Task<LibrarySymbolsDetailsResponse> HandleMessage(LibrarySymbolsDetailsRequest parameters, RequestContext<LibrarySymbolsDetailsResponse> context)
        {
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
#pragma warning restore 1998

    }
}
