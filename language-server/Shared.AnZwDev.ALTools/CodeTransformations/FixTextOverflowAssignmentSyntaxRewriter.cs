using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class FixTextOverflowAssignmentSyntaxRewriter : ALSemanticModelSyntaxRewriter
    {

        public FixTextOverflowAssignmentSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitAssignmentStatement(AssignmentStatementSyntax node)
        {
            return base.VisitAssignmentStatement(node);
        }

    }
}
