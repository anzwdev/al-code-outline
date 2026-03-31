using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using AnZwDev.AL.CodeAnalysis.Extensions;

namespace AnZwDev.AL.CodeAnalysis.Sorting
{
    public partial class SyntaxNodesGroup<T> where T: SyntaxNode
    {

        public SyntaxNodesGroup<T>? ParentGroup { get; set; } = null;
        public List<SyntaxNodesGroup<T>> ChildGroups { get; } = new List<SyntaxNodesGroup<T>>();
        public List<T> SyntaxNodes { get; set; } = new List<T>();
        public List<SyntaxTrivia>? LeadingTrivia { get; set; } = null;
        public List<SyntaxTrivia>? TrailingTrivia { get; set; } = null;

        public bool HasChildGroups
        {
            get { return (this.ChildGroups.Count > 0); }
        }

        public SyntaxNodesGroup()
        {
        }

        public void AddGroup(SyntaxNodesGroup<T> group)
        {
            this.ChildGroups.Add(group);
            group.ParentGroup = this;
        }

        public void RemoveSingleNodeGroups()
        {
            if ((this.ChildGroups == null) || (this.ChildGroups.Count == 0))
                return;

            var idx = 0;
            while (idx < this.ChildGroups.Count)
            {
                var group = this.ChildGroups[idx];
                if ((group.SyntaxNodes != null) && (group.SyntaxNodes.Count == 1))
                {
                    var syntaxNode = group.SyntaxNodes[0];
                    if (group.LeadingTrivia != null)
                        syntaxNode = syntaxNode.WithLeadingLeadingTrivia(group.LeadingTrivia);
                    if (group.TrailingTrivia != null)
                        syntaxNode = syntaxNode.WithTrailingTrailingTrivia(group.TrailingTrivia);
                    this.SyntaxNodes.Add(syntaxNode);
                    this.ChildGroups.RemoveAt(idx);
                }
                else
                    idx++;
            }

            for (int i = 0; i < this.ChildGroups.Count; i++)
                this.ChildGroups[i].RemoveSingleNodeGroups();
        }

        public void GetSyntaxNodes(List<T> list)
        {
            List<SyntaxTrivia> triviaList = new List<SyntaxTrivia>();
            this.GetSyntaxNodes(list, triviaList);

            //add remaining trivia to the trailing trivia list of the last node
            if (triviaList.Count > 0)
            {
                T syntaxNode = list[list.Count - 1];
                SyntaxTriviaList nodeTrivia = syntaxNode.GetTrailingTrivia();
                if (nodeTrivia.Count > 0)
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
                    if (nodeTrivia.Count > 0)
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

        public void GetAllGroups(List<SyntaxNodesGroup<T>> list)
        {
            list.Add(this);
            foreach (SyntaxNodesGroup<T> childGroup in this.ChildGroups)
            {
                childGroup.GetAllGroups(list);
            }
        }

    }
}
