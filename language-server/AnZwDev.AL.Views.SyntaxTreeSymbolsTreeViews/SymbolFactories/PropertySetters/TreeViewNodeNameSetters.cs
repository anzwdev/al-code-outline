using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.PropertySetters
{
    internal static class TreeViewNodeNameSetters
    {

        public static TreeViewNodeNameSetter Default { get; } = new TreeViewNodeIdentifierNameNameSetter();
        public static TreeViewNodeIdentifierNameNameSetter IdentifierName { get; } = new TreeViewNodeIdentifierNameNameSetter();
        public static TreeViewNodeKindNameSetter Kind { get; } = new TreeViewNodeKindNameSetter();
        public static TreeViewNodeKindWithIdentifierNameNameSetter KindWithIdentifierName { get; } = new TreeViewNodeKindWithIdentifierNameNameSetter();
        public static TreeViewNodeObjectWithIdNameSetter ObjectWithId { get; } = new TreeViewNodeObjectWithIdNameSetter();
        public static TreeViewNodeObjectWithoutIdNameSetter ObjectWithoutId { get; } = new TreeViewNodeObjectWithoutIdNameSetter();

    }
}
