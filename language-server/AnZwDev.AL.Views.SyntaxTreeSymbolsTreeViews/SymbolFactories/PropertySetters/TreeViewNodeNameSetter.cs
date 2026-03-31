using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.PropertySetters
{
    internal abstract class TreeViewNodeNameSetter
    {

        public abstract void SetName(SyntaxTreeSymbolsTreeViewNode node, NameSyntax? nameNode);

    }
}
