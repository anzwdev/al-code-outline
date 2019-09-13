'use strict';

import * as vscode from 'vscode';
import * as path from 'path';
import * as vscodelangclient from 'vscode-languageclient';
import { ALSyntaxHelper } from './alSyntaxHelper';
import { Version } from '../tools/version';
import { DocumentOnTypeFormattingRequest } from 'vscode-languageclient';

export class ALLangServerProxy {
    private langClient : vscodelangclient.LanguageClient | undefined;
    public extensionPath : string | undefined;
    public version : Version;

    constructor() {
        this.version = new Version();
        this.langClient = undefined;
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
            }

        }
        return true;
    }

    async getCompletionForSourceCode(progressMessage : string, sourceCode : string, posLine : number, posColumn : number, 
        lastSourceLine : number, lastSourceColumn : number) : Promise<vscode.CompletionList | undefined> {
        
            return await vscode.window.withProgress<vscode.CompletionList | undefined>({
                    location: vscode.ProgressLocation.Notification,
                    title: progressMessage
                }, async (progress) => {

                    if ((!vscode.workspace.workspaceFolders) || (vscode.workspace.workspaceFolders.length == 0))
                        return undefined;

                    try {

                        this.checkLanguageClient();
                        if (!this.langClient)
                            return undefined;

                        let docPath : string = path.join(vscode.workspace.workspaceFolders[0].uri.fsPath, '.vscode\\temp-al-proxy.al');
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

    async getNextObjectId(objectType : string) : Promise<string> {
        let fileContent = objectType + " 0 _symbolcache\n{\n}";
        let list = await this.getCompletionForSourceCode("Finding next free object id.", fileContent,
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

    async getTableList() : Promise<string[]> {
        let fileContent = "codeunit 0 _symbolcache\n{\nprocedure t()\nvar\nf:record ;\nbegin\nend;\n}";
        let list = await this.getCompletionForSourceCode("Loading list of tables.", fileContent,
            4, 9, 7, 1);

        //process results
        let out : string[] = [];
        
        if (list && list.items) {
            for (let i=0; i<list.items.length; i++) {
                let item = list.items[i];
                if (item.kind == vscode.CompletionItemKind.Class) {
                    out.push(ALSyntaxHelper.fromNameText(item.label));
                }
            }
        }

        return out;
        
    }

    async getAvailablePageFieldList(pageName : string) : Promise<string[]> {
        pageName = ALSyntaxHelper.toNameText(pageName);

        let fileContent = "pageextension 0 _symbolcache extends " + pageName + "\n{\nlayout\n{\naddfirst(undefined)\n{\nfield()\n}\n}\n}";
        let list = await this.getCompletionForSourceCode("Loading list of table fields.", fileContent,
            6, 6, 9, 1);

        //process results
        let out : string[] = [];
        
        if (list && list.items) {
            for (let i=0; i<list.items.length; i++) {
                let item = list.items[i];
                if (item.kind == vscode.CompletionItemKind.Field) {
                    out.push(ALSyntaxHelper.fromNameText(item.label));
                }
            }
        }

        return out;
    }

    async getFieldList(tableName : string) : Promise<string[]> {
        tableName = ALSyntaxHelper.toNameText(tableName);
        
        let fileContent = "codeunit 0 _symbolcache\n{\nprocedure t()\nvar\nf:record " + tableName + ";\nbegin\nf.;\nend;\n}";
        let list = await this.getCompletionForSourceCode("Loading list of table fields.", fileContent,
            6, 2, 8, 1);

        //process results
        let out : string[] = [];
        
        if (list && list.items) {
            for (let i=0; i<list.items.length; i++) {
                let item = list.items[i];
                if (item.kind == vscode.CompletionItemKind.Field) {
                    out.push(ALSyntaxHelper.fromNameText(item.label));
                }
            }
        }

        return out;
    }

    async getEnumList() : Promise<string[]> {
        let fileContent = "codeunit 0 _symbolcache\n{\nprocedure t()\nvar\nf:enum ;\nbegin\nend;\n}";
        let list = await this.getCompletionForSourceCode("Loading list of enums.", fileContent,
            4, 7, 7, 1);

        //process results
        let out : string[] = [];
        
        if (list && list.items) {
            for (let i=0; i<list.items.length; i++) {
                let item = list.items[i];
                if (item.kind == vscode.CompletionItemKind.Reference) {
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
            
            if ((!vscode.workspace.workspaceFolders) || (vscode.workspace.workspaceFolders.length == 0))
                return undefined;

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

                /*
                //run goToDefinition on virtual document
                let tokenSource : vscode.CancellationTokenSource = new vscode.CancellationTokenSource();
                let token : vscode.CancellationToken = tokenSource.token;
                let posLine = 4;
                let posColumn = 5 + objectType.length;

                let docPos : vscode.Location | undefined;

                let launchConfiguration = await this.getLaunchConfiguration();
                //let launchFilePath = path.join(vscode.workspace.workspaceFolders[0].uri.fsPath, '.vscode/launch.json');
                //let config = vscode.workspace.getConfiguration("launch", vscode.Uri.file(launchFilePath));
                //let allConfigList : any[] | undefined = config.get("configurations");
                //if (!allConfigList)
                //    return undefined;
                //let configList = allConfigList.filter(p => p.type === 'al');
                //if ((configList) && (configList.length > 0)) { 
                if (launchConfiguration) {

                    let docPosTemp : any = await this.langClient.sendRequest<any>('al/gotodefinition', {
                        launchConfiguration : launchConfiguration,
                        textDocumentPositionParams : {
                            textDocument : {
                                uri : docUri.toString()
                            },
                            position : {
                                line : posLine,
                                character : posColumn
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
                */

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

}