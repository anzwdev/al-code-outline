using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.SyntaxHelpers;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations.Namespaces
{

#if BC

    public class UodateUsingsListSyntaxRewriter : ALSyntaxRewriter
    {
        public bool RemoveUnusedUsings { get; set; } = true;
        public bool AddMissingUsings { get; set; } = true;

        private readonly SyntaxTreeUsingsCollector _usingsCollector = new SyntaxTreeUsingsCollector();

        public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node)
        {
            var newUsings = UpdateUsings(node);
            if (newUsings != null)
                node = node.WithUsings(SyntaxFactory.List(newUsings));
            return base.VisitCompilationUnit(node);
        }

        private List<UsingDirectiveSyntax> UpdateUsings(CompilationUnitSyntax node)
        {
            List<UsingDirectiveSyntax> newUsings = new List<UsingDirectiveSyntax>();
            HashSet<string> collectedUsingNames = new HashSet<string>();
            var namespacesInformation = _usingsCollector.CollectUsings(Project, node);
            List<SyntaxTrivia> remainingTrivia = null;
            var deleted = false;
            var added = false;

            //remove unused usings
            if ((node.Usings != null) && (node.Usings.Count > 0))
                for (int i = 0; i < node.Usings.Count; i++)
                {
                    var usingSyntax = node.Usings[i];

                    if (usingSyntax.ContainsDiagnostics)
                        return null;

                    var usingName = usingSyntax.Name?.ToString();
                    var validUsingName = !String.IsNullOrWhiteSpace(usingName);
                    var alreadyCollected = (validUsingName) && (collectedUsingNames.Contains(usingName));

                    if (
                        (!RemoveUnusedUsings) ||
                        ((validUsingName) && (!alreadyCollected) && (namespacesInformation.UsingRequired(usingName))))
                    {
                        if ((remainingTrivia != null) && (remainingTrivia.Count > 0))
                            usingSyntax = usingSyntax.WithLeadingTrailingTrivia(remainingTrivia);
                        remainingTrivia = null;

                        newUsings.Add(usingSyntax);
                        if ((validUsingName) && (!alreadyCollected))
                            collectedUsingNames.Add(usingName);
                    }
                    else
                    {
                        remainingTrivia = MergeLists(remainingTrivia, usingSyntax.GetLeadingTrivia());
                        remainingTrivia = MergeLists(remainingTrivia, usingSyntax.GetTrailingTrivia());
                        deleted = true;
                        NoOfChanges++;
                    }
                }

            //add missing usings
            if ((AddMissingUsings) && (namespacesInformation.Usings != null))
            {
                foreach (var usingInformation in namespacesInformation.Usings.Values)
                    if ((usingInformation.UsingRequired) && (!collectedUsingNames.Contains(usingInformation.Namespace)))
                    {
                        var usingSyntax = SyntaxFactory.UsingDirective(
                            SyntaxFactory.Token(SyntaxKind.UsingKeyword),
                            ExpressionFactory.NamespaceName(usingInformation.Namespace)
                                .WithLeadingTrivia(SyntaxFactory.WhiteSpace(" ")),
                            SyntaxFactory.Token(SyntaxKind.SemicolonToken));

                        if ((remainingTrivia != null) && (remainingTrivia.Count > 0))
                            usingSyntax = usingSyntax.WithLeadingTrivia(remainingTrivia);
                        remainingTrivia = null;

                        usingSyntax = usingSyntax.WithTrailingTrivia(
                            SyntaxFactory.EndOfLine("\n"));

                        newUsings.Add(usingSyntax);
                        added = true;
                        NoOfChanges++;
                    }

                if (added)
                    newUsings[newUsings.Count - 1] = newUsings[newUsings.Count - 1]
                        .WithTrailingTrivia(
                            SyntaxFactory.EndOfLine("\n"),
                            SyntaxFactory.WhiteSpace("    "),
                            SyntaxFactory.EndOfLine("\n")); ;
            }

            if (deleted || added)
                return newUsings;

            return null;
        }

        private List<SyntaxTrivia> MergeLists(List<SyntaxTrivia> syntaxTrivias, SyntaxTriviaList additionalTrivias)
        {
            if ((additionalTrivias != null) && (additionalTrivias.Count > 0))
            {
                if (syntaxTrivias == null)
                    syntaxTrivias = additionalTrivias.ToList();
                else
                    syntaxTrivias.AddRange(additionalTrivias);
            }
            return syntaxTrivias;
        }

        private Dictionary<string, UsingDirectiveSyntax> GetExistingUsings(CompilationUnitSyntax compilationUnitSyntax)
        {
            Dictionary<string, UsingDirectiveSyntax> usings = new Dictionary<string, UsingDirectiveSyntax>();
            if ((compilationUnitSyntax.Usings != null) && (compilationUnitSyntax.Usings.Count > 0))
                foreach (var usingSyntax in compilationUnitSyntax.Usings)
                {
                    var usingName = usingSyntax.Name?.ToString();
                    if ((usingSyntax.ContainsDiagnostics) || (String.IsNullOrWhiteSpace(usingName)))
                        return null;
                    if (!usings.ContainsKey(usingName))
                        usings.Add(usingName, usingSyntax);
                }
            return usings;
        }

    }
#endif
}
