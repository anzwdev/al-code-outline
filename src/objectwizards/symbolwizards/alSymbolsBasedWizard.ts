import * as vscode from 'vscode';
import { FileBuilder } from '../fileBuilder';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';

export class ALSymbolsBasedWizard {
  
    constructor() {
    }

    protected async showNewDocument(content : string, fileName?: string, relativeFileDir?: string) {
        let autoGenerateFile: boolean = this.shouldAutoGenerateFiles();
        if (autoGenerateFile && fileName) {
            this.showNewGeneratedFile(content, fileName, relativeFileDir);
        }
        else {
            FileBuilder.showNewUntitledDocument(content);
        }
    }

    private async showNewGeneratedFile(content : string, fileName : string, relativeFileDir: string | undefined) {
        let autoShowDocument = vscode.workspace.getConfiguration('alOutline').get<boolean>('autoShowFiles');
        let filePath = await FileBuilder.generateObjectFileInRelativeDir(content, fileName, relativeFileDir);
        if ((filePath) && (autoShowDocument))
            FileBuilder.showFile(filePath);
    }

    protected async getObjectId(promptText: string, defaultObjectId: number) : Promise<number> {
        let objectIdString : string | undefined = defaultObjectId.toString();
        if (this.shouldPromptForObjectId())
            objectIdString = await this.promptForObjectId(promptText, objectIdString);

        if (!objectIdString)
            return -1;
        
        let objectId : number = Number(objectIdString);
        if (isNaN(objectId)) {
            return -1;
        }

        return objectId;
    }

    protected async getObjectName(promptText: string, defaultObjectName : string) : Promise<string | undefined> {
        let objectName : string | undefined = defaultObjectName;
        if (this.shouldPromptForObjectName())
            objectName = await this.promptForObjectName(promptText, objectName);

        if (!objectName)
            return objectName;

        if (this.shouldStripCharacters())
            objectName = FileBuilder.stripNonAlphaNumericCharacters(objectName);

        return objectName;
    }

    protected async getRelativeFileDir(objectType : AZSymbolKind) : Promise<string | undefined> {
        let relativeFileDir : string | undefined = await FileBuilder.getPatternGeneratedRelativeFilePath(objectType);
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

    private getBoolSetting(name: string): boolean {
        //convert undefined to false
        if (vscode.workspace.getConfiguration('alOutline').get<boolean>(name))
            return true;
        return false;
    }

    private shouldPromptForObjectId() : boolean {
        return this.getBoolSetting('promptForObjectId');
    }

    private shouldPromptForObjectName() : boolean {
        return this.getBoolSetting('promptForObjectName');
    }

    private shouldPromptForFileDir() : boolean {
        return this.getBoolSetting('promptForFilePath');
    }

    private shouldStripCharacters() : boolean {
        return this.getBoolSetting('stripNonAlphanumericCharactersFromObjectNames');
    }

    protected shouldAutoGenerateFiles() : boolean {
        return this.getBoolSetting('autoGenerateFiles');
    }

    //#endregion
}