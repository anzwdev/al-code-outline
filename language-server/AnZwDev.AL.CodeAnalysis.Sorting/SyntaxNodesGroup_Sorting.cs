using AnZwDev.AL.CodeAnalysis.Sorting.Extensions;
using AnZwDev.System.Collections.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Sorting
{
    public partial class SyntaxNodesGroup<T> where T : SyntaxNode
    {

        public bool SortSyntaxNodes(IComparer<T> comparer)
        {
            bool sorted = false;

            if (this.SyntaxNodes.Count > 1)
            {
                sorted = !this.SyntaxNodes.IsOrdered(comparer);
                this.SyntaxNodes.Sort(comparer);
            }
            foreach (SyntaxNodesGroup<T> group in this.ChildGroups)
            {
                if (group.SortSyntaxNodes(comparer))
                    sorted = true;
            }

            return sorted;
        }

        public bool SortSyntaxNodesWithTrivia(IComparer<T> comparer)
        {
            bool sorted = false;

            if (this.SyntaxNodes.Count > 1)
            {
                sorted = this.SyntaxNodes.SortWithTrivia(comparer);
            }
            foreach (SyntaxNodesGroup<T> group in this.ChildGroups)
            {
                if (group.SortSyntaxNodesWithTrivia(comparer))
                    sorted = true;
            }

            return sorted;
        }

        public bool SortSyntaxNodesWithSortInfo(IComparer<SyntaxNodeSortInfo<T>> comparer)
        {
            bool sorted = false;

            if (this.SyntaxNodes.Count > 1)
            {
                List<SyntaxNodeSortInfo<T>> list = SyntaxNodeSortInfo<T>.FromNodesList(this.SyntaxNodes);
                sorted = list.SortWithTrivia(comparer);
                this.SyntaxNodes = SyntaxNodeSortInfo<T>.ToNodesList(list);
            }

            foreach (SyntaxNodesGroup<T> group in this.ChildGroups)
            {
                if (group.SortSyntaxNodesWithSortInfo(comparer))
                    sorted = true;
            }

            return sorted;
        }


    }
}
