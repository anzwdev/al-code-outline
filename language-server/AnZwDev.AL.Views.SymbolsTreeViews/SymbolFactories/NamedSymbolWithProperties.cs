using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal abstract class NamedSymbolWithPropertiesFactory<T> : NamedSymbolFactory<T> where T : NamedSymbolWithProperties
    {
    }
}
