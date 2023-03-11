using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppBaseSymbolsLibrarySource : ALSymbolsLibrarySource
    {

        public ALAppBaseSymbolsLibrarySource()
        {
        }

        protected void SetSource(ALSymbolSourceLocation location, ALAppSymbolReference symbolReference, ALAppObject alAppObject)
        {
            if (String.IsNullOrWhiteSpace(alAppObject.ReferenceSourceFileName))
            {
                location.schema = null;
                location.sourcePath = null;
            }
            else if ((symbolReference == null) || (String.IsNullOrWhiteSpace(symbolReference.ReferenceSourceFileName)))
            {
                location.schema = ALSymbolSourceLocationSchema.File;
                location.sourcePath = alAppObject.ReferenceSourceFileName;
            }
            else
            {
                location.schema = ALSymbolSourceLocationSchema.ALApp;
                location.sourcePath = symbolReference.ReferenceSourceFileName + "::" + alAppObject.ReferenceSourceFileName;
            }
        }

    }
}
