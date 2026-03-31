using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Providers
{
    public abstract class SymbolsProvider
    {

        protected ApplicationSymbol? Symbols { get; private set; }
        public string? AppId { get { return Symbols?.AppId; } }

        public SymbolsProvider()
        {
        }

        public virtual ApplicationSymbol? GetSymbols()
        {
            return Symbols;
        }

        protected virtual void SetSymbols(ApplicationSymbol? symbols)
        {
            Symbols = symbols;
        }

        public abstract void Load(bool metadataOnly);

    }
}
