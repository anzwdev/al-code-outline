using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Syntax;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeTreeViews
{
    public class SyntaxTreeTreeViewBuilder
    {

        public SyntaxTreeTreeViewNode? CreateView(SyntaxTree syntaxTree)
        {
            SyntaxNode node = syntaxTree.GetRoot();
            return CreateView(syntaxTree, node);
        }

        protected SyntaxTreeTreeViewNode? CreateView(SyntaxTree syntaxTree, SyntaxNode node)
        {
            //process node
            var alNode = CreateALNode(syntaxTree, node);
            if (alNode == null)
                return null;

            //process child nodes
            var list = node.ChildNodes();
            if (list != null)
                foreach (var childNode in list)
                    alNode.AddChildNode(CreateView(syntaxTree, childNode));

            return alNode;
        }

        protected SyntaxTreeTreeViewNode? CreateALNode(SyntaxTree syntaxTree, SyntaxToken token)
        {
            if (token.Kind == SyntaxKind.None)
                return null;

            return new SyntaxTreeTreeViewNode()
            {
                Kind = token.Kind.ToString(),
                FullSpan = syntaxTree.GetLineRange(token.FullSpan),
                Span = syntaxTree.GetLineRange(token.Span)
            };
        }

        protected SyntaxTreeTreeViewNode CreateALNode(SyntaxTree syntaxTree, SyntaxNode node)
        {
            //base syntax node properties
            var alNode = new SyntaxTreeTreeViewNode();
            alNode.Kind = node.Kind.ToString();

            alNode.FullSpan = syntaxTree.GetLineRange(node.FullSpan);
            alNode.Span = syntaxTree.GetLineRange(node.Span);

            Type nodeType = node.GetType();
            alNode.Name = ALLiteralParser.ParseName(nodeType.TryGetPropertyValueAsString(node, "Name"));

            if (node.ContainsDiagnostics)
                alNode.ContainsDiagnostics = true;

            var attributes = nodeType.TryGetPropertyValue<IEnumerable>(node, "Attributes");
            if (attributes != null)
                foreach (SyntaxNode childNode in attributes)
                    alNode.AddAttribute(CreateALNode(syntaxTree, childNode));

            alNode.OpenBraceToken = CreateALNode(syntaxTree, nodeType.TryGetStructPropertyValue<SyntaxToken>(node, "OpenBraceToken"));
            alNode.CloseBraceToken = CreateALNode(syntaxTree, nodeType.TryGetStructPropertyValue<SyntaxToken>(node, "CloseBraceToken"));
            alNode.VarKeyword = CreateALNode(syntaxTree, nodeType.TryGetStructPropertyValue<SyntaxToken>(node, "VarKeyword"));

            alNode.AccessModifier = nodeType.TryGetPropertyValueAsString(node, "AccessModifier");
            alNode.Identifier = nodeType.TryGetPropertyValueAsString(node, "Identifier");
            alNode.DataType = nodeType.TryGetPropertyValueAsString(node, "DataType");
            alNode.Temporary = nodeType.TryGetPropertyValueAsString(node, "Temporary");

            return alNode;
        }

    }
}
