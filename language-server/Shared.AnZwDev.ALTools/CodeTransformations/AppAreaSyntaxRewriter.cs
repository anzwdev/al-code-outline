using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class AppAreaSyntaxRewriter : ALSyntaxRewriter
    {

        public string ApplicationAreaName { get; set; } = null;
        public AppAreaMode ApplicationAreaMode { get; set; } = AppAreaMode.addToAllControls;
        private bool _childControlsCanInheritAppAreas = false;

        public AppAreaSyntaxRewriter()
        {
        }

        protected override SyntaxNode AfterVisitNode(SyntaxNode node)
        {
            if (this.NoOfChanges == 0)
                return null;
            return base.AfterVisitNode(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            PropertySyntax usageCategoryProperty = node.GetProperty("UsageCategory");
            bool hasUsageCategory = ((usageCategoryProperty != null) && (usageCategoryProperty.Value != null));
            if (hasUsageCategory)
            {
                string usageCategory = ALSyntaxHelper.DecodeName(usageCategoryProperty.Value.ToString());
                hasUsageCategory = ((!String.IsNullOrWhiteSpace(usageCategory)) && (usageCategory.ToLower() != "none"));
            }

            if ((
                    (hasUsageCategory) || 
                    (ApplicationAreaMode == AppAreaMode.inheritFromMainObject)
                ) && 
                (!this.HasApplicationArea(node)))
            {
                NoOfChanges++;
                node = node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
            }

            _childControlsCanInheritAppAreas = true;
            var newNode = base.VisitPage(node);
            _childControlsCanInheritAppAreas = false;
            
            return newNode;
        }

        public override SyntaxNode VisitReport(ReportSyntax node)
        {
            PropertySyntax usageCategoryProperty = node.GetProperty("UsageCategory");
            bool hasUsageCategory = ((usageCategoryProperty != null) && (usageCategoryProperty.Value != null));
            if (hasUsageCategory)
            {
                string usageCategory = ALSyntaxHelper.DecodeName(usageCategoryProperty.Value.ToString());
                hasUsageCategory = ((!String.IsNullOrWhiteSpace(usageCategory)) && (usageCategory.ToLower() != "none"));
            }

            if ((
                    (hasUsageCategory) ||
                    (ApplicationAreaMode == AppAreaMode.inheritFromMainObject)
                ) &&
                (!this.HasApplicationArea(node)))
            {
                NoOfChanges++;
                node = node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
            }

            //!!! Do not inherit application areas on report request pages
            //_childControlsCanInheritAppAreas = true;
            var newNode = base.VisitReport(node);
            //_childControlsCanInheritAppAreas = false;

            return newNode;
        }

        public override SyntaxNode VisitPageLabel(PageLabelSyntax node)
        {
            if ((InheritApplicationArea(node)) || (this.HasApplicationArea(node)))
                return base.VisitPageLabel(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
        }

        public override SyntaxNode VisitPageField(PageFieldSyntax node)
        {
            if ((InheritApplicationArea(node)) || (this.HasApplicationArea(node)))
                return base.VisitPageField(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
        }

        public override SyntaxNode VisitPageUserControl(PageUserControlSyntax node)
        {
            if ((InheritApplicationArea(node)) || (this.HasApplicationArea(node)))
                return base.VisitPageUserControl(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
        }

        public override SyntaxNode VisitPagePart(PagePartSyntax node)
        {
            if ((InheritApplicationArea(node)) || (this.HasApplicationArea(node)))
                return base.VisitPagePart(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
        }

        public override SyntaxNode VisitPageSystemPart(PageSystemPartSyntax node)
        {
            if ((InheritApplicationArea(node)) || (this.HasApplicationArea(node)))
                return base.VisitPageSystemPart(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
        }

#if BC
        public override SyntaxNode VisitPageChartPart(PageChartPartSyntax node)
        {
            if ((InheritApplicationArea(node)) || (this.HasApplicationArea(node)))
                return base.VisitPageChartPart(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
        }
#endif

        public override SyntaxNode VisitPageAction(PageActionSyntax node)
        {
            if ((InheritApplicationArea(node)) || (this.HasApplicationArea(node)))
                return base.VisitPageAction(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
        }

        protected bool HasApplicationArea(SyntaxNode node)
        {
            return node.HasNonEmptyProperty("ApplicationArea");
        }

        protected bool InheritApplicationArea(SyntaxNode node)
        {
            return
                (ApplicationAreaMode == AppAreaMode.inheritFromMainObject) &&
                (_childControlsCanInheritAppAreas);
        }

        protected PropertySyntax CreateApplicationAreaProperty(SyntaxNode node)
        {
            //calculate indent
            int indentLength = 4;
            string indent;
            SyntaxTriviaList leadingTrivia = node.GetLeadingTrivia();
            if (leadingTrivia != null)
            {
                indent = leadingTrivia.ToString();
                int newLinePos = indent.LastIndexOf("/n");
                if (newLinePos >= 0)
                    indent = indent.Substring(newLinePos + 1);
                indentLength += indent.Length;
            }
            indent = "".PadLeft(indentLength);

            SyntaxTriviaList leadingTriviaList = SyntaxFactory.ParseLeadingTrivia(indent, 0);
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);

            SeparatedSyntaxList<IdentifierNameSyntax> values = new SeparatedSyntaxList<IdentifierNameSyntax>();
            values = values.Add(SyntaxFactory.IdentifierName(this.ApplicationAreaName));

            //try to convert from string to avoid issues with enum ids changed between AL compiler versions
            PropertyKind propertyKind;
            try
            {
                propertyKind = (PropertyKind)Enum.Parse(typeof(PropertyKind), "ApplicationArea", true);
            }
            catch (Exception)
            {
                propertyKind = PropertyKind.ApplicationArea;
            }

            return SyntaxFactory.Property(propertyKind, SyntaxFactory.CommaSeparatedPropertyValue(values))
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }

    }
}
