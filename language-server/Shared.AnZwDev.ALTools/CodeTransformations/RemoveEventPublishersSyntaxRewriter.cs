using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{

#if BC

    public class RemoveEventPublishersSyntaxRewriter : ALSemanticModelSyntaxRewriter
    {

        public RemoveEventPublishersSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitCodeunit(CodeunitSyntax node)
        {
            var newMembers = RemoveEventPublishers(node);
            if (newMembers != null)
                node = node.WithMembers(SyntaxFactory.List(newMembers));
            return base.VisitCodeunit(node);
        }

        private List<MemberSyntax> RemoveEventPublishers(ApplicationObjectSyntax node)
        {
            //remove event publishers
            Dictionary<string, IMethodSymbol> deletedMethods = new Dictionary<string, IMethodSymbol>();
            List<MemberSyntax> newMembers = this.RemoveEventPublisherMembers(node.Members, deletedMethods);

            if (deletedMethods.Count > 0)
            {
                NoOfChanges += deletedMethods.Count;
                return this.RemoveEventPublisherCalls(newMembers, deletedMethods);
            }

            return null;
        }

        private List<MemberSyntax> RemoveEventPublisherMembers(SyntaxList<MemberSyntax> members, Dictionary<string, IMethodSymbol> deletedMethods)
        {
            List<MemberSyntax> newMembers = new List<MemberSyntax>();
            foreach (MemberSyntax member in members)
            {
                bool deleted = false;
                if (member is MethodDeclarationSyntax methodDeclaration)
                {
                    ISymbol symbol = this.SemanticModel.GetDeclaredSymbol(member);
                    if ((symbol is IMethodSymbol methodSymbol) && (methodSymbol.IsEvent))
                    {
                        deletedMethods.Add(methodSymbol.Name.ToLower(), methodSymbol);
                        deleted = true;
                    }
                }

                if (!deleted)
                    newMembers.Add(member);
            }

            return newMembers;
        }

        protected List<MemberSyntax> RemoveEventPublisherCalls(List<MemberSyntax> memberSyntaxes, Dictionary<string, IMethodSymbol> deletedMethods)
        {
            for (int i = 0; i < memberSyntaxes.Count; i++)
            {
                if (memberSyntaxes[i] is MethodDeclarationSyntax methodDeclaration)
                {
                    memberSyntaxes[i] = this.RemoveEventPublisherCalls(methodDeclaration, deletedMethods);
                }
            }

            return memberSyntaxes;
        }

        protected MethodDeclarationSyntax RemoveEventPublisherCalls(MethodDeclarationSyntax methodDeclaration, Dictionary<string, IMethodSymbol> deletedMethods)
        {
            List<SyntaxNode> nodesToDelete = new List<SyntaxNode>();
            this.CollectDeletedNodes(methodDeclaration.Body, deletedMethods, nodesToDelete);
            if (nodesToDelete.Count > 0)
            {
                NoOfChanges += nodesToDelete.Count;
                methodDeclaration = methodDeclaration.RemoveNodes(nodesToDelete, new SyntaxRemoveOptions());
            }
            return methodDeclaration;
        }

        protected void CollectDeletedNodes(SyntaxNode node, Dictionary<string, IMethodSymbol> deletedMethods, List<SyntaxNode> nodesToDelete)
        {
            if (node is InvocationExpressionSyntax invocationExpression)
            {
                SymbolInfo symbol = this.SemanticModel.GetSymbolInfo(node);
                if ((symbol != null) && (symbol.Symbol != null) && (symbol.Symbol.Kind == SymbolKind.Method))
                {
                    string name = symbol.Symbol.Name.ToLower();
                    if ((deletedMethods.ContainsKey(name)) && (deletedMethods[name] == symbol.Symbol))
                    {
                        nodesToDelete.Add(invocationExpression.Parent);
                    }
                }
            }

            IEnumerable<SyntaxNode> childNodes = node.ChildNodes();
            foreach (SyntaxNode childNode in childNodes)
            {
                this.CollectDeletedNodes(childNode, deletedMethods, nodesToDelete);
            }
        }

    }
#endif

}
