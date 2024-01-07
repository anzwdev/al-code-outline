using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.SyntaxHelpers
{
    public static class ExpressionFactory
    {

        public static StatementSyntax NotSupportedExceptionStatement(string message = null, params CodeExpressionSyntax[] arguments)
        {
            if (String.IsNullOrWhiteSpace(message))
                message = "Not Supported";
            
            var messageValue = SyntaxFactory.LiteralExpression(SyntaxFactory.StringLiteralValue(SyntaxFactory.Literal(message)));                       
            var messageArguments = SyntaxFactory.ArgumentList().AddArguments(messageValue);
            if ((arguments != null) && (arguments.Length > 0))
                messageArguments = messageArguments.AddArguments(arguments);

            var statement = SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.IdentifierName("Error"),
                    messageArguments),
                SyntaxFactory.Token(SyntaxKind.SemicolonToken));            

            return statement;
        }

#if BC

        public static NameSyntax NamespaceName(string namespaceName)
        {
            string[] parts = namespaceName.Split('.');
            if (parts.Length == 0)
                return null;
            if (parts.Length == 1)
                return SyntaxFactory.IdentifierName(parts[0]);
            var nameSyntax = SyntaxFactory.QualifiedName(
                SyntaxFactory.IdentifierName(parts[0]),
                SyntaxFactory.IdentifierName(parts[1]));
            for (var i = 2; i < parts.Length; i++)
                nameSyntax = SyntaxFactory.QualifiedName(
                    nameSyntax,
                    SyntaxFactory.IdentifierName(parts[i]));
            return nameSyntax;
        }
#endif

    }
}
