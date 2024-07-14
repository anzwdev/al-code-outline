'use strict';

import * as vscode from 'vscode';
import * as path from 'path';
import * as vscodelangclient from 'vscode-languageclient';
import { ALSyntaxHelper } from './alSyntaxHelper';
import { Version } from '../tools/version';
import { TextEditorHelper } from '../tools/textEditorHelper';
import { AppAreaMode } from '../alsyntaxmodifiers/appAreaMode';
import { ALFieldToolTipsLocation } from './alFieldToolTipsLocation';

export class ALLangServerProxy {
    //!!!private langClient : vscodelangclient.LanguageClient | undefined;
    public extensionPath : string | undefined;
    public version : Version;
    public alEditorService: any;

    constructor() {
        this.version = new Version();
        //!!!this.langClient = undefined;
        this.alEditorService = undefined;
        this.checkExtensionProperties();
    }

    protected getALExtension() : any {
        let alStoreExtension = vscode.extensions.getExtension("ms-dynamics-smb.al"); 
        let alFileExtension = vscode.extensions.getExtension("Microsoft.al");

        if ((alStoreExtension) && (alFileExtension)) {
            if (alStoreExtension.isActive)
                return alStoreExtension;
            if (alFileExtension.isActive)
                return alFileExtension;
            return alStoreExtension;
        }

        if (alStoreExtension)
            return alStoreExtension;

        return alFileExtension;
    }

    protected checkExtensionProperties() {
        let alExtension = this.getALExtension();
        if (alExtension) {
            this.extensionPath = alExtension.extensionPath;
            if (alExtension.packageJSON) 
                this.version.parse(alExtension.packageJSON.version);
        }
    }

    protected checkLanguageClient() : boolean {
        if (!this.alEditorService) {
            let alExtension = this.getALExtension();
            if ((!alExtension) || (!alExtension.isActive))
                return false;

            if (alExtension.exports) {
                //find editor service
                if (alExtension.exports.services) {
                    let alServices = alExtension.exports.services;
                    for (let sidx = 0; (sidx < alServices.length) && (!this.alEditorService); sidx++) {
                        if (alServices[sidx].setActiveWorkspace)
                            this.alEditorService = alServices[sidx];
                    }
                }
            }

        }
        return true;
    }

    protected getWorkspaceSettings(resourceUri: vscode.Uri, workspacePath: string): any {
        if (!resourceUri)
            resourceUri = vscode.Uri.file(workspacePath);            
        let alConfig = vscode.workspace.getConfiguration('al', resourceUri);

        let alPackages: string | string[] | undefined = alConfig.get("packageCachePath");
        if (alPackages) {
            if (typeof(alPackages) != 'string') {
                if (alPackages.length > 0)
                    alPackages = alPackages[0];
                else
                    alPackages = undefined;
            }
        }

        return {
            workspacePath: workspacePath,
            alResourceConfigurationSettings: {
                assemblyProbingPaths: alConfig.get("assemblyProbingPaths"),
                codeAnalyzers: alConfig.get("codeAnalyzers"),
                enableCodeAnalysis: alConfig.get("enableCodeAnalysis"),
                backgroundCodeAnalysis: alConfig.get("backgroundCodeAnalysis"),
                packageCachePath: alPackages,
                ruleSetPath: alConfig.get("ruleSetPath"),
                enableCodeActions: alConfig.get("enableCodeActions"),
                incrementalBuild: alConfig.get("incrementalBuild"),
            },
            setActiveWorkspace: true,
            dependencyParentWorkspacePath: undefined
        };
    }

    getCurrentWorkspaceFolderPath() : string | undefined {
        if ((!vscode.workspace.workspaceFolders) || (vscode.workspace.workspaceFolders.length == 0))
            return undefined;
        if (vscode.workspace.workspaceFolders.length > 1) {
            this.checkLanguageClient();
            if (this.alEditorService) {
                if (this.alEditorService.lastActiveWorkspacePath)
                    return this.alEditorService.lastActiveWorkspacePath;
            }
        }
        return vscode.workspace.workspaceFolders[0].uri.fsPath;
    }

    protected getSymbolLabels(list: vscode.CompletionList, kind: vscode.CompletionItemKind): string[] {
        let out : string[] = [];
        
        if (list && list.items) {
            for (let i=0; i<list.items.length; i++) {
                let item = list.items[i];
                if (item.kind === kind) {
                    out.push(ALSyntaxHelper.fromNameText(item.label.toString()));
                }
            }
        }
        return out;
    }

    async getWorkspaceSymbol(objectType : string, objectName : string) : Promise<vscode.Location | undefined> {       
        let list = await vscode.commands.executeCommand<vscode.SymbolInformation[] | undefined>('vscode.executeWorkspaceSymbolProvider', objectName);
        if ((!list) || (list.length == 0))
            return undefined;
        let fullName : string = objectType + ' ' + objectName;
        fullName = fullName.toLowerCase();        
        for (let i=0; i<list.length; i++) {
            let item = list[i];
            if (((item.kind == vscode.SymbolKind.Class) || (item.kind == vscode.SymbolKind.Object)) && (item.name.toLowerCase() == fullName) && (item.location)) {
                return item.location;
            }
        }
        return undefined;
    }

    /*
    !!!
    async getDefinitionLocationFromDocument(docUri: string, pos: vscode.Position) : Promise<vscode.Location | undefined> {
        let docPos : vscode.Location | undefined = undefined;
        try {
            this.checkLanguageClient();
            if (!this.langClient)
                return undefined;

            let tokenSource : vscode.CancellationTokenSource = new vscode.CancellationTokenSource();
            let token : vscode.CancellationToken = tokenSource.token;
            let launchConfiguration = await this.getLaunchConfiguration();

            if (launchConfiguration) {

                let docPosTemp : any = await this.langClient.sendRequest<any>('al/gotodefinition', {
                    launchConfiguration : launchConfiguration,
                    textDocumentPositionParams : {
                        textDocument : {
                            uri : docUri.toString()
                        },
                        position : {
                            line : pos.line,
                            character : pos.character
                        }
                    },
                    context : undefined
                }, token);

                if (docPosTemp) {
                    docPos = new vscode.Location(
                        vscode.Uri.parse(docPosTemp.uri),
                        new vscode.Range(docPosTemp.range.start.line, docPosTemp.range.start.character,
                            docPosTemp.range.end.line, docPosTemp.range.end.character));                    
                }

            }
        }   
        catch (e) {
            return undefined;
        }    

        return docPos; 
    }
    */

    async getLaunchConfiguration() : Promise<any|undefined> {
        if ((!vscode.workspace.workspaceFolders) || (vscode.workspace.workspaceFolders.length == 0))
            return undefined;
        let launchFilePath = path.join(vscode.workspace.workspaceFolders[0].uri.fsPath, '.vscode/launch.json');
        let config = vscode.workspace.getConfiguration("launch", vscode.Uri.file(launchFilePath));
        let allConfigList : any[] | undefined = config.get("configurations");
        if (!allConfigList)
            return undefined;
        let configList = allConfigList.filter(p => p.type === 'al');
        if ((!configList) || (configList.length == 0))
            return undefined;
        if (configList.length == 1)
            return configList[0];
        //select configuration from drop down list
        let configItems : string[] = [];
        for (let i=0; i<configList.length; i++) {
            if (configList[i].name)
                configItems.push(configList[i].name);
        }
        let selectedItem = await vscode.window.showQuickPick(configItems, {
            placeHolder: 'Please select launch configuration'
        });
        if (selectedItem) {
            for (let i=0; i<configList.length; i++) {
                if (configList[i].name == selectedItem)
                    return configList[i];
            }
        }

        return undefined;
    }

    getAppManifest(resourceUri: vscode.Uri | undefined): any {
        let folder: vscode.WorkspaceFolder | undefined;
        if (resourceUri) 
            folder = vscode.workspace.getWorkspaceFolder(resourceUri);
        else if (vscode.workspace.workspaceFolders)
            folder = vscode.workspace.workspaceFolders[0];        

        if (folder) {
            //load app.json
            let appFilePath = path.join(folder.uri.fsPath, "app.json");
            try {
                let fs = require('fs');
                let content = fs.readFileSync(appFilePath, 'utf8');
                
                //remove BOM from UTF-8
                if ((content) && (content.length > 0) && (content.charCodeAt(0) === 0xFEFF)) {
                    content = content.substr(1);
                }

                return JSON.parse(content);
            }
            catch (e) {
            }
        }

        return undefined;
    }

    getRuntimeVersion(resourceUri: vscode.Uri | undefined): Version {
        let version = new Version();
        let appData = this.getAppManifest(resourceUri);
        if ((appData) && (appData.runtime)) {
            version.parse(appData.runtime);
        }
        return version;
    }

    getIdRangeStart(resourceUri: vscode.Uri | undefined): number {
        let val: number = 0;
        let appData = this.getAppManifest(resourceUri);
        if (appData) {
            
            if ((appData.idRange) && (appData.idRange.from)) {
                val = Number.parseInt(appData.idRange.from);
                if (!isNaN(val))
                    return val;
            }
            
            if ((appData.idRanges) && (appData.idRanges.length)) {
                for (let i=0; i<appData.idRanges.length; i++) {
                    if (appData.idRanges[i].from) {
                        val = Number.parseInt(appData.idRanges[i].from);
                        if (!isNaN(val))
                            return val;
                    }
                }
            }
        }

        return 0;
    }

    supportsInterfaces(resourceUri: vscode.Uri | undefined) {
        let runtimeVersion = this.getRuntimeVersion(resourceUri);
        let interfacesVersion = Version.create("5.0");
        return runtimeVersion.isGreaterOrEqual(interfacesVersion);
    }

    supportsAppAreasInheritance(resourceUri: vscode.Uri | undefined): boolean {
        let runtimeVersion = this.getRuntimeVersion(resourceUri);
        let inheritAppAreasMinVersion = Version.create("10.0");
        return runtimeVersion.isGreaterOrEqual(inheritAppAreasMinVersion);
    }

    getAppAreaMode(resourceUri: vscode.Uri | undefined) : AppAreaMode {
        if (!this.supportsAppAreasInheritance(resourceUri))
            return AppAreaMode.addToAllControls;

        let settings = vscode.workspace.getConfiguration('alOutline', resourceUri);

        let appAreaModeValue = settings.get<string>('appAreaMode');
        if ((appAreaModeValue) && (appAreaModeValue != '')) {
            let type = (<any>AppAreaMode)[appAreaModeValue];
            if (type !== undefined)
                return type;
        }
       
        return AppAreaMode.inheritFromMainObject;
    }

    fieldToolTipsLocation(resourceUri: vscode.Uri | undefined) : ALFieldToolTipsLocation {
        let settings = vscode.workspace.getConfiguration('alOutline', resourceUri);
        let locationValue = settings.get<string>('fieldToolTipsLocation');
        let location = ALFieldToolTipsLocation.page;
        if ((locationValue) && (locationValue === 'table')) {
            let runtimeVersion = this.getRuntimeVersion(resourceUri);
            let tableToolTipsMinVersion = Version.create("13.0");
            if (runtimeVersion.isGreaterOrEqual(tableToolTipsMinVersion)) {
                location = ALFieldToolTipsLocation.table;
            }
        }
        return location;
    }

}