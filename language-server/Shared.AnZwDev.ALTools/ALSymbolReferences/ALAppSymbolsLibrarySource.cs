using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppSymbolsLibrarySource : ALAppBaseSymbolsLibrarySource
    {

        public ALBaseSymbolReference SymbolReference { get; private set; }

        public ALAppSymbolsLibrarySource(ALBaseSymbolReference symbolReference)
        {
            this.SymbolReference = symbolReference;
        }

        public override ALSymbolSourceLocation GetSymbolSourceLocation(ALSymbol symbol)
        {
            var objectTypeInformation = ALObjectTypesInformationCollection.Get(symbol.kind);
            var location = new ALSymbolSourceLocation(symbol);
            
            if ((objectTypeInformation != null) && (objectTypeInformation.ALObjectType != ALObjectType.None))
            {
                ALAppObject alAppObject = this.SymbolReference
                    .AllObjects
                    .GetObjectsCollection(objectTypeInformation.ALObjectType)
                    .FindFirst(null, symbol.name);
                if (alAppObject != null)
                    this.SetSource(location, this.SymbolReference, alAppObject);
            }
            return location;
        }

    }
}
