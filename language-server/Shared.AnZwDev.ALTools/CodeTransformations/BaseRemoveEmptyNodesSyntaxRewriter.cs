using AnZwDev.ALTools.CodeTransformations;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.Extensions;
using System.Linq;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class BaseRemoveEmptyNodesSyntaxRewriter : ALSyntaxRewriter
    {

        public bool IgnoreComments { get; set; }

        public BaseRemoveEmptyNodesSyntaxRewriter()
        {
        }

        protected (SyntaxList<T>, DirectiveTriviaMergedList remainingTrivia, bool) ProcessMembersList<T>(SyntaxList<T> sourceNodesCollection) where T : SyntaxNode
        {
            bool removed = false;
            List<T> newList = new List<T>();
            var mergedTrivia = new DirectiveTriviaMergedList();

            foreach (var sourceNode in sourceNodesCollection)
            {
                if (CanRemoveMember(sourceNode))
                {
                    removed = true;
                    CollectRemovedNodeTrivia(sourceNode, mergedTrivia);
                }
                else if (mergedTrivia.List.Count > 0)
                {
                    mergedTrivia.AddAllExceptClosingRegions(sourceNode.GetLeadingTrivia());
                    newList.Add(sourceNode.WithLeadingTrivia(mergedTrivia.List));
                    mergedTrivia.Clear();
                }
                else
                    newList.Add(sourceNode);
            }

            if (removed)
                return (SyntaxFactory.List(newList), mergedTrivia, true);
            return (sourceNodesCollection, null, false);
        }

        protected (T, DirectiveTriviaMergedList remainingTrivia, bool) ProcessChildNodes<T>(T node) where T : SyntaxNode
        {
#if BC
            bool removed = false;
            var mergedTrivia = new DirectiveTriviaMergedList();

            var childNodes = node.ChildNodes();
            if (childNodes != null)
                (node, removed) = ProcessChildNodes(node, mergedTrivia, childNodes, false);

            var members = node.GetObjectMembersEnumerable();
            if (members != null)
            {
                bool newRemoved;
                (node, newRemoved) = ProcessChildNodes(node, mergedTrivia, members, true);
                removed = removed || newRemoved;
            } 
            else
            {
                var triggers = node.GetNodeTriggersEnumerable();
                if (triggers != null)
                {
                    bool newRemoved;
                    (node, newRemoved) = ProcessChildNodes(node, mergedTrivia, triggers, true);
                    removed = removed || newRemoved;
                }
            }

            if (removed)
                return (node, mergedTrivia, true);
#endif
            return (node, null, false);
        }

#if BC
        protected (T, bool) ProcessChildNodes<T>(T node, DirectiveTriviaMergedList mergedTrivia, IEnumerable<SyntaxNode> childNodes, bool processMembers) where T : SyntaxNode
        {
            bool removed = false;
            foreach (var sourceNode in childNodes)
            {
                if ((processMembers) || (!(sourceNode is MemberSyntax)))
                {
                    if (CanRemoveMember(sourceNode))
                    {
                        removed = true;
                        CollectRemovedNodeTrivia(sourceNode, mergedTrivia);
                        node = node.RemoveNode(sourceNode, SyntaxRemoveOptions.KeepNoTrivia);
                    }
                    else if (mergedTrivia.List.Count > 0)
                    {
                        mergedTrivia.AddAllExceptClosingRegions(sourceNode.GetLeadingTrivia());
                        var newSourceNode = sourceNode.WithLeadingTrivia(mergedTrivia.List);
                        node = node.ReplaceNode(sourceNode, newSourceNode);
                        mergedTrivia.Clear();
                    }
                }
            }

            return (node, removed);
        }
#endif

        protected virtual bool CanRemoveMember(SyntaxNode node)
        {
            return false;
        }

        private void CollectRemovedNodeTrivia(SyntaxNode node, DirectiveTriviaMergedList mergedList)
        {
            mergedList.AddRange(node.GetLeadingTrivia());
            mergedList.AddRange(node.GetTrailingTrivia());
        }

        protected bool ContentIsNotEmpty(SyntaxToken? startToken, SyntaxToken? endToken)
        {
            return IgnoreComments ? ContentHasDirectives(startToken, endToken) : ContentHasNonEmptyTrivia(startToken, endToken);
        }

        protected bool ContentHasNonEmptyTrivia(SyntaxToken? startToken, SyntaxToken? endToken)
        {
            return
                ((startToken != null) && (!startToken.Value.TrailingTrivia.IsNullOrWhiteSpace())) ||
                ((endToken != null) && (!endToken.Value.LeadingTrivia.IsNullOrWhiteSpace()));
        }

        protected bool ContentHasDirectives(SyntaxToken? startToken, SyntaxToken? endToken)
        {
            return
                ((startToken != null) && (startToken.Value.TrailingTrivia.ContainsDirectives())) ||
                ((endToken != null) && (endToken.Value.LeadingTrivia.ContainsDirectives()));
        }

        protected bool HasNodes<T>(SyntaxList<T>? syntaxList) where T : SyntaxNode
        {
            return
                (syntaxList != null) &&
                (syntaxList.Value.Count > 0);
        }

        protected bool HasNodes<T>(SyntaxList<T> syntaxList) where T : SyntaxNode
        {
            return
                (syntaxList != null) &&
                (syntaxList.Count > 0);
        }

        protected bool HasNodes<T>(SeparatedSyntaxList<T> syntaxList) where T : SyntaxNode
        {
            return
                (syntaxList != null) &&
                (syntaxList.Count > 0);
        }

    }
}
