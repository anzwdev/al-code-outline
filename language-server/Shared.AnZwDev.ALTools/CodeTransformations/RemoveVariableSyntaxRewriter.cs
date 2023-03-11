using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{

    public class RemoveVariableSyntaxRewriter: ALSyntaxRewriter
    {

        public RemoveVariableSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            if (node.Span.IntersectsWith(this.Span))
                return null;
            return base.VisitVariableDeclaration(node);
        }

#if BC
        public override SyntaxNode VisitReturnValue(ReturnValueSyntax node)
        {
            if ((node.Name != null) && (node.Span.IntersectsWith(this.Span)))
                return node.RemoveNode(node.Name, 
                    SyntaxRemoveOptions.KeepDirectives &
                    SyntaxRemoveOptions.KeepLeadingTrivia &
                    SyntaxRemoveOptions.KeepTrailingTrivia);
            return base.VisitReturnValue(node);
        }
#endif

    }
}
