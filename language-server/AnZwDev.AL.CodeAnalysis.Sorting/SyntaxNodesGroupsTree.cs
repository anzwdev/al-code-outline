using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Xml.Linq;
using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.CodeAnalysis.Sorting.Extensions;

namespace AnZwDev.AL.CodeAnalysis.Sorting
{
    public partial class SyntaxNodesGroupsTree<T> where T: SyntaxNode
    {

        public SyntaxNodesGroup<T>? Root { get; set; } = null;

        public SyntaxNodesGroupsTree()
        {
        }

        public SyntaxList<T> CreateSyntaxList()
        {
            List<T> nodesList = new List<T>();
            Root?.GetSyntaxNodes(nodesList);
            return SyntaxFactory.List<T>(nodesList);
        }

        public SeparatedSyntaxList<T> CreateSeparatedSyntaxList()
        {
            List<T> nodesList = new List<T>();
            Root?.GetSyntaxNodes(nodesList);
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

    }
}
