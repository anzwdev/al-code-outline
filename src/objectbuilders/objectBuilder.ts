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
                            let autoShowDocument: boolean = vscode.workspace.getConfiguration('alOutline').get('autoShowFiles');
                            if (autoShowDocument) {
                                vscode.window.showTextDocument(document, {
                                    preview : false
                                });
                            }
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

    protected async getObjectId(promptText: string, defaultObjectId: number) : Promise<number> {
        let objectIdString : string = defaultObjectId.toString();
        if (this.shouldPromptForObjectId()) {
            objectIdString = await this.promptForObjectId(promptText, objectIdString);
        }

        if (!objectIdString) {
            objectIdString = "0";
        }
        
        let objectId : number = Number(objectIdString);
        if (isNaN(objectId)) {
            return 0;
        }

        return objectId;
    }

    protected async getObjectName(promptText: string, defaultObjectName : string) : Promise<string | undefined> {
        let objectName : string = defaultObjectName;
        if (this.shouldPromptForObjectName()) {
            objectName = await this.promptForObjectName(promptText, objectName);
        }

        if (!objectName) {
            return objectName;
        }

        if (this.shouldStripCharacters()) {
            objectName = FileBuilder.stripNonAlphaNumericCharacters(objectName);
        }

        return objectName;
    }

    //#region UI functions

    private promptForObjectId(promptText: string, defaultObjectId : string) : Thenable<string | undefined> {
        return vscode.window.showInputBox({
            value : defaultObjectId,
            prompt : promptText,
            validateInput: (text: string): string | undefined => {
                let objectId : number = Number(text);
                if (isNaN(objectId)) {
                    return 'Only numbers are allowed for object IDs.'
                }
                else {
                    return undefined;
                }
            }
        });
    }

    private promptForObjectName(promptText: string, defaultObjectName : string) : Thenable<string | undefined> {
        return vscode.window.showInputBox({
            value : defaultObjectName,
            prompt : promptText
        });
    }

    //#endregion

    //#region Setting Helper Functions

    private shouldPromptForObjectId() : boolean {
        return vscode.workspace.getConfiguration('alOutline').get('promptForObjectId');
    }

    private shouldPromptForObjectName() : boolean {
        return vscode.workspace.getConfiguration('alOutline').get('promptForObjectName');
    }

    private shouldStripCharacters() : boolean {
        return vscode.workspace.getConfiguration('alOutline').get('stripNonAlphanumericCharactersFromObjectNames');
    }

    //#endregion
}