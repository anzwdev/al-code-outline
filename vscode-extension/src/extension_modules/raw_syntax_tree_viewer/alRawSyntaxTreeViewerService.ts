import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { DevToolsExtensionService } from "../devToolsExtensionService";
import { ALRawSyntaxTreeViewerConst } from "./alRawSyntaxTreeViewerConst";
import { ALRawSyntaxTreeProvider } from './alRawSyntaxTreeProvider';
import { ALRawSyntaxTreeViewer } from './alRawSyntaxTreeViewer';

export class ALRawSyntaxTreeViewerService extends DevToolsExtensionService {
    
    constructor(context: DevToolsExtensionContext) {
        super(context);

        this.registerCommands();
    }

    private registerCommands() {
        this.subscriptions.push(
            vscode.commands.registerCommand(
                ALRawSyntaxTreeViewerConst.cmdShowSyntaxViewer,
                () => this.showRawSyntaxTreeViewer()
            )
        );
    }

    private async showRawSyntaxTreeViewer() {
        let editor = vscode.window.activeTextEditor;
        if ((editor) && (editor.document) && (editor.document.uri) && (editor.document.languageId === "al")) {
            let provider = new ALRawSyntaxTreeProvider(this.context, editor.document.uri, editor.document.fileName);
            let syntaxTreeViewer = new ALRawSyntaxTreeViewer(this.context, provider);
            syntaxTreeViewer.show();
        }   
    }

}
