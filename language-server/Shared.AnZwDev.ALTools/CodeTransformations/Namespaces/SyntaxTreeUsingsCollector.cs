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

namespace AnZwDev.ALTools.CodeTransformations.Namespaces
{

#if BC

    internal class SyntaxTreeUsingsCollector : SyntaxRewriter
    {

        public ALProject Project { get; set; }
        public Dictionary<string, UsingInformation> UsingInformationCollection { get; private set; }            
        public HashSet<string> UsingsNamespacesNames { get; private set; }

        public void CollectUsings(CompilationUnitSyntax node)
        {
            UsingInformationCollection = new Dictionary<string, UsingInformation>();
            UsingsNamespacesNames = node.Usings.GetUsingsNamespacesNames();

            if (UsingsNamespacesNames != null)
                foreach (var namespaceName in UsingsNamespacesNames)
                    AddUsing(namespaceName, null, true, false, false);

            this.Visit(node);
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

        private void AddType(string typeName, string subtypeName)
        {
            var alObjectReference = new ALObjectReference(UsingsNamespacesNames, subtypeName);
            var alObjectTypeInformation = ALObjectTypesInformationCollection.GetForVariableTypeName(typeName);
            if ((alObjectTypeInformation != null) && (alObjectTypeInformation.ALObjectType != ALObjectType.None))
            {
                var alObject = Project
                    .GetAllSymbolReferences()
                    .GetAllObjects(alObjectTypeInformation.ALObjectType)
                    .FindFirst(alObjectReference);

                if (alObject != null)
                {
                    var hasNamespace = alObjectReference.HasNamespace();
                    AddUsing(alObject.NamespaceName, alObject.ReferenceSourceFileName, false, hasNamespace, !hasNamespace);
                }
            }
        }

        private void AddUsing(string namespaceName, string filePath, bool inUsing, bool inVariableDeclaration, bool usingRequired)
        {
            UsingInformation usingInformation = new UsingInformation(namespaceName, filePath, inUsing, inVariableDeclaration, usingRequired);
            if (UsingInformationCollection.ContainsKey(usingInformation.Id))
                UsingInformationCollection[usingInformation.Id].Merge(usingInformation);
            else
                UsingInformationCollection.Add(usingInformation.Id, usingInformation);
        }

    }

#endif

}
