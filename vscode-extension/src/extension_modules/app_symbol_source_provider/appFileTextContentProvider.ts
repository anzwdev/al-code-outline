import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { LSSPSymbolSourceProviderClient } from '../../langserver/app_symbol_source_provider/lsspSymbolSourceProviderClient';
import { AppFileTextContentProviderConst } from './appFileTextContentProviderConst';

export class AppFileTextContentProvider implements vscode.TextDocumentContentProvider {
    onDidChange?: vscode.Event<vscode.Uri> | undefined;
    protected onDidChangeEmitter: vscode.EventEmitter<vscode.Uri>;
    protected context: DevToolsExtensionContext;
    protected _lsclient: LSSPSymbolSourceProviderClient;

    constructor(context: DevToolsExtensionContext) {        
        this.context = context;
        this.onDidChangeEmitter = new vscode.EventEmitter<vscode.Uri>();
        this._lsclient = new LSSPSymbolSourceProviderClient(context.lsConnector);

        this.onDidChange = this.onDidChangeEmitter.event;
    }

    async provideTextDocumentContent(uri: vscode.Uri, token: vscode.CancellationToken): Promise<string> {
        let fullPath = uri.path;
        let pos = fullPath.indexOf('::');
        let appPath = fullPath.substr(0, pos);
        let filePath = fullPath.substr(pos + 2);

        let fileContentResponse = await this._lsclient.getAppFileSymbolSource({
            appFilePath: appPath,
            sourceFilePath: filePath
        });

        if ((fileContentResponse) && (fileContentResponse.source)) {
            return fileContentResponse.source;
        }
        
        return '';
    }

    appFileChanged(appFileUri: vscode.Uri) {
        let appPathPart: string = appFileUri.fsPath + '::';
        let docList = vscode.workspace.textDocuments;       
        for (let i=0; i<docList.length; i++) {
            let docUri = docList[i].uri;
            if ((docUri.scheme === AppFileTextContentProviderConst.scheme) && (docUri.path.startsWith(appPathPart))) {
                this.onDidChangeEmitter.fire(docUri);
            }
        }
    }

}