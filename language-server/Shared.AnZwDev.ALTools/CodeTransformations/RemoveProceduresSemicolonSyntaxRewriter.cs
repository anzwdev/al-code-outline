using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RemoveProceduresSemicolonSyntaxRewriter : ALSyntaxRewriter
    {

        public bool IncludeInterfaces { get; set; } = false;


        public override SyntaxNode VisitTriggerDeclaration(TriggerDeclarationSyntax node)
        {
            if (HasSemicolon(node))
            {
                NoOfChanges++;

                var semicolon = node.SemicolonToken;
                node = node.WithSemicolonToken(CreateEmptySemicolonToken());

                if (node.ReturnValue != null)
                    node = node.WithReturnValue(UpdateReturnValue(node.ReturnValue, semicolon));
                else if ((node.ParameterList.CloseParenthesisToken != null) && (node.ParameterList.CloseParenthesisToken.Kind.ConvertToLocalType() == ConvertedSyntaxKind.CloseParenToken))
                    node = node.WithParameterList(UpdateParameterList(node.ParameterList, semicolon));
            }

            return base.VisitTriggerDeclaration(node);
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (HasSemicolon(node))
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
                    node = node.WithSemicolonToken(CreateEmptySemicolonToken());

                    if (node.ReturnValue != null)
                        node = node.WithReturnValue(UpdateReturnValue(node.ReturnValue, semicolon));
                    else if ((node.ParameterList.CloseParenthesisToken != null) && (node.ParameterList.CloseParenthesisToken.Kind.ConvertToLocalType() == ConvertedSyntaxKind.CloseParenToken))
                        node = node.WithParameterList(UpdateParameterList(node.ParameterList, semicolon));

                }
            }
            return base.VisitMethodDeclaration(node);
        }


        private SyntaxToken CreateEmptySemicolonToken()
        {
            var kind = ConvertedSyntaxKind.EmptyToken.Convert<ConvertedSyntaxKind, SyntaxKind>();
            return SyntaxFactory.Token(kind);
        }

        private ReturnValueSyntax UpdateReturnValue(ReturnValueSyntax node, SyntaxToken semicolon)
        {
            var newTrivia = node.GetTrailingTrivia();
            newTrivia = newTrivia
                .AddRange(semicolon.LeadingTrivia)
                .AddRange(semicolon.TrailingTrivia);
            var newReturnValue = node.WithTrailingTrivia(newTrivia);
            return newReturnValue;
        }

        private ParameterListSyntax UpdateParameterList(ParameterListSyntax node, SyntaxToken semicolon)
        {
            var newTrivia = node.CloseParenthesisToken.TrailingTrivia;
            newTrivia = newTrivia
                .AddRange(semicolon.LeadingTrivia)
                .AddRange(semicolon.TrailingTrivia);
            var newCloseParenthesisToken = node.CloseParenthesisToken.WithTrailingTrivia(newTrivia);
            var newParameterList = node.WithCloseParenthesisToken(newCloseParenthesisToken);
            return newParameterList;
        }

        private bool HasSemicolon(MethodOrTriggerDeclarationSyntax node)
        {
            return ((node.SemicolonToken != null) && (node.SemicolonToken.Kind.ConvertToLocalType() == ConvertedSyntaxKind.SemicolonToken));
        }

    }
}
