using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RemoveEmptySectionsSyntaxRewriter : BaseRemoveEmptyNodesSyntaxRewriter
    {

        public bool RemovePageFieldGroups { get; set; }
        public bool RemoveActionGroups { get; set; }
        public bool RemoveActions { get; set; }

        public RemoveEmptySectionsSyntaxRewriter()
        {
            this.IgnoreComments = true;
        }

        #region Visit main objects

        public override SyntaxNode VisitTable(TableSyntax node)
        {
            var newNode = base.VisitTable(node);
            node = newNode as TableSyntax;
            if (node == null)
                return newNode;

            (var newProcessedNode, var remainingTrivia, var updated) = ProcessChildNodes(node);
            if ((updated) &&
                (remainingTrivia != null) && 
                (remainingTrivia.List.Count > 0) && 
                (!newProcessedNode.CloseBraceToken.IsEmpty()))
            {
                remainingTrivia.AddAllExceptClosingRegions(newProcessedNode.CloseBraceToken.LeadingTrivia);
                newProcessedNode = newProcessedNode.WithCloseBraceToken(
                    newProcessedNode.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
            }

            return newProcessedNode;
        }

        public override SyntaxNode VisitTableExtension(TableExtensionSyntax node)
        {
            var newNode = base.VisitTableExtension(node);
            node = newNode as TableExtensionSyntax;
            if (node == null)
                return newNode;

            (var newProcessedNode, var remainingTrivia, var updated) = ProcessChildNodes(node);
            if ((updated) &&
                (remainingTrivia != null) &&
                (remainingTrivia.List.Count > 0) &&
                (!newProcessedNode.CloseBraceToken.IsEmpty()))
            {
                remainingTrivia.AddAllExceptClosingRegions(newProcessedNode.CloseBraceToken.LeadingTrivia);
                newProcessedNode = newProcessedNode.WithCloseBraceToken(
                    newProcessedNode.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
            }

            return newProcessedNode;
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            var newNode = base.VisitPage(node);
            node = newNode as PageSyntax;
            if (node == null)
                return newNode;

            (var newProcessedNode, var remainingTrivia, var updated) = ProcessChildNodes(node);
            if ((updated) &&
                (remainingTrivia != null) &&
                (remainingTrivia.List.Count > 0) &&
                (!newProcessedNode.CloseBraceToken.IsEmpty()))
            {
                remainingTrivia.AddAllExceptClosingRegions(newProcessedNode.CloseBraceToken.LeadingTrivia);
                newProcessedNode = newProcessedNode.WithCloseBraceToken(
                    newProcessedNode.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
            }

            return newProcessedNode;
        }

        public override SyntaxNode VisitPageExtension(PageExtensionSyntax node)
        {
            var newNode = base.VisitPageExtension(node);
            node = newNode as PageExtensionSyntax;
            if (node == null)
                return newNode;

            (var newProcessedNode, var remainingTrivia, var updated) = ProcessChildNodes(node);
            if ((updated) &&
                (remainingTrivia != null) &&
                (remainingTrivia.List.Count > 0) &&
                (!newProcessedNode.CloseBraceToken.IsEmpty()))
            {
                remainingTrivia.AddAllExceptClosingRegions(newProcessedNode.CloseBraceToken.LeadingTrivia);
                newProcessedNode = newProcessedNode.WithCloseBraceToken(
                    newProcessedNode.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
            }

            return newProcessedNode;
        }

        public override SyntaxNode VisitReport(ReportSyntax node)
        {
            var newNode = base.VisitReport(node);
            node = newNode as ReportSyntax;
            if (node == null)
                return newNode;

            (var newProcessedNode, var remainingTrivia, var updated) = ProcessChildNodes(node);
            if ((updated) &&
                (remainingTrivia != null) &&
                (remainingTrivia.List.Count > 0) &&
                (!newProcessedNode.CloseBraceToken.IsEmpty()))
            {
                remainingTrivia.AddAllExceptClosingRegions(newProcessedNode.CloseBraceToken.LeadingTrivia);
                newProcessedNode = newProcessedNode.WithCloseBraceToken(
                    newProcessedNode.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
            }

            return newProcessedNode;
        }

#if BC

        public override SyntaxNode VisitReportExtension(ReportExtensionSyntax node)
        {
            var newNode = base.VisitReportExtension(node);
            node = newNode as ReportExtensionSyntax;
            if (node == null)
                return newNode;

            (var newProcessedNode, var remainingTrivia, var updated) = ProcessChildNodes(node);
            if ((updated) &&
                (remainingTrivia != null) &&
                (remainingTrivia.List.Count > 0) &&
                (!newProcessedNode.CloseBraceToken.IsEmpty()))
            {
                remainingTrivia.AddAllExceptClosingRegions(newProcessedNode.CloseBraceToken.LeadingTrivia);
                newProcessedNode = newProcessedNode.WithCloseBraceToken(
                    newProcessedNode.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
            }

            return newProcessedNode;
        }

#endif

        #endregion

        public override SyntaxNode VisitRequestPage(RequestPageSyntax node)
        {
            var newNode = base.VisitRequestPage(node);
            node = newNode as RequestPageSyntax;
            if (node == null)
                return newNode;

            (var newProcessedNode, var remainingTrivia, var updated) = ProcessChildNodes(node);
            if ((updated) &&
                (remainingTrivia != null) &&
                (remainingTrivia.List.Count > 0) &&
                (!newProcessedNode.CloseBraceToken.IsEmpty()))
            {
                remainingTrivia.AddAllExceptClosingRegions(newProcessedNode.CloseBraceToken.LeadingTrivia);
                newProcessedNode = newProcessedNode.WithCloseBraceToken(
                    newProcessedNode.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
            }

            return newProcessedNode;
        }

        #region Visit page elements

        public override SyntaxNode VisitPageLayout(PageLayoutSyntax node)
        {
            var newNode = base.VisitPageLayout(node);
            node = newNode as PageLayoutSyntax;
            if (node == null)
                return newNode;

            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Areas);
            if (updated)
            {
                node = node.WithAreas(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return node;
        }

        public override SyntaxNode VisitPageArea(PageAreaSyntax node)
        {
            var newNode = base.VisitPageArea(node);
            node = newNode as PageAreaSyntax;
            if (node == null)
                return newNode;

            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Controls);
            if (updated)
            {
                node = node.WithControls(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return node;
        }

        public override SyntaxNode VisitPageGroup(PageGroupSyntax node)
        {
            var newNode = base.VisitPageGroup(node);
            node = newNode as PageGroupSyntax;
            if (node == null)
                return newNode;

            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Controls);
            if (updated)
            {
                node = node.WithControls(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return node;
        }

        public override SyntaxNode VisitPageActionList(PageActionListSyntax node)
        {
            var newNode = base.VisitPageActionList(node);
            node = newNode as PageActionListSyntax;
            if (node == null)
                return newNode;

            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Areas);
            if (updated)
            {
                node = node.WithAreas(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return node;
        }

        public override SyntaxNode VisitPageActionArea(PageActionAreaSyntax node)
        {
            var newNode = base.VisitPageActionArea(node);
            node = newNode as PageActionAreaSyntax;
            if (node == null)
                return newNode;

            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Actions);
            if (updated)
            {
                node = node.WithActions(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return node;
        }

        public override SyntaxNode VisitPageActionGroup(PageActionGroupSyntax node)
        {
            var newNode = base.VisitPageActionGroup(node);
            node = newNode as PageActionGroupSyntax;
            if (node == null)
                return newNode;

            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Actions);
            if (updated)
            {
                node = node.WithActions(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return node;
        }

        #endregion

        #region Visit extensions elements

        public override SyntaxNode VisitPageExtensionLayout(PageExtensionLayoutSyntax node)
        {
            var newNode = base.VisitPageExtensionLayout(node);
            node = newNode as PageExtensionLayoutSyntax;
            if (node == null)
                return newNode;

            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Changes);
            if (updated)
            {
                node = node.WithChanges(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return node;
        }


        public override SyntaxNode VisitPageExtensionActionList(PageExtensionActionListSyntax node)
        {
            var newNode = base.VisitPageExtensionActionList(node);
            node = newNode as PageExtensionActionListSyntax;
            if (node == null)
                return newNode;

            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Changes);
            if (updated)
            {
                node = node.WithChanges(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return node;
        }

        #endregion

        private bool CanDeleteNodeTrivia(SyntaxNode node)
        {
            return
                IgnoreComments ||
                (!node.HasNonEmptyTriviaInside());
        }

        protected override bool CanRemoveMember(SyntaxNode node)
        {
            switch (node)
            {
                case RequestPageSyntax requestPageSyntax:
                    return
                        (!HasNodes(requestPageSyntax.PropertyList?.Properties)) &&
                        (!HasNodes(requestPageSyntax.Layout?.Areas)) &&
                        (!HasNodes(requestPageSyntax.Actions?.Areas)) &&
                        (!HasNodes(requestPageSyntax.Members)) &&
                        (!ContentIsNotEmpty(requestPageSyntax.OpenBraceToken, requestPageSyntax.CloseBraceToken));
#if BC
                case RequestPageExtensionSyntax requestPageExtensionSyntax:
                    return
                        (!HasNodes(requestPageExtensionSyntax.PropertyList?.Properties)) &&
                        (!HasNodes(requestPageExtensionSyntax.Layout?.Changes)) &&
                        (!HasNodes(requestPageExtensionSyntax.Actions?.Changes)) &&
                        (!HasNodes(requestPageExtensionSyntax.Members)) &&
                        (!ContentIsNotEmpty(requestPageExtensionSyntax.OpenBraceToken, requestPageExtensionSyntax.CloseBraceToken));
#endif
                case PageLayoutSyntax pageLayoutSyntax:
                    return
                        (!HasNodes(pageLayoutSyntax.Areas)) && 
                        (!ContentIsNotEmpty(pageLayoutSyntax.OpenBraceToken, pageLayoutSyntax.CloseBraceToken));
                case PageAreaSyntax pageAreaSyntax:
                    return
                        (!HasNodes(pageAreaSyntax.Controls)) &&
                        (!ContentIsNotEmpty(pageAreaSyntax.OpenBraceToken, pageAreaSyntax.CloseBraceToken));
                case PageGroupSyntax pageGroupSyntax:
                    return
                        (RemovePageFieldGroups) &&
                        (!HasNodes(pageGroupSyntax.Controls)) &&
                        (!HasNodes(pageGroupSyntax.PropertyList?.Properties)) &&
                        (!ContentIsNotEmpty(pageGroupSyntax.OpenBraceToken, pageGroupSyntax.CloseBraceToken));
                case PageExtensionLayoutSyntax pageExtensionLayoutSyntax:
                    return
                        (!HasNodes(pageExtensionLayoutSyntax.Changes)) &&
                        (!ContentIsNotEmpty(pageExtensionLayoutSyntax.OpenBraceToken, pageExtensionLayoutSyntax.CloseBraceToken));

                case PageActionListSyntax pageActionListSyntax:
                    return
                        (!HasNodes(pageActionListSyntax.Areas)) &&
                        (!ContentIsNotEmpty(pageActionListSyntax.OpenBraceToken, pageActionListSyntax.CloseBraceToken));
                case PageActionAreaSyntax pageActionAreaSyntax:
                    return
                        (!HasNodes(pageActionAreaSyntax.Actions)) &&
                        (!ContentIsNotEmpty(pageActionAreaSyntax.OpenBraceToken, pageActionAreaSyntax.CloseBraceToken));
                case PageActionGroupSyntax pageActionGroupSyntax:
                    return
                        (RemoveActionGroups) &&
                        (!HasNodes(pageActionGroupSyntax.Actions)) &&
                        (!ContentIsNotEmpty(pageActionGroupSyntax.OpenBraceToken, pageActionGroupSyntax.CloseBraceToken));
                case PageActionSyntax pageActionSyntax:
                    return
                        (RemoveActions) &&
                        (!HasNodes(pageActionSyntax.PropertyList?.Properties)) &&
                        (!HasNodes(pageActionSyntax.Triggers)) &&
                        (!ContentIsNotEmpty(pageActionSyntax.OpenBraceToken, pageActionSyntax.CloseBraceToken));

                case PageExtensionActionListSyntax pageExtensionActionListSyntax:
                    return
                        (!HasNodes(pageExtensionActionListSyntax.Changes)) &&
                        (!ContentIsNotEmpty(pageExtensionActionListSyntax.OpenBraceToken, pageExtensionActionListSyntax.CloseBraceToken));

                case KeyListSyntax keyListSyntax:
                    return
                        (!HasNodes(keyListSyntax.Keys)) &&
                        (!ContentIsNotEmpty(keyListSyntax.OpenBraceToken, keyListSyntax.CloseBraceToken));
                case FieldGroupListSyntax fieldGroupListSyntax:
                    return
                        (!HasNodes(fieldGroupListSyntax.FieldGroups)) &&
                        (!ContentIsNotEmpty(fieldGroupListSyntax.OpenBraceToken, fieldGroupListSyntax.CloseBraceToken));

            }
            return base.CanRemoveMember(node);
        }

    }
}
