import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { CARulesViewer } from '../carulesviewer/carulesviewer';

export class CodeAnalyzersService {
    protected _context: DevToolsExtensionContext;

    constructor(newContext: DevToolsExtensionContext) {
        this._context = newContext;
        this.registerCommands();
    }

    protected registerCommands() {
        //code analyzers
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.showCodeAnalyzers',
                () => {
                    this.showCodeAnalyzersRules();
                }
            )
       );
    }

    showCodeAnalyzersRules() {
        let caRulesViewer: CARulesViewer = new CARulesViewer(this._context);
        caRulesViewer.show();
    }

}