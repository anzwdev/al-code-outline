import * as vscode from 'vscode';
import { ALSymbolKind } from '../alSymbolKind';
import { FileBuilder } from './fileBuilder';

export class ObjectBuilder {
    
    constructor() {
    }

    protected showNewDocument(content : string, fileName?: string, objectType?: ALSymbolKind) {
        let autoGenerateFile: boolean = vscode.workspace.getConfiguration('alOutline').get('autoGenerateFiles');
        if (autoGenerateFile && fileName) {
            this.showNewGeneratedFile(content, fileName, objectType);
        }
        else {
            this.showNewUntitledDocument(content);
        }
    }

    private showNewGeneratedFile(content : string, fileName : string, objectType : ALSymbolKind) {
        FileBuilder.generateObjectFile(content, fileName, objectType).then(
            path => {
                if (path) {
                    vscode.workspace.openTextDocument(path).then(
                        document => {
                            vscode.window.showTextDocument(document, {
                                preview : false
                            });
                        },
                        err => {
                            vscode.window.showErrorMessage(err);
                        }
                    );
                }
            },
            err => {
                vscode.window.showErrorMessage(err);
            }
        );
    }

    private showNewUntitledDocument(content: string) {
        vscode.workspace.openTextDocument({
            content : content,
            language : "al"
        }).then(
            document => { 
                vscode.window.showTextDocument(document, {
                    preview : false
                });
            },
            err => {
                vscode.window.showErrorMessage(err);
            });
    }
}