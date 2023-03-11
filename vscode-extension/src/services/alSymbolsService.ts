import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AppFileTextContentProvider } from '../editorextensions/appFileTextContentProvider';
import { ALAppSymbolsLibrary } from '../symbollibraries/alAppSymbolsLibrary';
import { ALProjectSymbolsLibrary } from '../symbollibraries/alProjectSymbolsLibrary';
import { AZDocumentSymbolsLibrary } from "../symbollibraries/azDocumentSymbolsLibrary";
import { AZSymbolsLibrary } from '../symbollibraries/azSymbolsLibrary';
import { DevToolsExtensionService } from './devToolsExtensionService';

export class ALSymbolsService extends DevToolsExtensionService {
    protected _appFilteTextContentProvider: AppFileTextContentProvider; 

    constructor(context: DevToolsExtensionContext) {
        super(context);
        this._appFilteTextContentProvider = new AppFileTextContentProvider(this._context);

        this.registerServices();
    }

    protected registerServices() {
        //register app document provider
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.workspace.registerTextDocumentContentProvider(AppFileTextContentProvider.scheme, this._appFilteTextContentProvider));

        //register commands

        //al app viewer
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.viewALApp',
                (fileUri) => {
                    let uri: vscode.Uri = fileUri;
                    let lib: AZSymbolsLibrary;

                    if (this._context.toolsLangServerClient.isEnabled()) {
                        lib = new ALAppSymbolsLibrary(this._context, uri.fsPath);
                        this._context.showSymbolsBrowser(lib);
                    }
                }));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.showAllProjectSymbols',
                () => {
                    let workspacePath = this._context.alLangProxy.getCurrentWorkspaceFolderPath();
                    if (workspacePath) {
                        let lib: ALProjectSymbolsLibrary = new ALProjectSymbolsLibrary(this._context,
                            true, workspacePath);
                        this._context.showSymbolsBrowser(lib);
                    }
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.showProjectSymbolsWithoutDep',
                () => {
                    let workspacePath = this._context.alLangProxy.getCurrentWorkspaceFolderPath();
                    if (workspacePath) {
                        let lib: ALProjectSymbolsLibrary = new ALProjectSymbolsLibrary(this._context,
                            false, workspacePath);
                        this._context.showSymbolsBrowser(lib);
                    }
                }
            )
        );

    }

    async loadDocumentSymbols(docUri: vscode.Uri) : Promise<AZDocumentSymbolsLibrary> {
        let symbolsLibrary: AZDocumentSymbolsLibrary = new AZDocumentSymbolsLibrary(this._context, docUri);
        await symbolsLibrary.loadAsync(false);
        return symbolsLibrary;
    }

    appFileChanged(uri: vscode.Uri) {
        this._appFilteTextContentProvider.appFileChanged(uri);
    }

}