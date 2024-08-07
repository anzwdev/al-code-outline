import * as vscode from 'vscode';
import * as path from 'path';
import * as fs from 'fs';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { CARuleInfo } from "../langserver/caRuleInfo";
import { CodeAnalyzerInfo } from "../codeanalyzers/codeAnalyzerInfo";
import { ToolsGetCodeAnalyzersRulesRequest } from '../langserver/toolsGetCodeAnalyzersRulesRequest';

export class CARulesCollection {
    protected _context: DevToolsExtensionContext;
    protected _analyzerFolderPrefix: string = '${analyzerFolder}';
    rules: CARuleInfo[] | undefined;
    analyzers: CodeAnalyzerInfo[];    

    constructor(devToolsContext : DevToolsExtensionContext) {
        this._context = devToolsContext;
        this.analyzers = [];
        this.loadCodeAnalyzers();
    }

    async loadRules() {
        this.rules = [];
        for (let analyzerIdx=0; analyzerIdx<this.analyzers.length; analyzerIdx++) {
            let request: ToolsGetCodeAnalyzersRulesRequest = 
                new ToolsGetCodeAnalyzersRulesRequest(this.analyzers[analyzerIdx].value);
            let response = 
                await this._context.toolsLangServerClient.getCodeAnalyzersRules(request);
            if ((response) && (response.rules)) {
                for (let ruleIdx = 0; ruleIdx < response.rules.length; ruleIdx++) {
                    response.rules[ruleIdx].analyzer = this.analyzers[analyzerIdx].label;
                    this.rules.push(response.rules[ruleIdx]);
                }
            }
        }
    }

    protected loadCodeAnalyzers() {
        this.addAnalyzer('${AppSourceCop}');
        this.addAnalyzer('${CodeCop}');
        this.addAnalyzer('${PerTenantExtensionCop}');
        this.addAnalyzer('${UICop}');
        this.addAnalyzer(this._analyzerFolderPrefix + 'BusinessCentral.LinterCop.dll');
        this.addAnalyzer('Compiler');

        let folders = vscode.workspace.workspaceFolders;
        if (folders) {
            for (let folderIdx=0; folderIdx<folders.length; folderIdx++) {
                this.loadCodeAnalyzersForUri(folders[folderIdx].uri);
            }
        }
        this.loadCodeAnalyzersForUri(undefined);
    }

    protected loadCodeAnalyzersForUri(uri: vscode.Uri | undefined) {
        let alConfig = vscode.workspace.getConfiguration('al', uri);
        let codeAnalyzersSetting = alConfig.get<string[]|undefined>("codeAnalyzers");
        if (codeAnalyzersSetting) {
            for (let analyzerIdx=0; analyzerIdx<codeAnalyzersSetting.length; analyzerIdx++) {
                let analyzerName = codeAnalyzersSetting[analyzerIdx].trim();
                this.addAnalyzer(analyzerName);
            }
        }
    }

    protected addAnalyzer(name: string) {
        let fullName = name;

        //convert to full path
        if ((name.toLowerCase().startsWith(this._analyzerFolderPrefix.toLowerCase())) && (this._context.alLangProxy.extensionPath)) {
            let fileName = name.substring(this._analyzerFolderPrefix.length);
            fullName = path.join(this._context.alLangProxy.extensionPath, 'bin', 'Analyzers', fileName);
        }

        //check if analyzer file exists and parse name from path
        if ((!fullName.startsWith('${')) && (fullName !== 'Compiler')) {
            if (!fs.existsSync(fullName)) {
                return;
            }
            name = path.parse(fullName).name;
        }

        let analyzerInfo = this.getAnalyzerInfo(fullName);
        if (!analyzerInfo) {
            this.analyzers.push(new CodeAnalyzerInfo(name, fullName, true));
        }
    }

    protected getAnalyzerInfo(value: string): CodeAnalyzerInfo | undefined {
        value = value.toLowerCase();
        for (let i=0; i<this.analyzers.length; i++) {
            if (this.analyzers[i].value.toLowerCase() === value) {
                return this.analyzers[i];
            }
        }
        return undefined;
    }

}