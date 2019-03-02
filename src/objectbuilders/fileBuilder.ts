import * as fs from 'fs';
import * as path from 'path';
import * as vscode from 'vscode';
import { ALSymbolKind } from '../alSymbolKind';
import { ALSymbolInfo } from '../alSymbolInfo';
import { CRSALLangExtHelper } from '../crsAlLangExtHelper';

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
        let relativeFileDir : string = await this.getPatternGeneratedRelativeFilePath(objectType);

        const promptForFilePath: boolean = vscode.workspace.getConfiguration('alOutline').get('promptForFilePath');
        if (promptForFilePath) {
            relativeFileDir = await vscode.window.showInputBox({
                value  : relativeFileDir,
                prompt : 'Please specify a directory, relative to the root, to create the new file in.',
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

    public static SymbolKindToCrsName(symbolKind : ALSymbolKind) {
        switch (symbolKind) {
            case ALSymbolKind.Table: return 'table';
            case ALSymbolKind.Codeunit: return 'codeunit';
            case ALSymbolKind.Page: return 'page';
            case ALSymbolKind.Report: return 'report';
            case ALSymbolKind.Query: return 'query';
            case ALSymbolKind.XmlPort: return 'xmlport';
            case ALSymbolKind.TableExtension: return 'tableextension';
            case ALSymbolKind.PageExtension: return 'pageextension';
            case ALSymbolKind.ControlAddIn: return 'controladdin';
            case ALSymbolKind.Profile: return 'profile';
            case ALSymbolKind.PageCustomization: return 'pagecustomization';
            case ALSymbolKind.Enum: return 'enum';
            case ALSymbolKind.DotNetPackage: return 'dotnetpackage';
            case ALSymbolKind.EnumExtension: return 'enumextension';
       }
        return '';
    }


    public static async getPatternGeneratedFullObjectFileName(objectType: ALSymbolKind, objectId: number, objectName: string) : Promise<string> {
        let crsLangExt = await CRSALLangExtHelper.GetCrsAlLangExt();
        if (crsLangExt)
            return crsLangExt.ObjectNamesApi.GetObjectFileName(this.SymbolKindToCrsName(objectType), objectId.toString(), objectName);
        return objectName + '.al';
   }

    public static async getPatternGeneratedExtensionObjectName(extensionType: ALSymbolKind, extensionId: number, baseSymbolInfo: ALSymbolInfo) : Promise<string> {
        let crsLangExt = await CRSALLangExtHelper.GetCrsAlLangExt();
        if (crsLangExt)
            return await crsLangExt.ObjectNamesApi.GetObjectExtensionName(
                this.SymbolKindToCrsName(extensionType), extensionId.toString(), '',
                this.getObjectIdFromSymbolInfo(baseSymbolInfo).toString(), this.getObjectNameFromSymbolInfo(baseSymbolInfo));
        return '';
    }

    public static async getPatternGeneratedExtensionObjectFileName(extensionType: ALSymbolKind, extensionId: number, extensionObjectName: string, baseSymbolInfo: ALSymbolInfo) : Promise<string> {
        let crsLangExt = await CRSALLangExtHelper.GetCrsAlLangExt();
        if (crsLangExt)
            return await crsLangExt.ObjectNamesApi.GetObjectExtensionFileName(
                this.SymbolKindToCrsName(extensionType), extensionId.toString(), extensionObjectName,
                this.getObjectIdFromSymbolInfo(baseSymbolInfo).toString(), this.getObjectNameFromSymbolInfo(baseSymbolInfo));
        return extensionObjectName + '.al';
    }

    private static async getPatternGeneratedRelativeFilePath(objectType: ALSymbolKind) : Promise<string> {
        let typeName = this.getObjectTypeFromSymbolInfo(objectType);
        let shortTypeName : string = '';                
        let output: string = vscode.workspace.getConfiguration('alOutline').get('autoGenerateFileDirectory');
        
        let crsLangExt = await CRSALLangExtHelper.GetCrsAlLangExt();
        if (crsLangExt)
            shortTypeName = crsLangExt.ObjectNamesApi.GetBestPracticeAbbreviatedObjectType(this.SymbolKindToCrsName(objectType));

        output = this.replacePattern(output, '<ObjectType>', typeName);
        output = this.replacePattern(output, '<ObjectTypeShort>', shortTypeName);

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

    private static replacePattern(name: string, pattern: string, replaceWith: string) : string {
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