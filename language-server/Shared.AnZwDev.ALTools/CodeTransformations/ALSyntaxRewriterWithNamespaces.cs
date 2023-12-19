using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class ALSyntaxRewriterWithNamespaces : ALSyntaxRewriter
    {

        public string NamespaceName { get; private set; } = null;
        public HashSet<string> Usings { get; } = new HashSet<string>();

#if BC

        protected void UpdateNamespace(CompilationUnitSyntax node)
        {
            NamespaceName = node.NamespaceDeclaration?.Name?.ToString();
        }

        public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node)
        {
            UpdateNamespace(node);
            return base.VisitCompilationUnit(node);
        }

#endif


    }
}
