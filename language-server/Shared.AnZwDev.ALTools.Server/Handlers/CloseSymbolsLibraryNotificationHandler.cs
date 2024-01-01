using AnZwDev.ALTools;
using AnZwDev.ALTools.Server.Contracts;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class CloseSymbolsLibraryNotificationHandler : RequestHandler
    {

        public CloseSymbolsLibraryNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/closesymbolslibrary", UseSingleObjectParameterDeserialization = true)]
        public void CloseSymbolsLibrary(CloseSymbolsLibraryRequest parameters)
        {
            this.Server.SymbolsLibraries.RemoveLibrary(parameters.libraryId);
        }

    }
}
