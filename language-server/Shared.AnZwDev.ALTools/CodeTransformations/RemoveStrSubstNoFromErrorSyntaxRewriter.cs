using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RemoveStrSubstNoFromErrorSyntaxRewriter : ALSemanticModelSyntaxRewriter
    {

        public RemoveStrSubstNoFromErrorSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                var name = ALSyntaxHelper.DecodeName(node.Expression?.ToString());
                if ((name != null) &&
                    (name.Equals("Error", StringComparison.CurrentCultureIgnoreCase)) &&
                    (node.ArgumentList?.Arguments != null) &&
                    (node.ArgumentList.Arguments.Count == 1) &&
                    (node.ArgumentList.Arguments[0] is InvocationExpressionSyntax argsInvocationExpressionSyntax))
                {
                    var operation = SemanticModel.GetOperation(node);
                    if ((operation != null) && (operation is IInvocationExpression invocationExpression))
                    {
                        if ((invocationExpression.Arguments != null) &&
                            (!invocationExpression.Arguments.IsEmpty) &&
                            ("error".Equals(invocationExpression.TargetMethod?.Name, StringComparison.CurrentCultureIgnoreCase)) &&
                            (invocationExpression.Arguments[0].Value is IInvocationExpression argInvocationExpression))
                        {
                            if ("strsubstno".Equals(argInvocationExpression.TargetMethod?.Name, StringComparison.CurrentCultureIgnoreCase))
                            {
                                var newArgsList = argsInvocationExpressionSyntax.ArgumentList;
                                node = node.WithArgumentList(newArgsList);
                                NoOfChanges++;
                            }
                        }
                    }
                }
            }
            return base.VisitInvocationExpression(node);
        }

    }
}
