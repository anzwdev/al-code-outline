using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Logging;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbols.Internal;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace AnZwDev.ALTools.ALSymbols.SymbolReaders
{
    public class ALSymbolInfoSyntaxTreeReader
    {

        public bool IncludeProperties { get; set; }
        private bool _tableHasKeys = false;
        private ALSymbolAccessModifier? _varAccessModifier = null;

        public ALSymbolInfoSyntaxTreeReader(bool includeProperties)
        {
            this.IncludeProperties = includeProperties;
        }

        #region Main processing

        public ALSymbol ProcessSourceFile(string path)
        {
            try
            {
                return ProcessSourceCode(System.IO.File.ReadAllText(path));
            }
            catch (Exception e)
            {
                MessageLog.LogError(e);
                return new ALSymbol(ALSymbolKind.Undefined, "LangServer Error: " + e.Message);
            }
        }

        public ALSymbol ProcessSourceCodeAndUpdateActiveDocument(string path, string source, ALWorkspace workspace, bool updateActiveDocument)
        {
            SyntaxTree syntaxTree = SyntaxTreeExtensions.SafeParseObjectText(source);
            if (updateActiveDocument)
                workspace.ActiveDocument.Update(path, syntaxTree);
            return ProcessSyntaxTree(syntaxTree);
        }

        private ALSymbol ProcessSourceCode(string source)
        {
            SyntaxTree syntaxTree = SyntaxTreeExtensions.SafeParseObjectText(source);
            return ProcessSyntaxTree(syntaxTree);
        }

        public ALSymbol ProcessSyntaxTree(SyntaxTree syntaxTree)
        {
            SyntaxNode node = syntaxTree.GetRoot();
            
            ALRegionDirective firstRegionDirective = CollectRegionDirectivesPositions(syntaxTree, node);
            ALSymbol root = new ALSymbol();
            ProcessSyntaxNode(root, root, firstRegionDirective, syntaxTree, null, node);           
            if ((root.childSymbols != null) && (root.childSymbols.Count == 1))
                root = root.childSymbols[0];
            root.UpdateFields();
            root.RemoveEmptyChildNodes();

            return root;
        }

        #endregion

        #region Processing syntax nodes

        protected (ALSymbol, ALRegionDirective) ProcessSyntaxNode(ALSymbol parentSymbol, ALSymbol parentSymbolOrRegion, ALRegionDirective currentRegion, SyntaxTree syntaxTree, SyntaxNode parentNode, SyntaxNode node)
        {
            var symbol = CreateSymbol(syntaxTree, parentNode, node);
            if (symbol != null)
            {
                (parentSymbolOrRegion, currentRegion) = AddChildSymbol(parentSymbolOrRegion, currentRegion, symbol);
                (var childNodesParentSymbolOrRegion, var childNodesCurrentRegion) = ProcessChildSyntaxNodesCollection(symbol, symbol, currentRegion, syntaxTree, node, node.ChildNodes());
                currentRegion = AddRemainingSymbolRegions(symbol, childNodesParentSymbolOrRegion, childNodesCurrentRegion);
            }
            return (parentSymbolOrRegion, currentRegion);
        }

        protected (ALSymbol, ALRegionDirective) ProcessChildSyntaxNodesCollection(ALSymbol symbol, ALSymbol parentSymbolOrRegion, ALRegionDirective currentRegion, SyntaxTree syntaxTree, SyntaxNode node, IEnumerable<SyntaxNode> childNodesCollection)
        {
            foreach (var childNode in childNodesCollection)
                (parentSymbolOrRegion, currentRegion) = ProcessChildSyntaxNode(symbol, parentSymbolOrRegion, currentRegion, syntaxTree, node, childNode);
            return (parentSymbolOrRegion, currentRegion);
        }

        protected (ALSymbol, ALRegionDirective) ProcessChildSyntaxNode(ALSymbol symbol, ALSymbol parentSymbolOrRegion, ALRegionDirective currentRegion, SyntaxTree syntaxTree, SyntaxNode parentNode, SyntaxNode childNode)
        {
            if (!ProcessChildNodeAsSymbolProperty(symbol, syntaxTree, childNode))
            {
                bool merged;
                (merged, parentSymbolOrRegion, currentRegion) = MergeWithChildNodes(symbol, parentSymbolOrRegion, currentRegion, syntaxTree, parentNode, childNode);
                if (!merged)
                    (parentSymbolOrRegion, currentRegion) = ProcessSyntaxNode(symbol, parentSymbolOrRegion, currentRegion, syntaxTree, parentNode, childNode);
            }

            return (parentSymbolOrRegion, currentRegion);
        }

        protected (bool, ALSymbol, ALRegionDirective) MergeWithChildNodes(ALSymbol symbol, ALSymbol parentSymbolOrRegion, ALRegionDirective currentRegion, SyntaxTree syntaxTree, SyntaxNode parentNode, SyntaxNode childNode)
        {
#if BC
            switch (childNode)
            {
                case VariableListDeclarationSyntax variableListDeclarationSyntax:
                    (parentSymbolOrRegion, currentRegion) = ProcessChildSyntaxNodesCollection(
                        symbol, parentSymbolOrRegion, currentRegion, syntaxTree, variableListDeclarationSyntax, variableListDeclarationSyntax.VariableNames);
                    return (true, parentSymbolOrRegion, currentRegion);
            }
#endif
            return (false, parentSymbolOrRegion, currentRegion);
        }

        protected (ALSymbol, ALRegionDirective) AddChildSymbol(ALSymbol parentSymbolOrRegion, ALRegionDirective currentRegion, ALSymbol childSymbol)
        {
            //go up or add regions closed before symbol
            (parentSymbolOrRegion, currentRegion) = CloseOrAddRegionsBeforePosition(parentSymbolOrRegion, currentRegion, childSymbol);

            //add regions opened before the symbol
            parentSymbolOrRegion.AddChildSymbol(childSymbol);

            return (parentSymbolOrRegion, currentRegion);
        }

        protected ALRegionDirective AddRemainingSymbolRegions(ALSymbol parentSymbol, ALSymbol parentSymbolOrRegion, ALRegionDirective currentRegionDirective)
        {
            while ((currentRegionDirective?.Next != null) && (currentRegionDirective.Next.SelectionRange.start.IsLowerOrEqual(parentSymbol.range.end)))
            {
                currentRegionDirective = currentRegionDirective.Next;
                parentSymbolOrRegion = ProcessRegionDirective(parentSymbolOrRegion, currentRegionDirective);
            }
            return currentRegionDirective;
        }

        protected (ALSymbol, ALRegionDirective) CloseOrAddRegionsBeforePosition(ALSymbol parentSymbolOrRegion, ALRegionDirective currentRegionDirective, ALSymbol childSymbol)
        {
            while ((currentRegionDirective?.Next != null) && (currentRegionDirective.Next.SelectionRange.start.IsLowerOrEqual(childSymbol.selectionRange.start)))
            {
                currentRegionDirective = currentRegionDirective.Next;
                parentSymbolOrRegion = ProcessRegionDirective(parentSymbolOrRegion, currentRegionDirective);
            }

            return (parentSymbolOrRegion, currentRegionDirective);
        }

        protected ALSymbol ProcessRegionDirective(ALSymbol parentSymbolOrRegion, ALRegionDirective currentRegionDirective)
        {
            if (currentRegionDirective.IsStartRegion)
            {
                var newRegion = new ALSymbol(ALSymbolKind.Region, "#region")
                {
                    fullName = currentRegionDirective.Name,
                    range = currentRegionDirective.Range,
                    selectionRange = currentRegionDirective.SelectionRange
                };
                parentSymbolOrRegion.AddChildSymbol(newRegion);
                parentSymbolOrRegion = newRegion;
            }
            else
            {
                if (parentSymbolOrRegion.kind == ALSymbolKind.Region)
                {
                    parentSymbolOrRegion.range.end = currentRegionDirective.Range.end;
                    parentSymbolOrRegion = parentSymbolOrRegion.ParentSymbol;
                }
            }
            return parentSymbolOrRegion;
        }

        protected bool CanAddChildSymbol(ALSymbol childSymbol)
        {
            switch (childSymbol.kind)
            {
                case ALSymbolKind.ParameterList: 
                    return (childSymbol.childSymbols != null) && (childSymbol.childSymbols.Count > 0);
                default: return true;
            }
        }

        protected bool ProcessChildNodeAsSymbolProperty(ALSymbol symbol, SyntaxTree syntaxTree, SyntaxNode node)
        {
            switch (node.Kind.ConvertToLocalType())
            {
                case ConvertedSyntaxKind.SimpleTypeReference:
                case ConvertedSyntaxKind.RecordTypeReference:
                case ConvertedSyntaxKind.DotNetTypeReference:
                    symbol.subtype = node.ToFullString();
                    symbol.elementsubtype = node.GetType().TryGetPropertyValueAsString(node, "DataType");
                    if (String.IsNullOrWhiteSpace(symbol.elementsubtype))
                        symbol.elementsubtype = symbol.subtype;
                    return true;
                case ConvertedSyntaxKind.ObjectId:
                    ObjectIdSyntax objectIdSyntax = (ObjectIdSyntax)node;
                    if ((objectIdSyntax.Value != null) && (objectIdSyntax.Value.Value != null))
                        symbol.id = (int)objectIdSyntax.Value.Value;
                    return true;
                case ConvertedSyntaxKind.IdentifierName:
                    var lineSpan = syntaxTree.GetLineSpan(node.Span);
                    symbol.selectionRange = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                        lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);
                    return true;
            }
            return false;
        }

        #endregion

        #region Creating symbol from syntax node details

        protected ALSymbol CreateSymbol(SyntaxTree syntaxTree, SyntaxNode parentNode, SyntaxNode node)
        {
            var alSymbolKind = SyntaxKindToALSymbolKind(node);
            if (alSymbolKind == ALSymbolKind.Undefined)
                return null;

            var symbol = new ALSymbol()
            {
                kind = alSymbolKind,
                name = node.GetNameStringValue(),
                range = syntaxTree.GetLineRange(node.FullSpan),
                selectionRange = syntaxTree.GetLineRange(node.Span)
            };
            if (node.ContainsDiagnostics)
                symbol.containsDiagnostics = true;
            
            ProcessNodeTypeSpecificProperties(symbol, syntaxTree, parentNode, node);

            return symbol;         
        }

        #endregion

        #region Processing node type specific properties

        protected void ProcessNodeTypeSpecificProperties(ALSymbol symbol, SyntaxTree syntaxTree, SyntaxNode parentNode, SyntaxNode node)
        {
            switch (node)
            {
                //general object nodes
                case PropertySyntax propertySyntax:
                    ProcessPropertyNode(symbol, propertySyntax);
                    break;

                //table
                case TableSyntax tableSyntax:
                    ProcessTableNode(symbol, tableSyntax);
                    break;
                case KeySyntax keySyntax:
                    ProcessKeyNode(symbol, keySyntax);
                    break;
                case FieldSyntax fieldSyntax:
                    ProcessFieldNode(symbol, fieldSyntax);
                    break;

                //page
                case PageSyntax pageSyntax:
                    ProcessPageNode(symbol, pageSyntax);
                    break;
                case PageFieldSyntax pageFieldSyntax:
                    ProcessPageFieldNode(symbol, pageFieldSyntax);
                    break;
                case PageGroupSyntax pageGroupSyntax:
                    ProcessPageGroupNode(syntaxTree, symbol, pageGroupSyntax);
                    break;
                case PageAreaSyntax pageAreaSyntax:
                    ProcessPageAreaNode(syntaxTree, symbol, pageAreaSyntax);
                    break;
                case PagePartSyntax pagePartSyntax:
                    ProcessPagePartNode(symbol, pagePartSyntax);
                    break;
                case PageSystemPartSyntax pageSystemPartSyntax:
                    ProcessPageSystemPartNode(symbol, pageSystemPartSyntax);
                    break;
#if BC
                case PageChartPartSyntax pageChartPartSyntax:
                    ProcessPageChartPartNode(symbol, pageChartPartSyntax);
                    break;
#endif
                //request page
                case RequestPageSyntax requestPageSyntax:
                    ProcessRequestPageNode(symbol, requestPageSyntax);
                    break;

                //report
                case ReportDataItemSyntax reportDataItemSyntax:
                    ProcessReportDataItemNode(syntaxTree, symbol, reportDataItemSyntax);
                    break;
                case ReportColumnSyntax reportColumnSyntax:
                    ProcessReportColumnNode(symbol, reportColumnSyntax);
                    break;

                //xmlport
                case XmlPortSyntax xmlPortSyntax:
                    ProcessXmlPortObjectNode(symbol, xmlPortSyntax);
                    break;
                case XmlPortTableElementSyntax xmlPortTableElementSyntax:
                    ProcessXmlPortTableElementNode(syntaxTree, symbol, xmlPortTableElementSyntax);
                    break;
                case XmlPortFieldNodeSyntax xmlPortFieldNodeSyntax:
                    ProcessXmlPortFieldNode(symbol, xmlPortFieldNodeSyntax);
                    break;

                //query
                case QuerySyntax querySyntax:
                    ProcessQueryNode(symbol, querySyntax);
                    break;
                case QueryDataItemSyntax queryDataItemSyntax:
                    ProcessQueryDataItemNode(syntaxTree, symbol, queryDataItemSyntax);
                    break;
                case QueryColumnSyntax queryColumnSyntax:
                    ProcessQueryColumnNode(symbol, queryColumnSyntax);
                    break;
                //table extension
                case TableExtensionSyntax tableExtensionSyntax:
                    ProcessTableExtensionObjectNode(symbol, tableExtensionSyntax);
                    break;

                //page extension
                case PageExtensionSyntax pageExtensionSyntax:
                    ProcessPageExtensionObjectNode(symbol, pageExtensionSyntax);
                    break;
                case ControlAddChangeSyntax controlAddChangeSyntax:
                    ProcessControlAddChangeNode(syntaxTree, symbol, controlAddChangeSyntax);
                    break;
                
                //page customization
                case PageCustomizationSyntax pageCustomizationSyntax:
                    ProcessPageCustomizationObjectNode(symbol, pageCustomizationSyntax);
                    break;

                //code
                case EventDeclarationSyntax eventDeclarationSyntax:
                    ProcessEventDeclarationNode(symbol, eventDeclarationSyntax);
                    break;
                case MethodDeclarationSyntax methodDeclarationSyntax:
                    ProcessMethodDeclarationNode(symbol, methodDeclarationSyntax);
                    break;
                case TriggerDeclarationSyntax triggerDeclarationSyntax:
                    ProcessTriggerDeclarationNode(symbol, triggerDeclarationSyntax);
                    break;
                case VariableDeclarationSyntax variableDeclarationSyntax:
                    ProcessVariableDeclarationNode(symbol, variableDeclarationSyntax);
                    break;
                case ParameterSyntax parameterSyntax:
                    ProcessParameterNode(symbol, parameterSyntax);
                    break;
                case VarSectionSyntax varSectionSyntax:
                    ProcessVarSection(syntaxTree, symbol, varSectionSyntax);
                    break;
#if BC
                case VariableDeclarationNameSyntax variableDeclarationNameSyntax:
                    ProcessVariableDeclarationNameNode(symbol, parentNode, variableDeclarationNameSyntax);
                    break;

                //Var and GlobalVar syntax nodes are different in Nav2018
                case GlobalVarSectionSyntax globalVarSectionSyntax:
                    ProcessGlobalVarSection(syntaxTree, symbol, globalVarSectionSyntax);
                    break;

                //enum
                case EnumValueSyntax enumValueSyntax:
                    ProcessEnumValueNode(symbol, enumValueSyntax);
                    break;

                //enum extension
                case EnumExtensionTypeSyntax enumExtensionTypeSyntax:
                    ProcessEnumExtensionTypeNode(symbol, enumExtensionTypeSyntax);
                    break;

                //report extension
                case ReportExtensionSyntax reportExtensionSyntax:
                    ProcessReportExtensionNode(symbol, reportExtensionSyntax);
                    break;
                case ReportExtensionDataSetAddDataItemSyntax reportExtensionDataSetAddDataItemSyntax:
                    ProcessReportExtensionDataItemChangeNode(symbol, reportExtensionDataSetAddDataItemSyntax);
                    break;
                case ReportExtensionDataSetAddColumnSyntax reportExtensionDataSetAddColumnSyntax:
                    ProcessReportExtensionAddColumnChangeNode(syntaxTree, symbol, reportExtensionDataSetAddColumnSyntax);
                    break;
#endif
            }

        }

        #region General nodes properties processing

        protected void ProcessPropertyNode(ALSymbol symbol, PropertySyntax syntax)
        {
            var name = ALSyntaxHelper.DecodeName(syntax.Name?.Identifier.Text);
            if (!String.IsNullOrWhiteSpace(name))
            {
                symbol.name = name;
                symbol.fullName = "Property " + name;
            }
        }

        #endregion

        #region Table properties processing

        protected void ProcessTableNode(ALSymbol symbol, TableSyntax syntax)
        {
            _tableHasKeys = false;
        }

        protected void ProcessFieldNode(ALSymbol symbol, FieldSyntax syntax)
        {
            //Type syntaxType = syntax.GetType();
            if ((syntax.No != null) && (Int32.TryParse(syntax.No.ToString(), out int id)))
                symbol.id = id;

            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.Type.ToFullString();

            var enabled = syntax.GetBoolPropertyValue("Enabled", true);
            var obsoleteState = syntax.GetIdentifierPropertyValue("ObsoleteState");
            
            if (!enabled)
            {
                symbol.subtype = "Disabled";
                symbol.fullName = symbol.fullName + " (Disabled)";
            } 
            else if (!String.IsNullOrWhiteSpace(obsoleteState))
            {
                if (obsoleteState.Equals("Pending", StringComparison.CurrentCultureIgnoreCase))
                {
                    symbol.subtype = "ObsoletePending";
                    symbol.fullName = symbol.fullName + " (Obsolete-Pending)";
                }
                else if (obsoleteState.Equals("Removed", StringComparison.CurrentCultureIgnoreCase))
                {
                    symbol.subtype = "ObsoleteRemoved";
                    symbol.fullName = symbol.fullName + " (Obsolete-Removed)";
                }
            }
        }

        protected void ProcessKeyNode(ALSymbol symbol, KeySyntax syntax)
        {
            if (!_tableHasKeys)
                symbol.kind = ALSymbolKind.PrimaryKey;
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.Fields.ToFullString();
            _tableHasKeys = true;
        }

        #endregion

        #region Page properties processing

        protected void ProcessPageNode(ALSymbol symbol, PageSyntax syntax)
        {
            symbol.source = syntax.GetIdentifierPropertyValue("SourceTable");
            symbol.subtype = syntax.GetIdentifierPropertyValue("PageType");
        }

        protected void ProcessPageFieldNode(ALSymbol symbol, PageFieldSyntax syntax)
        {
            if (syntax.Expression != null)
            {
                symbol.source = ALSyntaxHelper.DecodeName(syntax.Expression.ToString());
            }
        }

        protected void ProcessPagePartNode(ALSymbol symbol, PagePartSyntax syntax)
        {
            string name = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(symbol.name);
            if (syntax.PartName != null)
                symbol.fullName = name + ": " + syntax.PartName.ToFullString();
            symbol.fullName = name;
        }

        protected void ProcessPageSystemPartNode(ALSymbol symbol, PageSystemPartSyntax syntax)
        {
            string name = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(symbol.name);
            if (syntax.SystemPartType != null)
                symbol.fullName = name + ": " + syntax.SystemPartType.ToFullString();
            symbol.fullName = name;
        }

#if BC

        protected void ProcessPageChartPartNode(ALSymbol symbol, PageChartPartSyntax syntax)
        {
            string name = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(symbol.name);
            if (syntax.ChartPartType != null)
                symbol.fullName = name + ": " + syntax.ChartPartType.ToFullString();
            symbol.fullName = name;
        }
#endif

        protected void ProcessPageAreaNode(SyntaxTree syntaxTree, ALSymbol symbol, PageAreaSyntax syntax)
        {
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax, syntax.OpenBraceToken, syntax.CloseBraceToken);
        }

        protected void ProcessPageGroupNode(SyntaxTree syntaxTree, ALSymbol symbol, PageGroupSyntax syntax)
        {
            SyntaxToken controlKeywordToken = syntax.ControlKeyword;
            if ((controlKeywordToken != null) && (controlKeywordToken.Kind.ConvertToLocalType() == ConvertedSyntaxKind.PageRepeaterKeyword))
                symbol.kind = ALSymbolKind.PageRepeater;
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax, syntax.OpenBraceToken, syntax.CloseBraceToken);
        }

        #endregion

        #region Request page processing

        protected void ProcessRequestPageNode(ALSymbol symbol, RequestPageSyntax syntax)
        {
            symbol.source = syntax.GetIdentifierPropertyValue("SourceTable");
        }

        #endregion

        #region Reports properties processing

        protected void ProcessReportColumnNode(ALSymbol symbol, ReportColumnSyntax syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.SourceExpression.ToFullString();
            if (syntax.SourceExpression != null)
                symbol.source = ALSyntaxHelper.DecodeName(syntax.SourceExpression.ToString());
        }

        protected void ProcessReportDataItemNode(SyntaxTree syntaxTree, ALSymbol symbol, ReportDataItemSyntax syntax)
        {
            if (syntax.DataItemTable != null)
            {
                symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": Record " + syntax.DataItemTable.ToFullString();
                symbol.source = ALSyntaxHelper.DecodeName(syntax.DataItemTable.ToString());
            }
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax, syntax.OpenBraceToken, syntax.CloseBraceToken);
        }

        #endregion

        #region Report extensions processing

#if BC

        protected void ProcessReportExtensionNode(ALSymbol symbol, ReportExtensionSyntax syntax)
        {
            if (syntax.BaseObject != null)
                symbol.extends = ALSyntaxHelper.DecodeName(syntax.BaseObject.ToString());
        }

        protected void ProcessReportExtensionDataItemChangeNode(ALSymbol symbol, ReportExtensionDataSetAddDataItemSyntax syntax)
        {
            symbol.name = ALSyntaxHelper.FormatSyntaxNodeName(syntax.ChangeKeyword.ToString());
            symbol.fullName = symbol.name;
        }

        protected void ProcessReportExtensionAddColumnChangeNode(SyntaxTree syntaxTree, ALSymbol symbol, ReportExtensionDataSetAddColumnSyntax syntax)
        {
            if (syntax.Anchor != null)
            {
                //symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": Record " + syntax.Anchor.ToFullString();
                symbol.extends = ALSyntaxHelper.DecodeName(syntax.Anchor.ToString());
            }
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax, syntax.OpenBraceToken, syntax.CloseBraceToken);
        }

#endif

        #endregion

        #region XmlPort properties processing        

        protected void ProcessXmlPortObjectNode(ALSymbol symbol, SyntaxNode node)
        {
            symbol.format = node.GetPropertyValue("Format")?.ToString()?.ToLower();
        }

        protected void ProcessXmlPortTableElementNode(SyntaxTree syntaxTree, ALSymbol symbol, XmlPortTableElementSyntax syntax)
        {
            symbol.fullName = symbol.kind.ToName() + " " +
                ALSyntaxHelper.EncodeName(symbol.name) +
                ": Record " + syntax.SourceTable.ToFullString();
            symbol.source = ALSyntaxHelper.DecodeName(syntax.SourceTable.ToFullString());
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax, syntax.OpenBraceToken, syntax.CloseBraceToken);
        }

        protected void ProcessXmlPortFieldNode(ALSymbol symbol, XmlPortFieldNodeSyntax syntax)
        {
            if (syntax.SourceField != null)
            {
                symbol.source = ALSyntaxHelper.DecodeName(syntax.SourceField.ToString());
            }
        }

        #endregion

        #region Code properties processing

#if BC

        protected void ProcessGlobalVarSection(SyntaxTree syntaxTree, ALSymbol symbol, GlobalVarSectionSyntax syntax)
        {
            _varAccessModifier = null;

            var accessModifier = syntax.AccessModifier.ToString()?.Trim();
            if ((accessModifier != null) && (accessModifier.Equals("protected", StringComparison.CurrentCultureIgnoreCase)))
            {
                symbol.access = ALSymbolAccessModifier.Protected;
                _varAccessModifier = ALSymbolAccessModifier.Protected;
                symbol.fullName = "protected var";
            }

            ProcessNodeContentRangeFromChildren(syntaxTree, symbol, syntax);
        }

#endif

        protected void ProcessVarSection(SyntaxTree syntaxTree, ALSymbol symbol, SyntaxNode syntax)
        {
            _varAccessModifier = null;
            ProcessNodeContentRangeFromChildren(syntaxTree, symbol, syntax);
        }

        protected void ProcessParameterNode(ALSymbol symbol, ParameterSyntax syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.Type.ToFullString();
        }

        protected void ProcessVariableDeclarationNode(ALSymbol symbol, VariableDeclarationSyntax syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.Type.ToFullString();
            symbol.access = _varAccessModifier;
        }

#if BC

        protected void ProcessVariableDeclarationNameNode(ALSymbol symbol, SyntaxNode parentSyntax, VariableDeclarationNameSyntax syntax)
        {
            VariableListDeclarationSyntax variableListDeclarationSyntax = parentSyntax as VariableListDeclarationSyntax;
            if (variableListDeclarationSyntax != null)
            {
                string typeName = variableListDeclarationSyntax.Type.ToFullString();
                string elementDataType = typeName;
                if (variableListDeclarationSyntax.Type.DataType != null)
                    elementDataType = variableListDeclarationSyntax.Type.DataType.ToString();
                
                symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) +
                    ": " + typeName;
                symbol.subtype = typeName;
                symbol.elementsubtype = elementDataType;
                symbol.access = _varAccessModifier;
            }
        }

#endif

        protected void ProcessTriggerDeclarationNode(ALSymbol symbol, TriggerDeclarationSyntax syntax)
        {
            ProcessMethodOrTriggerDeclarationNode(symbol, syntax);
        }

        protected void ProcessMethodDeclarationNode(ALSymbol symbol, MethodDeclarationSyntax syntax)
        {
            ProcessMethodOrTriggerDeclarationNode(symbol, syntax);

            if (syntax.Attributes != null)
                foreach (var attribute in syntax.Attributes)
                {
                    string memberAttributeName = attribute.GetNameStringValue(); //.GetSyntaxNodeName().NotNull();
                    if (!String.IsNullOrWhiteSpace(memberAttributeName))
                    {
                        var newKind = ALSyntaxHelper.MemberAttributeToMethodKind(memberAttributeName);
                        if (newKind != ALSymbolKind.Undefined)
                            symbol.kind = newKind;
                        symbol.subtype = memberAttributeName;
                    }
                }
#if BC

#else
            
#endif
        }

        protected void ProcessMethodOrTriggerDeclarationNode(ALSymbol symbol, MethodOrTriggerDeclarationSyntax syntax)
        {
            string namePart = ProcessPatameterListSyntax(syntax.ParameterList);

            if ((syntax.ReturnValue != null) && (syntax.ReturnValue.Kind.ConvertToLocalType() != ConvertedSyntaxKind.None))
                namePart = namePart + " " + syntax.ReturnValue.ToFullString();

            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + namePart;
        }

        protected void ProcessEventDeclarationNode(ALSymbol symbol, EventDeclarationSyntax syntax)
        {
            string namePart = ProcessPatameterListSyntax(syntax.ParameterList);
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + namePart;
        }

        protected string ProcessPatameterListSyntax(ParameterListSyntax syntax)
        {
            string namePart = "(";
            if ((syntax != null))
                namePart = namePart + syntax.Parameters.ToFullString();
            namePart = namePart + ")";
            return namePart;
        }

    #endregion

        #region Table extension properties processing

        protected void ProcessTableExtensionObjectNode(ALSymbol symbol, TableExtensionSyntax syntax)
        {
            if (syntax.BaseObject != null)
                symbol.extends = ALSyntaxHelper.DecodeName(syntax.BaseObject.ToString());
        }

        #endregion

        #region Page extension properties processing

        protected void ProcessPageExtensionObjectNode(ALSymbol symbol, PageExtensionSyntax syntax)
        {
            if (syntax.BaseObject != null)
                symbol.extends = ALSyntaxHelper.DecodeName(syntax.BaseObject.ToString());
        }

        protected void ProcessControlAddChangeNode(SyntaxTree syntaxTree, ALSymbol symbol, ControlAddChangeSyntax syntax)
        {
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax, syntax.OpenBraceToken, syntax.CloseBraceToken);
        }

        #endregion

        #region Query properties processing

        protected void ProcessQueryNode(ALSymbol symbol, QuerySyntax syntax)
        {
            var queryTypeValue = syntax.GetPropertyValue("QueryType");
            if (queryTypeValue != null)
                symbol.subtype = ALSyntaxHelper.DecodeName(queryTypeValue.ToString());
        }

        protected void ProcessQueryColumnNode(ALSymbol symbol, QueryColumnSyntax syntax)
        {
            if (syntax.RelatedField != null)
                symbol.source = ALSyntaxHelper.DecodeName(syntax.RelatedField.ToString());
        }

        protected void ProcessQueryDataItemNode(SyntaxTree syntaxTree, ALSymbol symbol, QueryDataItemSyntax syntax)
        {
            if (syntax.DataItemTable != null)
                symbol.source = ALSyntaxHelper.DecodeName(syntax.DataItemTable.ToString());
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax, syntax.OpenBraceToken, syntax.CloseBraceToken);
        }

        #endregion

#region Page customization properties processing

        protected void ProcessPageCustomizationObjectNode(ALSymbol symbol, PageCustomizationSyntax syntax)
        {
            if (syntax.BaseObject != null)
                symbol.extends = ALSyntaxHelper.DecodeName(syntax.BaseObject.ToString());
        }

#endregion

#region Enum extension properties processing

#if BC
        protected void ProcessEnumExtensionTypeNode(ALSymbol symbol, EnumExtensionTypeSyntax syntax)
        {
            if (syntax.BaseObject != null)
                symbol.extends = ALSyntaxHelper.DecodeName(syntax.BaseObject.ToString());
        }
#endif

#endregion

#region Enum properties processing

#if BC

        protected void ProcessEnumValueNode(ALSymbol symbol, EnumValueSyntax syntax)
        {
            string idText = syntax.Id.ToString();
            if (!String.IsNullOrWhiteSpace(idText))
            {
                int id;
                if (Int32.TryParse(idText, out id))
                    symbol.id = id;
            }
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name); // + ": " + syntax.EnumValueToken.ToFullString();
        }
#endif

#endregion

#region Process content range

        protected void ProcessNodeContentRange(SyntaxTree syntaxTree, ALSymbol symbol, SyntaxNode node,
            SyntaxToken contentStartToken, SyntaxToken contentEndToken)
        {
            if ((contentStartToken != null) && (contentEndToken != null))
            {
                var startSpan = syntaxTree.GetLineSpan(contentStartToken.Span);
                var endSpan = syntaxTree.GetLineSpan(contentEndToken.Span);
                symbol.contentRange = new Range(startSpan.EndLinePosition.Line, startSpan.EndLinePosition.Character,
                    endSpan.StartLinePosition.Line, endSpan.StartLinePosition.Character);
            }
        }

        protected void ProcessNodeContentRangeFromChildren(SyntaxTree syntaxTree, ALSymbol symbol, SyntaxNode syntax)
        {
            IEnumerable<SyntaxNode> list = syntax.ChildNodes();
            if (list != null)
            {
                Range totalRange = null;
                foreach (SyntaxNode childNode in list)
                {
                    var lineSpan = syntaxTree.GetLineSpan(childNode.FullSpan);
                    Range nodeRange = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                        lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);
                    if (totalRange == null)
                        totalRange = nodeRange;
                    else
                    {
                        if (totalRange.start.IsGreater(nodeRange.start))
                            totalRange.start.Set(nodeRange.start);
                        if (totalRange.end.IsLower(nodeRange.end))
                            totalRange.end.Set(nodeRange.end);
                    }
                }
                symbol.contentRange = totalRange;
            }
        }

#endregion

        #endregion

        #region Syntax node kind to al kind conversion

        protected ALSymbolKind SyntaxKindToALSymbolKind(SyntaxNode node)
        {
            switch (node.Kind.ConvertToLocalType())
            {
                //file root
                case ConvertedSyntaxKind.CompilationUnit: return ALSymbolKind.CompilationUnit;
                //object types
                case ConvertedSyntaxKind.CodeunitObject: return ALSymbolKind.CodeunitObject;
                case ConvertedSyntaxKind.TableObject: return ALSymbolKind.TableObject;
                case ConvertedSyntaxKind.TableExtensionObject: return ALSymbolKind.TableExtensionObject;
                case ConvertedSyntaxKind.PageObject: return ALSymbolKind.PageObject;
                case ConvertedSyntaxKind.PageExtensionObject: return ALSymbolKind.PageExtensionObject;
                case ConvertedSyntaxKind.ReportObject: return ALSymbolKind.ReportObject;
                case ConvertedSyntaxKind.XmlPortObject: return ALSymbolKind.XmlPortObject;
                case ConvertedSyntaxKind.QueryObject: return ALSymbolKind.QueryObject;
                case ConvertedSyntaxKind.ControlAddInObject: return ALSymbolKind.ControlAddInObject;
                case ConvertedSyntaxKind.ProfileObject: return ALSymbolKind.ProfileObject;
                case ConvertedSyntaxKind.PageCustomizationObject: return ALSymbolKind.PageCustomizationObject;
                case ConvertedSyntaxKind.DotNetPackage: return ALSymbolKind.DotNetPackage;
                case ConvertedSyntaxKind.Interface: return ALSymbolKind.Interface;
                //case ConvertedSyntaxKind.ReportExtension: return ALSymbolKind.ReportExtension;
                case ConvertedSyntaxKind.ReportExtensionObject: return ALSymbolKind.ReportExtensionObject;
                case ConvertedSyntaxKind.PermissionSet: return ALSymbolKind.PermissionSet;
                case ConvertedSyntaxKind.PermissionSetExtension: return ALSymbolKind.PermissionSetExtension;
                case ConvertedSyntaxKind.Entitlement: return ALSymbolKind.Entitlement;

                //code elements
                case ConvertedSyntaxKind.MethodDeclaration:
                    MethodDeclarationSyntax methodSyntax = (MethodDeclarationSyntax)node;
                    try
                    {
                        //safe call as access modifier is not supported by Nav2018
                        return this.GetMethodALSymbolKind(methodSyntax);
                    }
                    catch (Exception) { }
                    return ALSymbolKind.MethodDeclaration;
                case ConvertedSyntaxKind.ParameterList: return ALSymbolKind.ParameterList;
                case ConvertedSyntaxKind.Parameter: return ALSymbolKind.Parameter;
                case ConvertedSyntaxKind.VarSection: return ALSymbolKind.VarSection;
                case ConvertedSyntaxKind.GlobalVarSection: return ALSymbolKind.GlobalVarSection;
                case ConvertedSyntaxKind.VariableDeclaration: return ALSymbolKind.VariableDeclaration;
                case ConvertedSyntaxKind.VariableDeclarationName: return ALSymbolKind.VariableDeclarationName;
                case ConvertedSyntaxKind.TriggerDeclaration: return ALSymbolKind.TriggerDeclaration;

                //table and table extensions
                case ConvertedSyntaxKind.FieldList: return ALSymbolKind.FieldList;
                case ConvertedSyntaxKind.Field: return ALSymbolKind.Field;
                case ConvertedSyntaxKind.DotNetAssembly: return ALSymbolKind.DotNetAssembly;
                case ConvertedSyntaxKind.DotNetTypeDeclaration: return ALSymbolKind.DotNetTypeDeclaration;
                case ConvertedSyntaxKind.FieldExtensionList: return ALSymbolKind.FieldExtensionList;
                case ConvertedSyntaxKind.FieldModification: return ALSymbolKind.FieldModification;
                case ConvertedSyntaxKind.KeyList: return ALSymbolKind.KeyList;
                case ConvertedSyntaxKind.Key: return ALSymbolKind.Key;
                case ConvertedSyntaxKind.FieldGroupList: return ALSymbolKind.FieldGroupList;
                case ConvertedSyntaxKind.FieldGroup: return ALSymbolKind.FieldGroup;

                //page, page extenstions and page customizations
                case ConvertedSyntaxKind.PageLayout: return ALSymbolKind.PageLayout;
                case ConvertedSyntaxKind.PageActionList: return ALSymbolKind.PageActionList;
                case ConvertedSyntaxKind.GroupActionList: return ALSymbolKind.GroupActionList;
                case ConvertedSyntaxKind.PageArea: return ALSymbolKind.PageArea;
                case ConvertedSyntaxKind.PageGroup: return ALSymbolKind.PageGroup;
                case ConvertedSyntaxKind.PageField: return ALSymbolKind.PageField;
                case ConvertedSyntaxKind.PageLabel: return ALSymbolKind.PageLabel;
                case ConvertedSyntaxKind.PagePart: return ALSymbolKind.PagePart;
                case ConvertedSyntaxKind.PageSystemPart: return ALSymbolKind.PageSystemPart;
                case ConvertedSyntaxKind.PageChartPart: return ALSymbolKind.PageChartPart;
                case ConvertedSyntaxKind.PageUserControl: return ALSymbolKind.PageUserControl;
                case ConvertedSyntaxKind.PageAction: return ALSymbolKind.PageAction;
                case ConvertedSyntaxKind.PageActionGroup: return ALSymbolKind.PageActionGroup;
                case ConvertedSyntaxKind.PageActionArea: return ALSymbolKind.PageActionArea;
                case ConvertedSyntaxKind.PageActionSeparator: return ALSymbolKind.PageActionSeparator;
                case ConvertedSyntaxKind.PageExtensionActionList: return ALSymbolKind.PageExtensionActionList;
                case ConvertedSyntaxKind.ActionAddChange: return ALSymbolKind.ActionAddChange;
                case ConvertedSyntaxKind.ActionMoveChange: return ALSymbolKind.ActionMoveChange;
                case ConvertedSyntaxKind.ActionModifyChange: return ALSymbolKind.ActionModifyChange;
                case ConvertedSyntaxKind.PageExtensionLayout: return ALSymbolKind.PageExtensionLayout;
                case ConvertedSyntaxKind.ControlAddChange: return ALSymbolKind.ControlAddChange;
                case ConvertedSyntaxKind.ControlMoveChange: return ALSymbolKind.ControlMoveChange;
                case ConvertedSyntaxKind.ControlModifyChange: return ALSymbolKind.ControlModifyChange;
                case ConvertedSyntaxKind.PageExtensionViewList: return ALSymbolKind.PageExtensionViewList;
                case ConvertedSyntaxKind.ViewAddChange: return ALSymbolKind.ViewAddChange;
                case ConvertedSyntaxKind.ViewMoveChange: return ALSymbolKind.ViewMoveChange;
                case ConvertedSyntaxKind.ViewModifyChange: return ALSymbolKind.ViewModifyChange;
                case ConvertedSyntaxKind.PageViewList: return ALSymbolKind.PageViewList;
                case ConvertedSyntaxKind.PageView: return ALSymbolKind.PageView;

                //xmlports
                case ConvertedSyntaxKind.XmlPortSchema: return ALSymbolKind.XmlPortSchema;
                case ConvertedSyntaxKind.XmlPortTableElement: return ALSymbolKind.XmlPortTableElement;
                case ConvertedSyntaxKind.XmlPortFieldElement: return ALSymbolKind.XmlPortFieldElement;
                case ConvertedSyntaxKind.XmlPortTextElement: return ALSymbolKind.XmlPortTextElement;
                case ConvertedSyntaxKind.XmlPortFieldAttribute: return ALSymbolKind.XmlPortFieldAttribute;
                case ConvertedSyntaxKind.XmlPortTextAttribute: return ALSymbolKind.XmlPortTextAttribute;
                case ConvertedSyntaxKind.RequestPage: return ALSymbolKind.RequestPage;

                //reports
                case ConvertedSyntaxKind.ReportDataSetSection: return ALSymbolKind.ReportDataSetSection;
                case ConvertedSyntaxKind.ReportLabelsSection: return ALSymbolKind.ReportLabelsSection;
                case ConvertedSyntaxKind.ReportDataItem: return ALSymbolKind.ReportDataItem;
                case ConvertedSyntaxKind.ReportColumn: return ALSymbolKind.ReportColumn;
                case ConvertedSyntaxKind.ReportLabel: return ALSymbolKind.ReportLabel;
                case ConvertedSyntaxKind.ReportLabelMultilanguage: return ALSymbolKind.ReportLabelMultilanguage;

                //report extensions
                case ConvertedSyntaxKind.ReportExtensionAddColumnChange: return ALSymbolKind.ReportExtensionAddColumnChange;
                case ConvertedSyntaxKind.ReportExtensionAddDataItemChange: return ALSymbolKind.ReportExtensionAddDataItemChange;
                case ConvertedSyntaxKind.ReportExtensionDataSetAddColumn: return ALSymbolKind.ReportExtensionDataSetAddColumn;
                case ConvertedSyntaxKind.ReportExtensionDataSetAddDataItem: return ALSymbolKind.ReportExtensionDataSetAddDataItem;
                case ConvertedSyntaxKind.ReportExtensionDataSetModify: return ALSymbolKind.ReportExtensionDataSetModify;
                case ConvertedSyntaxKind.ReportExtensionDataSetSection: return ALSymbolKind.ReportExtensionDataSetSection;
                case ConvertedSyntaxKind.ReportExtensionModifyChange: return ALSymbolKind.ReportExtensionModifyChange;
                case ConvertedSyntaxKind.RequestPageExtension: return ALSymbolKind.RequestPageExtension;

                //dotnet packages

                //control add-ins
                case ConvertedSyntaxKind.EventTriggerDeclaration: return ALSymbolKind.EventTriggerDeclaration;
                case ConvertedSyntaxKind.EventDeclaration: return ALSymbolKind.EventDeclaration;

                //queries
                case ConvertedSyntaxKind.QueryElements: return ALSymbolKind.QueryElements;
                case ConvertedSyntaxKind.QueryDataItem: return ALSymbolKind.QueryDataItem;
                case ConvertedSyntaxKind.QueryColumn: return ALSymbolKind.QueryColumn;
                case ConvertedSyntaxKind.QueryFilter: return ALSymbolKind.QueryFilter;


                //codeunits


                //enums and enum extensions
                case ConvertedSyntaxKind.EnumType: return ALSymbolKind.EnumType;
                case ConvertedSyntaxKind.EnumValue: return ALSymbolKind.EnumValue;
                case ConvertedSyntaxKind.EnumExtensionType: return ALSymbolKind.EnumExtensionType;

                //properties
                case ConvertedSyntaxKind.PropertyList: return ALSymbolKind.PropertyList;
                case ConvertedSyntaxKind.Property: return ALSymbolKind.Property;

            }
            return ALSymbolKind.Undefined;
        }

        private ALSymbolKind GetMethodALSymbolKind(MethodDeclarationSyntax methodSyntax)
        {
#if BC
            if (methodSyntax.AccessModifier != null)
            {
                switch (methodSyntax.AccessModifier.Kind.ConvertToLocalType())
                {
                    case ConvertedSyntaxKind.ProtectedKeyword:
                        return ALSymbolKind.ProtectedMethodDeclaration;
                    case ConvertedSyntaxKind.LocalKeyword:
                        return ALSymbolKind.LocalMethodDeclaration;
                    case ConvertedSyntaxKind.InternalKeyword:
                        return ALSymbolKind.InternalMethodDeclaration;
                }
            }
#endif
            return ALSymbolKind.MethodDeclaration;
        }

#endregion

        #region Regions processing

        protected ALRegionDirective CollectRegionDirectivesPositions(SyntaxTree syntaxTree, SyntaxNode node)
        {
            ALRegionDirective firstRegionDirective = new ALRegionDirective();
            ALRegionDirective lastRegionDirective = firstRegionDirective;
            int level = 0;

            var syntaxTriviasCollection = node.DescendantTrivia();

            if (syntaxTriviasCollection != null)
                foreach (var triviaSyntax in syntaxTriviasCollection)
                {
                    var kind = triviaSyntax.Kind.ConvertToLocalType();
                    if ((kind == ConvertedSyntaxKind.RegionDirectiveTrivia) || (kind == ConvertedSyntaxKind.EndRegionDirectiveTrivia))
                    {
                        var isStartRegion = (kind == ConvertedSyntaxKind.RegionDirectiveTrivia);
                        if (isStartRegion)
                            level++;
                        else
                            level--;

                        var name = (isStartRegion) ? triviaSyntax.ToString() : "";
                        var newRegionDirective = new ALRegionDirective(
                            isStartRegion, level, name,
                            syntaxTree.GetLineRange(triviaSyntax.FullSpan),
                            syntaxTree.GetLineRange(triviaSyntax.Span));
                        
                        lastRegionDirective.Next = newRegionDirective;
                        lastRegionDirective = newRegionDirective;
                    }
                }

            return firstRegionDirective;
        }

        #endregion

    }
}
