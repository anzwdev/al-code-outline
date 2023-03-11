using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.ALTools.CodeTransformations
{

#if BC

    public class GenerateCSVXmlPortHeadersSyntaxRewriter : ALSyntaxRewriter
    {

        public override SyntaxNode VisitXmlPort(XmlPortSyntax node)
        {
            if (node.XmlPortSchema?.XmlPortSchema != null)
            {
                (bool updated, SyntaxList<XmlPortNodeSyntax> newNodesList) = UpdateSchema(node.XmlPortSchema.XmlPortSchema);
                if (updated)
                    node = node.WithXmlPortSchema(
                        node.XmlPortSchema.WithXmlPortSchema(newNodesList));
            }        
            return base.VisitXmlPort(node);
        }

        private (bool, SyntaxList<XmlPortNodeSyntax>) UpdateSchema(SyntaxList<XmlPortNodeSyntax> schema)
        {
            bool updated = false;
            bool prevTableWasHeader = false;

            List<XmlPortNodeSyntax> newNodesList = new List<XmlPortNodeSyntax>();
            foreach (var node in schema)
            {
                if ((node is XmlPortTableElementSyntax xmlPortTableElementSyntax) && (this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
                {
                    if (IsTableHeader(xmlPortTableElementSyntax))
                        prevTableWasHeader = true;
                    else
                    {
                        var newTableHeader = CreateTableHeader(xmlPortTableElementSyntax);
                        if ((prevTableWasHeader) && (newNodesList.Count > 0) && (newNodesList[newNodesList.Count - 1].Name.ToString() == newTableHeader.Name.ToString()))
                            newNodesList[newNodesList.Count - 1] = newTableHeader;
                        else
                            newNodesList.Add(newTableHeader);
                        prevTableWasHeader = false;
                    }
                    updated = true;
                }
                else
                    prevTableWasHeader = false;
                (var nodeUpdated, var newNode) = UpdateNode(node);
                
                updated |= nodeUpdated;
                newNodesList.Add(newNode);
            }

            if (updated)
                return (true, SyntaxFactory.List(newNodesList));

            return (false, schema);
        }

        private (bool, XmlPortNodeSyntax) UpdateNode(XmlPortNodeSyntax node)
        {
            if (node.Schema != null)
            {
                (var updated, var newSchema) = UpdateSchema(node.Schema);
                if (updated)
                    return (true, ReplaceNodeSchema(node, newSchema));
            }
            return (false, node);
        }

        private XmlPortNodeSyntax ReplaceNodeSchema(XmlPortNodeSyntax node, SyntaxList<XmlPortNodeSyntax> schema)
        {
            switch (node)
            {
                case XmlPortTableElementSyntax xmlPortTableElementSyntax:
                    return xmlPortTableElementSyntax.WithSchema(schema);
                case XmlPortTextElementSyntax xmlPortTextElementSyntax:
                    return xmlPortTextElementSyntax.WithSchema(schema);
                case XmlPortTextAttributeSyntax xmlPortTextAttributeSyntax:
                    return xmlPortTextAttributeSyntax.WithSchema(schema);
                case XmlPortFieldElementSyntax xmlPortFieldElementSyntax:
                    return xmlPortFieldElementSyntax.WithSchema(schema);
                case XmlPortFieldAttributeSyntax xmlPortFieldAttributeSyntax:
                    return xmlPortFieldAttributeSyntax.WithSchema(schema);
            }
            return node;
        }

        private XmlPortTableElementSyntax CreateTableHeader(XmlPortTableElementSyntax tableElementSyntax)
        {
            List<XmlPortNodeSyntax> fieldsList = new List<XmlPortNodeSyntax>();

            foreach (var sourceField in tableElementSyntax.Schema)
            {
                var newField = CreateFieldHeader(sourceField);
                if (newField != null)
                    fieldsList.Add(newField);
            }

            SeparatedSyntaxList<IdentifierNameSyntax> sortingFieldsSyntax = new SeparatedSyntaxList<IdentifierNameSyntax>();
            sortingFieldsSyntax = sortingFieldsSyntax.Add(SyntaxFactory.IdentifierName("Number"));
            var sortingSyntax = SyntaxFactory.Sorting(sortingFieldsSyntax);

            var whereConditions = new SeparatedSyntaxList<PropertyExpressionSyntax>();
            whereConditions = whereConditions.Add(
                SyntaxFactory.ConstExpression(
                    SyntaxFactory.IdentifierName("Number"),
                    SyntaxFactory.IdentifierOrLiteralOrOptionAccessExpression(
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Int32SignedLiteralValue(SyntaxFactory.ParseToken("1"))))));
            var tableViewPropertyValueSyntax = 
                SyntaxFactory.TableViewPropertyValue(sortingSyntax, null,
                    SyntaxFactory.WhereExpression(
                        SyntaxFactory.TableFilterExpression(whereConditions)));
            var tableViewPropertySyntax = SyntaxFactory.Property("SourceTableView", tableViewPropertyValueSyntax);

            var sourceTable = SyntaxFactory.ObjectNameOrId(SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("Integer")));
            var headerSyntax = SyntaxFactory.XmlPortTableElement(CreateTableHeaderName(tableElementSyntax), sourceTable)
                .AddPropertyListProperties(tableViewPropertySyntax)
                .WithSchema(SyntaxFactory.List(fieldsList));

            return headerSyntax;
        }

        private XmlPortTextElementSyntax CreateFieldHeader(XmlPortNodeSyntax node)
        {
            switch (node)
            {
                case XmlPortTextNodeSyntax xmlPortTextNodeSyntax:
                    return CreateTextNodeHeader(xmlPortTextNodeSyntax);
                case XmlPortFieldNodeSyntax xmlPortFieldNodeSyntax:
                    return CreateFieldNodeHeader(xmlPortFieldNodeSyntax);
            }
            return null;
        }

        private XmlPortTextElementSyntax CreateFieldNodeHeader(XmlPortFieldNodeSyntax node)
        {
            string newName = CreateFieldHeaderName(node);
            var headerNode = SyntaxFactory.XmlPortTextElement(newName);

            CodeExpressionSyntax assignmentSource;
            if (node.SourceField is MemberAccessExpressionSyntax sourceFieldMemberAccessExpression)
            {
                var argumentsSeparatedList = (new SeparatedSyntaxList<CodeExpressionSyntax>()).Add(sourceFieldMemberAccessExpression.Name);
                var statementArguments = SyntaxFactory.ArgumentList(argumentsSeparatedList);
                assignmentSource = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(sourceFieldMemberAccessExpression.Expression, SyntaxFactory.IdentifierName("FieldCaption")),
                    statementArguments);
            }
            else
                assignmentSource = node.SourceField;

            StatementSyntax triggerStatement =
                SyntaxFactory.AssignmentStatement(
                    SyntaxFactory.IdentifierName(newName),
                    assignmentSource)
                .WithSemicolonToken(
                    SyntaxFactory.Token(SyntaxKind.SemicolonToken));

            StatementSyntax[] triggerStatementsList = { triggerStatement };

            headerNode = headerNode.AddTriggers(
                SyntaxFactory
                    .TriggerDeclaration("OnBeforePassVariable")
                    .WithBody(
                        SyntaxFactory.Block(
                            SyntaxFactory.List(triggerStatementsList))
                        .WithSemicolonToken(
                            SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                        ));

            return headerNode;
        }

        private XmlPortTextElementSyntax CreateTextNodeHeader(XmlPortTextNodeSyntax node)
        {
            string newName = CreateFieldHeaderName(node);
            var headerNode = SyntaxFactory.XmlPortTextElement(newName);

            StatementSyntax triggerStatement =
                SyntaxFactory.AssignmentStatement(
                    SyntaxFactory.IdentifierName(newName),
                    SyntaxFactory.LiteralExpression(
                        SyntaxFactory.StringLiteralValue(SyntaxFactory.Literal(newName))))
                .WithSemicolonToken(
                    SyntaxFactory.Token(SyntaxKind.SemicolonToken));
            StatementSyntax[] triggerStatementsList = { triggerStatement };

            headerNode = headerNode.AddTriggers(
                SyntaxFactory
                    .TriggerDeclaration("OnBeforePassVariable")
                    .WithBody(
                        SyntaxFactory.Block(
                            SyntaxFactory.List(triggerStatementsList))
                        .WithSemicolonToken(
                            SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                        ));

            return headerNode;
        }

        private string CreateTableHeaderName(XmlPortTableElementSyntax node)
        {
            return "THeader_" + node.Name.ToString();
        }

        private string CreateFieldHeaderName(XmlPortNodeSyntax node)
        {
            return "FHeader_" + node.Name.ToString();
        }

        private bool IsTableHeader(XmlPortTableElementSyntax node)
        {
            string name = node.Name?.ToString();           
            return (name != null) && (name.StartsWith("THeader_"));
        }


    }

#endif
}
