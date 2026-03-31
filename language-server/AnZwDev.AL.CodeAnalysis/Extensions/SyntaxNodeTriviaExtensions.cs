using AnZwDev.System.Collections.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Extensions
{
    public static class SyntaxNodeTriviaExtensions
    {

        public static bool HasNonEmptyTriviaInside(this SyntaxNode node)
        {
            foreach (var token in node.DescendantTokens())
                if ((!token.LeadingTrivia.IsNullOrWhiteSpace()) || (!token.TrailingTrivia.IsNullOrWhiteSpace()))
                    return true;

            return false;
        }

        public static void CollectDirectiveTrivias(this SyntaxNode node, List<SyntaxTrivia> targetCollection)
        {
            node.CollectLeadingDirectiveTrivias(targetCollection);
            node.CollectTrailingDirectiveTrivias(targetCollection);
        }

        public static void CollectLeadingDirectiveTrivias(this SyntaxNode node, List<SyntaxTrivia> targetCollection)
        {
            targetCollection.AddRange(node.GetLeadingTrivia().Where(p => (p.IsDirective)));
        }

        public static void CollectTrailingDirectiveTrivias(this SyntaxNode node, List<SyntaxTrivia> targetCollection)
        {
            targetCollection.AddRange(node.GetTrailingTrivia().Where(p => (p.IsDirective)));
        }

        public static T WithLeadingLeadingTrivia<T>(this T node, List<SyntaxTrivia> targetCollection) where T : SyntaxNode
        {
            if (targetCollection.Count == 0)
                return node;

            IEnumerable<SyntaxTrivia> newList = targetCollection;
            var existingTrivia = node.GetLeadingTrivia();
            if (existingTrivia.Count > 0)
                newList = targetCollection.MergeWith(existingTrivia);

            return node.WithLeadingTrivia(SyntaxFactory.TriviaList(newList));
        }

        public static T WithLeadingTrailingTrivia<T>(this T node, List<SyntaxTrivia> targetCollection) where T : SyntaxNode
        {
            if (targetCollection.Count == 0)
                return node;

            IEnumerable<SyntaxTrivia> newList = targetCollection;
            var existingTrivia = node.GetTrailingTrivia();
            if (existingTrivia.Count > 0)
                newList = targetCollection.MergeWith(existingTrivia);

            return node.WithTrailingTrivia(SyntaxFactory.TriviaList(newList));
        }

        public static T WithTrailingTrailingTrivia<T>(this T node, List<SyntaxTrivia> targetCollection) where T : SyntaxNode
        {
            if (targetCollection.Count == 0)
                return node;

            IEnumerable<SyntaxTrivia> newList = targetCollection;
            var existingTrivia = node.GetTrailingTrivia();
            if (existingTrivia.Count > 0)
                newList = existingTrivia.MergeWith(targetCollection);

            return node.WithTrailingTrivia(SyntaxFactory.TriviaList(newList));
        }


    }
}
