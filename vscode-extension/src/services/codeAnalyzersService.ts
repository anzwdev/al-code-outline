import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { CARulesViewer } from '../carulesviewer/caRulesViewer';
import { DevToolsExtensionService } from './devToolsExtensionService';

export class CodeAnalyzersService extends DevToolsExtensionService {
    protected _codeAnalyzersViewer: CARulesViewer | undefined;

    constructor(newContext: DevToolsExtensionContext) {
        super(newContext);
        this._codeAnalyzersViewer = undefined;
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
        if (!this._codeAnalyzersViewer) {
            this._codeAnalyzersViewer = new CARulesViewer(this._context);
            this._codeAnalyzersViewer.show();
        } else {
            this._codeAnalyzersViewer.reveal();
        }
    }

    onCodeAnalyzersViewerClosed() {
        this._codeAnalyzersViewer = undefined;
    }

}