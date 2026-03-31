using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Merging
{
    internal abstract class SymbolMerger<TSymbol, TSymbolExtension> where TSymbol : ObjectSymbol where TSymbolExtension : ObjectExtensionSymbol
    {

        public TSymbol Merge(TSymbol mainSymbol, IEnumerable<TSymbolExtension> symbolExtensionsEnumerable)
        {
            var mergedSymbol = CloneSymbol(mainSymbol);

            foreach (var symbolExtension in symbolExtensionsEnumerable)
                ApplyExtension(mergedSymbol, symbolExtension);
        
            return mergedSymbol;
        }

        protected abstract TSymbol CloneSymbol(TSymbol mainSymbol);
        protected abstract void ApplyExtension(TSymbol mainSymbol, TSymbolExtension symbolExtension);

    }
}
