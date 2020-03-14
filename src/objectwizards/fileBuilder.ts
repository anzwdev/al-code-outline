import * as fs from 'fs';
import * as path from 'path';
import * as vscode from 'vscode';
import { CRSALLangExtHelper } from '../crsAlLangExtHelper';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';

export class FileBuilder {
    
    public static showFile(filePath : string) {
        vscode.workspace.openTextDocument(filePath).then(
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

    public static showNewUntitledDocument(content: string) {
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

    public static async generateObjectFileInRelativeDir(content: string, fileName: string, relativeFileDir: string): Promise<string | undefined> {
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

    public static generateObjectFileInDir(dirPath: string, fileName: string, content: string) : string {
        let parsedPath = path.parse(fileName);
        let newFilePath : string = path.join(dirPath, fileName);        
        let fileIndex = 0;
        while (fs.existsSync(newFilePath)) {
            fileIndex++;
            fileName = parsedPath.name + ' ' + fileIndex.toString() + parsedPath.ext;
            newFilePath = path.join(dirPath, fileName);
        }
        this.mkdirFullPathRecursiveSync(dirPath);
        fs.appendFileSync(newFilePath, content);
        return newFilePath;
    }

    private static mkdirFullPathRecursiveSync(destPath: string) {
        if (fs.existsSync(destPath))
            return;
        let parsedPath = path.parse(destPath);
        if (parsedPath.dir)
            this.mkdirFullPathRecursiveSync(parsedPath.dir);
        fs.mkdirSync(destPath);
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

    public static SymbolKindToCrsName(symbolKind : AZSymbolKind) {
        switch (symbolKind) {
            case AZSymbolKind.TableObject: return 'table';
            case AZSymbolKind.CodeunitObject: return 'codeunit';
            case AZSymbolKind.PageObject: return 'page';
            case AZSymbolKind.ReportObject: return 'report';
            case AZSymbolKind.QueryObject: return 'query';
            case AZSymbolKind.XmlPortObject: return 'xmlport';
            case AZSymbolKind.TableExtensionObject: return 'tableextension';
            case AZSymbolKind.PageExtensionObject: return 'pageextension';
            case AZSymbolKind.ControlAddInObject: return 'controladdin';
            case AZSymbolKind.ProfileObject: return 'profile';
            case AZSymbolKind.PageCustomizationObject: return 'pagecustomization';
            case AZSymbolKind.EnumType: return 'enum';
            case AZSymbolKind.DotNetPackage: return 'dotnetpackage';
            case AZSymbolKind.EnumExtensionType: return 'enumextension';
            case AZSymbolKind.Interface: return 'interface';
       }
        return '';
    }

    public static SymbolKindToCamelCaseName(symbolKind : AZSymbolKind) {
        switch (symbolKind) {
            case AZSymbolKind.TableObject: return 'Table';
            case AZSymbolKind.CodeunitObject: return 'Codeunit';
            case AZSymbolKind.PageObject: return 'Page';
            case AZSymbolKind.ReportObject: return 'Report';
            case AZSymbolKind.QueryObject: return 'Query';
            case AZSymbolKind.XmlPortObject: return 'XmlPort';
            case AZSymbolKind.TableExtensionObject: return 'TableExtension';
            case AZSymbolKind.PageExtensionObject: return 'PageExtension';
            case AZSymbolKind.ControlAddInObject: return 'ControlAddIn';
            case AZSymbolKind.ProfileObject: return 'Profile';
            case AZSymbolKind.PageCustomizationObject: return 'PageCustomization';
            case AZSymbolKind.EnumType: return 'Enum';
            case AZSymbolKind.DotNetPackage: return 'DotNetPackage';
            case AZSymbolKind.EnumExtensionType: return 'EnumExtension';
            case AZSymbolKind.Interface: return 'Interface';
       }
        return '';
    }
    public static getAutoGenerateFiles() : boolean {
        return vscode.workspace.getConfiguration('alOutline').get('autoGenerateFiles');
    }

    public static getPromptForObjectName() : boolean {
        return vscode.workspace.getConfiguration('alOutline').get('promptForObjectName');
    }

    public static checkCrsFileNamePatternRequired() : boolean {
        // If files are automatically generated AND no file naming pattern is available, then give an error that the file name pattern setting needs to be added.
        if (FileBuilder.getAutoGenerateFiles() && !FileBuilder.hasCrsFileNamePattern()) {
            vscode.window.showErrorMessage('File name pattern for new objects has not been specified in the "CRS.FileNamePattern" setting. Please add this setting to your VS Code workspace or user settings.');
            return false;
        }
        return true;
    }

    public static checkCrsExtensionFileNamePatternRequired() : boolean {
        // If files are automatically generated AND no extension file naming pattern is available, then give an error that the extension file name pattern setting needs to be added.
        if (FileBuilder.getAutoGenerateFiles() && !FileBuilder.hasCrsExtensionFileNamePattern()) {
            vscode.window.showErrorMessage('File name pattern for extension objects has not been specified in the "CRS.FileNamePatternExtensions" setting. Please add this setting to your VS Code workspace or user settings.');
            return false;
        }
        return true;
    }

    public static checkCrsExtensionObjectNamePatternRequired(multipleObjects: boolean) : boolean {
        // When multiple objects are generated, a prompt for the extension object name is not shown, so we need a pattern -> if not available, then this setting needs to be set first.
        if (multipleObjects && !FileBuilder.hasCrsExtensionObjectNamePattern()) {
            vscode.window.showErrorMessage('Extension object name pattern has not been specified in the "CRS.ExtensionObjectNamePattern" setting. Please add this setting to your VS Code workspace or user settings.');
            return false;
        }
        // When a single object is generated, the name can either come from a pattern or from the user via a prompt -> if both are unavailable/disabled, then either of these settings need to be added/enabled.
        else if (!multipleObjects && !FileBuilder.hasCrsExtensionObjectNamePattern() && !FileBuilder.getPromptForObjectName()) {
            vscode.window.showErrorMessage('The "CRS.ExtensionObjectNamePattern" setting is undefined and the "alOutline.promptForObjectName" setting is disabled. Please update your VS Code workspace or user settings.');
            return false;
        }
        return true;
    }

    public static hasCrsFileNamePattern() : boolean {
        let patternText = vscode.workspace.getConfiguration('CRS', null).get('FileNamePattern');
        if (!patternText)
            return false;
        return true;
    }

    public static hasCrsExtensionFileNamePattern() : boolean {
        let patternText = vscode.workspace.getConfiguration('CRS', null).get('FileNamePatternExtensions');
        if (!patternText)
            return false;
        return true;
    }

    public static hasCrsExtensionObjectNamePattern() {
        let patternText = vscode.workspace.getConfiguration('CRS', null).get('ExtensionObjectNamePattern');
        if (!patternText)
            return false;
        return true;
    }

    public static async getPatternGeneratedFullObjectFileName(objectType: AZSymbolKind, objectId: number, objectName: string) : Promise<string> {
        let crsLangExt = await CRSALLangExtHelper.GetCrsAlLangExt();
        if ((crsLangExt) && (this.hasCrsFileNamePattern()))
            return crsLangExt.ObjectNamesApi.GetObjectFileName(this.SymbolKindToCrsName(objectType), objectId.toString(), objectName);
        return objectName + '.al';
   }

    public static async getPatternGeneratedExtensionObjectName(extensionType: AZSymbolKind, extensionId: number, baseSymbolInfo: AZSymbolInformation) : Promise<string> {       
        let crsLangExt = await CRSALLangExtHelper.GetCrsAlLangExt();
        if ((crsLangExt) && (this.hasCrsExtensionObjectNamePattern()))
            return await crsLangExt.ObjectNamesApi.GetObjectExtensionName(
                this.SymbolKindToCrsName(extensionType), extensionId.toString(), '',
                this.getObjectIdFromSymbolInfo(baseSymbolInfo).toString(), this.getObjectNameFromSymbolInfo(baseSymbolInfo));
        return '';
    }

    public static async getPatternGeneratedExtensionObjectFileName(extensionType: AZSymbolKind, extensionId: number, extensionObjectName: string, baseSymbolInfo: AZSymbolInformation) : Promise<string> {
        let crsLangExt = await CRSALLangExtHelper.GetCrsAlLangExt();
        if ((crsLangExt) && (this.hasCrsExtensionFileNamePattern()))
            return await crsLangExt.ObjectNamesApi.GetObjectExtensionFileName(
                this.SymbolKindToCrsName(extensionType), extensionId.toString(), extensionObjectName,
                this.getObjectIdFromSymbolInfo(baseSymbolInfo).toString(), this.getObjectNameFromSymbolInfo(baseSymbolInfo));
        return extensionObjectName + '.al';
    }

    public static async getPatternGeneratedRelativeFilePath(objectType: AZSymbolKind) : Promise<string> {       
        let typeName = this.getObjectTypeFromSymbolInfo(objectType);
        let typeNameCC = this.SymbolKindToCamelCaseName(objectType);
        let shortTypeName : string = '';                
        let output: string = vscode.workspace.getConfiguration('alOutline').get('autoGenerateFileDirectory');
        
        let crsLangExt = await CRSALLangExtHelper.GetCrsAlLangExt();
        if (crsLangExt)
            shortTypeName = crsLangExt.ObjectNamesApi.GetBestPracticeAbbreviatedObjectType(this.SymbolKindToCrsName(objectType));

        output = this.replacePattern(output, '<ObjectTypeLC>', typeName);
        output = this.replacePattern(output, '<ObjectType>', typeNameCC);
        output = this.replacePattern(output, '<ObjectTypeShort>', shortTypeName);

        return output;
    } 

    private static getObjectIdFromSymbolInfo(alSymbolInfo: AZSymbolInformation): number {
        return alSymbolInfo.id;
    }

    private static getObjectNameFromSymbolInfo(alSymbolInfo: AZSymbolInformation): string {
        return this.stripNonAlphaNumericCharacters(alSymbolInfo.name);
    }

    private static getObjectTypeFromSymbolInfo(alSymbolKind: AZSymbolKind): string {
        //return ALSymbolKind[alSymbolKind];
        return this.SymbolKindToCrsName(alSymbolKind);
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