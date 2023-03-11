using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RemoveEmptyTriggersSyntaxRewriter : BaseRemoveEmptyNodesSyntaxRewriter
    {

        public bool RemoveTriggers { get; set; }
        public bool RemoveSubscribers { get; set; }

        public RemoveEmptyTriggersSyntaxRewriter()
        {
        }

        #region Visit objects declaration members

        public override SyntaxNode VisitTable(TableSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Members);
            if (updated)
            {
                node = node.WithMembers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitTable(node);
        }

        public override SyntaxNode VisitTableExtension(TableExtensionSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Members);
            if (updated)
            {
                node = node.WithMembers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitTableExtension(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Members);
            if (updated)
            {
                node = node.WithMembers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitPage(node);
        }

        public override SyntaxNode VisitPageExtension(PageExtensionSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Members);
            if (updated)
            {
                node = node.WithMembers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitPageExtension(node);
        }

        public override SyntaxNode VisitReport(ReportSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Members);
            if (updated)
            {
                node = node.WithMembers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitReport(node);
        }

#if BC
        public override SyntaxNode VisitReportExtension(ReportExtensionSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Members);
            if (updated)
            {
                node = node.WithMembers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitReportExtension(node);
        }
#endif

        public override SyntaxNode VisitQuery(QuerySyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Members);
            if (updated)
            {
                node = node.WithMembers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitQuery(node);
        }

        public override SyntaxNode VisitCodeunit(CodeunitSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Members);
            if (updated)
            {
                node = node.WithMembers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitCodeunit(node);
        }

        public override SyntaxNode VisitXmlPort(XmlPortSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Members);
            if (updated)
            {
                node = node.WithMembers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitXmlPort(node);
        }

        #endregion

        #region Visit nodes with triggers

        #region Tables and Table Extensions

        public override SyntaxNode VisitFieldModification(FieldModificationSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Triggers);
            if (updated)
            {
                node = node.WithTriggers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitFieldModification(node);
        }

        public override SyntaxNode VisitField(FieldSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Triggers);
            if (updated)
            {
                node = node.WithTriggers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitField(node);
        }

        #endregion

        #region Pages and Page Extensions

        public override SyntaxNode VisitActionModifyChange(ActionModifyChangeSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Triggers);
            if (updated)
            {
                node = node.WithTriggers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitActionModifyChange(node);
        }

        public override SyntaxNode VisitControlModifyChange(ControlModifyChangeSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Triggers);
            if (updated)
            {
                node = node.WithTriggers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitControlModifyChange(node);
        }

        public override SyntaxNode VisitPageField(PageFieldSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Triggers);
            if (updated)
            {
                node = node.WithTriggers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitPageField(node);
        }

        public override SyntaxNode VisitPageAction(PageActionSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Triggers);
            if (updated)
            {
                node = node.WithTriggers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitPageAction(node);
        }

        public override SyntaxNode VisitPageUserControl(PageUserControlSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Triggers);
            if (updated)
            {
                node = node.WithTriggers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitPageUserControl(node);
        }

        #endregion

        #region XmlPorts

        public override SyntaxNode VisitXmlPortFieldAttribute(XmlPortFieldAttributeSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Triggers);
            if (updated)
            {
                node = node.WithTriggers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitXmlPortFieldAttribute(node);
        }

        public override SyntaxNode VisitXmlPortFieldElement(XmlPortFieldElementSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Triggers);
            if (updated)
            {
                node = node.WithTriggers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitXmlPortFieldElement(node);
        }

        public override SyntaxNode VisitXmlPortTableElement(XmlPortTableElementSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Triggers);
            if (updated)
            {
                node = node.WithTriggers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitXmlPortTableElement(node);
        }

        public override SyntaxNode VisitXmlPortTextAttribute(XmlPortTextAttributeSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Triggers);
            if (updated)
            {
                node = node.WithTriggers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitXmlPortTextAttribute(node);
        }

        public override SyntaxNode VisitXmlPortTextElement(XmlPortTextElementSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Triggers);
            if (updated)
            {
                node = node.WithTriggers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitXmlPortTextElement(node);
        }

        #endregion

        #region Reports and Report Extensions

        public override SyntaxNode VisitReportDataItem(ReportDataItemSyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Triggers);
            if (updated)
            {
                node = node.WithTriggers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitReportDataItem(node);
        }

#if BC
        public override SyntaxNode VisitReportExtensionDataSetModify(ReportExtensionDataSetModifySyntax node)
        {
            (var newMembers, var remainingTrivia, var updated) = ProcessMembersList(node.Triggers);
            if (updated)
            {
                node = node.WithTriggers(newMembers);
                if ((remainingTrivia != null) && (remainingTrivia.List.Count > 0) && (!node.CloseBraceToken.IsEmpty()))
                {
                    remainingTrivia.AddAllExceptClosingRegions(node.CloseBraceToken.LeadingTrivia);
                    node = node.WithCloseBraceToken(
                        node.CloseBraceToken.WithLeadingTrivia(remainingTrivia.List));
                }
            }
            return base.VisitReportExtensionDataSetModify(node);
        }
#endif

        #endregion

        #endregion

        protected override bool CanRemoveMember(SyntaxNode node)
        {
            switch (node)
            {
                case TriggerDeclarationSyntax triggerSyntax:
                    return (RemoveTriggers && IsEmptyMethod(triggerSyntax));
                case MethodDeclarationSyntax methodSyntax:
                    return (RemoveSubscribers && IsEmptyMethod(methodSyntax) && (methodSyntax.IsEventSubscriber()));
            }
            return base.CanRemoveMember(node);
        }

        private bool IsEmptyMethod(MethodOrTriggerDeclarationSyntax syntax)
        {
            bool hasTrivia = ContentIsNotEmpty(syntax.Body?.BeginKeywordToken, syntax.Body?.EndKeywordToken);
            bool hasStatements = HasNodes(syntax.Body?.Statements);
            //bool hasDirectivesOutside = syntax.GetLeadingTrivia().ContainsDirectives() || syntax.GetTrailingTrivia().ContainsDirectives();

            return
                (!hasTrivia) && (!hasStatements);// && (!hasDirectivesOutside);
        }

    }
}
