﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.ALSymbols.Internal;
using System.Xml.Linq;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SyntaxNodesGroupsTree<T> where T: SyntaxNode
    {

        public SyntaxNodesGroup<T> Root { get; set; }

        public SyntaxNodesGroupsTree()
        {
            this.Root = null;
        }

        #region Add nodes to the tree

        public (bool, SyntaxToken?, bool) AddNodes(IEnumerable<T> nodesCollection, bool dontGroupSingleNodeRegions, SyntaxToken? closingToken = null)
        {
            var closingTokenModified = false;

            this.Root = new SyntaxNodesGroup<T>();
            SyntaxNodesGroup<T> group = this.Root;
            foreach (T node in nodesCollection)
            {
                group = this.AddNode(group, node);
                if (group == null)
                {
                    this.Root = null;
                    return (false, closingToken, closingTokenModified);
                }
            }

            if (closingToken != null)
            {
                var hasGroups = false;
                (group, hasGroups, closingToken, closingTokenModified) = ProcessClosingSyntaxTokenLeadingTrivias(group, closingToken.Value);
                if (group == null)
                {
                    this.Root = null;
                    return (false, closingToken, closingTokenModified);
                }
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

        protected SyntaxNodesGroup<T> AddNode(SyntaxNodesGroup<T> group, T node)
        {
#if BC
            /*
            var directives = node.GetDirectives((directive) => {
                var kind = directive.Kind.ConvertToLocalType();
                return
                    (kind != ConvertedSyntaxKind.RegionDirectiveTrivia) &&
                    (kind != ConvertedSyntaxKind.EndRegionDirectiveTrivia);
            });
            if (directives.Count > 0)
                return null;
            */

#endif

            SyntaxTriviaList triviaList = node.GetLeadingTrivia();

            if ((triviaList != null) && (triviaList.Count > 0))
            {
                //collect regions
                List<SyntaxTrivia> triviaCache = new List<SyntaxTrivia>();
                bool hasGroups = false;

                foreach (SyntaxTrivia trivia in triviaList)
                {
                    triviaCache.Add(trivia);
                    ConvertedSyntaxKind localTriviaKind = trivia.Kind.ConvertToLocalType();

                    switch (localTriviaKind)
                    {
                        case ConvertedSyntaxKind.RegionDirectiveTrivia:
                            SyntaxNodesGroup<T> childGroup = new SyntaxNodesGroup<T>();
                            childGroup.LeadingTrivia = triviaCache;
                            group.AddGroup(childGroup);
                            group = childGroup;
                            triviaCache = new List<SyntaxTrivia>();
                            hasGroups = true;
                            break;
                        case ConvertedSyntaxKind.EndRegionDirectiveTrivia:
                            group.TrailingTrivia = triviaCache;
                            group = group.ParentGroup;
                            if (group == null)
                                return null;
                            triviaCache = new List<SyntaxTrivia>();
                            hasGroups = true;
                            break;
                        default:
#if BC
                            //do not sort if code contains other directives
                            if (trivia.IsDirective)
                                return null;
#endif
                            break;
                    }
                }

                if (hasGroups)
                    node = node.WithLeadingTrivia(triviaCache);
            }

#if BC
            if (node.HasOpenDirectives())
                return null;
#endif

            group.SyntaxNodes.Add(node);
            return group;
        }

        public (SyntaxNodesGroup<T>, bool, SyntaxToken, bool) ProcessClosingSyntaxTokenLeadingTrivias(SyntaxNodesGroup<T> group, SyntaxToken token)
        {
            bool hasGroups = false;
            bool tokenModified = false;
            SyntaxTriviaList syntaxTrivias = token.LeadingTrivia;

            if ((syntaxTrivias != null) && (syntaxTrivias.Count > 0))
            {
                //collect regions
                List<SyntaxTrivia> triviaCache = new List<SyntaxTrivia>();

                foreach (SyntaxTrivia trivia in syntaxTrivias)
                {
                    triviaCache.Add(trivia);
                    ConvertedSyntaxKind localTriviaKind = trivia.Kind.ConvertToLocalType();

                    switch (localTriviaKind)
                    {
                        case ConvertedSyntaxKind.RegionDirectiveTrivia:
                            SyntaxNodesGroup<T> childGroup = new SyntaxNodesGroup<T>();
                            childGroup.LeadingTrivia = triviaCache;
                            group.AddGroup(childGroup);
                            group = childGroup;
                            triviaCache = new List<SyntaxTrivia>();
                            hasGroups = true;
                            break;
                        case ConvertedSyntaxKind.EndRegionDirectiveTrivia:
                            group.TrailingTrivia = triviaCache;
                            group = group.ParentGroup;
                            if (group == null)
                                return (null, hasGroups, token, tokenModified);
                            triviaCache = new List<SyntaxTrivia>();
                            hasGroups = true;
                            break;
                        default:
#if BC
                            //do not sort if code contains other directives
                            if (trivia.IsDirective)
                                return (null, hasGroups, token, tokenModified);
#endif
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

        #endregion

        public SyntaxList<T> CreateSyntaxList()
        {
            List<T> nodesList = new List<T>();
            this.Root.GetSyntaxNodes(nodesList);
            return SyntaxFactory.List<T>(nodesList);
        }

        public SeparatedSyntaxList<T> CreateSeparatedSyntaxList()
        {
            List<T> nodesList = new List<T>();
            this.Root.GetSyntaxNodes(nodesList);
            SeparatedSyntaxList<T> separatedList = new SeparatedSyntaxList<T>();
            return separatedList.AddRange(nodesList);
        }


        public List<SyntaxNodesGroup<T>> GetAllGroups()
        {
            List<SyntaxNodesGroup<T>> list = new List<SyntaxNodesGroup<T>>();
            if (this.Root != null)
                this.Root.GetAllGroups(list);
            return list;
        }

        public bool SortSyntaxNodes(IComparer<T> comparer)
        {
            if (this.Root != null)
                return this.Root.SortSyntaxNodes(comparer);
            return false;
        }

        public bool SortSyntaxNodesWithTrivia(IComparer<T> comparer)
        {
            if (this.Root != null)
                return this.Root.SortSyntaxNodesWithTrivia(comparer);
            return false;
        }

        public bool SortSyntaxNodesWithSortInfo(IComparer<SyntaxNodeSortInfo<T>> comparer)
        {
            if (this.Root != null)
                return this.Root.SortSyntaxNodesWithSortInfo(comparer);
            return false;
        }

        public static SyntaxList<T> SortSyntaxList(SyntaxList<T> syntaxList, IComparer<T> comparer, bool sortSingleNodeRegions, out bool sorted)
        {
            sorted = false;

            if (syntaxList.Count < 2)
                return syntaxList;

            //build list with regions
            SyntaxNodesGroupsTree<T> nodesGroupsTree = new SyntaxNodesGroupsTree<T>();
            nodesGroupsTree.AddNodes(syntaxList, sortSingleNodeRegions);

            //somethis went wrong - do not sort
            if (nodesGroupsTree.Root == null)
                return syntaxList;

            //does not have any child groups
            if (!nodesGroupsTree.Root.HasChildGroups)
            {
                List<T> list = syntaxList.ToList();
                sorted = list.SortWithTrivia(comparer);
                return SyntaxFactory.List(list);
            }

            sorted = nodesGroupsTree.SortSyntaxNodesWithTrivia(comparer);
            return nodesGroupsTree.CreateSyntaxList();
        }

        public static SeparatedSyntaxList<T> SortSeparatedSyntaxList(SeparatedSyntaxList<T> syntaxList, IComparer<T> comparer, bool sortSingleNodeRegions, out bool sorted)
        {
            sorted = false;

            if (syntaxList.Count < 2)
                return syntaxList;

            //move NewLine characters to the front of node
            bool removeNewLineFromFirstNode = false;
            SyntaxTrivia newLineTrivia = SyntaxFactory.WhiteSpace("\r\n");
            List<T> updatedNodes = new List<T>();
            for (int i=0; i<syntaxList.Count; i++)
            {
                T node = syntaxList[i];
                //add crlf at the beginning
                SyntaxTriviaList leadingTrivias = node.GetLeadingTrivia();
                if ((leadingTrivias.Count == 0) || (leadingTrivias[0].Kind.ConvertToLocalType() != ConvertedSyntaxKind.EndOfLineTrivia))
                {
                    node = node.WithLeadingTrivia(leadingTrivias.Insert(0, newLineTrivia));
                    if (i == 0)
                        removeNewLineFromFirstNode = true;
                }
                //remove crlf from the end
                SyntaxTriviaList trailingTrivias = node.GetTrailingTrivia();
                bool updateTrailingTrivias = false;
                while ((trailingTrivias.Count > 0) && (trailingTrivias[trailingTrivias.Count - 1].Kind.ConvertToLocalType() == ConvertedSyntaxKind.EndOfLineTrivia))
                {
                    updateTrailingTrivias = true;
                    trailingTrivias = trailingTrivias.RemoveAt(trailingTrivias.Count - 1);
                }
                if (updateTrailingTrivias)
                    node = node.WithTrailingTrivia(trailingTrivias);
                updatedNodes.Add(node);
            }

            //build list with regions
            SyntaxNodesGroupsTree<T> nodesGroupsTree = new SyntaxNodesGroupsTree<T>();
            nodesGroupsTree.AddNodes(updatedNodes, sortSingleNodeRegions);

            //somethis went wrong - do not sort
            if (nodesGroupsTree.Root == null)
                return syntaxList;

            //does not have any child groups
            if (!nodesGroupsTree.Root.HasChildGroups)
            {
                List<T> list = updatedNodes;

                sorted = !list.IsOrdered(comparer);

                list.Sort(comparer);

                if (removeNewLineFromFirstNode)
                {
                    SyntaxTriviaList leadingTrivias = list[0].GetLeadingTrivia();
                    if (leadingTrivias.Count > 0)
                        list[0] = list[0].WithLeadingTrivia(leadingTrivias.RemoveAt(0));
                }

                SeparatedSyntaxList<T> newSyntaxList = new SeparatedSyntaxList<T>();
                return newSyntaxList.AddRange(list);
            }

            sorted = nodesGroupsTree.SortSyntaxNodes(comparer);
            return nodesGroupsTree.CreateSeparatedSyntaxList();
        }

        public static SyntaxList<T> SortSyntaxListWithSortInfo(SyntaxList<T> syntaxList, IComparer<SyntaxNodeSortInfo<T>> comparer, bool sortSingleNodeRegions, out bool sorted)
        {
            sorted = false;

            if (syntaxList.Count < 2)
                return syntaxList;

            //build list with regions
            SyntaxNodesGroupsTree<T> nodesGroupsTree = new SyntaxNodesGroupsTree<T>();
            nodesGroupsTree.AddNodes(syntaxList, sortSingleNodeRegions);

            //somethis went wrong - do not sort
            if (nodesGroupsTree.Root == null)
                return syntaxList;

            //does not have any child groups
            if (!nodesGroupsTree.Root.HasChildGroups)
            {
                List<SyntaxNodeSortInfo<T>> list =
                    SyntaxNodeSortInfo<T>.FromSyntaxList(syntaxList);

                for (int i=0; i<list.Count; i++)
                {
                    SyntaxTriviaList leadingTrivia = list[i].Node.GetLeadingTrivia();
                    SyntaxTriviaList trailingTrivia = list[i].Node.GetTrailingTrivia();
                }

                sorted = list.SortWithTrivia(comparer);
                return SyntaxNodeSortInfo<T>.ToSyntaxList(list);
            }

            sorted = nodesGroupsTree.SortSyntaxNodesWithSortInfo(comparer);
            return nodesGroupsTree.CreateSyntaxList();
        }


    }
}
