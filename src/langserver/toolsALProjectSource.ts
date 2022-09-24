import * as vscode from 'vscode';

export class ToolsALProjectSource {
    folderPath: string;
    packageCachePath: string | undefined;
    codeAnalyzers: string[] | undefined;
    additionalMandatoryAffixesPatterns: string[] | undefined;

    constructor(folderUri: vscode.Uri) {
        this.folderPath = folderUri.fsPath;
        
        let config = vscode.workspace.getConfiguration('al', folderUri);
        this.packageCachePath = config.get<string>('packageCachePath');
        this.codeAnalyzers = config.get<string[]>('codeAnalyzers');

        let outlineConfig = vscode.workspace.getConfiguration('alOutline', folderUri);
        this.additionalMandatoryAffixesPatterns = outlineConfig.get<string[]>('additionalMandatoryAffixesPatterns');
    }
}