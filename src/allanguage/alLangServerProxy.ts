'use strict';

import * as vscode from 'vscode';
import * as path from 'path';
import * as vscodelangclient from 'vscode-languageclient';
import { ALSyntaxHelper } from './alSyntaxHelper';
import { Version } from '../tools/version';
import { TextEditorHelper } from '../tools/textEditorHelper';
import { ObjectCaptionsModifier } from '../alsyntaxmodifiers/objectCaptionsModifier';

export class ALLangServerProxy {
    private langClient : vscodelangclient.LanguageClient | undefined;
    public extensionPath : string | undefined;
    public version : Version;
    public alEditorService: any;

    constructor() {
        this.version = new Version();
        this.langClient = undefined;
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
        if (!this.langClient) {
            let alExtension = this.getALExtension();
            if ((!alExtension) || (!alExtension.isActive))
                return false;

            if (alExtension.exports) {
                //find language client
                if (alExtension.exports.languageServerClient) {
                    if (alExtension.exports.languageServerClient.languageClient)
                        this.langClient = alExtension.exports.languageServerClient.languageClient;
                    else
                        this.langClient = alExtension.exports.languageServerClient;
                 } else if (alExtension.exports.services) {
                    let services = alExtension.exports.services;
                    let langClientFound = false;
                    for (let sidx = 0; ((sidx < services.length) && (!langClientFound)); sidx++) {
                        if (services[sidx].languageServerClient) {                        
                            this.langClient = services[sidx].languageServerClient;
                            langClientFound = true;
                        }
                    }
                }

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
        return {
            workspacePath: workspacePath,
            alResourceConfigurationSettings: {
                assemblyProbingPaths: alConfig.get("assemblyProbingPaths"),
                codeAnalyzers: alConfig.get("codeAnalyzers"),
                enableCodeAnalysis: alConfig.get("enableCodeAnalysis"),
                backgroundCodeAnalysis: alConfig.get("backgroundCodeAnalysis"),
                packageCachePath: alConfig.get("packageCachePath"),
                ruleSetPath: alConfig.get("ruleSetPath"),
                enableCodeActions: alConfig.get("enableCodeActions"),
                incrementalBuild: alConfig.get("incrementalBuild"),
            },
            setActiveWorkspace: true,
            dependencyParentWorkspacePath: undefined
        };
    }

    async switchWorkspace(resourceUri: vscode.Uri, workspacePath: string) {
        if ((!this.langClient) ||
            (!this.alEditorService) || 
            (this.alEditorService.lastActiveWorkspacePath === workspacePath) ||
            (!vscode.workspace.workspaceFolders) || 
            (vscode.workspace.workspaceFolders.length <= 1))
            return;

        //switch workspace
        let result = await this.langClient.sendRequest('al/setActiveWorkspace', {
            currentWorkspaceFolderPath: workspacePath,
            settings: this.getWorkspaceSettings(resourceUri, workspacePath)
        })

        this.alEditorService.lastActiveWorkspacePath = undefined;
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

    async getCompletionForSourceCode(resourceUri: vscode.Uri | undefined, progressMessage : string, sourceCode : string, posLine : number, posColumn : number, 
        lastSourceLine : number, lastSourceColumn : number) : Promise<vscode.CompletionList | undefined> {
        
            return await vscode.window.withProgress<vscode.CompletionList | undefined>({
                    location: vscode.ProgressLocation.Notification,
                    title: progressMessage
                }, async (progress) => {

                    if ((!vscode.workspace.workspaceFolders) || (vscode.workspace.workspaceFolders.length == 0))
                        return undefined;

                    //find workspace folder for resource
                    let rootFsPath: string;
                    if (!resourceUri) {
                        //resource not defined, find workspace folder for active document
                        let editor = vscode.window.activeTextEditor;
                        if ((editor) && (editor.document))
                            resourceUri = editor.document.uri;
                    }
                    
                    if (!resourceUri) {
                        rootFsPath = vscode.workspace.workspaceFolders[0].uri.fsPath;
                    } else {
                        let rootFolder = vscode.workspace.getWorkspaceFolder(resourceUri);
                        if ((rootFolder) && (rootFolder.uri))
                            rootFsPath = rootFolder.uri.fsPath;
                        else {
                            let rootUri = TextEditorHelper.getActiveWorkspaceFolderUri();
                            if (rootUri)
                                rootFsPath = rootUri.fsPath;
                            else
                                rootFsPath = resourceUri.fsPath;
                        }
                    }

                    try {
                        this.checkLanguageClient();
                        if (!this.langClient)
                            return undefined;
                        
                        if (resourceUri)
                            await this.switchWorkspace(resourceUri, rootFsPath);

                        let docPath : string = path.join(rootFsPath, '.vscode\\temp-al-proxy.al');
                        let docUri : vscode.Uri = vscode.Uri.file(docPath);
    
                        //let fs = require('fs');
                        //if (!fs.existsSync(docPath)) {
                        //    fs.writeFileSync(docPath, '', 'utf8');
                        //}
                    
                        //open virtual document
                        this.langClient.sendNotification('textDocument/didOpen', { textDocument: {
                            uri : docUri.toString(),
                            languageId : 'al',
                            version : 1,
                            text : sourceCode
                        }});

                        //run intellisense on virtual document
                        let tokenSource : vscode.CancellationTokenSource = new vscode.CancellationTokenSource();
                        let token : vscode.CancellationToken = tokenSource.token;
                
                        let list = await this.langClient.sendRequest<vscode.CompletionItem[]|vscode.CompletionList|undefined>('textDocument/completion', {
                            textDocument : {
                                uri : docUri.toString()
                            },
                            position : {
                                line : posLine,
                                character : posColumn
                            },
                            context : undefined
                        }, token);

                        //clear document content
                        this.langClient.sendNotification('textDocument/didChange', {
                            textDocument: {
                                uri: docUri.toString()                                
                            },
                            contentChanges: [
                                {
                                    range: {
                                        start: {
                                            line: 0,
                                            character: 0
                                        },
                                        end: {
                                            line: lastSourceLine,
                                            character: lastSourceColumn
                                        }

                                    },
                                    rangeLength: sourceCode.length,
                                    text: ''
                                }
                            ]
                        });

                        //close document
                        this.langClient.sendNotification('textDocument/didClose', { textDocument: {
                            uri: docUri.toString()
                        }});

                        if (!list)
                            return undefined;

                        let complList : vscode.CompletionList;
                        if (list instanceof vscode.CompletionList) 
                            complList = list;
                        else {
                            let items : vscode.CompletionItem[] = list;
                            complList = new vscode.CompletionList(items);
                        }

                        //fix type kind
                        if (complList.items) {
                            for (let i = 0; i<complList.items.length; i++) {
                                let complItem = complList.items[i];
                                if (complItem.kind)
                                    complItem.kind = complItem.kind - 1;
                            }
                        }

                        return complList;
                    }
                    catch (e) {
                        return undefined;
                    }
            });
    }

    async getNextObjectId(resourceUri: vscode.Uri | undefined, objectType : string) : Promise<string> {
        let fileContent = objectType + " 0 _symbolcache\n{\n}";
        let list = await this.getCompletionForSourceCode(resourceUri, "Finding next free object id.", fileContent,
            0, objectType.length + 1, 2, 1); 

        //process results        
        if (list && list.items) {
            for (let i=0; i<list.items.length; i++) {
                let item = list.items[i];
                if (item.kind == vscode.CompletionItemKind.Reference)
                    return item.label;
            }
        }

        return "";
    }

    async getPageList(resourceUri: vscode.Uri | undefined) : Promise<string[]> {
        let fileContent = "page 0 _symbolcache\n{\nprocedure t()\nvar\nf:page ;\nbegin\nend;\n}";
        let list = await this.getCompletionForSourceCode(resourceUri, "Loading list of pages.", fileContent,
            4, 7, 7, 1);

        //process results
        if (list)
            return this.getSymbolLabels(list, vscode.CompletionItemKind.Class);
        return [];
    }

    async getReportList(resourceUri: vscode.Uri | undefined) : Promise<string[]> {
        let fileContent = "codeunit 0 _symbolcache\n{\nprocedure t()\nvar\nf:report ;\nbegin\nend;\n}";
        let list = await this.getCompletionForSourceCode(resourceUri, "Loading list of reports.", fileContent,
            4, 9, 7, 1);

        //process results
        if (list)
            return this.getSymbolLabels(list, vscode.CompletionItemKind.Reference);
        return [];
    }

    async getTableList(resourceUri: vscode.Uri | undefined) : Promise<string[]> {
        let fileContent = "codeunit 0 _symbolcache\n{\nprocedure t()\nvar\nf:record ;\nbegin\nend;\n}";
        let list = await this.getCompletionForSourceCode(resourceUri, "Loading list of tables.", fileContent,
            4, 9, 7, 1);

        //process results
        if (list)
            return this.getSymbolLabels(list, vscode.CompletionItemKind.Class);
        return [];
    }

    async getCodeunitList(resourceUri: vscode.Uri | undefined) : Promise<string[]> {
        let fileContent = "codeunit 0 _symbolcache\n{\nprocedure t()\nvar\nf:codeunit ;\nbegin\nend;\n}";
        let list = await this.getCompletionForSourceCode(resourceUri, "Loading list of codeunits.", fileContent,
            4, 11, 7, 1);

        //process results
        if (list)
            return this.getSymbolLabels(list, vscode.CompletionItemKind.Class);
        return [];
    }

    async getInterfaceList(resourceUri: vscode.Uri | undefined) : Promise<string[]> {
        let fileContent = "codeunit 0 _symbolcache implements  \n{\n}";
        let list = await this.getCompletionForSourceCode(resourceUri, "Loading list of interfaces.", fileContent,
            0, 36, 2, 1);

        //process results
        if (list)
            return this.getSymbolLabels(list, vscode.CompletionItemKind.Reference);
        return [];
    }

    async getObjectMethods(resourceUri: vscode.Uri | undefined, objectType: string, objectName: string) : Promise<string[] | undefined> {
        if ((!objectName) || (objectName == ""))
            return undefined;
        
        objectName = ALSyntaxHelper.toNameText(objectName);
        
        let fileContent = "codeunit 0 _symbolcache\n{\nprocedure t()\nvar\nf: " + objectType + " " + objectName + ";\nbegin\nf.;\nend;\n}";
        let list = await this.getCompletionForSourceCode(resourceUri, "Loading list of interface methods.", fileContent,
            6, 2, 8, 1);

        //process results
        let out : string[] = [];
        
        if (list && list.items) {
            for (let i=0; i<list.items.length; i++) {
                let item = list.items[i];
                if ((item.detail) && (item.kind == vscode.CompletionItemKind.Method)) {
                    out.push(ALSyntaxHelper.fromNameText(item.detail));
                }
            }
        }

        return out;
    }

    async getAvailablePageFieldList(resourceUri: vscode.Uri | undefined, pageName : string) : Promise<string[]> {
        pageName = ALSyntaxHelper.toNameText(pageName);

        let fileContent = "pageextension 0 _symbolcache extends " + pageName + "\n{\nlayout\n{\naddfirst(undefined)\n{\nfield()\n}\n}\n}";
        let list = await this.getCompletionForSourceCode(resourceUri, "Loading list of table fields.", fileContent,
            6, 6, 9, 1);

        //process results
        if (list)
            return this.getSymbolLabels(list, vscode.CompletionItemKind.Field);
        return [];
    }

    async getFieldList(resourceUri: vscode.Uri | undefined, tableName : string) : Promise<string[]> {
        tableName = ALSyntaxHelper.toNameText(tableName);
        
        let fileContent = "codeunit 0 _symbolcache\n{\nprocedure t()\nvar\nf:record " + tableName + ";\nbegin\nf.;\nend;\n}";
        let list = await this.getCompletionForSourceCode(resourceUri, "Loading list of table fields.", fileContent,
            6, 2, 8, 1);

        //process results
        if (list)
            return this.getSymbolLabels(list, vscode.CompletionItemKind.Field);
        return [];
    }

    async getEnumList(resourceUri: vscode.Uri | undefined) : Promise<string[]> {
        let fileContent = "codeunit 0 _symbolcache\n{\nprocedure t()\nvar\nf:enum ;\nbegin\nend;\n}";
        let list = await this.getCompletionForSourceCode(resourceUri, "Loading list of enums.", fileContent,
            4, 7, 7, 1);

        //process results
        if (list)
            return this.getSymbolLabels(list, vscode.CompletionItemKind.Reference);       
        return [];
    }

    protected getSymbolLabels(list: vscode.CompletionList, kind: vscode.CompletionItemKind): string[] {
        let out : string[] = [];
        
        if (list && list.items) {
            for (let i=0; i<list.items.length; i++) {
                let item = list.items[i];
                if (item.kind === kind) {
                    out.push(ALSyntaxHelper.fromNameText(item.label));
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

    async getDefinitionLocation(objectType : string, objectName : string) : Promise<vscode.Location | undefined> {
        return await vscode.window.withProgress<vscode.Location | undefined>({
            location: vscode.ProgressLocation.Notification,
            title: 'Loading object definition'
        }, async (progress) => {
            let sourceCode : string = 'codeunit 0 _symbolcache\n{\nprocedure t()\nvar\nf: ' + 
                objectType + ' ' + 
                ALSyntaxHelper.toNameText(objectName) + 
                ';\nbegin\nend;\n}';
            let lastSourceLine = 7;
            let lastSourceColumn = 1;
            
            if ((!vscode.workspace.workspaceFolders) || (vscode.workspace.workspaceFolders.length == 0))
                return undefined;

            /*
            let objectId = 22;
            let val = true;
            let tmpUri = vscode.Uri.parse('al-preview://allang/' + vscode.workspace.workspaceFolders[0].name + '/' + objectType + '/' + objectId + '/' + objectName + '.dal');
            let location = new vscode.Location(tmpUri, new vscode.Position(0, 0));
            if (val)
                return location;
            */

            try {

                this.checkLanguageClient();
                if (!this.langClient)
                    return undefined;

                let docPath : string = path.join(vscode.workspace.workspaceFolders[0].uri.fsPath, '.al-lang-proxy\\tempalfile.al');
                let docUri : vscode.Uri = vscode.Uri.file(docPath);

                //open virtual document
                this.langClient.sendNotification('textDocument/didOpen', { textDocument: {
                    uri : docUri.toString(),
                    languageId : 'al',
                    version : 1,
                    text : sourceCode
                }});

                let srcPos = new vscode.Position(4, 5 + objectType.length);
                let docPos : vscode.Location | undefined = await this.getDefinitionLocationFromDocument(docUri.toString(), srcPos);

                //clear document content
                this.langClient.sendNotification('textDocument/didChange', {
                    textDocument: {
                        uri: docUri.toString()                                
                    },
                    contentChanges: [
                        {
                            range: {
                                start: {
                                    line: 0,
                                    character: 0
                                },
                                end: {
                                    line: lastSourceLine,
                                    character: lastSourceColumn
                                }

                            },
                            rangeLength: sourceCode.length,
                            text: ''
                        }
                    ]
                });

                //close document
                this.langClient.sendNotification('textDocument/didClose', { textDocument: {
                    uri: docUri.toString()
                }});

                return docPos;
            }
            catch (e) {
                return undefined;
            }
        });
    }

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
        if ((appData) && (appData.runtime))
            version.parse(appData.runtime);
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

}