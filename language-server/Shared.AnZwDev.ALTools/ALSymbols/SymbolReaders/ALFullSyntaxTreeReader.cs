using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Logging;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.SymbolReaders
{
    public class ALFullSyntaxTreeReader
    {

        public ALFullSyntaxTreeReader()
        {
        }

        #region Main processing methods

        public ALFullSyntaxTreeNode ProcessSourceFile(string fileName)
        {
            string sourceCode;
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(fileName);
                sourceCode = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();

                return ProcessSourceCode(sourceCode);
            }
            catch (Exception e)
            {
                MessageLog.LogError(e);
                return new ALFullSyntaxTreeNode(e);
            }

        }

        public ALFullSyntaxTreeNode ProcessSourceCode(string source)
        {
            try
            {
                SyntaxTree sourceTree = SyntaxTreeExtensions.SafeParseObjectText(source);
                return ProcessSyntaxTree(sourceTree);
            }
            catch (Exception e)
            {
                MessageLog.LogError(e);
                return new ALFullSyntaxTreeNode(e);
            }
        }

        public ALFullSyntaxTreeNode ProcessSyntaxTree(SyntaxTree syntaxTree)
        {
            SyntaxNode node = syntaxTree.GetRoot();
            return ProcessSyntaxTreeNode(syntaxTree, node);
        }

        #endregion

        #region Processing nodes

        protected ALFullSyntaxTreeNode ProcessSyntaxTreeNode(SyntaxTree syntaxTree, SyntaxNode node)
        {
            //process node
            ALFullSyntaxTreeNode alNode = CreateALNode(syntaxTree, node);
            if (alNode == null)
                return null;

            //process child nodes
            IEnumerable<SyntaxNode> list = node.ChildNodes();
            if (list != null)
            {
                foreach (SyntaxNode childNode in list)
                {
                    ALFullSyntaxTreeNode childALNode = ProcessSyntaxTreeNode(syntaxTree, childNode);
                    if (childALNode != null)
                        alNode.AddChildNode(childALNode);
                }
            }

            return alNode;
        }

        protected ALFullSyntaxTreeNode CreateALNode(SyntaxTree syntaxTree, SyntaxToken token)
        {
            //base syntax node properties
            ALFullSyntaxTreeNode alNode = new ALFullSyntaxTreeNode();
            alNode.kind = token.Kind.ToString();
            
            FileLinePositionSpan lineSpan = syntaxTree.GetLineSpan(token.FullSpan);
            alNode.fullSpan = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);

            lineSpan = syntaxTree.GetLineSpan(token.Span);
            alNode.span = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character);

            return alNode;
        }

        protected ALFullSyntaxTreeNode CreateALNode(SyntaxTree syntaxTree, SyntaxNode node)
        {
            //base syntax node properties
            ALFullSyntaxTreeNode alNode = new ALFullSyntaxTreeNode();
            alNode.kind = node.Kind.ToString();

            FileLinePositionSpan lineSpan = syntaxTree.GetLineSpan(node.FullSpan);
            alNode.fullSpan = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);

            lineSpan = syntaxTree.GetLineSpan(node.Span);
            alNode.span = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character);

            //additional properties
            Type nodeType = node.GetType();
            
            alNode.name = ALSyntaxHelper.DecodeName(nodeType.TryGetPropertyValueAsString(node, "Name"));

            if (node.ContainsDiagnostics)
                alNode.containsDiagnostics = true;

            IEnumerable attributes = nodeType.TryGetPropertyValue<IEnumerable>(node, "Attributes");
            if (attributes != null)
            {
                foreach (SyntaxNode childNode in attributes)
                {
                    alNode.AddAttribute(CreateALNode(syntaxTree, childNode));
                }
            }

            SyntaxToken specialToken = nodeType.TryGetPropertyValue<SyntaxToken>(node, "OpenBraceToken");
            if ((specialToken != null) && (specialToken.Kind != SyntaxKind.None))
                alNode.openBraceToken = CreateALNode(syntaxTree, specialToken);

            specialToken = nodeType.TryGetPropertyValue<SyntaxToken>(node, "CloseBraceToken");
            if ((specialToken != null) && (specialToken.Kind != SyntaxKind.None))
                alNode.closeBraceToken = CreateALNode(syntaxTree, specialToken);

            specialToken = nodeType.TryGetPropertyValue<SyntaxToken>(node, "VarKeyword");
            if ((specialToken != null) && (specialToken.Kind != SyntaxKind.None))
                alNode.varKeyword = CreateALNode(syntaxTree, specialToken);          

            alNode.accessModifier = nodeType.TryGetPropertyValueAsString(node, "AccessModifier");
            alNode.identifier = nodeType.TryGetPropertyValueAsString(node, "Identifier");
            alNode.dataType = nodeType.TryGetPropertyValueAsString(node, "DataType");
            alNode.temporary = nodeType.TryGetPropertyValueAsString(node, "Temporary");

            return alNode;
        }

        #endregion

    }
}
