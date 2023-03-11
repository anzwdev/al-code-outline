using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppBaseElement : IComparable
    {

        public ALAppBaseElement()
        {
        }

        public virtual ALSymbol ToALSymbol()
        {
            ALSymbol symbol = this.CreateMainALSymbol();
            this.AddChildALSymbols(symbol);
            return symbol;
        }

        protected virtual ALSymbol CreateMainALSymbol()
        {
            return new ALSymbol();
        }

        protected virtual void AddChildALSymbols(ALSymbol symbol)
        {
        }

        public virtual ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.Undefined;
        }

        public virtual ALSymbolAccessModifier? GetAccessModifier()
        {
            return null;
        }

        public virtual string GetSourceCode()
        {
            return "";
        }

        public virtual int CompareTo(object obj)
        {
            return 0;
        }

    }
}
