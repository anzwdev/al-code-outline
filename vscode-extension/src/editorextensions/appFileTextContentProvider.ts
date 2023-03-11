import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { ToolsGetALAppContentRequest } from '../langserver/toolsGetALAppContentRequest';

export class AppFileTextContentProvider implements vscode.TextDocumentContentProvider {
    static scheme: string = 'alOutlineApp';
    onDidChange?: vscode.Event<vscode.Uri> | undefined;
    protected onDidChangeEmitter: vscode.EventEmitter<vscode.Uri>;
    protected _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {        
        this._context = context;
        this.onDidChangeEmitter = new vscode.EventEmitter<vscode.Uri>();
        this.onDidChange = this.onDidChangeEmitter.event;
    }

    async provideTextDocumentContent(uri: vscode.Uri, token: vscode.CancellationToken): Promise<string> {
        let fullPath = uri.path;
        let pos = fullPath.indexOf('::');
        let appPath = fullPath.substr(0, pos);
        let filePath = fullPath.substr(pos + 2);

        let fileContentResponse = await this._context.toolsLangServerClient.getALAppContent(new ToolsGetALAppContentRequest(
            appPath, filePath));

        if ((fileContentResponse) && (fileContentResponse.source))
            return fileContentResponse.source;
        
        return '';
    }

    appFileChanged(appFileUri: vscode.Uri) {
        let appPathPart: string = appFileUri.fsPath + '::';
        let docList = vscode.workspace.textDocuments;       
        for (let i=0; i<docList.length; i++) {
            let docUri = docList[i].uri;
            if ((docUri.scheme == AppFileTextContentProvider.scheme) && (docUri.path.startsWith(appPathPart)))
                this.onDidChangeEmitter.fire(docUri);
        }
    }

}