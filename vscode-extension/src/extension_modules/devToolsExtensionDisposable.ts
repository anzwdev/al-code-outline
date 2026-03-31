import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";

export class DevToolsExtensionDisposable implements vscode.Disposable {
    protected context: DevToolsExtensionContext;
    protected subscriptions: vscode.Disposable[] = [];

    constructor(context: DevToolsExtensionContext) {
        this.context = context;
    }

    dispose() {
        for (let i = 0; i < this.subscriptions.length; i++) {
            this.subscriptions[i].dispose();
        }
        this.subscriptions = [];
    }
}