import * as vscode from 'vscode';
import { FileBuilder } from './fileBuilder';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';

export class ObjectBuilder {
  
    constructor() {
    }

    protected showNewDocument(content : string, fileName?: string, relativeFileDir?: string) {
        let autoGenerateFile: boolean = this.shouldAutoGenerateFiles();
        if (autoGenerateFile && fileName) {
            this.showNewGeneratedFile(content, fileName, relativeFileDir);
        }
        else {
            this.showNewUntitledDocument(content);
        }
    }

    private showNewGeneratedFile(content : string, fileName : string, relativeFileDir: string) {
        FileBuilder.generateObjectFile(content, fileName, relativeFileDir).then(
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
            return -1;
        }
        
        let objectId : number = Number(objectIdString);
        if (isNaN(objectId)) {
            return -1;
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

    protected async getRelativeFileDir(objectType : AZSymbolKind) : Promise<string | undefined> {
        let relativeFileDir : string = await FileBuilder.getPatternGeneratedRelativeFilePath(objectType);
        if (this.shouldPromptForFileDir() && this.shouldAutoGenerateFiles()) {
            relativeFileDir = await this.promptForFileDir('Please specify a directory, relative to the root, to create the new file(s) in.', relativeFileDir);
        }
        return relativeFileDir;
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

    private promptForFileDir(promptText: string, defaultFilePath: string) : Thenable<string | undefined> {
        return vscode.window.showInputBox({
            value  : defaultFilePath,
            prompt : promptText,
            ignoreFocusOut: true
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

    private shouldPromptForFileDir() : boolean {
        return vscode.workspace.getConfiguration('alOutline').get('promptForFilePath');
    }

    private shouldStripCharacters() : boolean {
        return vscode.workspace.getConfiguration('alOutline').get('stripNonAlphanumericCharactersFromObjectNames');
    }

    protected shouldAutoGenerateFiles() : boolean {
        return vscode.workspace.getConfiguration('alOutline').get('autoGenerateFiles');
    }

    //#endregion
}