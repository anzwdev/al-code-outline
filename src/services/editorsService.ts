import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { JsonEditorProvider } from '../editors/jsonEditorProvider';
import { AppPackageEditorProvider } from '../editors/appPackageEditorProvider';
//import { AppJsonEditorProvider } from '../editors/appJsonEditorProvider';
//import { RuleSetEditorProvider } from '../editors/ruleSetEditorProvider';
//import { AppSourceCopEditorProvider } from '../editors/appSourceCopEditorProvider';

export class EditorsService {
    protected _context: DevToolsExtensionContext;   

    constructor(newContext: DevToolsExtensionContext) {
        this._context = newContext;
        this.registerEditors();
    }

    protected registerEditors() {
        let options = {
            webviewOptions: {
                retainContextWhenHidden: true
            }
        };

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.window.registerCustomEditorProvider('azALDevTools.appPackageEditor',
                new AppPackageEditorProvider(this._context), options));
        
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.window.registerCustomEditorProvider('azALDevTools.jsonEditor',
                new JsonEditorProvider(this._context), options));
        
        //This code is disabled because of a bug in visual studio code
        //VS Code does not allow to have different default editors for files
        //with the same extension
        /*
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.window.registerCustomEditorProvider('azALDevTools.appJsonEditor',
                new AppJsonEditorProvider(this._context)));
        
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.window.registerCustomEditorProvider('azALDevTools.ruleSetEditor',
                new RuleSetEditorProvider(this._context)));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.window.registerCustomEditorProvider('azALDevTools.appSourceCopEditor',
                new AppSourceCopEditorProvider(this._context)));
        */
    }

}