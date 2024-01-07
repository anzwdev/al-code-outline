using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace.SymbolReferences;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RefreshToolTipsSyntaxRewriter : BaseToolTipsSyntaxRewriter
    {

        public Dictionary<int, Dictionary<string, List<LabelInformation>>> ToolTipsCache { get; set; }

        public RefreshToolTipsSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitPageField(PageFieldSyntax node)
        {
            if ((!node.ContainsDiagnostics) && 
                (this.ToolTipsCache != null) && 
                (this.Table != null) &&
                (!node.HasProperty("ToolTipML")))
            {
                if ((!this.HasToolTip(node)) && (this.ToolTipDisabled(node)))
                    return base.VisitPageField(node);

                //find first tooltip from other pages
                TableFieldCaptionInfo captionInfo = this.GetFieldCaption(node);
                if (this.ToolTipsCache.ContainsKey(this.Table.Id))
                {
                    Dictionary<string, List<LabelInformation>> tableToolTipsCache = this.ToolTipsCache[this.Table.Id];
                    string fieldNameKey = captionInfo.FieldName.ToLower();
                    if (tableToolTipsCache.ContainsKey(fieldNameKey))
                    {
                        List<LabelInformation> fieldToolTipsCache = tableToolTipsCache[fieldNameKey];
                        if ((fieldToolTipsCache.Count > 0) && (!String.IsNullOrWhiteSpace(fieldToolTipsCache[0].Value)))
                        {
                            var newToolTip = fieldToolTipsCache[0];

                            PageFieldSyntax newNode = this.SetPageFieldToolTip(node, newToolTip);
                            if (newNode != null)
                            {
                                NoOfChanges++;
                                return newNode;
                            }
                        }
                    }
                }
            }

            return base.VisitPageField(node);
        }

    }
}
