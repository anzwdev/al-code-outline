import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { DevToolsExtensionService } from "../devToolsExtensionService";
import { CodeOutlineWebViewConst } from './codeOutlineWebViewConst';
import { CodeOutlineWebView } from './codeOutlineWebView';
import { SourceCodeOutlineWebViewTreeProvider } from './sourceCodeOutlineWebViewTreeProvider';

export class CodeOutlineWebViewService extends DevToolsExtensionService {
    protected _openCodeOutlineWebViews : { [id: string] : CodeOutlineWebView | undefined };

    constructor(context: DevToolsExtensionContext) {
        super(context);

        this._openCodeOutlineWebViews = {};
        this.registerCommands();
    }

    private registerCommands() {
        this.subscriptions.push(
            vscode.commands.registerCommand(
                CodeOutlineWebViewConst.cmdGoToDefinitionSymbolTree,
                () => this.goToDefinitionSymbolTree()
            )
        );

        this.subscriptions.push(
            vscode.commands.registerCommand(
                CodeOutlineWebViewConst.cmdShowDocumentSymbols,
                () => this.showEditorSymbolsTreeView()
            )
        );        
    }

    async showDocumentSymbols(docUri: vscode.Uri, range: vscode.Range | undefined) {
        let webView : CodeOutlineWebView | undefined = this._openCodeOutlineWebViews[docUri.toString()];
        if (!webView) {
            let provider = new SourceCodeOutlineWebViewTreeProvider(this.context, docUri);
            webView = new CodeOutlineWebView(this.context, provider);
            webView.selectedSymbolRange = range;
            //this.addUriSymbolsTreeView(docUri, webView);
            webView.show();
        } else {
            webView.reveal();
            if (range) {
                webView.selectSymbolInRange(range);
            }
        }
    }

    private async goToDefinitionSymbolTree() {
        let editor = vscode.window.activeTextEditor;
        if ((editor) && (editor.document) && (editor.document.uri)) {
            let locationList : vscode.Location[] | undefined = await vscode.window.withProgress<vscode.Location[] | undefined>({
                location: vscode.ProgressLocation.Notification,
                title: 'Loading object definition'
            }, async (progress) => {
                return await vscode.commands.executeCommand<vscode.Location[] | undefined>('vscode.executeDefinitionProvider', editor!.document.uri,
                    editor!.selection.start);
            });

            if ((locationList) && (locationList.length > 0)) {
                let range = locationList[0].range;               
                this.showDocumentSymbols(locationList[0].uri, range);
            } else {
                this.showDocumentSymbols(editor.document.uri, undefined);
            }
        }
    }

    private showEditorSymbolsTreeView() {        
        let editor = vscode.window.activeTextEditor;
        if ((editor) && (editor.document) && (editor.document.uri)) {
            this.showDocumentSymbols(editor.document.uri, new vscode.Range(editor.selection.start, editor.selection.end));        
        }
    }



    private addUriSymbolsTreeView(uri: vscode.Uri, symbolsTreeView: CodeOutlineWebView) {
        this._openCodeOutlineWebViews[uri.toString()] = symbolsTreeView;
    }

    removeUriSymbolsTreeView(uri: vscode.Uri) {
        this._openCodeOutlineWebViews[uri.toString()] = undefined;
    }

}