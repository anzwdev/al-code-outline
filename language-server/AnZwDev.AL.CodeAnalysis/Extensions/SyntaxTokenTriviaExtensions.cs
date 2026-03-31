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
    public static class SyntaxTokenTriviaExtensions
    {

        public static SyntaxToken WithLeadingLeadingTrivia(this SyntaxToken token, List<SyntaxTrivia> targetCollection)
        {
            if (targetCollection.Count == 0)
                return token;

            IEnumerable<SyntaxTrivia> newList = targetCollection;
            var existingTrivia = token.LeadingTrivia;
            if (existingTrivia.Count > 0)
                newList = targetCollection.MergeWith(existingTrivia);

            return token.WithLeadingTrivia(SyntaxFactory.TriviaList(newList));
        }


    }
}
