'use strict';

import * as vscode from 'vscode';
import { ALLangServerProxy } from './allanguage/alLangServerProxy';

export class DevToolsExtensionContext implements vscode.Disposable {
    alLangProxy : ALLangServerProxy;    
    vscodeExtensionContext : vscode.ExtensionContext;

    constructor(context : vscode.ExtensionContext) {
        this.alLangProxy = new ALLangServerProxy()
        this.vscodeExtensionContext = context;

        let alExtensionPath : string = "";
        if (this.alLangProxy.extensionPath)
            alExtensionPath = this.alLangProxy.extensionPath;
    }

    dispose() {
    }

}