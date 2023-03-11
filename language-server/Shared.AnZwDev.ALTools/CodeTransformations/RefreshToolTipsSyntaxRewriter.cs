using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RefreshToolTipsSyntaxRewriter : BaseToolTipsSyntaxRewriter
    {

        public Dictionary<string, Dictionary<string, List<string>>> ToolTipsCache { get; set; }

        public RefreshToolTipsSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitPageField(PageFieldSyntax node)
        {
            if ((!node.ContainsDiagnostics) && 
                (this.ToolTipsCache != null) && 
                (!String.IsNullOrWhiteSpace(this.TableName)) && 
                (!node.HasProperty("ToolTipML")))
            {
                if ((!this.HasToolTip(node)) && (this.ToolTipDisabled(node)))
                    return base.VisitPageField(node);

                //find first tooltip from other pages
                TableFieldCaptionInfo captionInfo = this.GetFieldCaption(node);
                string tableNameKey = this.TableName.ToLower();
                if (this.ToolTipsCache.ContainsKey(tableNameKey))
                {
                    Dictionary<string, List<string>> tableToolTipsCache = this.ToolTipsCache[tableNameKey];
                    string fieldNameKey = captionInfo.FieldName.ToLower();
                    if (tableToolTipsCache.ContainsKey(fieldNameKey))
                    {
                        List<string> fieldToolTipsCache = tableToolTipsCache[fieldNameKey];
                        if ((fieldToolTipsCache.Count > 0) && (!String.IsNullOrWhiteSpace(fieldToolTipsCache[0])))
                        {
                            string newToolTip = fieldToolTipsCache[0];

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
