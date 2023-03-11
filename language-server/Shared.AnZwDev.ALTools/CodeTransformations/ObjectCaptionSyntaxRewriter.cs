using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class ObjectCaptionSyntaxRewriter : ALCaptionsSyntaxRewriter
    {

        public ObjectCaptionSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitTable(TableSyntax node)
        {
            if (!node.HasProperty("CaptionML"))
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                if (propertySyntax == null)
                {
                    NoOfChanges++;
                    node = node.AddPropertyListProperties(
                        this.CreateCaptionPropertyFromName(node, false));
                }
                else
                    node = UpdateCaptionFromName(node, propertySyntax, false);
            }
            return base.VisitTable(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            //check page type
            string pageType = ALSyntaxHelper.DecodeName(node.GetProperty("PageType")?.Value?.ToString());
            if ((pageType == null) || (!pageType.Equals("API", StringComparison.CurrentCultureIgnoreCase)))
            {
                if (!node.HasProperty("CaptionML"))
                {
                    PropertySyntax propertySyntax = node.GetProperty("Caption");
                    if (propertySyntax == null)
                    {
                        NoOfChanges++;
                        node = node.AddPropertyListProperties(
                            this.CreateCaptionPropertyFromName(node, false));
                    }
                    else
                        node = UpdateCaptionFromName(node, propertySyntax, false);
                }
            }
            return base.VisitPage(node);
        }

        public override SyntaxNode VisitReport(ReportSyntax node)
        {
            if (!node.HasProperty("CaptionML"))
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                if (propertySyntax == null)
                {
                    NoOfChanges++;
                    node = node.AddPropertyListProperties(
                        this.CreateCaptionPropertyFromName(node, false));
                }
                else
                    node = UpdateCaptionFromName(node, propertySyntax, false);
            }
            return base.VisitReport(node);
        }

        public override SyntaxNode VisitQuery(QuerySyntax node)
        {
            if (!node.HasProperty("CaptionML"))
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                if (propertySyntax == null)
                {
                    NoOfChanges++;
                    node = node.AddPropertyListProperties(
                        this.CreateCaptionPropertyFromName(node, false));
                }
                else
                    node = UpdateCaptionFromName(node, propertySyntax, false);
            }
            return base.VisitQuery(node);
        }

        public override SyntaxNode VisitXmlPort(XmlPortSyntax node)
        {
            if (!node.HasProperty("CaptionML"))
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                if (propertySyntax == null)
                {
                    NoOfChanges++;
                    node = node.AddPropertyListProperties(
                        this.CreateCaptionPropertyFromName(node, false));
                }
                else
                    node = UpdateCaptionFromName(node, propertySyntax, false);
            }
            return base.VisitXmlPort(node);
        }

    }
}
