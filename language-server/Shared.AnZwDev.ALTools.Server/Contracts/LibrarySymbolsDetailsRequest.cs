using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class LibrarySymbolsDetailsRequest
    {

        public int libraryId { get; set; }
        public ALSymbolKind kind { get; set; }
        public int[][] paths { get; set; }

    }
}
