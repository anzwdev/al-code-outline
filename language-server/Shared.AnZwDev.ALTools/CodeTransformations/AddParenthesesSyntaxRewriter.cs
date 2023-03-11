using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{

#if BC

    public class AddParenthesesSyntaxRewriter : ALSemanticModelSyntaxRewriter
    {

        public AddParenthesesSyntaxRewriter()
        {
        }

        public override SyntaxNode Visit(SyntaxNode node)
        {
            return base.Visit(node);
        }

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                (SyntaxNode newNode, bool updated) = this.UpdateSyntaxNode(node);
                if (updated)
                    return newNode;
            }

            return base.VisitIdentifierName(node);
        }

        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                (SyntaxNode newNode, bool updated) = this.UpdateSyntaxNode(node);
                if (updated)
                    return newNode;
            }

            return base.VisitMemberAccessExpression(node);
        }

        protected (SyntaxNode, bool) UpdateSyntaxNode(CodeExpressionSyntax node)
        {
            bool isAssignmentTarget = false;
            if (node.Parent is AssignmentStatementSyntax assignmentNode)
                isAssignmentTarget = (assignmentNode.Target == node);

            if (!isAssignmentTarget)
            {
                IOperation o = this.SemanticModel.GetOperation(node);
                if (this.SemanticModel.GetOperation(node) is IInvocationExpression operation)
                {
                    if (operation.Arguments.Length == 0)
                    {
                        SymbolInfo symbolInfo = this.SemanticModel.GetSymbolInfo(node);
                        bool invalidSymbol = false;
                        if ((symbolInfo != null) && (symbolInfo.Symbol != null))
                        {
                            ConvertedSymbolKind symbolKind = symbolInfo.Symbol.Kind.ConvertToLocalType();
                            invalidSymbol = (symbolKind != ConvertedSymbolKind.Method);
                        }

                        if (!invalidSymbol)
                        {
                            IMethodSymbol targetMethod = operation.TargetMethod;
                            ConvertedMethodKind targetMethodKind = (targetMethod == null) ? ConvertedMethodKind.Method : targetMethod.MethodKind.ConvertToLocalType();
                            bool isBuiltInProperty = false;
                            if ((targetMethodKind == ConvertedMethodKind.BuiltInMethod) && (targetMethod is IBuiltInMethodTypeSymbol builtInMethodTypeSymbol))
                                isBuiltInProperty = builtInMethodTypeSymbol.IsProperty;

                            bool skipProcessing =
                                (targetMethod != null) &&
                                (
                                    (targetMethodKind == ConvertedMethodKind.Property) ||
                                    (isBuiltInProperty)
                                );

                            if (!skipProcessing)
                            {
                                SyntaxToken lastToken = operation.Syntax.GetLastToken();
                                bool hasCloseParenToken = ((lastToken != null) && (lastToken.Kind.ConvertToLocalType() == ConvertedSyntaxKind.CloseParenToken));

                                if (!hasCloseParenToken)
                                {
                                    SyntaxNode operationNode = operation.Syntax;
                                    if (operationNode == node)
                                    {
                                        this.NoOfChanges++;
                                        return (SyntaxFactory.InvocationExpression(node).WithTriviaFrom(node), true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return (node, false);
        }


        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            return base.VisitInvocationExpression(node);
        }


    }

#endif

}
