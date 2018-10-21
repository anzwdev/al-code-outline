import { SymbolInformation, SymbolKind, ExtensionContext, Location } from "vscode";
import * as path from 'path';
import { ALSymbolKind } from "./alSymbolKind";
import { WSANOTINITIALISED } from "constants";
import { ALControlKind } from "./objectlibrary/alControlKind";
import { ALActionKind } from "./objectlibrary/alActionKind";

export class ALSymbolInfo {
    lspSymbol : SymbolInformation;  //Symbol Information provided by language server protocol
    childItems : ALSymbolInfo[];
    parentItem : ALSymbolInfo;
    name : string;
    symbolName : string;
    alKind : ALSymbolKind;
    languageId : string;
    alElementId : number;
    private hasKeys : boolean;
    private childGroups : any = {};

    constructor(symbol: SymbolInformation, newLanguageId : string) {
        this.alElementId = 0;
        this.parentItem = null;
        this.hasKeys = false;
        this.childItems = [];
        this.lspSymbol = symbol;
        this.languageId = newLanguageId;
        if (this.lspSymbol) {
            this.name = this.lspSymbol.name;
            this.alKind = ALSymbolKind.Undefined;
        } else {
            this.name = "";
            this.alKind = ALSymbolKind.TreeRoot;
        }
        this.updateSymbol();
    }

    //----------------------------------------------------
    // clone with groups
    //----------------------------------------------------

    public cloneWithGroups() : ALSymbolInfo {
        let item : ALSymbolInfo = new ALSymbolInfo(this.lspSymbol, this.languageId);
        //copy all fields        
        item.lspSymbol = this.lspSymbol;
        item.name = this.name;
        item.symbolName = this.symbolName;
        item.alKind = this.alKind;
        item.languageId = this.languageId;
        item.alElementId = this.alElementId;
        //clone child items
        for (let i=0; i < this.childItems.length; i++) {
            item.addChild(this.childItems[i].cloneWithGroups(), true);
        }
        return item;
    }

    //----------------------------------------------------
    // child nodes operations
    //----------------------------------------------------

    private findParent(prevNode : ALSymbolInfo, currNode : ALSymbolInfo) : ALSymbolInfo {
        var parentNode = prevNode;
        while ((parentNode) && (parentNode.lspSymbol) && ((parentNode.lspSymbol.name != currNode.lspSymbol.containerName) || (parentNode.alKind == ALSymbolKind.SymbolGroup))) {
            parentNode = parentNode.parentItem;
        }
        if (parentNode)
            return parentNode;
        return this;
    }

    public appendChildNodes(childNodes : ALSymbolInfo[], groupSymbols : boolean) {
        var symbolKindFixDetected : boolean = false;
        var newSymbolKindFixValue : number = 0;
        
        //add nodes one by one
        this.hasKeys = false;
        var prevChildNode : ALSymbolInfo = this;
        for (var i = 0; i<childNodes.length;i++) {
            var currNode = childNodes[i];
            //append child node
            var parentNode = this.findParent(prevChildNode, currNode);
            parentNode.addChild(currNode, groupSymbols);
            prevChildNode = currNode;
        }
    }  

    protected getGroup(name : string, forSymbol : ALSymbolInfo) : ALSymbolInfo {
        //try to find symbol
        if (this.childGroups[name])
            return this.childGroups[name];
        
        let symbolInfo : SymbolInformation = null;
        if (forSymbol.lspSymbol)
            symbolInfo = new SymbolInformation(name, SymbolKind.Null, forSymbol.lspSymbol.containerName, forSymbol.lspSymbol.location);

        let symbol : ALSymbolInfo = new ALSymbolInfo(symbolInfo, 'al');
        symbol.alKind = ALSymbolKind.SymbolGroup;
        symbol.name = name;        
        this.addChild(symbol, true);

        this.childGroups[name] = symbol;

        return symbol;
    }

    protected getGroupName() : string {
        switch (this.alKind) {
            case ALSymbolKind.Method:
                return 'Procedures';
            case ALSymbolKind.PrimaryKey:
            case ALSymbolKind.Key:
                return 'Keys';
            case ALSymbolKind.Field:
                return 'Fields';
            case ALSymbolKind.Variable:
                return 'Variables'; 
            case ALSymbolKind.Trigger:
                return 'Triggers';
        }
        return "";
    }

    protected isALObject() : boolean {
        return ((this.alKind == ALSymbolKind.Table) ||
        (this.alKind == ALSymbolKind.Codeunit) ||
        (this.alKind == ALSymbolKind.Page) ||
        (this.alKind == ALSymbolKind.Report) ||
        (this.alKind == ALSymbolKind.Query) ||
        (this.alKind == ALSymbolKind.XmlPort) ||
        (this.alKind == ALSymbolKind.TableExtension) ||
        (this.alKind == ALSymbolKind.PageExtension) ||
        (this.alKind == ALSymbolKind.ControlAddIn) ||
        (this.alKind == ALSymbolKind.Enum) ||
        (this.alKind == ALSymbolKind.EnumExtension) ||
        (this.alKind == ALSymbolKind.DotNetPackage));
    }

    public addChild(childItem : ALSymbolInfo, groupSymbols : boolean) {       
        if ((groupSymbols) && (this.isALObject())) {
            let groupName : string = childItem.getGroupName();
            if (groupName != "") {
                let group : ALSymbolInfo = this.getGroup(groupName, childItem);
                group.addChild(childItem, true);
                return;
            }
        }
        
        //add item to the child items
        this.childItems.push(childItem);
        childItem.setParent(this);       
        //detect primary key
        if ((!this.hasKeys) && (childItem.alKind == ALSymbolKind.Key)) {
            this.hasKeys = true;
            childItem.alKind = ALSymbolKind.PrimaryKey;
        }
    }

    public setParent(parentItem : ALSymbolInfo) {
        this.parentItem = parentItem;
    }

    //-----------------------------------------------------
    // symbol definition processing
    //-----------------------------------------------------

    private updateSymbol() {
        if (!this.lspSymbol)
            return;
        //name
        this.name = this.lspSymbol.name;
        this.alKind = this.GetALKind();
        this.symbolName = this.name;

        if ((this.alKind != ALSymbolKind.Undefined) && (this.name) && (this.name.length > 0)) {
            let startPos : number = this.name.indexOf("\"");
            let endPos : number = this.name.lastIndexOf("\"");
            if ((startPos >= 0) && (startPos < endPos))
                this.symbolName = this.name.substr(startPos + 1, endPos - startPos - 1);
            else {
                startPos = this.name.indexOf(" ");
                if (startPos >= 0)
                    this.symbolName = this.name.substr(startPos + 1);
            }
        }

    }

    private GetALKind() : ALSymbolKind {
        if (this.languageId !== 'al')
            return ALSymbolKind.Undefined;
        
        //try to detect al symbol kind
        switch (this.lspSymbol.kind) {
            case SymbolKind.Class:
                return this.GetALObjectKind();
            case SymbolKind.Variable:
                return this.GetALVariableKind();
            case SymbolKind.Method:
                return ALSymbolKind.Method;
            case SymbolKind.Function:
                return ALSymbolKind.Trigger;
            case SymbolKind.Field:
                return ALSymbolKind.Field;
            case SymbolKind.Property:
                return ALSymbolKind.Property;
            case SymbolKind.Object:
                return this.GetALUndefinedSymbolKind();
            default:
                return ALSymbolKind.Undefined;
        }
    }

    private GetALObjectKind() : ALSymbolKind {
        let symbolName = this.name.toLowerCase();
        if (symbolName.startsWith("table"))
            return ALSymbolKind.Table;
        if (symbolName.startsWith("codeunit"))
            return ALSymbolKind.Codeunit;
        if (symbolName.startsWith("page"))
            return ALSymbolKind.Page;
        if (symbolName.startsWith("report"))
            return ALSymbolKind.Report;
        if (symbolName.startsWith("query"))
            return ALSymbolKind.Query;
        if (symbolName.startsWith("xmlport"))
            return ALSymbolKind.XmlPort;
        if (symbolName.startsWith("tableextension"))
            return ALSymbolKind.TableExtension;
        if (symbolName.startsWith("pageextension"))
            return ALSymbolKind.PageExtension;
        return ALSymbolKind.UndefinedObject;
    }

    private GetALUndefinedSymbolKind() : ALSymbolKind {
        let symbolName = this.name.toLowerCase();
        if (symbolName.startsWith("tableextension"))
            return ALSymbolKind.TableExtension;
        if (symbolName.startsWith("pageextension"))
            return ALSymbolKind.PageExtension;
        if (symbolName.startsWith("key "))
            return ALSymbolKind.Key;
        if (symbolName.startsWith("fieldgroup"))
            return ALSymbolKind.FieldGroup;
        if (symbolName.startsWith("action "))
            return ALSymbolKind.Action;
        
        //page fields
        if (symbolName.startsWith("field "))
            return ALSymbolKind.Field;
        if (symbolName.startsWith("usercontrol "))
            return ALSymbolKind.Field;
        if (symbolName.startsWith("label "))
            return ALSymbolKind.Constant;

        //page field group controls
        if (symbolName.startsWith("group "))
            return ALSymbolKind.Group;
        if (symbolName.startsWith("cuegroup "))
            return ALSymbolKind.Group;
        if (symbolName.startsWith("fixed "))
            return ALSymbolKind.Group;
        if (symbolName.startsWith("grid "))
            return ALSymbolKind.Group;
        if (symbolName.startsWith("area "))
            return ALSymbolKind.Group;
        if (symbolName.startsWith("repeater "))
            return ALSymbolKind.Group;
        if (symbolName.startsWith("part "))
            return ALSymbolKind.Group;
        if (symbolName.startsWith("systempart "))
            return ALSymbolKind.Group;

        //enum
        if (symbolName.startsWith("enum value "))
            return ALSymbolKind.EnumValue;
        if (symbolName.startsWith("enum "))
            return ALSymbolKind.Enum;
        if (symbolName.startsWith("enumextension "))
            return ALSymbolKind.EnumExtension;

        return ALSymbolKind.Undefined;       
    }

    private GetALVariableKind() : ALSymbolKind {
        return ALSymbolKind.Variable;    
    }

    //-------------------------------------------------------
    // symbol icon
    //-------------------------------------------------------

    private getIconName() : string {
        switch (this.alKind)
        {
            case ALSymbolKind.TreeRoot:
                return "tree-undefined.svg";
            case ALSymbolKind.Undefined:
                return this.getStdIconName();
            case ALSymbolKind.Table:
                return "tree-table.svg";
            case ALSymbolKind.UndefinedObject:
                return "tree-class.svg";
            case ALSymbolKind.Codeunit:
                return "tree-codeunit.svg";
            case ALSymbolKind.Page:
                return "tree-page.svg";
            case ALSymbolKind.Report:
                return "tree-report.svg";
            case ALSymbolKind.Query:
                return "tree-query.svg";
            case ALSymbolKind.XmlPort:
                return "tree-xmlport.svg";
            case ALSymbolKind.TableExtension:
                return "tree-tableextension.svg";
            case ALSymbolKind.PageExtension:
                return "tree-pageextension.svg";
            case ALSymbolKind.ControlAddIn:
                return "tree-controladdin.svg";
            case ALSymbolKind.Profile:
                return "tree-profile.svg";
            case ALSymbolKind.PageCustomization:
                return "tree-pagecustomization.svg";
            //Object specific child symbols
            case ALSymbolKind.Property:
                return "tree-property.svg";
            //Code
            case ALSymbolKind.Variable:
                return "tree-variable.svg";
            case ALSymbolKind.Constant:
                return "tree-constant.svg";
            case ALSymbolKind.Parameter:
                return "tree-parameter.svg";
            case ALSymbolKind.Method:
                return "tree-method.svg";
            case ALSymbolKind.Trigger:
                return "tree-trigger.svg";
            case ALSymbolKind.EventPublisher:
                return "tree-eventpublisher.svg";
            case ALSymbolKind.EventSubscriber:
                return "tree-eventsubscriber.svg"
            //Tables
            case ALSymbolKind.Field:
                return "tree-field.svg";
            case ALSymbolKind.Key:
                return "tree-key.svg";
            case ALSymbolKind.PrimaryKey:
                return "tree-primarykey.svg";
            case ALSymbolKind.FieldGroup:
                return "tree-fieldgroup.svg";
            //Pages
            case ALSymbolKind.Group:
                return "tree-group.svg";
            case ALSymbolKind.Action:
                return "tree-action.svg";
            //Enums
            case ALSymbolKind.Enum:
                return 'tree-module.svg';
            case ALSymbolKind.EnumValue:
                return 'tree-field.svg';
            case ALSymbolKind.EnumExtension:
                return 'tree-module.svg';
            //dotnet
            case ALSymbolKind.DotNetPackage:
                return 'tree-module.svg';
            case ALSymbolKind.DotNetAssembly:
                return 'tree-module.svg';
            case ALSymbolKind.DotNetType:
                return 'tree-class.svg';
            
            //groups
            case ALSymbolKind.SymbolGroup:
                return this.getSymbolGroupIconName();

        }
        return null;
    }

    private getSymbolGroupIconName() : string {
        return null;
    }

    //standard icon names for langages different than al
    private getStdIconName() : string {
        switch (this.lspSymbol.kind) {
            case SymbolKind.Class:
                return 'tree-class.svg';
            case SymbolKind.Constant:
                return 'tree-constant.svg';
            case SymbolKind.Event:
                return 'tree-trigger.svg';
            case SymbolKind.Field:
                return 'tree-field.svg';
            case SymbolKind.Property:
                return 'tree-property.svg';
            case SymbolKind.Variable:
                return 'tree-variable.svg';
            case SymbolKind.Function:
                return 'tree-function.svg';
            case SymbolKind.Method:
                return 'tree-method.svg';
            case SymbolKind.Key:
                return 'tree-key.svg';
            case SymbolKind.Module:
                return 'tree-module.svg';
            case SymbolKind.Constructor:
                return 'tree-function.svg';
            case SymbolKind.Interface:
                return 'tree-interface.svg';
            
            //!!! TO-DO !!!
            //!!! Add more icons for these symbol kinds !!!
            case SymbolKind.Array:
            case SymbolKind.Boolean:
            case SymbolKind.Enum:
            case SymbolKind.EnumMember:
            case SymbolKind.File:
            case SymbolKind.Null:
            case SymbolKind.Number:
            case SymbolKind.Object:
            case SymbolKind.Operator:
            case SymbolKind.Package:
            case SymbolKind.String:
            case SymbolKind.Struct:
            case SymbolKind.TypeParameter:
                return 'tree-undefined.svg';
        }

        return "tree-undefined.svg";

    }

    public getKindName() : string {
        switch (this.alKind) {
            case ALSymbolKind.Table:
                return "ALTable";
            case ALSymbolKind.Page:
                return "ALPage";
            case ALSymbolKind.Report:
                return "ALReport";
            case ALSymbolKind.Codeunit:
                return "ALCodeunit";
            case ALSymbolKind.XmlPort:
                return "ALXmlPort";
            case ALSymbolKind.Query:
                return "ALQuery";
            case ALSymbolKind.ControlAddIn:
                return "ALControlAddIn";
        }
        
        return this.alKind.toString();
    }

    public getIcon(context : ExtensionContext): any {
        let iconName = this.getIconName();
        if (iconName)
            return {
                light: context.asAbsolutePath(path.join("resources", "light", iconName)),
                dark: context.asAbsolutePath(path.join("resources", "dark", iconName))
            }
        return null;
	}

    static createFromSymbolReference(newSymbolKind : ALSymbolKind, newTypeName : string, symbolReference : any) : ALSymbolInfo {
        var symbolInfo = new ALSymbolInfo(null, null);
        symbolInfo.loadFromSymbolReference(newSymbolKind, newTypeName, symbolReference);
        return symbolInfo;
    }

    loadFromSymbolReference(newSymbolKind : ALSymbolKind, newTypeName : string, symbolReference : any) {
        this.lspSymbol = null;
        this.childItems = [];
        this.parentItem = null;
        this.symbolName = symbolReference.Name;
        if (!this.symbolName)
            this.symbolName = '';
        if (newTypeName !== '')
            this.name = newTypeName + ' ' + this.symbolName;
        else
            this.name = this.symbolName;
        this.alKind = newSymbolKind;
        this.languageId = 'al';
        this.hasKeys = false;
        if (symbolReference.Id)
            this.alElementId = symbolReference.Id;
        else
            this.alElementId = 0;
    }

    loadFromEnumValue(symbolIndex : number, symbolReference : any) {
        this.alKind = ALSymbolKind.EnumValue;
        if (symbolReference.Name)
            this.symbolName = symbolReference.Name;

        let idx : string = symbolIndex.toString();
        if (symbolReference.Ordinal)
            idx = symbolReference.Ordinal;

        this.name = this.symbolName + " (" + idx + ")"; 
    }

    loadFromDotNetType(symbolReference : any) {
        this.alKind = ALSymbolKind.DotNetType;
        if (symbolReference.TypeName)
            this.symbolName = symbolReference.TypeName;
        if (symbolReference.AliasName)
            this.name = symbolReference.AliasName + " (" + this.symbolName + ")";
        else
            this.name = this.symbolName;
    }

    loadFromObjectSymbolReference(newSymbolKind : ALSymbolKind, newTypeName : string, symbolReference : any) {
        //init symbol header
        this.loadFromSymbolReference(newSymbolKind, newTypeName, symbolReference);
        //init child items
        if (symbolReference.Fields)
            this.loadBasicSymbolReferences(ALSymbolKind.Field, symbolReference.Fields);
        if (symbolReference.Keys)
            this.loadBasicSymbolReferences(ALSymbolKind.Key, symbolReference.Keys);
        if (symbolReference.Controls)
            this.loadControlSymbolReferences(symbolReference.Controls);
        if (symbolReference.Actions)
            this.loadActionSymbolReferences(symbolReference.Actions);   
         //this.loadBasicSymbolReferences(ALSymbolKind.Field, symbolReference.DataItems);
        
        if (symbolReference.DataItems)
            this.loadDataItemSymbolReferences(symbolReference.DataItems);   
        if (symbolReference.Elements)
            this.loadDataItemSymbolReferences(symbolReference.Elements);   
        
        if (symbolReference.ActionChanges)
            this.loadChangesSymbolReferences('Action Changes', symbolReference.ActionChanges);
        if (symbolReference.ControlChanges)
            this.loadChangesSymbolReferences('Control Changes', symbolReference.ControlChanges);

        if (symbolReference.Variables)
            this.loadBasicSymbolReferences(ALSymbolKind.Variable, symbolReference.Variables);
        if (symbolReference.Methods)
            this.loadMethodSymbolReferences(ALSymbolKind.Method, symbolReference.Methods);
        //control Add-In triggers
        if (symbolReference.Events)
            this.loadMethodSymbolReferences(ALSymbolKind.Trigger, symbolReference.Events);

        //enum values
        if (symbolReference.Values)
            this.loadEnumValues(symbolReference.Values);

        //.net assemblies
        if (symbolReference.AssemblyDeclarations)
            this.loadAssemblyDeclarations(symbolReference.AssemblyDeclarations);

    }

    private loadEnumValues(symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (let i=0; i<symbolReferenceList.length; i++) {
                let symbolRef = symbolReferenceList[i];
                let symbolInfo = new ALSymbolInfo(null, 'al');
                symbolInfo.loadFromEnumValue(i, symbolRef);
                this.addChild(symbolInfo, false);
            }
        }
    }

    private loadAssemblyDeclarations(symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (let i=0; i<symbolReferenceList.length; i++) {
                var symbolRef = symbolReferenceList[i];
                var symbolInfo = ALSymbolInfo.createFromSymbolReference(ALSymbolKind.DotNetAssembly, '', symbolRef); 
                this.addChild(symbolInfo, false);
                //load type definitions
                if (symbolRef.TypeDeclarations)
                    symbolInfo.loadDotNetTypeDeclarations(symbolRef.TypeDeclarations);
            }
        }
    }

    private loadDotNetTypeDeclarations(symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (let i=0; i<symbolReferenceList.length; i++) {
                var symbolRef = symbolReferenceList[i];
                var symbolInfo = new ALSymbolInfo(null, 'al');
                symbolInfo.loadFromDotNetType(symbolRef);
                this.addChild(symbolInfo, false);
            }
        }
    }

    private loadBasicSymbolReferences(symbolKind : ALSymbolKind, symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (var i = 0; i<symbolReferenceList.length; i++) {
                this.addChild(ALSymbolInfo.createFromSymbolReference(symbolKind, '', symbolReferenceList[i]), false);
            }
        }
    }

    loadChangesSymbolReferences(changesName : string, symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (var i = 0; i<symbolReferenceList.length; i++) {
                var symbolRef = symbolReferenceList[i];
                var symbolInfo = ALSymbolInfo.createFromSymbolReference(ALSymbolKind.Group, changesName, symbolRef); 
                this.addChild(symbolInfo, false);
                //load symbol actions
                if (symbolRef.Actions)
                    symbolInfo.loadActionSymbolReferences(symbolRef.Actions);
                if (symbolRef.Controls)
                    symbolInfo.loadControlSymbolReferences(symbolRef.Controls);
            }
        }
    }


    loadControlSymbolReferences(symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (var i = 0; i<symbolReferenceList.length; i++) {
                var symbolRef = symbolReferenceList[i];
                var controlKind : ALControlKind = symbolRef.Kind;
                var symbolInfo = ALSymbolInfo.createFromSymbolReference(this.alControlKindToSymbolKind(controlKind), ALControlKind[controlKind], symbolRef); 
                this.addChild(symbolInfo, false);
                //load symbol controls
                if (symbolRef.Controls)
                    symbolInfo.loadControlSymbolReferences(symbolRef.Controls);
            }
        }
    }

    loadActionSymbolReferences(symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (var i = 0; i<symbolReferenceList.length; i++) {
                var symbolRef = symbolReferenceList[i];
                var actionKind : ALActionKind = symbolRef.Kind;
                var symbolInfo = ALSymbolInfo.createFromSymbolReference(this.alActionKindToSymbolKind(actionKind), ALActionKind[actionKind], symbolRef); 
                this.addChild(symbolInfo, false);
                //load symbol actions
                if (symbolRef.Actions)
                    symbolInfo.loadActionSymbolReferences(symbolRef.Actions);
            }
        }
    }

    loadDataItemSymbolReferences(symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (var i = 0; i<symbolReferenceList.length; i++) {
                var symbolRef = symbolReferenceList[i];
                var symbolInfo = ALSymbolInfo.createFromSymbolReference(ALSymbolKind.FieldGroup, 'DataItem', symbolRef); 
                this.addChild(symbolInfo, false);
                //load symbol items
                if (symbolRef.Columns)
                    symbolInfo.loadBasicSymbolReferences(ALSymbolKind.Field, symbolRef.Columns);
                if (symbolRef.DataItems)
                    symbolInfo.loadActionSymbolReferences(symbolRef.DataItems);
            }
        }
    }

    private alControlKindToSymbolKind(controlKind : ALControlKind) : ALSymbolKind {
        switch (controlKind) {
            case ALControlKind.Area:
            case ALControlKind.Group:
            case ALControlKind.CueGroup:
            case ALControlKind.Repeater:
            case ALControlKind.Fixed:
            case ALControlKind.Grid:
                return ALSymbolKind.Group;
            case ALControlKind.Part:
            case ALControlKind.SystemPart:
                return ALSymbolKind.Page;
            case ALControlKind.Field:
            case ALControlKind.Label:
            case ALControlKind.UserControl:
            case ALControlKind.Chart:
                return ALSymbolKind.Field; 
        }
        return ALSymbolKind.Undefined;
    }

    private alActionKindToSymbolKind(actionKind : ALActionKind) : ALSymbolKind {
        switch (actionKind) {
            case ALActionKind.Group:
            case ALActionKind.Area:
                return ALSymbolKind.Group;
            case ALActionKind.Action:
                return ALSymbolKind.Action;
            case ALActionKind.Separator:
                return ALSymbolKind.Undefined;
        }
        return ALSymbolKind.Undefined;
    }

    private loadMethodSymbolReferences(symbolKind : ALSymbolKind, symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (var i = 0; i<symbolReferenceList.length; i++) {
                var symbolInfo = ALSymbolInfo.createFromSymbolReference(symbolKind, '', symbolReferenceList[i]);
                //build procedure name
                var parameters = symbolReferenceList[i].Parameters;
                var paramText = '';
                if (parameters) {
                    for (var p=0; p < parameters.length; p++) {
                        if (p > 0)
                            paramText = paramText + ', ';
                        paramText = paramText + parameters[p].Name;
                    }
                }
                symbolInfo.name = symbolInfo.symbolName + '(' + paramText + ')';
                
                this.addChild(symbolInfo, false);
            }
        }
    }

}
