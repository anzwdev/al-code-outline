using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetNewFileRequiredInterfacesRequest
    {

        public bool useFolderStructure {  get; set; }
        public string path { get; set; }
        public string rootNamespace { get; set; }
        public List<SymbolReference> referencedObjects { get; set; }

    }
}
