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

    }
}
