import * as vscode from "vscode";
import * as path from 'path';
import { TextDocumentWebViewEditor } from "../webviews/textDocumentWebViewEditor";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";

export class FormEditor extends TextDocumentWebViewEditor {
    protected _currentFields: any;

    constructor(devToolsContext : DevToolsExtensionContext, title : string | undefined) {
        super(devToolsContext, title);
        this._currentFields = undefined;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'editors', 'formeditor', 'formeditor.html');
    }

    protected getViewType() : string {
        return 'azALDevTools.FormEditor';
    }

    protected onDocumentLoaded() {
        this.updateData(true);
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;

        switch (message.command) {
            case 'dataChanged':
                this.onDataChanged(message.data);
                return true;
        }

        return false;
    }

    protected onDataChanged(data: any) {
    }

    protected onTextDocumentChanged() {
        this.updateData(false);
    }    

    protected updateData(withDef: boolean) {
        let data = this.getDocumentData();
        if (withDef) {
            this._currentFields = this.getFieldsDefinition();
            this.sendMessage({
                command : 'setData',
                fields : this._currentFields,
                data : data
            });
        } else {
            this.sendMessage({
                command : 'setData',
                data : data
            });
        }
    }

    protected getDocumentData(): any {
        return undefined;
    }

    protected getFieldsDefinition(): any {
        return undefined;
    }

}