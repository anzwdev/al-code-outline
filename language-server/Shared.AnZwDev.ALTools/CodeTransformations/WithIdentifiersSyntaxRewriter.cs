using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{

#if BC
    public class WithIdentifiersSyntaxRewriter : ALSemanticModelSyntaxRewriter
    {
        public WithIdentifiersSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            bool skip =
                (node.Parent.Kind.ConvertToLocalType() == ConvertedSyntaxKind.PageField) &&
                (((PageFieldSyntax)node.Parent).Name == node);

            //check if it is field access parameter that should not have record variable name at the fromt
            //i.e. Rec.Setrange(FieldName, ...);
            if (node.Parent.Kind.ConvertToLocalType() == ConvertedSyntaxKind.ArgumentList)
            {
                //get parameter index
                ArgumentListSyntax argumentList = (ArgumentListSyntax)node.Parent;
                if (argumentList.Arguments.Contains(node))
                {
                    int parameterIndex = argumentList.Arguments.IndexOf(node);
                    IOperation methodOperation = this.SemanticModel.GetOperation(argumentList.Parent);
                    if ((methodOperation != null) && (methodOperation.Kind.ConvertToLocalType() == ConvertedOperationKind.InvocationExpression))
                    {
                        IInvocationExpression invocationExpression = methodOperation as IInvocationExpression;
                        if ((invocationExpression != null) && (invocationExpression.TargetMethod != null) && (invocationExpression.TargetMethod.Parameters != null))
                        {
                            if (parameterIndex >= invocationExpression.TargetMethod.Parameters.Length)
                                parameterIndex = invocationExpression.TargetMethod.Parameters.Length - 1;
                            if (parameterIndex < 0)
                                parameterIndex = 0;

                            if (invocationExpression.TargetMethod.Parameters.Length > parameterIndex)
                            {
                                IParameterSymbol parameter = invocationExpression.TargetMethod.Parameters[parameterIndex];
                                if (parameter.MemberMustBeOnSame)
                                    skip = true;
                            }
                        }
                    }
                }
            }

            if (!skip)
            {

                IOperation operation = this.SemanticModel.GetOperation(node);
                if (operation != null)
                {
                    IOperation operationInstance = this.GetOperationInstance(operation);

                    if ((operationInstance != null) &&
                        (operationInstance.Syntax != null))
                    {
                        //part of with?
                        if ((operationInstance.Syntax.Parent != null) &&
                        (operationInstance.Syntax.Parent.Kind.ConvertToLocalType() == ConvertedSyntaxKind.WithStatement))
                        {
                            this.NoOfChanges++;
                            return SyntaxFactory.MemberAccessExpression(
                                (CodeExpressionSyntax)operationInstance.Syntax.WithoutTrivia(),
                                node.WithoutTrivia()).WithTriviaFrom(node);
                        }

                        //global variable reference?
                        else if ((operationInstance.Kind.ConvertToLocalType() == ConvertedOperationKind.GlobalReferenceExpression) &&
                            (node.Parent.Kind.ConvertToLocalType() != ConvertedSyntaxKind.MemberAccessExpression))
                        {
                            IGlobalReferenceExpression globalRef = (IGlobalReferenceExpression)operationInstance;
                            string name = globalRef.GlobalVariable.Name.ToString();

                            this.NoOfChanges++;
                            return SyntaxFactory.MemberAccessExpression(
                                SyntaxFactory.IdentifierName(name),
                                node.WithoutTrivia()).WithTriviaFrom(node);
                        }
                    }
                }
            }

            return base.VisitIdentifierName(node);
        }

        protected IOperation GetOperationInstance(IOperation operation)
        {
            if (operation != null)
            {
                switch (operation.Kind.ConvertToLocalType())
                {
                    case ConvertedOperationKind.TestActionAccess:
                        ITestActionAccess testActionAccess = operation as ITestActionAccess;
                        if (testActionAccess != null)
                            return testActionAccess.Instance;
                        break;
                    case ConvertedOperationKind.TestFieldAccess:
                        ITestFieldAccess testFieldAccess = operation as ITestFieldAccess;
                        if (testFieldAccess != null)
                            return testFieldAccess.Instance;
                        break;
                    case ConvertedOperationKind.TestFilterAccess:
                        ITestFilterAccess testFilterAccess = operation as ITestFilterAccess;
                        if (testFilterAccess != null)
                            return testFilterAccess.Instance;
                        break;
                    case ConvertedOperationKind.TestFilterFieldAccess:
                        ITestFilterFieldAccess testFilterFieldAccess = operation as ITestFilterFieldAccess;
                        if (testFilterFieldAccess != null)
                            return testFilterFieldAccess.Instance;
                        break;
                    case ConvertedOperationKind.TestPartAccess:
                        ITestPartAccess testPartAccess = operation as ITestPartAccess;
                        if (testPartAccess != null)
                            return testPartAccess.Instance;
                        break;
                    case ConvertedOperationKind.PagePartAccess:
                        IPagePartAccess pagePartAccess = operation as IPagePartAccess;
                        if (pagePartAccess != null)
                            return pagePartAccess.Instance;
                        break;
                    case ConvertedOperationKind.FieldAccess:
                        IFieldAccess fieldAccess = operation as IFieldAccess;
                        if (fieldAccess != null)
                            return fieldAccess.Instance;
                        break;
                    case ConvertedOperationKind.InvocationExpression:
                        IInvocationExpression invocationExpression = operation as IInvocationExpression;
                        if (invocationExpression != null)
                            return invocationExpression.Instance;
                        break;
                }
            }
            return null;
        }

    }
#endif
}
