using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class SyntaxTokenExtensions
    {

        public static bool IsEmpty(this SyntaxToken token)
        {
            return (token.Kind.ConvertToLocalType() == ALSymbols.Internal.ConvertedSyntaxKind.None);
        }

        public static bool IsEmptyOrBefore(this SyntaxToken token, int position)
        {
            return (token.IsEmpty()) || (token.Span.End <= position);
        }

        public static bool IsEmptyOrAfter(this SyntaxToken token, int position)
        {
            return (token.IsEmpty()) || (token.Span.Start >= position);
        }

        internal static SyntaxToken WithLeadingLeadingTrivia(this SyntaxToken node, List<SyntaxTrivia> targetCollection)
        {
            if (targetCollection.Count == 0)
                return node;

            IEnumerable<SyntaxTrivia> newList = targetCollection;
            var existingTrivia = node.LeadingTrivia;
            if ((existingTrivia != null) && (existingTrivia.Count > 0))
                newList = targetCollection.MergeWith(existingTrivia);

            return node.WithLeadingTrivia(SyntaxFactory.TriviaList(newList));
        }

        internal static SyntaxToken WithTrailingTrailingTrivia(this SyntaxToken node, List<SyntaxTrivia> targetCollection)
        {
            if (targetCollection.Count == 0)
                return node;

            IEnumerable<SyntaxTrivia> newList = targetCollection;
            var existingTrivia = node.TrailingTrivia;
            if ((existingTrivia != null) && (existingTrivia.Count > 0))
                newList = existingTrivia.MergeWith(targetCollection);

            return node.WithTrailingTrivia(SyntaxFactory.TriviaList(newList));
        }

    }
}

