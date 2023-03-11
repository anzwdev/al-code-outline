using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class GetLibrarySymbolLocationRequest
    {
        public int libraryId { get; set; }
        public int[] path { get; set; }

    }
}
