using AnZwDev.AL.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal abstract class NamedSymbolWithIdAndPropertiesFactory<T> : NamedSymbolWithIdFactory<T> where T : NamedSymbolWithIdAndProperties
    {
    }
}
