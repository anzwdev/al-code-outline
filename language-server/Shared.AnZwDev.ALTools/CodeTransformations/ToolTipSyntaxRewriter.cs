using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class ToolTipSyntaxRewriter : BasePageWithSourceSyntaxRewriter
    {
        public string PageFieldTooltip { get; set; }
        public string PageFieldTooltipComment { get; set; }
        public string PageActionTooltip { get; set; }
        public bool UseFieldDescription { get; set; }
        
        public Dictionary<string, Dictionary<string, List<string>>> ToolTipsCache { get; set; }

        public ToolTipSyntaxRewriter()
        {
            this.PageActionTooltip = "Executes the %1 action.";
            this.PageFieldTooltip = "Specifies the value of %1 field.";
            this.PageFieldTooltipComment = "%Caption.Comment%";
            this.UseFieldDescription = false;
            this.ToolTipsCache = null;
        }

        protected override SyntaxNode AfterVisitNode(SyntaxNode node)
        {
            if (this.NoOfChanges == 0)
                return null;
            return base.AfterVisitNode(node);
        }

        public override SyntaxNode VisitPageField(PageFieldSyntax node)
        {
            if ((!CanAddToolTip(node)) || (this.HasToolTip(node)) || (this.ToolTipDisabled(node)))
                return base.VisitPageField(node);
            this.NoOfChanges++;

            //try to find source field caption
            string forceToolTipValue = null;
            TableFieldCaptionInfo captionInfo = this.GetFieldCaption(node);
            if (this.UseFieldDescription)
                forceToolTipValue = captionInfo.Description;
            else if ((this.ToolTipsCache != null) && (!String.IsNullOrWhiteSpace(this.TableName)) && (!String.IsNullOrWhiteSpace(captionInfo.FieldName)))
            {
                //find first tooltip from other pages
                string tableNameKey = this.TableName.ToLower();
                if (this.ToolTipsCache.ContainsKey(tableNameKey))
                {
                    Dictionary<string, List<string>> tableToolTipsCache = this.ToolTipsCache[tableNameKey];
                    string fieldNameKey = captionInfo.FieldName.ToLower();
                    if (tableToolTipsCache.ContainsKey(fieldNameKey))
                    {
                        List<string> fieldToolTipsCache = tableToolTipsCache[fieldNameKey];
                        if (fieldToolTipsCache.Count > 0)
                            forceToolTipValue = fieldToolTipsCache[0];
                    }
                }
            }

            return node.AddPropertyListProperties(this.CreateToolTipProperty(node, captionInfo.Caption.Value, captionInfo.Caption.Comment, forceToolTipValue));
        }

        public override SyntaxNode VisitPageAction(PageActionSyntax node)
        {
            if ((!CanAddToolTip(node)) || (this.HasToolTip(node)))
                return base.VisitPageAction(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateToolTipProperty(node));
        }

        protected PropertySyntax CreateToolTipProperty(SyntaxNode node, string caption = null, string comment = null, string forceToolTipValue = null)
        {
            string toolTipValue = "";
            string toolTipComment = "";

            if (String.IsNullOrWhiteSpace(forceToolTipValue))
            {
                //get caption from control caption
                LabelInformation controlCaptionInformation = node.GetCaptionPropertyInformation();
                if ((controlCaptionInformation != null) && (!String.IsNullOrWhiteSpace(controlCaptionInformation.Value)))
                {
                    caption = controlCaptionInformation.Value;
                    comment = controlCaptionInformation.Comment;
                }
                //get caption from control name
                else if (String.IsNullOrWhiteSpace(caption))
                {
                    caption = node.GetNameStringValue().RemovePrefixSuffix(
                        this.Project.MandatoryPrefixes, this.Project.MandatorySuffixes, this.Project.MandatoryAffixes, this.Project.AdditionalMandatoryAffixesPatterns);
                    comment = null;
                }

                switch (node.Kind.ConvertToLocalType())
                {
                    case ConvertedSyntaxKind.PageField:
                        toolTipValue = PageFieldTooltip;
                        toolTipComment = PageFieldTooltipComment;
                        break;
                    case ConvertedSyntaxKind.PageAction:
                        toolTipValue = PageActionTooltip;
                        break;
                }

                toolTipValue = ApplyTextTemplate(toolTipValue, caption, comment);
                toolTipComment = ApplyTextTemplate(toolTipComment, caption, comment);
            }
            else
            {
                toolTipValue = forceToolTipValue;
            }

            var propertySyntax = SyntaxFactoryHelper.ToolTipProperty(toolTipValue, toolTipComment, false);

            /*
            SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);
            propertySyntax = propertySyntax
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
            */

            return propertySyntax;
        }

        protected string ApplyTextTemplate(string template, string caption, string comment)
        {
            if (template == null)
                return "";

            if (comment == null)
                comment = "";
            if (caption == null)
                caption = "";

            if (template.Contains("%1"))
                template = template.Replace("%1", caption);
            if (template.Contains("%Caption%"))
                template = template.Replace("%Caption%", caption);
            if (template.Contains("%Caption.Comment%"))
                template = template.Replace("%Caption.Comment%", comment);

            return template;
        }

        protected bool CanAddToolTip(SyntaxNode node)
        {
            while (node != null)
            {
                var kind = node.Kind.ConvertToLocalType();
                switch (kind)
                {
                    case ConvertedSyntaxKind.RequestPage:
                    case ConvertedSyntaxKind.RequestPageExtension:
                        return true;
                    case ConvertedSyntaxKind.PageObject:
                        string pageType = ALSyntaxHelper.DecodeName(node.GetProperty("PageType")?.Value?.ToString());
                        return ((pageType == null) || (!pageType.Equals("API", StringComparison.CurrentCultureIgnoreCase)));
                    case ConvertedSyntaxKind.PageExtensionObject:
                        var pageExtension = node as PageExtensionSyntax;
                        string basePageName = ALSyntaxHelper.DecodeName(pageExtension?.BaseObject?.ToString());
                        if (!String.IsNullOrWhiteSpace(basePageName))
                        {
                            var basePage = this.Project.AllSymbols.Pages.FindObject(basePageName);
                            if ((basePage != null) && (basePage.Properties != null))
                            {
                                var basePageType = basePage.Properties.Where(p => ((p.Name != null) && (p.Name.Equals("PageType", StringComparison.CurrentCultureIgnoreCase)))).FirstOrDefault();
                                return ((basePageType?.Value == null) || (!basePageType.Value.Equals("API", StringComparison.CurrentCultureIgnoreCase)));
                            }
                        }
                        return true;
                }
                node = node.Parent;
            }
            return true;
        }


    }
}
