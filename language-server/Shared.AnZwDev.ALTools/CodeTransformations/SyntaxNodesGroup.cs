using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SyntaxNodesGroup<T> where T: SyntaxNode
    {
        #region Public properties

        public SyntaxNodesGroup<T> ParentGroup { get; set; }
        public List<SyntaxNodesGroup<T>> ChildGroups { get; }
        public List<T> SyntaxNodes { get; set; }
        public List<SyntaxTrivia> LeadingTrivia { get; set; }
        public List<SyntaxTrivia> TrailingTrivia { get; set; }

        public bool HasChildGroups
        {
            get { return (this.ChildGroups.Count > 0); }
        }

        #endregion

        #region Initialization

        public SyntaxNodesGroup()
        {
            this.ParentGroup = null;
            this.LeadingTrivia = null;
            this.TrailingTrivia = null;
            this.ChildGroups = new List<SyntaxNodesGroup<T>>();
            this.SyntaxNodes = new List<T>();
        }

        #endregion

        public void AddGroup(SyntaxNodesGroup<T> group)
        {
            this.ChildGroups.Add(group);
            group.ParentGroup = this;
        }

        #region Get syntax nodes

        public void GetSyntaxNodes(List<T> list)
        {
            List<SyntaxTrivia> triviaList = new List<SyntaxTrivia>();
            this.GetSyntaxNodes(list, triviaList);

            //add remaining trivia to the trailing trivia list of the last node
            if (triviaList.Count > 0)
            {
                T syntaxNode = list[list.Count - 1];
                SyntaxTriviaList nodeTrivia = syntaxNode.GetTrailingTrivia();
                if ((nodeTrivia != null) && (nodeTrivia.Count > 0))
                    triviaList.InsertRange(0, nodeTrivia);
                list[list.Count - 1] = syntaxNode.WithTrailingTrivia(triviaList);
            }
        }

        public void GetSyntaxNodes(List<T> list, List<SyntaxTrivia> triviaList)
        {
            if ((this.LeadingTrivia != null) && (this.LeadingTrivia.Count > 0))
                triviaList.AddRange(this.LeadingTrivia);
            
            //add nodes
            for (int i=0; i<this.SyntaxNodes.Count; i++)
            {
                T syntaxNode = this.SyntaxNodes[i];
                //add leading trivia to the syntax node
                if (triviaList.Count > 0)
                {
                    SyntaxTriviaList nodeTrivia = syntaxNode.GetLeadingTrivia();
                    if ((nodeTrivia != null) && (nodeTrivia.Count > 0))
                        triviaList.AddRange(nodeTrivia);
                    syntaxNode = syntaxNode.WithLeadingTrivia(triviaList);
                    triviaList.Clear();
                }
                //add syntax node to the list
                list.Add(syntaxNode);
            }

            //add groups
            foreach (SyntaxNodesGroup<T> childGroup in this.ChildGroups)
            {
                childGroup.GetSyntaxNodes(list, triviaList);
            }

            //add trailing trivia to the trivia list
            if ((this.TrailingTrivia != null) && (this.TrailingTrivia.Count > 0))
            {
                triviaList.AddRange(this.TrailingTrivia);
            }
        }

        #endregion

        #region Get all groups 

        public void GetAllGroups(List<SyntaxNodesGroup<T>> list)
        {
            list.Add(this);
            foreach (SyntaxNodesGroup<T> childGroup in this.ChildGroups)
            {
                childGroup.GetAllGroups(list);
            }
        }

        #endregion


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
