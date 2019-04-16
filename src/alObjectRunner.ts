import * as vscode from 'vscode';

import { AZSymbolInformation } from './symbollibraries/azSymbolInformation';
import { AZSymbolKind } from './symbollibraries/azSymbolKind';

export class ALObjectRunner {

    public static runInWebClient(alSymbolInfo : AZSymbolInformation) {
        if (alSymbolInfo.id > 0)
        {
            switch (alSymbolInfo.kind)
            {
                case AZSymbolKind.TableObject:
                    ALObjectRunner.runObjectInWebClient('Table', alSymbolInfo.id);
                    break;
                case AZSymbolKind.PageObject:
                    ALObjectRunner.runObjectInWebClient('Page', alSymbolInfo.id);
                    break;
                case AZSymbolKind.ReportObject:
                    ALObjectRunner.runObjectInWebClient('Report', alSymbolInfo.id);
                    break;
            }
        }
    }

    private static async runObjectInWebClient(objectType : String, objectId : Number) {
        let opn = require('opn');
        let webClientPort = vscode.workspace.getConfiguration('alOutline').get('webClientPort');

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
                    value : webClientPort.toString(),
                    prompt : 'Please enter Web Client port number, use 0 for default http/https port.'});
                if (!newPortNoText)
                    return;
                
                var newPortNo : number = parseInt(newPortNoText, 10);
                if (!isNaN(newPortNo))
                    webClientPort = newPortNo;

                //build url
                if (webClientPort != 0)
                    serverName = serverName + ':' + webClientPort.toString();
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