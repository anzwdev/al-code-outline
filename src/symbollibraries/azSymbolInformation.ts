'use strict';

import { AZSymbolKind } from "./azSymbolKind";
import { TextRange } from "./textRange";

export class AZSymbolInformation {
    id : number;
    idx: number;
    name : string;
    subtype : string | undefined;
    elementsubtype : string | undefined;
    fullName : string;
    kind : AZSymbolKind;
    icon : string;
    childSymbols : AZSymbolInformation[] | undefined;
    range: TextRange | undefined;
    selectionRange : TextRange | undefined;
    contentRange: TextRange | undefined;
    source: string | undefined;
    extends: string | undefined;
    parent: AZSymbolInformation | undefined;

    constructor() {
        this.id = 0;
        this.idx = -1;
        this.name = '';
        this.fullName = '';
        this.subtype = undefined;
        this.elementsubtype = undefined;
        this.icon = '';
        this.kind = AZSymbolKind.Undefined;
        this.childSymbols = undefined;
        this.range = undefined;
        this.selectionRange = undefined;
        this.contentRange = undefined;
        this.source = undefined;
        this.extends = undefined;
        this.parent = undefined;
    }

    public static create(newKind : AZSymbolKind, newName : string) {
        let symbol = new AZSymbolInformation();
        symbol.kind = newKind;
        symbol.name = newName;
        symbol.fullName = symbol.name;        
        return symbol;
    }

    public static createWithId(newId : number, newKind : AZSymbolKind, newName : string) {
        let symbol = AZSymbolInformation.create(newKind, newName);
        symbol.id = newId;
        return symbol;
    }

    public static fromAny(source : any) : AZSymbolInformation {
        let obj : AZSymbolInformation = new AZSymbolInformation();
        if (source.id)
            obj.id = source.id;
        if (source.name)
            obj.name = source.name;
        if (source.fullName)
            obj.fullName = source.fullName;
        if (source.kind)
            obj.kind = source.kind;        
        if (source.range)
            obj.range = TextRange.fromAny(source.range);
        if (source.selectionRange)
            obj.selectionRange = TextRange.fromAny(source.selectionRange);
        if (source.contentRange)
            obj.contentRange = TextRange.fromAny(source.contentRange);
        if (source.source)
            obj.source = source.source;
        if (source.extends)
            obj.extends = source.extends;
        if (source.subtype)
            obj.subtype = source.subtype;
        if (source.elementsubtype)
            obj.elementsubtype = source.elementsubtype;

        if (source.childSymbols)
            for (let i=0; i<source.childSymbols.length; i++)
                obj.addChildItem(AZSymbolInformation.fromAny(source.childSymbols[i]));
        return obj;
    }    

    public isALObject() : boolean {
        return ((this.kind == AZSymbolKind.TableObject) ||
        (this.kind == AZSymbolKind.CodeunitObject) ||
        (this.kind == AZSymbolKind.PageObject) ||
        (this.kind == AZSymbolKind.ReportObject) ||
        (this.kind == AZSymbolKind.QueryObject) ||
        (this.kind == AZSymbolKind.XmlPortObject) ||
        (this.kind == AZSymbolKind.TableExtensionObject) ||
        (this.kind == AZSymbolKind.PageExtensionObject) ||
        (this.kind == AZSymbolKind.ControlAddInObject) ||
        (this.kind == AZSymbolKind.EnumType) ||
        (this.kind == AZSymbolKind.EnumExtensionType) ||
        (this.kind == AZSymbolKind.DotNetPackage) ||
        (this.kind == AZSymbolKind.ProfileObject) ||
        (this.kind == AZSymbolKind.PageCustomizationObject) ||
        (this.kind == AZSymbolKind.Interface));
    }

    public createSharedCopy() : AZSymbolInformation {
        let symbol : AZSymbolInformation = this.createCopy(false);
        symbol.childSymbols = this.childSymbols;
        return symbol;
    }

    public createCopy(copyChildren : boolean) : AZSymbolInformation {
        let obj : AZSymbolInformation = new AZSymbolInformation();
        obj.id = this.id;
        obj.name = this.name;
        obj.fullName = this.fullName;
        obj.kind = this.kind;
        obj.icon = this.icon;
        obj.idx = this.idx;

        if ((copyChildren) && (this.childSymbols))
            for (let i=0; i<this.childSymbols.length; i++)
                obj.addChildItem(this.childSymbols[i].createCopy(copyChildren));

        return obj;
    }

    public toObjectTree() : AZSymbolInformation {
        let obj : AZSymbolInformation = this.createCopy(false);
        
        if (!this.isALObject()) {
            if (this.childSymbols)
                for (let i=0; i<this.childSymbols.length; i++) {
                    obj.addChildItem(this.childSymbols[i].toObjectTree());
            }
        }     

        return obj;
    }

    public addChildItem(childItem : AZSymbolInformation | undefined) {
        if (childItem) {
            if (!this.childSymbols)
                this.childSymbols = [];
            this.childSymbols.push(childItem);
        }
    }

    public updateTree(updateChildItems : boolean, twoWayTree: boolean) {
        //update icon
        this.icon = this.getIconName();

        //update child items
        if ((updateChildItems) && (this.childSymbols)) {
            for (let i=0; i<this.childSymbols.length; i++)
            {
                this.childSymbols[i].idx = i;
                if (twoWayTree)
                    this.childSymbols[i].parent = this;
                this.childSymbols[i].updateTree(true, twoWayTree);
            }
        }
    }

    protected getIconName() : string {
        switch (this.kind) {
            case AZSymbolKind.Class : return 'class';
            case AZSymbolKind.Package : return 'module';
            case AZSymbolKind.SymbolGroup : return 'module';
            case AZSymbolKind.Undefined : return 'undefined';
            case AZSymbolKind.TableObject : return 'table';
            case AZSymbolKind.CodeunitObject : return 'codeunit';
            case AZSymbolKind.PageObject : return 'page';
            case AZSymbolKind.ReportObject : return 'report';
            case AZSymbolKind.QueryObject : return 'query';
            case AZSymbolKind.XmlPortObject : return 'xmlport';
            case AZSymbolKind.TableExtensionObject : return 'tableextension';
            case AZSymbolKind.PageExtensionObject : return 'pageextension';
            case AZSymbolKind.ControlAddInObject : return 'controladdin';
            case AZSymbolKind.ProfileObject : return 'profile';
            case AZSymbolKind.PageCustomizationObject : return 'pagecustomization';
            case AZSymbolKind.EnumType : return 'enum';
            case AZSymbolKind.DotNetPackage : return 'dotnetlib';
            case AZSymbolKind.Interface: return 'interface';
            case AZSymbolKind.Property : return 'property';
            case AZSymbolKind.VariableDeclaration : return 'variable';
            case AZSymbolKind.VariableDeclarationName : return 'variable';
            case AZSymbolKind.Constant : return 'constant';
            case AZSymbolKind.Parameter : return 'parameter';
            case AZSymbolKind.MethodDeclaration : return 'method';
            case AZSymbolKind.LocalMethodDeclaration : return 'methodprivate';
            case AZSymbolKind.TriggerDeclaration : return 'trigger';
            //events
            case AZSymbolKind.IntegrationEventDeclaration: return 'eventpublisher';
            case AZSymbolKind.BusinessEventDeclaration: return 'eventpublisher';
            case AZSymbolKind.EventSubscriberDeclaration: return 'eventsubscriber';
            //tests
            case AZSymbolKind.TestDeclaration: return 'test';
            case AZSymbolKind.ConfirmHandlerDeclaration: return 'testcontroller';
            case AZSymbolKind.FilterPageHandlerDeclaration: return 'testcontroller';
            case AZSymbolKind.HyperlinkHandlerDeclaration: return 'testcontroller';
            case AZSymbolKind.MessageHandlerDeclaration: return 'testcontroller';
            case AZSymbolKind.ModalPageHandlerDeclaration: return 'testcontroller';
            case AZSymbolKind.PageHandlerDeclaration: return 'testcontroller';
            case AZSymbolKind.ReportHandlerDeclaration: return 'testcontroller';
            case AZSymbolKind.RequestPageHandlerDeclaration: return 'testcontroller';
            case AZSymbolKind.SendNotificationHandlerDeclaration: return 'testcontroller';
            case AZSymbolKind.SessionSettingsHandlerDeclaration: return 'testcontroller';
            case AZSymbolKind.StrMenuHandlerDeclaration: return 'testcontroller';

            case AZSymbolKind.Field : 
                if (this.subtype == 'ObsoletePending')
                    return 'fieldpending';
                if (this.subtype == 'ObsoleteRemoved')
                    return 'fieldobsolete';
                if (this.subtype == "Disabled")
                    return "fielddisabled";
                return 'field';
            case AZSymbolKind.PrimaryKey : return 'primarykey';
            case AZSymbolKind.Key : return 'key';
            case AZSymbolKind.FieldGroup : return 'fieldgroup';
            case AZSymbolKind.PageArea: return 'group';
            case AZSymbolKind.PageGroup : return 'group';
            case AZSymbolKind.PageRepeater: return 'repeater';
            case AZSymbolKind.PagePart: return 'pagepart';
            case AZSymbolKind.PageChartPart: return 'chartpart';
            case AZSymbolKind.PageSystemPart: return 'systempart';
            
            case AZSymbolKind.PageActionGroup: return 'group';
            case AZSymbolKind.PageActionArea: return 'group';
            case AZSymbolKind.PageAction : return 'action';
            case AZSymbolKind.EnumValue : return 'enumval';
            case AZSymbolKind.EnumExtensionType : return 'enumext';
            case AZSymbolKind.DotNetAssembly : return 'dotnetasm';
            case AZSymbolKind.DotNetTypeDeclaration : return 'dotnetclass';

            case AZSymbolKind.PageField: return 'field';
            case AZSymbolKind.FieldModification: return 'field';

            case AZSymbolKind.EventDeclaration: return 'trigger';
            case AZSymbolKind.EventTriggerDeclaration: return 'trigger';

            case AZSymbolKind.XmlPortSchema: return 'codeunit';
            case AZSymbolKind.XmlPortTableElement: return 'table';
            case AZSymbolKind.XmlPortFieldElement: return 'variable';
            case AZSymbolKind.XmlPortTextElement: return 'variable';
            case AZSymbolKind.XmlPortFieldAttribute: return 'parameter';
            case AZSymbolKind.XmlPortTextAttribute: return 'parameter';
            case AZSymbolKind.RequestPage: return 'page';
        
            //reports
            case AZSymbolKind.ReportDataSetSection: return 'codeunit';
            case AZSymbolKind.ReportLabelsSection: return 'codeunit';
            case AZSymbolKind.ReportDataItem: return 'table';
            case AZSymbolKind.ReportColumn: return 'field';
            case AZSymbolKind.ReportLabel: return 'variable';
            case AZSymbolKind.ReportLabelMultilanguage: return 'variable';
        
            //queries
            case AZSymbolKind.QueryElements: return 'codeunit';
            case AZSymbolKind.QueryDataItem: return 'table';
            case AZSymbolKind.QueryColumn: return 'field';
            case AZSymbolKind.QueryFilter: return 'parameter';

            //groups
            case AZSymbolKind.PageLayout:
            case AZSymbolKind.VarSection:
            case AZSymbolKind.GlobalVarSection:
            case AZSymbolKind.ParameterList:
            case AZSymbolKind.KeyList:
            case AZSymbolKind.FieldList:
            case AZSymbolKind.FieldGroupList:
            case AZSymbolKind.FieldExtensionList:
            case AZSymbolKind.PageViewList:
            case AZSymbolKind.PageActionList:
            case AZSymbolKind.GroupActionList:
            case AZSymbolKind.PageExtensionViewList:
            case AZSymbolKind.PageExtensionActionList:
                return 'codeunit';
        }
        return 'undefined';
    }

    public collectObjectSymbols(outList : AZSymbolInformation[]) {
        if (this.isALObject())
            outList.push(this);
        else if (this.childSymbols)
            for (let i=0; i<this.childSymbols.length; i++)
                this.childSymbols[i].collectObjectSymbols(outList);
    }
    
    public collectChildSymbols(symbolKind: AZSymbolKind, includeAllLevels: boolean, outList : AZSymbolInformation[]) {
        if (this.childSymbols) {
            for (let i=0; i<this.childSymbols.length; i++) {
                if (this.childSymbols[i].kind == symbolKind)
                    outList.push(this.childSymbols[i]);
                if (includeAllLevels)
                    this.childSymbols[i].collectChildSymbols(symbolKind, includeAllLevels, outList);
            }
        }
    }

    public collectChildSymbolsByKindList(symbolKindList: AZSymbolKind[], includeAllLevels: boolean, outList : AZSymbolInformation[]) {
        if (this.childSymbols) {
            for (let i=0; i<this.childSymbols.length; i++) {
                if (symbolKindList.indexOf(this.childSymbols[i].kind) >= 0)
                    outList.push(this.childSymbols[i]);
                if (includeAllLevels)
                    this.childSymbols[i].collectChildSymbolsByKindList(symbolKindList, includeAllLevels, outList);
            }
        }
    }

    public collectChildSymbolsByName(findName: string, includeAllLevels: boolean, outList : AZSymbolInformation[]) {
        if (this.childSymbols) {
            for (let i=0; i<this.childSymbols.length; i++) {
                if (this.childSymbols[i].name == findName)
                    outList.push(this.childSymbols[i]);
                if (includeAllLevels)
                    this.childSymbols[i].collectChildSymbolsByName(findName, includeAllLevels, outList);
            }
        }
    }


    public findFirstSymbolByKind(symbolKind: AZSymbolKind) : AZSymbolInformation | undefined {
        if (this.kind == symbolKind)
            return this;
        let symbol : AZSymbolInformation | undefined;

        if (this.childSymbols) {
            for (let i=0; i<this.childSymbols.length; i++) {
                symbol = this.childSymbols[i].findFirstSymbolByKind(symbolKind);
                if (symbol)
                    return symbol;
            }
        }
        return undefined;
    }

    public findFirstObjectSymbol() : AZSymbolInformation | undefined {
        if (this.isALObject())
            return this;
        let symbol : AZSymbolInformation | undefined;

        if (this.childSymbols) {
            for (let i=0; i<this.childSymbols.length; i++) {
                symbol = this.childSymbols[i].findFirstObjectSymbol();
                if (symbol)
                    return symbol;
            }
        }
        return undefined;
    }

    public findParentByKind(parentKind: AZSymbolKind) {
        let symbol = this.parent;
        while ((symbol) && (symbol.kind != parentKind))
            symbol = symbol.parent;
        return symbol;
    }

    public findParentByKindList(parentKind: AZSymbolKind[]): AZSymbolInformation | undefined {
        let symbol = this.parent;
        while ((symbol) && (parentKind.indexOf(symbol.kind) < 0))
            symbol = symbol.parent;
        return symbol;
    }

    public findParentObject(): AZSymbolInformation | undefined {
        let symbol: AZSymbolInformation | undefined = this;
        while ((symbol) && (!symbol.isALObject()))
            symbol = symbol.parent;
        return symbol;
    }

    public getPath() : number[] {
        let dataPath : number[] = [];
        let symbol : AZSymbolInformation | undefined = this;
        while (symbol) {
            dataPath.push(symbol.idx);
            symbol = symbol.parent;
        }
        return dataPath;
    }

    getObjectTypeName() : string {
        switch (this.kind)
        {
            case AZSymbolKind.TableObject:
                return "Table";
            case AZSymbolKind.PageObject:
                return "Page";
            case AZSymbolKind.ReportObject:
                return "Report";
            case AZSymbolKind.XmlPortObject:
                return "XmlPort";
            case AZSymbolKind.QueryObject:
                return "Query";
            case AZSymbolKind.CodeunitObject:
                return "Codeunit";
            case AZSymbolKind.ControlAddInObject:
                return "ControlAddIn";
            case AZSymbolKind.PageExtensionObject:
                return "PageExtension";
            case AZSymbolKind.TableExtensionObject:
                return "TableExtension";
            case AZSymbolKind.ProfileObject:
                return "Profile";
            case AZSymbolKind.PageCustomizationObject:
                return "PageCustomization";
            case AZSymbolKind.EnumType:
                return "Enum";
            case AZSymbolKind.DotNetPackage:
                return "DotNetPackage";
            case AZSymbolKind.EnumExtensionType:
                return "EnumExtension";
            case AZSymbolKind.Interface:
                return "Interface";
            default:
                return "Undefined";
        }
    }

}
