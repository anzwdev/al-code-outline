import * as vscode from 'vscode';
import * as path from 'path';
import * as fs from 'fs';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { DevToolsExtensionService } from "./devToolsExtensionService";
import { ALConditionalCompilationParser } from '../editorextensions/alConditionalCompilationParser';
import { ALConditionalCompilationSection } from '../editorextensions/alConditionalCompilationSection';

export class ALDecorationService extends DevToolsExtensionService {
    directiveDisabledCode = vscode.window.createTextEditorDecorationType({
        color: 'var(--vscode-editorCodeLens-foreground)',
        fontStyle: 'italic'     
      });
    
    private currentWorkspaceFolder: string | undefined;
    private currentDirectives: string[] | undefined;

    constructor(context: DevToolsExtensionContext) {
        super(context);

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.workspace.onDidChangeTextDocument(event => {
                if (event.document.languageId === "al") {
                    const openEditor = vscode.window.visibleTextEditors.filter(
                        editor => editor.document.uri === event.document.uri
                    )[0];
                    this.applyDecorations(openEditor);
                }
            }));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.window.onDidChangeActiveTextEditor(editor => {
                if ((editor) && (editor.document.languageId === "al")) {
                    this.applyDecorations(editor);
                }
            }));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.workspace.onDidSaveTextDocument(document => {
                if ((document.uri.scheme === "file") && (document.uri.path.endsWith("app.json"))) {
                    this.currentDirectives = undefined;
                }
            }));

        if ((vscode.window.activeTextEditor) && (vscode.window.activeTextEditor.document.languageId === "al")) {
            this.applyDecorations(vscode.window.activeTextEditor);
        }
    }

    private applyDecorations(editor: vscode.TextEditor) {
        let document = editor.document;
        let decorationsArray: vscode.DecorationOptions[] = [];

        this.loadDirectives(document);

        let parser = new ALConditionalCompilationParser(this.currentDirectives);
        let sections = parser.parseDocument(document);

        this.createDecorations(sections, decorationsArray);
        editor.setDecorations(this.directiveDisabledCode, decorationsArray);
    }

    private createDecorations(sections: ALConditionalCompilationSection[], decorations: vscode.DecorationOptions[]) {
        for (let i = 0; i < sections.length; i++) {
            let section = sections[i];
            if (section.enabled) {
                this.createDecorations(section.childSections, decorations);
            } else {
                let decoration: vscode.DecorationOptions = { range: new vscode.Range(new vscode.Position(section.start + 1, 0), new vscode.Position(section.end, 0))};
                decorations.push(decoration);
            }
        }
    }

    private loadDirectives(document: vscode.TextDocument) {
        var folder = vscode.workspace.getWorkspaceFolder(document.uri);
        if ((folder) && ((!this.currentWorkspaceFolder) || (!this.currentDirectives) || (this.currentWorkspaceFolder !== folder.uri.fsPath))) {
            this.currentWorkspaceFolder = folder.uri.fsPath;

            try {
                let filePath = path.join(folder.uri.fsPath, "app.json");
                let appJson = JSON.parse(fs.readFileSync(filePath, 'utf8'));
                if ((appJson) && (appJson.preprocessorSymbols)) {
                    this.currentDirectives = [];
                    for (let i = 0; i < appJson.preprocessorSymbols.length; i++) {
                        this.currentDirectives?.push(appJson.preprocessorSymbols[i]);
                    }
                }
            }
            catch (e) {
                this.currentDirectives = [];
            }
        }
    }

}