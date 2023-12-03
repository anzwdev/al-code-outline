using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RemoveProceduresSemicolonSyntaxRewriter : ALSyntaxRewriter
    {

        public bool IncludeInterfaces { get; set; } = false;

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if ((node.SemicolonToken != null) && (node.SemicolonToken.Kind.ConvertToLocalType() == ConvertedSyntaxKind.SemicolonToken))
            {
                var validParentObject = true;
                if (!IncludeInterfaces)
                {
                    var applicationObject = node.FindParentApplicationObject();
                    validParentObject = (applicationObject == null) || (applicationObject.Kind.ConvertToLocalType() != ConvertedSyntaxKind.Interface);
                }

                if (validParentObject)
                {
                    NoOfChanges++;
                    
                    var semicolon = node.SemicolonToken;

                    var kind = ConvertedSyntaxKind.EmptyToken.Convert<ConvertedSyntaxKind, SyntaxKind>();
                    var newSemicolon = SyntaxFactory.Token(kind);
                    node = node.WithSemicolonToken(newSemicolon);

                    if (node.ReturnValue != null)
                    {
                        var newTrivia = node.ReturnValue.GetTrailingTrivia();
                        newTrivia = newTrivia
                            .AddRange(semicolon.LeadingTrivia)
                            .AddRange(semicolon.TrailingTrivia);
                        var newReturnValue = node.ReturnValue.WithTrailingTrivia(newTrivia);
                        node = node.WithReturnValue(newReturnValue);
                    }
                    else if ((node.ParameterList.CloseParenthesisToken != null) && (node.ParameterList.CloseParenthesisToken.Kind.ConvertToLocalType() == ConvertedSyntaxKind.CloseParenToken))
                    {
                        var newTrivia = node.ParameterList.CloseParenthesisToken.TrailingTrivia;
                        newTrivia = newTrivia
                            .AddRange(semicolon.LeadingTrivia)
                            .AddRange(semicolon.TrailingTrivia);
                        var newCloseParenthesisToken = node.ParameterList.CloseParenthesisToken.WithTrailingTrivia(newTrivia);
                        var newParameterList = node.ParameterList.WithCloseParenthesisToken(newCloseParenthesisToken);
                        node = node.WithParameterList(newParameterList);
                    }

                }
            }
            return base.VisitMethodDeclaration(node);
        }

    }
}
