import { SymbolInformation, SymbolKind, ExtensionContext } from "vscode";
import * as path from 'path';
import { ALSymbolKind } from "./alSymbolKind";
import { WSANOTINITIALISED } from "constants";

export class ALSymbolInfo {
    lspSymbol : SymbolInformation;  //Symbol Information provided by language server protocol
    childItems : ALSymbolInfo[];
    parentItem : ALSymbolInfo;
    name : string;
    private alKind : ALSymbolKind;
    private hasKeys : boolean;

    constructor(symbol?: SymbolInformation) {
        this.parentItem = null;
        this.hasKeys = false;
        this.childItems = [];
        this.lspSymbol = symbol;
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
        //sort child nodes by position

        //add nodes one by one
        var prevChildNode : ALSymbolInfo = this;
        for (var i = 0; i<childNodes.length;i++) {
            var currNode = childNodes[i];
            var parentNode = this.findParent(prevChildNode, currNode);
            parentNode.addChild(currNode);
            prevChildNode = currNode;
        }
    }

    public addChild(childItem : ALSymbolInfo) {
        //detect primary key
        if ((!this.hasKeys) && (childItem.alKind == ALSymbolKind.Key)) {
            this.hasKeys = true;
            childItem.alKind = ALSymbolKind.PrimaryKey;
        }
        //add item to the child items
        this.childItems.push(childItem);
        childItem.setParent(this);
    }

    public setParent(parentItem : ALSymbolInfo) {
        this.parentItem = parentItem;
        this.updateSymbol();
    }

    //-----------------------------------------------------
    // symbol definition processing
    //-----------------------------------------------------

    private updateSymbol() {
        //name
        this.name = this.lspSymbol.name;
        this.alKind = this.GetALKind();
        
        //uncomment line below for debug messages
        //this.name = this.alKind.toString() + ": " + this.lspSymbol.kind.toString() + ": " + this.name;
    }

    private GetALKind() : ALSymbolKind {
        var symbolKind : SymbolKind;
        symbolKind = this.lspSymbol.kind + 1;
        
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
            case ALSymbolKind.Undefined:
                return "tree-undefined.svg";
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

    public getIcon(context : ExtensionContext): any {
        let iconName = this.getIconName();
        if (iconName)
            return {
                light: context.asAbsolutePath(path.join("resources", "light", iconName)),
                dark: context.asAbsolutePath(path.join("resources", "dark", iconName))
            }
        return null;
	}


}