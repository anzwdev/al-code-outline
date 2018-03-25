import * as vscode from 'vscode';
import { ALOutlineProvider } from './alOutline';
import { ALSymbolInfo } from './alSymbolInfo';
import { ALSymbolKind } from './alSymbolKind';
var opn = require('opn');

export class ALObjectRunner {
    private alOutlineProvider : ALOutlineProvider;
    private webClientPort : number;

    constructor(newALOutlineProvider : ALOutlineProvider) {
        this.alOutlineProvider = newALOutlineProvider;
        this.webClientPort = vscode.workspace.getConfiguration('alOutline').get('webClientPort');
    }

    runInWebClient(alSymbolInfo : ALSymbolInfo) {
        this.alOutlineProvider.findSymbolObjectId(alSymbolInfo);
        if (alSymbolInfo.alElementId > 0)
        {
            switch (alSymbolInfo.alKind)
            {
                case ALSymbolKind.Table:
                    this.runObjectInWebClient('Table', alSymbolInfo.alElementId);
                    break;
                case ALSymbolKind.Page:
                    this.runObjectInWebClient('Page', alSymbolInfo.alElementId);
                    break;
                case ALSymbolKind.Report:
                    this.runObjectInWebClient('Report', alSymbolInfo.alElementId);
                    break;
            }
        }
    }

    private async runObjectInWebClient(objectType : String, objectId : Number) {
        if ((objectType) && (objectId > 0)) {
        
            var launchConfig = vscode.workspace.getConfiguration('launch');
            var configList = launchConfig['configurations'];
            if ((configList) && (configList.length > 0)) {
                //collect settings
                var config = configList[0];
                var serverName = config.server;
                var serverInstance = config.serverInstance;
                var tenant = config.tenant;
                //ask for web client port
                var newPortNoText : string = await vscode.window.showInputBox({
                    value : this.webClientPort.toString(),
                    prompt : 'Please enter Web Client port number, use 0 for default http/https port.'});
                if (!newPortNoText)
                    return;
                
                var newPortNo : number = parseInt(newPortNoText, 10);
                if (!isNaN(newPortNo))
                    this.webClientPort = newPortNo;

                //build url
                if (this.webClientPort != 0)
                    serverName = serverName + ':' + this.webClientPort.toString();
                var webClientUrl = serverName + '/' + serverInstance + '/WebClient?';
                if ((tenant) && (tenant != ''))
                    webClientUrl = webClientUrl + 'tenant=' + tenant + '&';
                webClientUrl = webClientUrl + objectType + '=' + objectId.toString();
                //open url in web client
                opn(webClientUrl);
            }
                
        }     

    }

}