'use strict';

import * as path from 'path';
import * as vscode from 'vscode';
import { ToolsPackageSymbolsRequest } from '../langserver/toolsPackageSymbolsRequest';
import { ToolsPackageSymbolsResponse } from '../langserver/toolsPackageSymbolsResponse';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolsLibrary } from './azSymbolsLibrary';
import { AZSymbolInformation } from './azSymbolInformation';
import { AZSymbolKind } from './azSymbolKind';
import { ALControlKind } from './alControlKind';
import { ALActionKind } from './alActionKind';
import { AsyncFileManager } from '../tools/asyncFileManager';
import { ALSyntaxHelper } from '../allanguage/alSyntaxHelper';

export class ALAppSymbolsLibrary extends AZSymbolsLibrary {
    filePath : string;
    applicationId : string;
    name : string;
    publisher : string;
    version : string;
    private _fileSize : number;
    private _fileModified : number;
    protected _context : DevToolsExtensionContext;

    constructor(context : DevToolsExtensionContext, sourceFilePath : string) {
        super();
        
        this._context = context;
        this.filePath = sourceFilePath;
        this.applicationId = '';
        this.displayName = path.parse(sourceFilePath).base; 
        this.name = '';
        this.publisher = '';
        this.version = '';

        this._fileSize = -1;
        this._fileModified = -1;
    }
  
    protected async loadInternalAsync(forceReload : boolean) : Promise<boolean> {
        try {
            let request : ToolsPackageSymbolsRequest = new ToolsPackageSymbolsRequest(this.filePath);
            let response : ToolsPackageSymbolsResponse | undefined = await this._context.toolsLangServerClient.GetAppPackageSymbols(request);
            if ((response) && (response.root))
                this.rootSymbol = AZSymbolInformation.fromAny(response.root);
            else
                this.rootSymbol = AZSymbolInformation.create(AZSymbolKind.Document, this.displayName);
        }
        catch (e) {
            let msg : string = 'Loading symbols from file "' + this.filePath + '" failed.';
            if (e.message)
                msg = msg + ' (' + e.message + ')';
            else
                msg = msg + ' (UNDEFINED ERROR)';
            vscode.window.showErrorMessage(msg);
            return false;
        }
        return true;
    }
   

    protected async loadInternalAsync2(forceReload : boolean) : Promise<boolean> {
        try {
            let fileManager : AsyncFileManager = new AsyncFileManager();
            let AdmZip = require('adm-zip');

            //load zip file into memory
            let offset = 40;
            let stats = await fileManager.statAsync(this.filePath);

            //check if file should be reloaded
            if ((!forceReload) && (stats.size === this._fileSize) && (stats.mtimeMs === this._fileModified))
                return false;

            this._fileSize = stats.size;
            this._fileModified = stats.mtimeMs;

            let fileSizeInBytes = stats.size - offset;
            let buffer : Buffer = Buffer.alloc(fileSizeInBytes);
            
            let fileDesc = await fileManager.openAsync(this.filePath, 'r', 0o666);
            await fileManager.readAsync(fileDesc, buffer, 0, fileSizeInBytes, offset);
            await fileManager.closeAsync(fileDesc);

            //load symbol references json from zip file
            let zip = new AdmZip(buffer);      
            let zipEntry = zip.getEntry('SymbolReference.json');
            let jsonObjectList : string = zip.readAsText(zipEntry).trim();

            //parse symbol references
            this.loadSymbols(JSON.parse(jsonObjectList));
        }
        catch (e) {
            let msg : string = 'Loading symbols from file "' + this.filePath + '" failed.';
            if (e.message)
                msg = msg + ' (' + e.message + ')';
            else
                msg = msg + ' (UNDEFINED ERROR)';
            vscode.window.showErrorMessage(msg);
            return false;
        }

        return true;
    }

    protected loadSymbols(symbolReferences : any) {
        //load app file header information    
        this.applicationId = symbolReferences.AppId?symbolReferences.AppId:'';
        this.name = symbolReferences.Name?symbolReferences.Name:'';
        this.publisher = symbolReferences.Publisher?symbolReferences.Publisher:'';
        this.version = symbolReferences.Version?symbolReferences.Version:'';

        let fullName : string = this.publisher + ' ' + this.name + ' ' + this.version;
        fullName = fullName.trim();

        //load root symbol
        this.rootSymbol = AZSymbolInformation.create(AZSymbolKind.Library, fullName);

        //load symbol references
        this.rootSymbol.addChildItem(this.loadObjectSymbolList('Tables', 'Table', AZSymbolKind.TableObjectList, AZSymbolKind.TableObject, symbolReferences.Tables));
        this.rootSymbol.addChildItem(this.loadObjectSymbolList('Pages', 'Page', AZSymbolKind.PageObjectList,  AZSymbolKind.PageObject, symbolReferences.Pages));
        this.rootSymbol.addChildItem(this.loadObjectSymbolList('Reports', 'Report', AZSymbolKind.ReportObjectList, AZSymbolKind.ReportObject, symbolReferences.Reports));
        this.rootSymbol.addChildItem(this.loadObjectSymbolList('Xml Ports', 'XmlPort', AZSymbolKind.XmlPortObjectList, AZSymbolKind.XmlPortObject, symbolReferences.XmlPorts));
        this.rootSymbol.addChildItem(this.loadObjectSymbolList('Queries', 'Query', AZSymbolKind.QueryObjectList, AZSymbolKind.QueryObject, symbolReferences.Queries));
        this.rootSymbol.addChildItem(this.loadObjectSymbolList('Codeunits', 'Codeunit', AZSymbolKind.CodeunitObjectList, AZSymbolKind.CodeunitObject, symbolReferences.Codeunits));
        this.rootSymbol.addChildItem(this.loadObjectSymbolList('Control Add-ins', 'ControlAddIn', AZSymbolKind.ControlAddInObjectList, AZSymbolKind.ControlAddInObject, symbolReferences.ControlAddIns));
        this.rootSymbol.addChildItem(this.loadObjectSymbolList('Page Extensions', 'PageExtension', AZSymbolKind.PageExtensionObjectList, AZSymbolKind.PageExtensionObject, symbolReferences.PageExtensions));
        this.rootSymbol.addChildItem(this.loadObjectSymbolList('Table Extensions', 'TableExtension', AZSymbolKind.TableExtensionObjectList, AZSymbolKind.TableExtensionObject, symbolReferences.TableExtensions));
        this.rootSymbol.addChildItem(this.loadObjectSymbolList('Profiles', 'Profile', AZSymbolKind.ProfileObjectList, AZSymbolKind.ProfileObject, symbolReferences.Profiles));
        this.rootSymbol.addChildItem(this.loadObjectSymbolList('Page Customizations', 'PageCustomization', AZSymbolKind.PageCustomizationObjectList, AZSymbolKind.PageCustomizationObject, symbolReferences.PageCustomizations));
        this.rootSymbol.addChildItem(this.loadObjectSymbolList('DotNet Packages', 'DotNetPackage', AZSymbolKind.DotNetPackageList, AZSymbolKind.DotNetPackage, symbolReferences.DotNetPackages));
        this.rootSymbol.addChildItem(this.loadObjectSymbolList('Enums', 'Enum', AZSymbolKind.EnumTypeList, AZSymbolKind.EnumType, symbolReferences.EnumTypes));
    }

    protected loadObjectSymbolList(name : string, symbolPrefix : string, groupKind: AZSymbolKind, symbolKind : AZSymbolKind, data : any) : AZSymbolInformation | undefined {       
        //load object symbols
        if ((data) && (data.length > 0)) {
            let groupSymbol : AZSymbolInformation = AZSymbolInformation.create(groupKind, name);
            
            for (let i=0; i<data.length; i++) {
                groupSymbol.addChildItem(this.loadObjectSymbol(symbolKind, symbolPrefix, data[i]));
            }

            return groupSymbol;
        }

        return undefined;
    }

    protected loadObjectSymbol(symbolKind : AZSymbolKind, namePrefix : string, data : any) : AZSymbolInformation {
        let symbol : AZSymbolInformation = this.loadSymbolFromReference(symbolKind, namePrefix, data, true);

        //init child items
        if (data.Fields)
            this.loadBasicSymbolReferences('fields', symbol, AZSymbolKind.FieldList, AZSymbolKind.Field, data.Fields, (symbolKind == AZSymbolKind.TableObject));
        if (data.Keys)
            this.loadBasicSymbolReferences('keys', symbol, AZSymbolKind.KeyList, AZSymbolKind.Key, data.Keys, false);
        if (data.Controls)
            this.loadControlSymbolReferences(symbol, data.Controls);
        if (data.Actions)
            this.loadActionSymbolReferences(symbol, data.Actions);   
         //this.loadBasicSymbolReferences(ALSymbolKind.Field, symbolReference.DataItems);
        
        if (data.DataItems)
            this.loadDataItemSymbolReferences(symbol, data.DataItems);   
        if (data.Elements)
            this.loadDataItemSymbolReferences(symbol, data.Elements);   
        
        if (data.ActionChanges)
            this.loadChangesSymbolReferences(symbol, 'Action Changes', data.ActionChanges);
        if (data.ControlChanges)
            this.loadChangesSymbolReferences(symbol, 'Control Changes', data.ControlChanges);

        if (data.Variables)
            this.loadBasicSymbolReferences('var', symbol, AZSymbolKind.VarSection, AZSymbolKind.VariableDeclaration, data.Variables, false);
        if (data.Methods)
            this.loadMethodSymbolReferences('procedures', symbol, AZSymbolKind.MethodDeclaration, AZSymbolKind.LocalMethodDeclaration, data.Methods);
        //control Add-In triggers
        if (data.Events)
            this.loadMethodSymbolReferences('Ttriggers', symbol, AZSymbolKind.TriggerDeclaration, AZSymbolKind.TriggerDeclaration, data.Events);

        //enum values
        if (data.Values)
            this.loadEnumValues(symbol, data.Values);

        //.net assemblies
        if (data.AssemblyDeclarations)
            this.loadAssemblyDeclarations(symbol, data.AssemblyDeclarations);

        return symbol;
    }

    protected loadFromDotNetTypeReference(symbolReference : any) : AZSymbolInformation {
        let name : string = (symbolReference.TypeName)?symbolReference.TypeName:'';        
        let alSymbol : AZSymbolInformation = AZSymbolInformation.create(AZSymbolKind.DotNetTypeDeclaration, name);

        if (symbolReference.AliasName)
            alSymbol.fullName = symbolReference.AliasName + " (" + alSymbol.name + ")";
        else
            alSymbol.fullName = alSymbol.name;
        return alSymbol;
    }

    protected loadDotNetTypeDeclarations(parent : AZSymbolInformation, symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (let i=0; i<symbolReferenceList.length; i++) {
                let alSymbol = this.loadFromDotNetTypeReference(symbolReferenceList[i]);
                parent.addChildItem(alSymbol);
            }
        }
    }

    protected loadAssemblyDeclarations(parent : AZSymbolInformation, symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (let i=0; i<symbolReferenceList.length; i++) {
                let symbolRef = symbolReferenceList[i];
                let alSymbol = this.loadSymbolFromReference(AZSymbolKind.DotNetAssembly, '', symbolRef, false); 
                parent.addChildItem(alSymbol);
                //load type definitions
                if (symbolRef.TypeDeclarations)
                    this.loadDotNetTypeDeclarations(alSymbol, symbolRef.TypeDeclarations);
            }
        }
    }

    protected loadFromEnumValueReference(symbolIndex : number, symbolReference : any) : AZSymbolInformation {
        let alSymbol : AZSymbolInformation = this.loadSymbolFromReference(AZSymbolKind.EnumType, '', symbolReference, true);
        
        let idx : string = symbolIndex.toString();
        if (symbolReference.Ordinal)
            idx = symbolReference.Ordinal;

        alSymbol.fullName = alSymbol.name + " (" + idx + ")"; 

        return alSymbol;
    }

    protected loadEnumValues(parent : AZSymbolInformation, symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (let i=0; i<symbolReferenceList.length; i++) {
                let alSymbol = this.loadFromEnumValueReference(i, symbolReferenceList[i]);
                parent.addChildItem(alSymbol);
            }
        }
    }

    protected loadMethodSymbolReferences(groupName : string, parent : AZSymbolInformation, symbolKind : AZSymbolKind, privateSymbolKind : AZSymbolKind, symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            let listParent : AZSymbolInformation;
            if (groupName != '') {
                listParent = AZSymbolInformation.create(AZSymbolKind.SymbolGroup, groupName);
                parent.addChildItem(listParent);
            } else
                listParent = parent;

            for (let i = 0; i<symbolReferenceList.length; i++) {
                let kind : AZSymbolKind;
                if (symbolReferenceList[i].IsLocal)
                    kind = privateSymbolKind;
                else
                    kind = symbolKind;                
                let alSymbol = this.loadSymbolFromReference(kind, '', symbolReferenceList[i], false);

                //build procedure name
                let parameters = symbolReferenceList[i].Parameters;
                let paramText = '';
                if (parameters) {
                    for (let p=0; p < parameters.length; p++) {
                        if (p > 0)
                            paramText = paramText + ', ';
                        paramText = paramText + parameters[p].Name;
                    }
                }
                alSymbol.fullName = alSymbol.name + '(' + paramText + ')';

                listParent.addChildItem(alSymbol);
            }
        }
    }

    protected loadChangesSymbolReferences(parent : AZSymbolInformation, changesName : string, symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (let i = 0; i<symbolReferenceList.length; i++) {
                let symbolRef = symbolReferenceList[i];
                let alSymbol = this.loadSymbolFromReference(AZSymbolKind.Undefined, changesName, symbolRef, false); 
                parent.addChildItem(alSymbol);
                //load symbol actions
                if (symbolRef.Actions)
                    this.loadActionSymbolReferences(alSymbol, symbolRef.Actions);
                if (symbolRef.Controls)
                    this.loadControlSymbolReferences(alSymbol, symbolRef.Controls);
            }
        }
    }

    protected loadDataItemSymbolReferences(parent : AZSymbolInformation, symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            for (let i = 0; i<symbolReferenceList.length; i++) {
                let symbolRef = symbolReferenceList[i];
                let alSymbol = this.loadSymbolFromReference(AZSymbolKind.ReportDataItem, 'DataItem', symbolRef, false);
                parent.addChildItem(alSymbol);
                //load symbol items
                if (symbolRef.Columns)
                    this.loadBasicSymbolReferences('', alSymbol, AZSymbolKind.SymbolGroup, AZSymbolKind.ReportColumn, symbolRef.Columns, false);
                if (symbolRef.DataItems)
                    this.loadDataItemSymbolReferences(alSymbol, symbolRef.DataItems);
            }
        }
    }

    protected loadActionSymbolReferences(parent : AZSymbolInformation, symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            //page layout
            if ((parent.kind == AZSymbolKind.PageObject) || 
                (parent.kind == AZSymbolKind.PageExtensionObject) ||
                (parent.kind == AZSymbolKind.RequestPage)) {
                let actions = AZSymbolInformation.create(AZSymbolKind.PageActionList, 'actions');
                parent.addChildItem(actions);
                parent = actions;
            }

            for (let i = 0; i<symbolReferenceList.length; i++) {
                let symbolRef = symbolReferenceList[i];
                let actionKind : ALActionKind = symbolRef.Kind;
                let alSymbol : AZSymbolInformation = this.loadSymbolFromReference(this.alActionKindToSymbolKind(actionKind), ALActionKind[actionKind], symbolRef, false); 
                parent.addChildItem(alSymbol);
                //load symbol actions
                if (symbolRef.Actions)
                    this.loadActionSymbolReferences(alSymbol, symbolRef.Actions);
            }
        }
    }

    protected loadControlSymbolReferences(parent : AZSymbolInformation, symbolReferenceList : any[]) {
        if (symbolReferenceList) {
            //page layout
            if ((parent.kind == AZSymbolKind.PageObject) || 
                (parent.kind == AZSymbolKind.PageExtensionObject) ||
                (parent.kind == AZSymbolKind.RequestPage)) {
                let layout = AZSymbolInformation.create(AZSymbolKind.PageLayout, 'layout');
                parent.addChildItem(layout);
                parent = layout;
            }

            for (let i = 0; i<symbolReferenceList.length; i++) {
                let symbolRef = symbolReferenceList[i];
                let controlKind : ALControlKind = symbolRef.Kind;
                let alSymbol : AZSymbolInformation = this.loadSymbolFromReference(this.alControlKindToSymbolKind(controlKind), ALControlKind[controlKind], symbolRef, false);
                parent.addChildItem(alSymbol);                
                //load symbol controls
                if (symbolRef.Controls)
                    this.loadControlSymbolReferences(alSymbol, symbolRef.Controls);
            }
        }
    }

    protected loadBasicSymbolReferences(groupName : string, parent : AZSymbolInformation, groupKind: AZSymbolKind, symbolKind : AZSymbolKind, symbolReferenceList : any[], loadIds : boolean) {
        if (symbolReferenceList) {
            let listParent : AZSymbolInformation;
            if (groupName != '') {
                listParent = AZSymbolInformation.create(groupKind, groupName);
                parent.addChildItem(listParent);
            } else
                listParent = parent;

            for (var i = 0; i<symbolReferenceList.length; i++) {
                let importSymbolKind : AZSymbolKind = symbolKind;
                if ((i == 0) && (symbolKind == AZSymbolKind.Key))
                    importSymbolKind = AZSymbolKind.PrimaryKey;
                listParent.addChildItem(this.loadSymbolFromReference(importSymbolKind, '', symbolReferenceList[i], loadIds));
            }
        }
    }
   
    protected loadSymbolFromReference(symbolKind : AZSymbolKind, namePrefix : string, data : any, loadIds : boolean) : AZSymbolInformation {
        let name : string = (data.Name)?data.Name:'';
        let fullName : string = ALSyntaxHelper.toNameText(name);
        if (namePrefix !== '')
            fullName = namePrefix + ' ' + fullName;
        
        let symbol : AZSymbolInformation = AZSymbolInformation.create(symbolKind, name);
        symbol.fullName = fullName.trim();

        if ((data.TypeDefinition) && (data.TypeDefinition.Name))
            symbol.fullName = symbol.fullName + ": " + data.TypeDefinition.Name;

        if ((data.FieldNames) && (data.FieldNames.length > 0)) {
            let fieldList = '';
            for (let i=0; i<data.FieldNames.length; i++) {
                if (i > 0)
                    fieldList = fieldList + ", ";
                fieldList = fieldList + ALSyntaxHelper.toNameText(data.FieldNames[i]);
            }
            symbol.fullName = symbol.fullName + ": " + fieldList;
        }

        if ((data.Id) && (loadIds))
            symbol.id = data.Id;


        return symbol;
    }

    private alControlKindToSymbolKind(controlKind : ALControlKind) : AZSymbolKind {
        switch (controlKind) {
            case ALControlKind.Area:
                return AZSymbolKind.PageArea;
            case ALControlKind.Group:
            return AZSymbolKind.PageGroup;
            case ALControlKind.CueGroup:
            return AZSymbolKind.PageGroup;
            case ALControlKind.Repeater:
            return AZSymbolKind.PageGroup;
            case ALControlKind.Fixed:
            case ALControlKind.Grid:
                return AZSymbolKind.PageGroup;
            case ALControlKind.Part:
            return AZSymbolKind.PagePart;
            case ALControlKind.SystemPart:
                return AZSymbolKind.PageSystemPart;
            case ALControlKind.Field:
                return AZSymbolKind.PageField;
            case ALControlKind.Label:
                return AZSymbolKind.PageLabel;
            case ALControlKind.UserControl:
                return AZSymbolKind.PageUserControl;
            case ALControlKind.Chart:
                return AZSymbolKind.PageChartPart;
        }
        return AZSymbolKind.Undefined;
    }

    private alActionKindToSymbolKind(actionKind : ALActionKind) : AZSymbolKind {
        switch (actionKind) {
            case ALActionKind.Group:
                return AZSymbolKind.PageActionGroup;
            case ALActionKind.Area:
                return AZSymbolKind.PageActionArea;
            case ALActionKind.Action:
                return AZSymbolKind.PageAction;
            case ALActionKind.Separator:
                return AZSymbolKind.PageActionSeparator;
        }
        return AZSymbolKind.Undefined;
    }

} 