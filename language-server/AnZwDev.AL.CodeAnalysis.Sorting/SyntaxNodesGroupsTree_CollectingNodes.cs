using AnZwDev.AL.CodeAnalysis.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Sorting
{
    public partial class SyntaxNodesGroupsTree<T> where T : SyntaxNode
    {

        public (bool, SyntaxToken?, bool) AddNodes(IEnumerable<T> nodesCollection, bool dontGroupSingleNodeRegions, SyntaxToken? closingToken = null)
        {
            var closingTokenModified = false;

            this.Root = new SyntaxNodesGroup<T>();
            SyntaxNodesGroup<T> group = this.Root;
            SyntaxNodesGroup<T>? newGroup = null;

            foreach (T node in nodesCollection)
            {
                newGroup = this.AddNode(group, node);
                if (newGroup == null)
                {
                    this.Root = null;
                    return (false, closingToken, closingTokenModified);
                }

                group = newGroup;
            }

            if (closingToken != null)
            {
                var hasGroups = false;
                (newGroup, hasGroups, closingToken, closingTokenModified) = ProcessClosingSyntaxTokenLeadingTrivias(group, closingToken.Value);
                if (newGroup == null)
                {
                    this.Root = null;
                    return (false, closingToken, closingTokenModified);
                }
                group = newGroup;
            }

            //something went wrong and we are not back at the top group (missing endregion directives)
            if (group.ParentGroup != null)
            {
                this.Root = null;
                return (false, closingToken, closingTokenModified);
            }

            if (dontGroupSingleNodeRegions)
                this.Root.RemoveSingleNodeGroups();

            return (true, closingToken, closingTokenModified);
        }

        protected SyntaxNodesGroup<T>? AddNode(SyntaxNodesGroup<T> group, T node)
        {
            SyntaxTriviaList triviaList = node.GetLeadingTrivia();

            if (triviaList.Count > 0)
            {
                //collect regions
                List<SyntaxTrivia> triviaCache = new List<SyntaxTrivia>();
                bool hasGroups = false;

                foreach (SyntaxTrivia trivia in triviaList)
                {
                    triviaCache.Add(trivia);

                    switch (trivia.Kind)
                    {
                        case SyntaxKind.RegionDirectiveTrivia:
                            SyntaxNodesGroup<T> childGroup = new SyntaxNodesGroup<T>();
                            childGroup.LeadingTrivia = triviaCache;
                            group.AddGroup(childGroup);
                            group = childGroup;
                            triviaCache = new List<SyntaxTrivia>();
                            hasGroups = true;
                            break;
                        case SyntaxKind.EndRegionDirectiveTrivia:
                            group.TrailingTrivia = triviaCache;
                            if (group.ParentGroup == null)
                                return null;
                            group = group.ParentGroup;
                            triviaCache = new List<SyntaxTrivia>();
                            hasGroups = true;
                            break;
                        default:
                            //do not sort if code contains other directives
                            if (trivia.IsDirective)
                                return null;
                            break;
                    }
                }

                if (hasGroups)
                    node = node.WithLeadingTrivia(triviaCache);
            }

            if (node.HasOpenDirectives())
                return null;

            group.SyntaxNodes.Add(node);
            return group;
        }

        public (SyntaxNodesGroup<T>?, bool, SyntaxToken, bool) ProcessClosingSyntaxTokenLeadingTrivias(SyntaxNodesGroup<T> group, SyntaxToken token)
        {
            bool hasGroups = false;
            bool tokenModified = false;
            SyntaxTriviaList syntaxTrivias = token.LeadingTrivia;

            if (syntaxTrivias.Count > 0)
            {
                //collect regions
                List<SyntaxTrivia> triviaCache = new List<SyntaxTrivia>();

                foreach (SyntaxTrivia trivia in syntaxTrivias)
                {
                    triviaCache.Add(trivia);

                    switch (trivia.Kind)
                    {
                        case SyntaxKind.RegionDirectiveTrivia:
                            SyntaxNodesGroup<T> childGroup = new SyntaxNodesGroup<T>();
                            childGroup.LeadingTrivia = triviaCache;
                            group.AddGroup(childGroup);
                            group = childGroup;
                            triviaCache = new List<SyntaxTrivia>();
                            hasGroups = true;
                            break;
                        case SyntaxKind.EndRegionDirectiveTrivia:
                            group.TrailingTrivia = triviaCache;
                            if (group.ParentGroup == null)
                                return (null, hasGroups, token, tokenModified);
                            group = group.ParentGroup;
                            triviaCache = new List<SyntaxTrivia>();
                            hasGroups = true;
                            break;
                        default:
                            //do not sort if code contains other directives
                            if (trivia.IsDirective)
                                return (null, hasGroups, token, tokenModified);
                            break;
                    }
                }

                if (hasGroups)
                {
                    token = token.WithLeadingTrivia(triviaCache);
                    tokenModified = true;
                }
            }

            return (group, hasGroups, token, tokenModified);
        }

    }
}
