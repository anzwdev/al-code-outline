import * as vscode from 'vscode';

import { AZSymbolInformation } from './symbollibraries/azSymbolInformation';
import { AZSymbolKind } from './symbollibraries/azSymbolKind';
import { DevToolsExtensionContext } from './devToolsExtensionContext';

export class ALObjectRunner {
    private _context : DevToolsExtensionContext;

    constructor(context : DevToolsExtensionContext) {
        this._context = context;
    }

    async runSymbolAsync(alSymbolInfo : AZSymbolInformation) {
        if (alSymbolInfo.id > 0)
        {
            switch (alSymbolInfo.kind)
            {
                case AZSymbolKind.TableObject:
                    await this.runObjectAsync('Table', alSymbolInfo.id);
                    break;
                case AZSymbolKind.PageObject:
                    await this.runObjectAsync('Page', alSymbolInfo.id);
                    break;
                case AZSymbolKind.ReportObject:
                    await this.runObjectAsync('Report', alSymbolInfo.id);
                    break;
            }
        }
    }

    async runObjectAsync(objectType: string, objectId : number) {
        if ((objectType) && (objectId > 0)) {
            if (this._context.alLangProxy.version.major >= 3)
                await this.runObjectWithAL30Async(objectType, objectId);
            else
                await this.runObjectInWebClientAsync(objectType, objectId);
        }
    }

    private async runObjectWithAL30Async(objectType : string, objectId : number) {
        if ((vscode.workspace.workspaceFolders) && (vscode.workspace.workspaceFolders.length > 0)) {
            let launchConfiguration = await this._context.alLangProxy.getLaunchConfiguration();
            if (!launchConfiguration)
                return;

            let workspaceFolder = vscode.workspace.workspaceFolders[0];

            let config = {
                name: launchConfiguration.name,
                type: 'al',
                request: 'launch',
                noDebug: false,
                isRad: false,
                justDebug: true,
                authentication: launchConfiguration.authentication,
                port: launchConfiguration.port,
                schemaUpdateMode: launchConfiguration.schemaUpdateMode,
                server: launchConfiguration.server,
                serverinstance: launchConfiguration.serverInstance,
                startupObjectId: objectId,
                startupObjectType: objectType,
                tenant: launchConfiguration.tenant,
                applicationFamily: launchConfiguration.applicationFamily,
                breakOnError: launchConfiguration.breakOnError,
                breakOnRecordWrite: launchConfiguration.breakOnRecordWrite,
                skipCodeunit1: launchConfiguration.skipCodeunit1,
                launchBrowser: launchConfiguration.launchBrowser,
                sandboxName: launchConfiguration.sandboxName
            };
            
            vscode.debug.startDebugging(workspaceFolder, config);
        }
    }

    private async runObjectInWebClientAsync(objectType : String, objectId : Number) {
        let launchConfiguration = await this._context.alLangProxy.getLaunchConfiguration();
        if (!launchConfiguration)
            return;

        let opn = require('opn');
        let webClientPort = vscode.workspace.getConfiguration('alOutline').get<number>('webClientPort');

        //collect settings
        let serverName = launchConfiguration.server;
        let serverInstance = launchConfiguration.serverInstance;
        let tenant = launchConfiguration.tenant;
        //ask for web client port
        let newPortNoText = await vscode.window.showInputBox({
            value : webClientPort?.toString(),
            prompt : 'Please enter Web Client port number, use 0 for default http/https port.'});
        if (!newPortNoText)
            return;
        
        var newPortNo : number = parseInt(newPortNoText, 10);
        if (!isNaN(newPortNo))
            webClientPort = newPortNo;

        //build url
        if ((webClientPort) && (webClientPort != 0))
            serverName = serverName + ':' + webClientPort.toString();
        var webClientUrl = serverName + '/' + serverInstance + '/WebClient?';
        if ((tenant) && (tenant != ''))
            webClientUrl = webClientUrl + 'tenant=' + tenant + '&';
        webClientUrl = webClientUrl + objectType + '=' + objectId.toString();
        //open url in web client
        opn(webClientUrl);
    }

}
