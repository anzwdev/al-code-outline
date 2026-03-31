using AnZwDev.AL.Syntax;
using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.PropertySetters;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class ObjectSymbolFactory
    {

        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(ObjectSyntax node, ObjectIdSyntax? objectIdSyntax, SyntaxTreeSymbolsTreeViewNode? parentNode, ALSyntaxNodeKind symbolTreeNodeKind)
        {
            TreeViewNodeNameSetter nameSetter;
            var objectId = 0;
            if (objectIdSyntax != null)
            {
                var idString = objectIdSyntax?.Value.ValueText;
                if ((!String.IsNullOrEmpty(idString)) && (Int32.TryParse(idString, out int id)))
                    objectId = id;
                nameSetter = TreeViewNodeNameSetters.ObjectWithId;
            } 
            else
                nameSetter = TreeViewNodeNameSetters.ObjectWithoutId;

            var compilationUnit = parentNode?.FindThisOrParent(ALSyntaxNodeKind.CompilationUnit);
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                objectId, node, node.Name, 
                symbolTreeNodeKind, 
                nameSetter,
                compilationUnit?.NamespaceName, compilationUnit?.Usings);

            return symbol;
        }

    }
}
