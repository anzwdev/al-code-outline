import * as fs from 'fs';
import * as path from 'path';
import * as vscode from 'vscode';
import { ALSymbolKind } from '../alSymbolKind';
import { ALSymbolInfo } from '../alSymbolInfo';

export class FileBuilder {
    public static async generateObjectFile(content: string, fileName: string, objectType: ALSymbolKind): Promise<string | undefined> {
        // Determine the project root directory.
        let workspaceFolderCount: number = vscode.workspace.workspaceFolders.length;
        if (workspaceFolderCount < 1) {
            return undefined;
        }

        let workspaceFolder: vscode.WorkspaceFolder = null;
        if (workspaceFolderCount == 1) {
            workspaceFolder = vscode.workspace.workspaceFolders[0];
        }
        else {
            workspaceFolder = await vscode.window.showWorkspaceFolderPick();
        }
        if (!workspaceFolder || workspaceFolder.uri.scheme !== 'file') {
            return undefined;
        }

        // Determine the directory to place the files in; create the directory if it does not exist yet.
        const baseFileDir : string = workspaceFolder.uri.fsPath;
        let relativeFileDir : string = this.getPatternGeneratedRelativeFilePath(objectType);

        const promptForFilePath: boolean = vscode.workspace.getConfiguration('alOutline').get('promptForFilePath');
        if (promptForFilePath) {
            relativeFileDir = await vscode.window.showInputBox({
                value  : relativeFileDir,
                prompt : 'Please enter a directory, relative to the root, to create the new file in.',
                ignoreFocusOut: true
            });
        }
        
        const newFileDirectory : string = path.join(baseFileDir, relativeFileDir);

        if (relativeFileDir) {
            try {
                if (!fs.existsSync(newFileDirectory)) {
                    this.mkdirRecursiveSync(baseFileDir, relativeFileDir);
                }
            }
            catch (err) {
                vscode.window.showErrorMessage('Could not create the path due to the following error: ' + err);
                return undefined;
            }
        }
        
        
        // Determine the file path for the new file to generate. Do not overwrite the file if it already exists.
        const newFilePath : string = path.join(newFileDirectory, fileName);
        const fileAlreadyExists = await this.fileExists(newFilePath);
        if (!fileAlreadyExists) {
            fs.appendFileSync(newFilePath, content);
        }
        return newFilePath;
    }

    private static mkdirRecursiveSync(baseDir:string, relativeDir: string) {
        relativeDir.split(/[\/\\]/g).reduce((prevDirPath, dirToCreate) => {
            const curDirPathToCreate = path.resolve(baseDir, prevDirPath, dirToCreate);
            try {
                fs.mkdirSync(curDirPathToCreate);
            } 
            catch (err) {
                if (err.code !== 'EEXIST') {
                    throw err;
                }
            }
    
            return curDirPathToCreate;
        }, '');
    }

    private static fileExists(path: string): Promise<boolean> {
        return new Promise((resolve, reject) => {
          fs.exists(path, exists => {
            resolve(exists);
          });
        });
    }

    //#region Object / File Pattern

    public static getPatternGeneratedExtensionObjectName(extensionType: ALSymbolKind, extensionId: number, baseSymbolInfo: ALSymbolInfo) {
        let pattern: string = vscode.workspace.getConfiguration('alOutline').get('extensionObjectNamePattern');
        let objectName: string = this.replaceAllExtensionPatterns(pattern, extensionType, extensionId, '', baseSymbolInfo);

        let stripChars: string = vscode.workspace.getConfiguration('alOutline').get('stripNonAlphanumericCharactersFromObjectNames');
        if (stripChars) {
            objectName = this.stripNonAlphaNumericCharacters(objectName);
        }
        return objectName;
    }

    public static getPatternGeneratedExtensionObjectFileName(extensionType: ALSymbolKind, extensionId: number, extensionObjectName: string, baseSymbolInfo: ALSymbolInfo) {
        let pattern: string = vscode.workspace.getConfiguration('alOutline').get('extensionObjectFileNamePattern');
        return this.replaceAllExtensionPatterns(pattern, extensionType, extensionId, extensionObjectName, baseSymbolInfo) + '.al';
    }

    private static replaceAllExtensionPatterns(pattern: string, extensionType: ALSymbolKind, extensionId: number, extensionObjectName: string, baseSymbolInfo: ALSymbolInfo) {
        let output = pattern;

        output = this.replacePattern(output, '<ObjectType>', this.getObjectTypeFromSymbolInfo(extensionType));
        output = this.replacePattern(output, '<ObjectTypeShort>', this.getObjectTypeAbbreviation(extensionType));
        output = this.replacePattern(output, '<ObjectId>', extensionId.toString());
        if (extensionObjectName) {
            output = this.replacePattern(output, '<ObjectName>', this.stripNonAlphaNumericCharacters(extensionObjectName));
        }
        output = this.replacePattern(output, '<BaseName>', this.getObjectNameFromSymbolInfo(baseSymbolInfo));
        output = this.replacePattern(output, '<BaseId>', this.getObjectIdFromSymbolInfo(baseSymbolInfo).toString());

        return output;
    }

    private static getPatternGeneratedRelativeFilePath(objectType: ALSymbolKind) {
        let output: string = vscode.workspace.getConfiguration('alOutline').get('autoGenerateFileDirectory');
        
        output = this.replacePattern(output, '<ObjectType>', this.getObjectTypeFromSymbolInfo(objectType));
        output = this.replacePattern(output, '<ObjectTypeShort>', this.getObjectTypeAbbreviation(objectType));

        return output;
    } 

    private static getObjectIdFromSymbolInfo(alSymbolInfo: ALSymbolInfo): number {
        return alSymbolInfo.alElementId;
    }

    private static getObjectNameFromSymbolInfo(alSymbolInfo: ALSymbolInfo): string {
        return this.stripNonAlphaNumericCharacters(alSymbolInfo.symbolName);
    }

    private static getObjectTypeFromSymbolInfo(alSymbolKind: ALSymbolKind): string {
        return ALSymbolKind[alSymbolKind];
    }

    private static getObjectTypeAbbreviation(alSymbolKind: ALSymbolKind): string {
        switch (alSymbolKind) {
            case ALSymbolKind.Table: return 'Tab';
            case ALSymbolKind.Page: return 'Pag';
            case ALSymbolKind.Report: return 'Rep';
            case ALSymbolKind.XmlPort: return 'Xml';
            case ALSymbolKind.Query: return 'Que';
            case ALSymbolKind.Codeunit: return 'Cod';
            case ALSymbolKind.PageExtension: return 'Pag';
            case ALSymbolKind.TableExtension: return 'Tab';
            case ALSymbolKind.Profile: return 'Prof';
            case ALSymbolKind.PageCustomization: return 'Pag';
            case ALSymbolKind.Enum: return 'Enu';
            case ALSymbolKind.EnumExtension: return 'Enu';
            case ALSymbolKind.DotNetPackage: return 'DotNet';
            default: return "";
        }
    }

    private static replacePattern(name: string, pattern: string, replaceWith: string) {
        return name.replace(new RegExp(this.escapeRegExpInPattern(pattern), 'g'), replaceWith);
    }

    /**
    * Escapes any regex operators/characters in a string.
    * 
    * For further reference, please see [this](https://stackoverflow.com/questions/3446170/escape-string-for-use-in-javascript-regex) StackOverflow page.
    * 
    * @param {string} name The string in which to escape all regex characters.
    * @returns The input string with any regex characters escaped.
    */
    private static escapeRegExpInPattern(name: string)  {
        return name.replace(/([.*+?^${}()|\[\]\/\\=!:])/g, "\\$1");
    }

    /**
     * Removes any characters that are not in the alphabet (\[A-Za-z\]), are numbers (\[0-9\]), or the underscore character ('_').
     * 
     * @param {string} name The string from which to strip characters.
     * @returns The input string with any non-alphameric characters removed.
     */
    public static stripNonAlphaNumericCharacters(name: string): string {
        return name.replace(/\W/g,"");
    }

    //#endregion
}