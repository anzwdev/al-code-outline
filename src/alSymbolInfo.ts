import { SymbolInformation, SymbolKind, ExtensionContext } from "vscode";
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
    symbolKindEnumFix : number;
    private hasKeys : boolean;


    constructor(symbol: SymbolInformation, newLanguageId : string) {
        this.symbolKindEnumFix = 1;
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
    }

    //----------------------------------------------------
    // child nodes operations
    //----------------------------------------------------

    private findParent(prevNode : ALSymbolInfo, currNode : ALSymbolInfo) : ALSymbolInfo {
        var parentNode = prevNode;
        while ((parentNode) && (parentNode.lspSymbol) && (parentNode.lspSymbol.name != currNode.lspSymbol.containerName)) {
            parentNode = parentNode.parentItem;
        }
        if (parentNode)
            return parentNode;
        return this;
    }

    public appendChildNodes(childNodes : ALSymbolInfo[]) {
        var symbolKindFixDetected : boolean = false;
        var newSymbolKindFixValue : number = 0;
        
        //add nodes one by one
        this.hasKeys = false;
        var prevChildNode : ALSymbolInfo = this;
        for (var i = 0; i<childNodes.length;i++) {
            var currNode = childNodes[i];
            
            //detect if SymbolKind bug has been fixed in this version
            if (!symbolKindFixDetected) {
                if (childNodes[i].lspSymbol) {
                    if ((childNodes[i].lspSymbol.kind + 1) == SymbolKind.Class)
                        newSymbolKindFixValue = 1;
                    else
                        newSymbolKindFixValue = 0;
                }
                this.symbolKindEnumFix = newSymbolKindFixValue;
                symbolKindFixDetected = true;
            }

            //append child nide
            var parentNode = this.findParent(prevChildNode, currNode);
            parentNode.addChild(currNode);
            prevChildNode = currNode;
        }
    }

    public addChild(childItem : ALSymbolInfo) {
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
        if (parentItem)
            this.symbolKindEnumFix = parentItem.symbolKindEnumFix;
        this.updateSymbol();
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
        var symbolKind : SymbolKind;
        if (this.languageId !== 'al')
            return ALSymbolKind.Undefined;
        
        symbolKind = this.lspSymbol.kind + this.symbolKindEnumFix;
        
        //try to detect al symbol kind
        switch (symbolKind) {
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
        }
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
        if (this.alKind == ALSymbolKind.Table)
            return "ALTable";
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
    }

    private loadBasicSymbolReferences(symbolKind : ALSymbolKind, symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (var i = 0; i<symbolReferenceList.length; i++) {
                this.addChild(ALSymbolInfo.createFromSymbolReference(symbolKind, '', symbolReferenceList[i]));
            }
        }
    }

    loadChangesSymbolReferences(changesName : string, symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (var i = 0; i<symbolReferenceList.length; i++) {
                var symbolRef = symbolReferenceList[i];
                var symbolInfo = ALSymbolInfo.createFromSymbolReference(ALSymbolKind.Group, changesName, symbolRef); 
                this.addChild(symbolInfo);
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
                this.addChild(symbolInfo);
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
                this.addChild(symbolInfo);
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
                this.addChild(symbolInfo);
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
                
                this.addChild(symbolInfo);
            }
        }
    }

}
