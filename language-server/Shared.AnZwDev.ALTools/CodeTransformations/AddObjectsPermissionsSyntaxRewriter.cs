using AnZwDev.ALTools.ALSymbolReferences.MergedPermissions;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class AddObjectsPermissionsSyntaxRewriter : ALSyntaxRewriter
    {

        public bool ExcludePermissionsFromIncludedPermissionSets { get; set; } = true;
        public bool ExcludePermissionsFromExcludedPermissionSets { get; set; } = true;

        public AddObjectsPermissionsSyntaxRewriter()
        {
        }

#if BC

        public override SyntaxNode VisitPermissionSet(PermissionSetSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
            {
                (bool isPermissionSet, string includedPermissionSetNames, string excludedPermissionSetNames) =
                    GetPermissionSetDetails(node);

                PropertyListSyntax properties = this.UpdateProperties(node.PropertyList, isPermissionSet, includedPermissionSetNames, excludedPermissionSetNames);
                return node.WithPropertyList(properties);
            }
            return base.VisitPermissionSet(node);
        }

        public override SyntaxNode VisitPermissionSetExtension(PermissionSetExtensionSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
            {
                (bool isPermissionSet, string includedPermissionSetNames, string excludedPermissionSetNames) =
                    GetPermissionSetDetails(node);

                PropertyListSyntax properties = this.UpdateProperties(node.PropertyList, isPermissionSet, includedPermissionSetNames, excludedPermissionSetNames);
                return node.WithPropertyList(properties);
            }
            return base.VisitPermissionSetExtension(node);
        }

#endif

        public override SyntaxNode VisitPermissionPropertyValue(PermissionPropertyValueSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
            {
                ApplicationObjectSyntax applicationObjectSyntax = node.GetContainingApplicationObjectSyntax();

                (bool isPermissionSet, string includedPermissionSetNames, string excludedPermissionSetNames) =
                    GetPermissionSetDetails(applicationObjectSyntax);

                node = this.UpdatePermissionPropertyValue(node, isPermissionSet, includedPermissionSetNames, excludedPermissionSetNames);
            }
            return base.VisitPermissionPropertyValue(node);
        }

        protected PropertyListSyntax UpdateProperties(PropertyListSyntax properties, bool isPermissionSet, string includedPermissioSetNames, string excludedPermissionSetNames)
        {
            PropertySyntax permissionsPropertySyntax = properties.GetPropertyEntry("Permissions");
            if (permissionsPropertySyntax == null)
            {
                permissionsPropertySyntax = this.CreatePermissionsProperty(isPermissionSet, includedPermissioSetNames, excludedPermissionSetNames);
                properties = properties.AddProperties(permissionsPropertySyntax);
            } 
            else
            {
                PropertySyntax newProperty = this.UpdatePermissionsProperty(permissionsPropertySyntax, isPermissionSet, includedPermissioSetNames, excludedPermissionSetNames);
                properties = properties.WithProperties(properties.Properties.Replace(permissionsPropertySyntax, newProperty));
            }
            return properties;
        }

        protected PropertySyntax CreatePermissionsProperty(bool isPermissionSet, string includedPermissioSetNames, string excludedPermissionSetNames)
        {
            PermissionPropertyValueSyntax propertyValueSyntax = this.CreatePermissionPropertyValue(isPermissionSet, includedPermissioSetNames, excludedPermissionSetNames);
            return SyntaxFactory.Property("Permissions", propertyValueSyntax);
        }

        protected PropertySyntax UpdatePermissionsProperty(PropertySyntax propertySyntax, bool isPermissionSet, string includedPermissioSetNames, string excludedPermissionSetNames)
        {
            PermissionPropertyValueSyntax propertyValueSyntax = propertySyntax.Value as PermissionPropertyValueSyntax;
            if (propertyValueSyntax != null)
                propertyValueSyntax = this.UpdatePermissionPropertyValue(propertyValueSyntax, isPermissionSet, includedPermissioSetNames, excludedPermissionSetNames);
            else
                propertyValueSyntax = this.CreatePermissionPropertyValue(isPermissionSet, includedPermissioSetNames, excludedPermissionSetNames);
            return propertySyntax.WithValue(propertyValueSyntax);            
        }

        protected PermissionPropertyValueSyntax UpdatePermissionPropertyValue(PermissionPropertyValueSyntax node, bool isPermissionSet, string includedPermissioSetNames, string excludedPermissionSetNames)
        {
            //collect existing permissions
            HashSet<string> existingPermissions = new HashSet<string>();
            if (node.PermissionProperties != null)
            {
                foreach (PermissionSyntax permissionSyntax in node.PermissionProperties)
                {
                    if ((permissionSyntax.ObjectType != null) &&
                        (permissionSyntax.ObjectReference != null) &&
                        (permissionSyntax.ObjectReference.Identifier != null))
                    {
                        string objectUID = this.CreateObjectUID(
                            ALSyntaxHelper.DecodeName(permissionSyntax.ObjectType.ToString()),
                            ALSyntaxHelper.DecodeName(permissionSyntax.ObjectReference.Identifier.ToString()));
                        if (objectUID != null)
                            existingPermissions.Add(objectUID);
                    }
                }
            }
            existingPermissions = CollectIncludedAndExcludedPermissions(existingPermissions, includedPermissioSetNames, excludedPermissionSetNames);

            List<PermissionSyntax> permissionSyntaxList = this.CreatePermissions(existingPermissions, isPermissionSet);
            return node.AddPermissionProperties(permissionSyntaxList.ToArray());
        }

        protected PermissionPropertyValueSyntax CreatePermissionPropertyValue(bool isPermissionSet, string includedPermissionSetsNames, string excludedPermissionSetsNames)
        {
            HashSet<string> existingPermissions = CollectIncludedAndExcludedPermissions(null, includedPermissionSetsNames, excludedPermissionSetsNames);
            List<PermissionSyntax> permissionSyntaxList = this.CreatePermissions(existingPermissions, isPermissionSet);
            SeparatedSyntaxList<PermissionSyntax> permissions = new SeparatedSyntaxList<PermissionSyntax>();
            permissions = permissions.AddRange(permissionSyntaxList);
            return SyntaxFactory.PermissionPropertyValue(permissions);
        }

        protected string CreateObjectUID(string type, string name)
        {
            if ((String.IsNullOrWhiteSpace(type)) || (String.IsNullOrWhiteSpace(name)))
                return null;
            return type.ToLower() + "_" + name.ToLower();
        }

        protected HashSet<string> CollectIncludedAndExcludedPermissions(HashSet<string> existingPermissions, string includedPermissionSetsNames, string excludedPermissionSetsNames)
        {
            if (
                ((ExcludePermissionsFromIncludedPermissionSets) && (!String.IsNullOrWhiteSpace(includedPermissionSetsNames))) ||
                ((ExcludePermissionsFromExcludedPermissionSets) && (!String.IsNullOrWhiteSpace(excludedPermissionSetsNames))))
            {
                var informationProvider = new PermissionSetInformationProvider();
                var permissionSet = informationProvider.GetMergedPermissionSet(Project, includedPermissionSetsNames, excludedPermissionSetsNames);
                
                if (ExcludePermissionsFromIncludedPermissionSets)
                    CollectExistingPermissions(existingPermissions, permissionSet.IncludedPermissions?.GetAllPermissions());
                if (ExcludePermissionsFromExcludedPermissionSets)
                    CollectExistingPermissions(existingPermissions, permissionSet.ExcludedPermissions?.GetAllPermissions());
            }

            return existingPermissions;
        }

        protected HashSet<string> CollectExistingPermissions(HashSet<string> existingPermissions, IEnumerable<MergedALAppPermission> sourcePermissions)
        {
            if (sourcePermissions != null)
                foreach (var permission in sourcePermissions)
                    if ((!String.IsNullOrWhiteSpace(permission.ObjectName)) && (permission.ObjectName != "*"))
                    {
                        string objectUID = this.CreateObjectUID(
                                permission.PermissionObject.ToString(),
                                permission.ObjectName);

                        if (existingPermissions == null)
                            existingPermissions = new HashSet<string>();

                        if (!existingPermissions.Contains(objectUID))
                            existingPermissions.Add(objectUID);
                    }

            return existingPermissions;
        }

        protected List<PermissionSyntax> CreatePermissions(HashSet<string> existingPermissions, bool isPermissionSet)
        {
            HashSet<ALSymbolKind> includeObjects = new HashSet<ALSymbolKind>();
            if (isPermissionSet)
                includeObjects.AddObjectsWithPermissions();
            else
                includeObjects.Add(ALSymbolKind.TableObject);

            ObjectInformationProvider provider = new ObjectInformationProvider();
            List<ObjectInformation> objectsList = provider.GetProjectObjects(this.Project, includeObjects, false, false);

            SyntaxTriviaList leadingTriviaList = SyntaxFactory.TriviaList(SyntaxFactory.WhiteSpace("\r\n"), SyntaxFactory.WhiteSpace("        "));
            SyntaxTriviaList spaceTriviaList = SyntaxFactory.TriviaList(SyntaxFactory.WhiteSpace(" "));

#if BC
            SyntaxToken equalsToken = SyntaxFactoryHelper.Token(spaceTriviaList, "EqualsToken", spaceTriviaList);
#else
            SyntaxToken equalsToken = SyntaxFactoryHelper.Token("SyntaxKind.EqualsToken").WithLeadingTrivia(spaceTriviaList).WithTrailingTrivia(spaceTriviaList);
#endif

            SyntaxToken executePermissions = SyntaxFactory.Identifier("X");
            SyntaxToken tableDataPermissions = SyntaxFactory.Identifier("RMID");

            List<PermissionSyntax> permissionsList = new List<PermissionSyntax>();
            foreach (ObjectInformation objectInformation in objectsList)
            {
                //for permission sets all object types are allowed, for all other types only tabledata can be used
                if (isPermissionSet)
                    this.AddPermission(existingPermissions, permissionsList, objectInformation.Type, objectInformation.Name, equalsToken, executePermissions, leadingTriviaList, spaceTriviaList);

                if ((objectInformation.Type != null) && (objectInformation.Type.Equals("table", StringComparison.CurrentCultureIgnoreCase)))
                    this.AddPermission(existingPermissions, permissionsList, "tabledata", objectInformation.Name, equalsToken, tableDataPermissions, leadingTriviaList, spaceTriviaList);
            }

            permissionsList.Sort(new PermissionComparer());
            return permissionsList;
        }

        protected void AddPermission(HashSet<string> existingPermissions, List<PermissionSyntax> permissionsList, string type, string name, SyntaxToken equalsToken, SyntaxToken permissionsToken, SyntaxTriviaList leadingTriviaList, SyntaxTriviaList spaceTriviaList)
        {
            string uid = this.CreateObjectUID(type, name);
            if ((uid == null) || ((existingPermissions != null) && (existingPermissions.Contains(uid))))
                return;

            SyntaxToken objectType = this.CreateObjectTypeToken(type);
            ObjectNameOrIdSyntax objectName = SyntaxFactory.ObjectNameOrId(SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(name)))
                .WithLeadingTrivia(spaceTriviaList);
#if BC
            PermissionSyntax permission = SyntaxFactory.Permission(objectType, objectName, default(SyntaxToken), equalsToken, permissionsToken)
                .WithLeadingTrivia(leadingTriviaList);
#else
            PermissionSyntax permission = SyntaxFactory.Permission(objectType, objectName, equalsToken, permissionsToken)
                .WithLeadingTrivia(leadingTriviaList);
#endif

            permissionsList.Add(permission);
        }

        protected SyntaxToken CreateObjectTypeToken(string type)
        {
            if (type != null)
            {
                type = type.ToLower();
                switch (type)
                {
                    case "table":
                        return SyntaxFactoryHelper.Token("TableKeyword");
                    case "tabledata":
                        return SyntaxFactoryHelper.Token("TableDataKeyword");
                    case "page":
                        return SyntaxFactoryHelper.Token("PageKeyword");
                    case "codeunit":
                        return SyntaxFactoryHelper.Token("CodeunitKeyword");
                    case "xmlport":
                        return SyntaxFactoryHelper.Token("XmlPortKeyword");
                    case "report":
                        return SyntaxFactoryHelper.Token("ReportKeyword");
                    case "query":
                        return SyntaxFactoryHelper.Token("QueryKeyword");
                }
            }
            return SyntaxFactoryHelper.Token("CodeunitKeyword");
        }

        protected (bool, string, string) GetPermissionSetDetails(ApplicationObjectSyntax node)
        {
            string includedPermissionSetsNames = null;
            string excludedPermissionSetsNames = null;
            bool isPermissionSetObject = false;
            if (node != null)
            {
                ConvertedSyntaxKind kind = node.Kind.ConvertToLocalType();
                isPermissionSetObject = ((kind == ConvertedSyntaxKind.PermissionSet) || (kind == ConvertedSyntaxKind.PermissionSetExtension));
#if BC
                if (isPermissionSetObject)
                {
                    includedPermissionSetsNames = node.GetPropertyValue("IncludedPermissionSets")?.ToString();
                    excludedPermissionSetsNames = node.GetPropertyValue("ExcludedPermissionSets")?.ToString();
                }
#endif
            }
            return (isPermissionSetObject, includedPermissionSetsNames, excludedPermissionSetsNames);
        }


    }
}
