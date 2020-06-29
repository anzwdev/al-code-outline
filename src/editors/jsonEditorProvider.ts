import * as vscode from 'vscode';
import * as path from 'path';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { JsonFormEditor } from './jsonFormEditor';
import { AppJsonEditor } from './appJsonEditor';
import { RuleSetEditor } from './ruleSetEditor';
import { AppSourceCopEditor } from './appSourceCopEditor';

//This class has been created beacuse of a bug in visual sudio code
//VS Code allows to definde different file patterns for custom editors
//but when one of these editors is set as default, it becomes default
//for all files with the same extension and we need separate editors
//for app.json and *.ruleset.json files
export class JsonEditorProvider implements vscode.CustomTextEditorProvider {
    protected _devToolsContext: DevToolsExtensionContext;

    constructor(devToolsContext: DevToolsExtensionContext) {
        this._devToolsContext = devToolsContext;
    }

    public async resolveCustomTextEditor(document: vscode.TextDocument, webviewPanel: vscode.WebviewPanel, token: vscode.CancellationToken): Promise<void> {
        let filePath = path.parse(document.uri.fsPath);
        let fileName = filePath.base.toLowerCase();

        let editor: JsonFormEditor | undefined = undefined;
        if (fileName == "app.json")
            editor = new AppJsonEditor(this._devToolsContext);
        else if ((fileName.endsWith(".ruleset.json")) || (fileName == "ruleset.json"))
            editor = new RuleSetEditor(this._devToolsContext);
        else if (fileName == "appsourcecop.json")
            editor = new AppSourceCopEditor(this._devToolsContext);

        if (editor)
            editor.resolveCustomTextEditor(document, webviewPanel);    
    }

}