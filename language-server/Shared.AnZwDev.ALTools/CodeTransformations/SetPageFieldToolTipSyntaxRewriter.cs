using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SetPageFieldToolTipSyntaxRewriter : BaseToolTipsSyntaxRewriter
    {

        public string ToolTip { get; set; }
        public string Comment { get; set; }

        public SetPageFieldToolTipSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitPageField(PageFieldSyntax node)
        {
            if ((!node.ContainsDiagnostics) && (!String.IsNullOrWhiteSpace(this.ToolTip)) && (this.Span != null) && (node.Span.IntersectsWith(this.Span)))
            {
                PageFieldSyntax newNode = this.SetPageFieldToolTip(node, this.ToolTip, this.Comment);
                if (newNode != null)
                {
                    NoOfChanges++;
                    return newNode;
                }
            }
            return base.VisitPageField(node);
        }

    }
}
