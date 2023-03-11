using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Compiler;
using AnZwDev.ALTools.ALSymbolReferences.Extensions;
using AnZwDev.ALTools.ALSymbolReferences.MergedPermissions;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.ALTools.CodeTransformations
{

#if BC

    public class AddReferencedTablesPermissionsSyntaxRewriter : ALSemanticModelSyntaxRewriter
    {

        protected MergedALAppPermissionsCollection RequiredPermissions { get; } = new MergedALAppPermissionsCollection();
        protected bool PermissionsChanged { get; set; }
        protected bool CurrentXmlPortCanImportData { get; set; }

        public override SyntaxNode VisitCodeunit(CodeunitSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
            {

                RequiredPermissions.Clear();
                var existingPermissionsValue = node.GetPropertyValue("Permissions") as PermissionPropertyValueSyntax;
                if (existingPermissionsValue != null)
                    CollectExistingPermissions(existingPermissionsValue);
                PermissionsChanged = false;

                var newNode = base.VisitCodeunit(node);

                node = newNode as CodeunitSyntax;
                if (node == null)
                    return newNode;

                if (PermissionsChanged)
                {
                    var existingPropertySyntax = node.GetProperty("Permissions");
                    var newPropertySyntax = CreatePermissionsProperty();
                    if (existingPropertySyntax == null)
                        node = node.AddPropertyListProperties(newPropertySyntax);
                    else
                        node = node.WithPropertyList(
                            node.PropertyList.WithProperties(
                                node.PropertyList.Properties.Replace(
                                    existingPropertySyntax, newPropertySyntax)));
                    NoOfChanges++;
                }

                return node;
            }

            return base.VisitCodeunit(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
            {

                RequiredPermissions.Clear();
                var existingPermissionsValue = node.GetPropertyValue("Permissions") as PermissionPropertyValueSyntax;
                if (existingPermissionsValue != null)
                    CollectExistingPermissions(existingPermissionsValue);
                PermissionsChanged = false;

                CollectPageAndRequestPageSourceTable(node);

                var newNode = base.VisitPage(node);

                node = newNode as PageSyntax;
                if (node == null)
                    return newNode;

                if (PermissionsChanged)
                {
                    var existingPropertySyntax = node.GetProperty("Permissions");
                    var newPropertySyntax = CreatePermissionsProperty();
                    if (existingPropertySyntax == null)
                        node = node.AddPropertyListProperties(newPropertySyntax);
                    else
                        node = node.WithPropertyList(
                            node.PropertyList.WithProperties(
                                node.PropertyList.Properties.Replace(
                                    existingPropertySyntax, newPropertySyntax)));
                    NoOfChanges++;
                }

                return node;
            }

            return base.VisitPage(node);
        }

        public override SyntaxNode VisitRequestPage(RequestPageSyntax node)
        {
            CollectPageAndRequestPageSourceTable(node);
            return base.VisitRequestPage(node);
        }

        private void CollectPageAndRequestPageSourceTable(ObjectSyntax node)
        {
            //collect source table permissions
            var sourceTable = ALSyntaxHelper.DecodeName(node.GetPropertyValue("SourceTable")?.ToString());
            if ((!String.IsNullOrWhiteSpace(sourceTable)) && (!node.CheckIfPropertyValueEquals("SourceTableTemporary", true)))
            {
                ALAppPermissionValue sourceTablePermissionValue = ALAppPermissionValue.R;
                if (!node.CheckIfPropertyValueEquals("Editable", false))
                {
                    if (!node.CheckIfPropertyValueEquals("InsertAllowed", false))
                        sourceTablePermissionValue |= ALAppPermissionValue.I;
                    if (!node.CheckIfPropertyValueEquals("ModifyAllowed", false))
                        sourceTablePermissionValue |= ALAppPermissionValue.M;
                    if (!node.CheckIfPropertyValueEquals("DeleteAllowed", false))
                        sourceTablePermissionValue |= ALAppPermissionValue.D;
                }
                CollectPermission(ALAppPermissionObjectType.TableData, sourceTable, sourceTablePermissionValue);
            }
        }

        public override SyntaxNode VisitReport(ReportSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
            {

                RequiredPermissions.Clear();
                var existingPermissionsValue = node.GetPropertyValue("Permissions") as PermissionPropertyValueSyntax;
                if (existingPermissionsValue != null)
                    CollectExistingPermissions(existingPermissionsValue);
                PermissionsChanged = false;

                var newNode = base.VisitReport(node);

                node = newNode as ReportSyntax;
                if (node == null)
                    return newNode;

                if (PermissionsChanged)
                {
                    var existingPropertySyntax = node.GetProperty("Permissions");
                    var newPropertySyntax = CreatePermissionsProperty();
                    if (existingPropertySyntax == null)
                        node = node.AddPropertyListProperties(newPropertySyntax);
                    else
                        node = node.WithPropertyList(
                            node.PropertyList.WithProperties(
                                node.PropertyList.Properties.Replace(
                                    existingPropertySyntax, newPropertySyntax)));
                    NoOfChanges++;
                }

                return node;
            }

            return base.VisitReport(node);
        }

        public override SyntaxNode VisitQuery(QuerySyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
            {

                RequiredPermissions.Clear();
                var existingPermissionsValue = node.GetPropertyValue("Permissions") as PermissionPropertyValueSyntax;
                if (existingPermissionsValue != null)
                    CollectExistingPermissions(existingPermissionsValue);
                PermissionsChanged = false;

                var newNode = base.VisitQuery(node);

                node = newNode as QuerySyntax;
                if (node == null)
                    return newNode;

                if (PermissionsChanged)
                {
                    var existingPropertySyntax = node.GetProperty("Permissions");
                    var newPropertySyntax = CreatePermissionsProperty();
                    if (existingPropertySyntax == null)
                        node = node.AddPropertyListProperties(newPropertySyntax);
                    else
                        node = node.WithPropertyList(
                            node.PropertyList.WithProperties(
                                node.PropertyList.Properties.Replace(
                                    existingPropertySyntax, newPropertySyntax)));
                    NoOfChanges++;
                }

                return node;
            }

            return base.VisitQuery(node);
        }

        public override SyntaxNode VisitTable(TableSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
            {

                RequiredPermissions.Clear();
                var existingPermissionsValue = node.GetPropertyValue("Permissions") as PermissionPropertyValueSyntax;
                if (existingPermissionsValue != null)
                    CollectExistingPermissions(existingPermissionsValue);
                PermissionsChanged = false;

                var newNode = base.VisitTable(node);

                node = newNode as TableSyntax;
                if (node == null)
                    return newNode;

                if (PermissionsChanged)
                {
                    var existingPropertySyntax = node.GetProperty("Permissions");
                    var newPropertySyntax = CreatePermissionsProperty();
                    if (existingPropertySyntax == null)
                        node = node.AddPropertyListProperties(newPropertySyntax);
                    else
                        node = node.WithPropertyList(
                            node.PropertyList.WithProperties(
                                node.PropertyList.Properties.Replace(
                                    existingPropertySyntax, newPropertySyntax)));
                    NoOfChanges++;
                }

                return node;
            }

            return base.VisitTable(node);
        }

        public override SyntaxNode VisitXmlPort(XmlPortSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
            {

                RequiredPermissions.Clear();
                var existingPermissionsValue = node.GetPropertyValue("Permissions") as PermissionPropertyValueSyntax;
                if (existingPermissionsValue != null)
                    CollectExistingPermissions(existingPermissionsValue);
                PermissionsChanged = false;

                CurrentXmlPortCanImportData = !node.CheckIfPropertyValueEquals("Direction", "Export");

                var newNode = base.VisitXmlPort(node);

                node = newNode as XmlPortSyntax;
                if (node == null)
                    return newNode;

                if (PermissionsChanged)
                {
                    var existingPropertySyntax = node.GetProperty("Permissions");
                    var newPropertySyntax = CreatePermissionsProperty();
                    if (existingPropertySyntax == null)
                        node = node.AddPropertyListProperties(newPropertySyntax);
                    else
                        node = node.WithPropertyList(
                            node.PropertyList.WithProperties(
                                node.PropertyList.Properties.Replace(
                                    existingPropertySyntax, newPropertySyntax)));
                    NoOfChanges++;
                }

                return node;
            }

            return base.VisitXmlPort(node);
        }

        public override SyntaxNode VisitXmlPortTableElement(XmlPortTableElementSyntax node)
        {
            var tableName = ALSyntaxHelper.DecodeName(node.SourceTable?.ToString());
            if ((!String.IsNullOrWhiteSpace(tableName)) && (!node.CheckIfPropertyValueEquals("UseTemporary", true)))
            {
                ALAppPermissionValue permissionValue = ALAppPermissionValue.R;
                if ((CurrentXmlPortCanImportData) && (!node.CheckIfPropertyValueEquals("AutoSave", false)))
                    permissionValue |= (ALAppPermissionValue.M | ALAppPermissionValue.I);
                CollectPermission(ALAppPermissionObjectType.TableData, tableName, permissionValue);
            }

            return base.VisitXmlPortTableElement(node);
        }

        public override SyntaxNode VisitReportDataItem(ReportDataItemSyntax node)
        {
            var tableName = ALSyntaxHelper.DecodeName(node.DataItemTable?.ToString());
            if ((!String.IsNullOrWhiteSpace(tableName)) && (!node.CheckIfPropertyValueEquals("UseTemporary", true)))
                CollectPermission(ALAppPermissionObjectType.TableData, tableName, ALAppPermissionValue.R);

            return base.VisitReportDataItem(node);
        }

        public override SyntaxNode VisitQueryDataItem(QueryDataItemSyntax node)
        {
            var tableName = ALSyntaxHelper.DecodeName(node.DataItemTable?.ToString());
            if (!String.IsNullOrWhiteSpace(tableName))
                CollectPermission(ALAppPermissionObjectType.TableData, tableName, ALAppPermissionValue.R);

            return base.VisitQueryDataItem(node);
        }

        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            var symbolInfo = this.SemanticModel.GetSymbolInfo(node);
            if ((symbolInfo != null) && (symbolInfo.Symbol != null))
            {
                var symbol = symbolInfo.Symbol;
                var kind = symbol.Kind.ConvertToLocalType();

                var parentSymbolInfo = this.SemanticModel.GetSymbolInfo(node.Expression);
                if ((parentSymbolInfo != null) && (parentSymbolInfo.Symbol != null))
                {
                    var type = parentSymbolInfo.Symbol.GetTypeSymbol();
                    if (type != null)
                    {
                        var permissionObjectType = ALAppPermissionObjectTypeExtensions.FromSymbolKind(type.Kind);
                        if (permissionObjectType == ALAppPermissionObjectType.Table)
                        {
                            var recordTypeSymbol = type as IRecordTypeSymbol;
                            if ((recordTypeSymbol == null) || (!recordTypeSymbol.Temporary))
                                CollectPermission(ALAppPermissionObjectType.TableData, type.Name, GetTableDataPermission(symbol, kind));
                        }
                    }
                }
            }
            return base.VisitMemberAccessExpression(node);
        }

        private ALAppPermissionValue GetTableDataPermission(ISymbol symbol, ConvertedSymbolKind kind)
        {
            ALAppPermissionValue tableDataPermission = ALAppPermissionValue.R;

            if (kind == ConvertedSymbolKind.Method)
            {
                IMethodSymbol methodSymbol = symbol as IMethodSymbol;
                if ((methodSymbol != null) && (methodSymbol.MethodKind.ConvertToLocalType() == ConvertedMethodKind.BuiltInMethod))
                {
                    string methodName = symbol.Name.ToLower();
                    switch (methodName)
                    {
                        case "insert":
                            tableDataPermission |= ALAppPermissionValue.I;
                            break;
                        case "modifyall":
                        case "modify":
                            tableDataPermission |= ALAppPermissionValue.M;
                            break;
                        case "deleteall":
                        case "delete":
                            tableDataPermission |= ALAppPermissionValue.D;
                            break;
                    }
                }
            }

            return tableDataPermission;
        }

        private void CollectPermission(ALAppPermissionObjectType permissionObjectType, string objectName, ALAppPermissionValue value)
        {
            RequiredPermissions.Add(new ALAppPermission()
            {
                PermissionObject = permissionObjectType,
                ObjectName = objectName,
                Value = value
            });
            PermissionsChanged = true;
        }

        private void CollectExistingPermissions(PermissionPropertyValueSyntax permissions)
        {
            var alSymbolReferenceCompiler = new ALSymbolReferenceCompiler();
            var permissionsList = alSymbolReferenceCompiler.CreatePermissionsList(permissions);
            if (permissionsList.Count > 0)
                RequiredPermissions.AddRange(permissionsList);
        }

        protected PropertySyntax CreatePermissionsProperty()
        {
            SyntaxTriviaList leadingTriviaList = SyntaxFactory.TriviaList(SyntaxFactory.WhiteSpace("    "));
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.TriviaList(SyntaxFactory.WhiteSpace("\r\n"));

            PermissionPropertyValueSyntax propertyValueSyntax = this.CreatePermissionPropertyValue();
            return SyntaxFactory.Property("Permissions", propertyValueSyntax)
                .WithEqualsToken(CreateEqualsToken())
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }

        protected PropertySyntax UpdatePermissionsProperty(PropertySyntax propertySyntax)
        {
            var propertyValueSyntax = this.CreatePermissionPropertyValue();
            return propertySyntax.WithValue(propertyValueSyntax);
        }

        protected PermissionPropertyValueSyntax CreatePermissionPropertyValue()
        {
            var permissionSyntaxList = this.CreatePermissionsSyntaxList(RequiredPermissions.GetAllPermissions());
            SeparatedSyntaxList<PermissionSyntax> permissions = new SeparatedSyntaxList<PermissionSyntax>();
            permissions = permissions.AddRange(permissionSyntaxList);
            return SyntaxFactory.PermissionPropertyValue(permissions);
        }

        private SyntaxToken CreateEqualsToken()
        {
            SyntaxTriviaList spaceTriviaList = SyntaxFactory.TriviaList(SyntaxFactory.WhiteSpace(" "));
#if BC
            SyntaxToken equalsToken = SyntaxFactoryHelper.Token(spaceTriviaList, "EqualsToken", spaceTriviaList);
#else
            SyntaxToken equalsToken = SyntaxFactoryHelper.Token("EqualsToken").WithLeadingTrivia(spaceTriviaList).WithTrailingTrivia(spaceTriviaList);
#endif
            return equalsToken;
        }

        protected List<PermissionSyntax> CreatePermissionsSyntaxList(IEnumerable<MergedALAppPermission> alAppPermissionsEnumerable)
        {
            SyntaxTriviaList spaceTriviaList = SyntaxFactory.TriviaList(SyntaxFactory.WhiteSpace(" "));
            SyntaxTriviaList leadingTriviaList = SyntaxFactory.TriviaList(SyntaxFactory.WhiteSpace("\r\n"), SyntaxFactory.WhiteSpace("        "));
            SyntaxToken equalsToken = CreateEqualsToken();

            List<PermissionSyntax> permissionsList = new List<PermissionSyntax>();
            foreach (var alAppPermission in alAppPermissionsEnumerable)
            {
                var permissionValueSyntax = SyntaxFactory.Identifier(alAppPermission.Value.ToALString());
                var objectTypeSyntax = this.CreateObjectTypeToken(alAppPermission.PermissionObject);
                var objectNameSyntax = SyntaxFactory.ObjectNameOrId(SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(alAppPermission.ObjectName)))
                    .WithLeadingTrivia(spaceTriviaList);
#if BC
                PermissionSyntax permissionSyntax = SyntaxFactory.Permission(objectTypeSyntax, objectNameSyntax, default(SyntaxToken), equalsToken, permissionValueSyntax)
                    .WithLeadingTrivia(leadingTriviaList);
#else
                PermissionSyntax permissionSyntax = SyntaxFactory.Permission(objectTypeSyntax, objectNameSyntax, equalsToken, permissionsTokenSyntax)
                    .WithLeadingTrivia(leadingTriviaList);
#endif

                permissionsList.Add(permissionSyntax);
            }

            permissionsList.Sort(new PermissionComparer());
            return permissionsList;
        }

        protected SyntaxToken CreateObjectTypeToken(ALAppPermissionObjectType type)
        {
            switch (type)
            {
                case ALAppPermissionObjectType.Table:
                    return SyntaxFactoryHelper.Token("TableKeyword");
                case ALAppPermissionObjectType.TableData:
                    return SyntaxFactoryHelper.Token("TableDataKeyword");
                case ALAppPermissionObjectType.Page:
                    return SyntaxFactoryHelper.Token("PageKeyword");
                case ALAppPermissionObjectType.Codeunit:
                    return SyntaxFactoryHelper.Token("CodeunitKeyword");
                case ALAppPermissionObjectType.XmlPort:
                    return SyntaxFactoryHelper.Token("XmlPortKeyword");
                case ALAppPermissionObjectType.Report:
                    return SyntaxFactoryHelper.Token("ReportKeyword");
                case ALAppPermissionObjectType.Query:
                    return SyntaxFactoryHelper.Token("QueryKeyword");
            }
            return SyntaxFactoryHelper.Token("CodeunitKeyword");
        }

    }

#endif

}
