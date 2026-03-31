import * as vscode from 'vscode';

export class LSALProjectSource {
    rootNamespace: string | undefined;
    folderPath: string;
    packageCachePath: string | undefined;
    codeAnalyzers: string[] | undefined;
    additionalMandatoryAffixesPatterns: string[] | undefined;

    constructor(folderUri: vscode.Uri) {
        this.folderPath = folderUri.fsPath;
        
        let config = vscode.workspace.getConfiguration('al', folderUri);

        let alPackages: string | string[] | undefined = config.get("packageCachePath");
        if (alPackages) {
            if (typeof(alPackages) !== 'string') {
                if (alPackages.length > 0) {
                    alPackages = alPackages[0];
                } else {
                    alPackages = undefined;
                }
            }
        }

        this.packageCachePath = alPackages;
        this.codeAnalyzers = config.get<string[]>('codeAnalyzers');
        this.rootNamespace = config.get<string>('rootNamespace');

        let outlineConfig = vscode.workspace.getConfiguration('alOutline', folderUri);
        this.additionalMandatoryAffixesPatterns = outlineConfig.get<string[]>('additionalMandatoryAffixesPatterns');
    }

}