import * as vscode from 'vscode';
import { DevToolsExtensionService } from './devToolsExtensionService';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';

export class DocumentInformationService<T> extends DevToolsExtensionService {

    //event publisher for document changes
    private _onDidActiveDocumentContentChangedEmiter: vscode.EventEmitter<vscode.Uri | undefined>;
    readonly onDidActiveDocumentContentChanged: vscode.Event<vscode.Uri | undefined>;

    private _activeDocumentDirty: Boolean;
    private _activeDocumentContent: T | undefined;
    private _activeDocumentUri: vscode.Uri | undefined;
   
    constructor(context: DevToolsExtensionContext) {
        super(context);

        this._activeDocumentDirty = true;
        this._activeDocumentContent = undefined;
        this._onDidActiveDocumentContentChangedEmiter = new vscode.EventEmitter<vscode.Uri | undefined>();
        this.onDidActiveDocumentContentChanged = this._onDidActiveDocumentContentChangedEmiter.event;

        this.subscriptions.push(context.changeTrackingService.onDidChangeDocumentSymbols(e => {
            if ((e) && (vscode.window.activeTextEditor) && (e.fsPath === vscode.window.activeTextEditor.document.uri.fsPath)) {
                this.activeDocumentContentChanged();
            }
        }));

        this.subscriptions.push(vscode.window.onDidChangeActiveTextEditor(e => {
            this.activeDocumentContentChanged();
        }));
    }

    protected activeDocumentContentChanged() {
        this._activeDocumentDirty = true;
        this._activeDocumentContent = undefined;
        this._activeDocumentUri = vscode.window.activeTextEditor?.document?.uri;

        if (this._onDidActiveDocumentContentChangedEmiter) {
            this._onDidActiveDocumentContentChangedEmiter.fire(this._activeDocumentUri);
        }
    }

    async getActiveDocumentContent(): Promise<T | undefined> {
        if (this._activeDocumentDirty) {
            this._activeDocumentUri = vscode.window.activeTextEditor?.document?.uri;
            this._activeDocumentContent = await this.loadContent(this._activeDocumentUri);
            this._activeDocumentDirty = false;
        }
        return this._activeDocumentContent;
    }
   
    async getContent(documentUri: vscode.Uri): Promise<T | undefined> {
        if ((documentUri) && (this._activeDocumentUri) && (documentUri.fsPath === this._activeDocumentUri.fsPath)) {
            return await this.getActiveDocumentContent();
        }
        return await this.loadContent(documentUri);
    }

    protected async loadContent(documentUri: vscode.Uri | undefined): Promise<T | undefined> {
        return undefined;
    }

}