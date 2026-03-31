using AnZwDev.AL.CodeAnalysis.Sorting.Extensions;
using AnZwDev.System.Collections.Extensions;
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

        public bool SortSyntaxNodes(IComparer<T> comparer)
        {
            if (Root != null)
                return Root.SortSyntaxNodes(comparer);
            return false;
        }

        public bool SortSyntaxNodesWithTrivia(IComparer<T> comparer)
        {
            if (Root != null)
                return Root.SortSyntaxNodesWithTrivia(comparer);
            return false;
        }

        public bool SortSyntaxNodesWithSortInfo(IComparer<SyntaxNodeSortInfo<T>> comparer)
        {
            if (Root != null)
                return Root.SortSyntaxNodesWithSortInfo(comparer);
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
            for (int i = 0; i < syntaxList.Count; i++)
            {
                T node = syntaxList[i];
                //add crlf at the beginning
                SyntaxTriviaList leadingTrivias = node.GetLeadingTrivia();
                if ((leadingTrivias.Count == 0) || (leadingTrivias[0].Kind != SyntaxKind.EndOfLineTrivia))
                {
                    node = node.WithLeadingTrivia(leadingTrivias.Insert(0, newLineTrivia));
                    if (i == 0)
                        removeNewLineFromFirstNode = true;
                }
                //remove crlf from the end
                SyntaxTriviaList trailingTrivias = node.GetTrailingTrivia();
                bool updateTrailingTrivias = false;
                while ((trailingTrivias.Count > 0) && (trailingTrivias[trailingTrivias.Count - 1].Kind == SyntaxKind.EndOfLineTrivia))
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

                for (int i = 0; i < list.Count; i++)
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
