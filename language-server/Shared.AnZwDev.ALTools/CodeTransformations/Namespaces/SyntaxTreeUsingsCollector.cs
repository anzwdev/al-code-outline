using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolReferences;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace AnZwDev.ALTools.CodeTransformations.Namespaces
{

#if BC

    internal class SyntaxTreeUsingsCollector : SyntaxRewriter
    {

        protected FileNamespacesInformation _namespacesInformation;
        protected HashSet<string> _usings;
        protected HashSet<string> _usingsAndNamespace;

        public FileNamespacesInformation CollectUsings(ALProject project, CompilationUnitSyntax node)
        {
            return CollectUsings(project, node.SyntaxTree, node.SyntaxTree?.FilePath, node);
        }

        public FileNamespacesInformation CollectUsings(ALProject project, SyntaxTree syntaxTree, string filePath)
        {
            CompilationUnitSyntax node = syntaxTree.GetRoot() as CompilationUnitSyntax;
            if (node == null)
                return null;
            return CollectUsings(project, syntaxTree, filePath, node);
        }

        private FileNamespacesInformation CollectUsings(ALProject project, SyntaxTree syntaxTree, string filePath, CompilationUnitSyntax node)
        {
            _namespacesInformation = new FileNamespacesInformation(project, filePath, syntaxTree);
            _usings = node.Usings.GetUsingsNamespacesNames();

            _usingsAndNamespace = new HashSet<string>();
            _usingsAndNamespace.AddRange(_usings);

            _namespacesInformation.Namespace = node.NamespaceDeclaration?.Name?.ToString();
            _namespacesInformation.AddFileUsings(_usings);

            if ((!String.IsNullOrWhiteSpace(_namespacesInformation.Namespace)) && (!_usingsAndNamespace.Contains(_namespacesInformation.Namespace)))
                _usingsAndNamespace.Add(_namespacesInformation.Namespace);

            this.Visit(node);

            return _namespacesInformation;
        }


        public override SyntaxNode VisitTable(TableSyntax node)
        {
            AddType(ALObjectType.Page, node.GetPropertyValue("LookupPageId")?.ToString());
            AddType(ALObjectType.Page, node.GetPropertyValue("DrillDownPageId")?.ToString());
            AddPermissions(node.GetPropertyValue("Permissions") as PermissionPropertyValueSyntax);

            return base.VisitTable(node);
        }

        public override SyntaxNode VisitTableExtension(TableExtensionSyntax node)
        {
            AddType(ALObjectType.Table, node.BaseObject?.ToString());
            
            return base.VisitTableExtension(node);
        }

        public override SyntaxNode VisitField(FieldSyntax node)
        {
            return base.VisitField(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            AddType(ALObjectType.Table, node.GetPropertyValue("SourceTable")?.ToString());
            AddType(ALObjectType.Page, node.GetPropertyValue("CardPageId")?.ToString());
            AddPermissions(node.GetPropertyValue("Permissions") as PermissionPropertyValueSyntax);

            return base.VisitPage(node);
        }

        public override SyntaxNode VisitPageExtension(PageExtensionSyntax node)
        {
            AddType(ALObjectType.Page, node.BaseObject?.ToString());
            AddType(ALObjectType.Page, node.GetPropertyValue("CardPageId")?.ToString());

            return base.VisitPageExtension(node);
        }

        public override SyntaxNode VisitCodeunit(CodeunitSyntax node)
        {
            AddType(ALObjectType.Table, node.GetPropertyValue("TableNo")?.ToString());
            AddImplementedInterfaces(node.Interfaces);

            return base.VisitCodeunit(node);
        }

        public override SyntaxNode VisitReportExtension(ReportExtensionSyntax node)
        {
            AddType(ALObjectType.Report, node.BaseObject?.ToString());

            return base.VisitReportExtension(node);
        }

        public override SyntaxNode VisitEnumType(EnumTypeSyntax node)
        {
            AddImplementedInterfaces(node.Interfaces);

            return base.VisitEnumType(node);
        }

        public override SyntaxNode VisitEnumExtensionType(EnumExtensionTypeSyntax node)
        {
            AddType(ALObjectType.EnumType, node.BaseObject?.ToString());

            return base.VisitEnumExtensionType(node);
        }

        public override SyntaxNode VisitPermissionSet(PermissionSetSyntax node)
        {
            AddPermissionSetsListProperty(node.GetPropertyValue("IncludedPermissionSets") as CommaSeparatedObjectNameReferencesPropertyValueSyntax);
            AddPermissionSetsListProperty(node.GetPropertyValue("ExcludedPermissionSets") as CommaSeparatedObjectNameReferencesPropertyValueSyntax);

            return base.VisitPermissionSet(node);
        }

        public override SyntaxNode VisitPermissionSetExtension(PermissionSetExtensionSyntax node)
        {
            AddPermissionSetsListProperty(node.GetPropertyValue("IncludedPermissionSets") as CommaSeparatedObjectNameReferencesPropertyValueSyntax);
            AddPermissionSetsListProperty(node.GetPropertyValue("ExcludedPermissionSets") as CommaSeparatedObjectNameReferencesPropertyValueSyntax);
            AddType(ALObjectType.PermissionSet, node.BaseObject?.ToString());

            return base.VisitPermissionSetExtension(node);
        }

        public override SyntaxNode VisitTableRelationPropertyValue(TableRelationPropertyValueSyntax node)
        {
            var field = node.RelatedTableField?.ToString();
            AddFieldReference(field);

            //var type = node.GetType().Name;
            return base.VisitTableRelationPropertyValue(node);
        }

        public override SyntaxNode VisitFieldCalculationFormula(FieldCalculationFormulaSyntax node)
        {
            return base.VisitFieldCalculationFormula(node);
        }

        public override SyntaxNode VisitTableCalculationFormula(TableCalculationFormulaSyntax node)
        {
            AddType(ALObjectType.Table, node.Table.ToString());

            return base.VisitTableCalculationFormula(node);
        }

        public override SyntaxNode VisitPermissionPropertyValue(PermissionPropertyValueSyntax node)
        {
            AddPermissions(node);
            return base.VisitPermissionPropertyValue(node);
        }

        public override SyntaxNode VisitEnumValue(EnumValueSyntax node)
        {
            var implementation = node.GetPropertyValue("Implementation") as CommaSeparatedIdentifierEqualsIdentifierListPropertyValueSyntax;
            if (implementation?.List?.Values != null)
                foreach (var implementationEntry in implementation.List.Values)
                {
                    AddType(ALObjectType.Interface, implementationEntry.LeftIdentifier?.ToString());
                    AddType(ALObjectType.Codeunit, implementationEntry.RightIdentifier?.ToString());
                }

            return base.VisitEnumValue(node);
        }

        public override SyntaxNode VisitReportDataItem(ReportDataItemSyntax node)
        {
            AddType(ALObjectType.Table, node.DataItemTable?.ToString());
            
            return base.VisitReportDataItem(node);
        }

        public override SyntaxNode VisitQueryDataItem(QueryDataItemSyntax node)
        {
            AddType(ALObjectType.Table, node.DataItemTable?.ToString());

            return base.VisitQueryDataItem(node);
        }

        public override SyntaxNode VisitXmlPortTableElement(XmlPortTableElementSyntax node)
        {
            AddType(ALObjectType.Table, node.SourceTable?.ToString());

            return base.VisitXmlPortTableElement(node);
        }

        public override SyntaxNode VisitOptionAccessExpression(OptionAccessExpressionSyntax node)
        {
            var value = node.Name?.ToString();
            var expression = node.Expression?.ToString();
            if (!String.IsNullOrWhiteSpace(expression) && (!String.IsNullOrWhiteSpace(value)))
                AddOptionAccessType(expression, value);

            return base.VisitOptionAccessExpression(node);
        }

        private void AddOptionAccessType(string expression, string value)
        {
            var objectType = OptionNameToObjectType(expression);
            if (objectType != ALObjectType.None)
                AddType(objectType, value);
        }

        private ALObjectType OptionNameToObjectType(string type)
        {
            type = type.ToLower();
            switch (type)
            {
                case "database":
                    return ALObjectType.Table;
                case "page":
                    return ALObjectType.Page;
                case "codeunit":
                    return ALObjectType.Codeunit;
                case "xmlport":
                    return ALObjectType.XmlPort;
                case "report":
                    return ALObjectType.Report;
                case "query":
                    return ALObjectType.Query;
            }
            return ALObjectType.None;
        }

        private void AddPermissionSetsListProperty(CommaSeparatedObjectNameReferencesPropertyValueSyntax permissionSetsList)
        {
            if (permissionSetsList?.Values == null)
                return;
            foreach (var permissionSet in permissionSetsList.Values)
                AddType(ALObjectType.PermissionSet, permissionSet.ToString());
        }

        private void AddPermissions(PermissionPropertyValueSyntax permissionPropertyValue)
        {
            if (permissionPropertyValue?.PermissionProperties == null)
                return;

            foreach (var permissionProperty in permissionPropertyValue.PermissionProperties) 
            {
                var objectType = permissionProperty.ObjectType.ToString();
                var objectName = permissionProperty.ObjectReference.ToString();
                if ((!String.IsNullOrWhiteSpace(objectType)) && (!String.IsNullOrWhiteSpace(objectName)))
                {
                    if ((objectType.Equals("TableData", StringComparison.OrdinalIgnoreCase)) ||
                        (objectType.Equals("Table", StringComparison.OrdinalIgnoreCase)))
                        objectType = "Record";
                }
                AddType(objectType, objectName);
            }
        }

        private void AddImplementedInterfaces(SeparatedSyntaxList<ObjectNameReferenceSyntax> interfacesList)
        {
            if (interfacesList != null)
                foreach (var codeunitInterface in interfacesList)
                    AddType(ALObjectType.Interface, codeunitInterface.ToString());
        }


        public override SyntaxNode VisitSubtypedDataType(SubtypedDataTypeSyntax node)
        {
            string typeName = node.TypeName.ValueText;
            string subtypeName = node.Subtype.ToString();
            AddType(typeName, subtypeName);
            return base.VisitSubtypedDataType(node);
        }

        public override SyntaxNode VisitEnumDataType(EnumDataTypeSyntax node)
        {
            string typeName = node.TypeName.ValueText;
            string enumName = node.EnumTypeName.ToString();
            AddType(typeName, enumName);
            return base.VisitEnumDataType(node);
        }

        private void AddFieldReference(string fieldReference)
        {
            if (!String.IsNullOrWhiteSpace(fieldReference))
            {
                //try to find object, maybe reference is just a <table> or <namespaces>.<table>?
                var alObjectReference = new ALObjectReference(_usings, fieldReference);
                var alObject = _namespacesInformation.Project
                    .SymbolsWithDependencies
                    .Tables
                    .FindFirst(alObjectReference);
                    
                //object not found, maybe it was <table>.<field> or <namespaces>.<table>.<field>?
                if ((alObject == null) && (alObjectReference.HasNamespace))
                {
                    alObjectReference = new ALObjectReference(_usings, alObjectReference.NamespaceName);
                    alObject = _namespacesInformation.Project
                        .SymbolsWithDependencies
                        .Tables
                        .FindFirst(alObjectReference);
                }

                if (alObject != null)
                    AddObject(alObject, alObjectReference);
            }
        }

        private void AddType(string typeName, string subtypeName)
        {
            if (!String.IsNullOrWhiteSpace(typeName))
            {
                var alObjectTypeInformation = ALObjectTypesInformationCollection.GetForVariableTypeName(typeName);
                if ((alObjectTypeInformation != null) && (alObjectTypeInformation.ALObjectType != ALObjectType.None))
                    AddType(alObjectTypeInformation.ALObjectType, subtypeName);
            }
        }

        private void AddType(ALObjectType alObjectType, string subtypeName)
        { 
            if (!String.IsNullOrWhiteSpace(subtypeName))
            {
                var alObjectReference = new ALObjectReference(_usings, subtypeName);
                var alObject = _namespacesInformation.Project
                    .SymbolsWithDependencies
                    .FindFirst(alObjectType, alObjectReference);
                if (alObject != null)
                    AddObject(alObject, alObjectReference);
            }
        }

        private void AddObject(ALAppObject alObject, ALObjectReference alObjectReference)
        {
            var hasNamespace = alObjectReference.HasNamespace;
            _namespacesInformation.AddNamespaceReference(
                alObject.NamespaceName,
                alObject.ReferenceSourceFileName,
                false,
                hasNamespace,
                !hasNamespace);
        }

    }

#endif

}
