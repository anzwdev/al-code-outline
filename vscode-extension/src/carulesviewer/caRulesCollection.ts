import * as vscode from 'vscode';
import * as path from 'path';
import * as fs from 'fs';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { CARuleInfo } from "../langserver/caRuleInfo";
import { CodeAnalyzerInfo } from "../codeanalyzers/codeAnalyzerInfo";
import { ToolsGetCodeAnalyzersRulesRequest } from '../langserver/toolsGetCodeAnalyzersRulesRequest';

export class CARulesCollection {
    protected _context: DevToolsExtensionContext;
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
        this.analyzers.push(new CodeAnalyzerInfo('${AppSourceCop}', '${AppSourceCop}', true));
        this.analyzers.push(new CodeAnalyzerInfo('${CodeCop}', '${CodeCop}', true));
        this.analyzers.push(new CodeAnalyzerInfo('${PerTenantExtensionCop}', '${PerTenantExtensionCop}', true));
        this.analyzers.push(new CodeAnalyzerInfo('${UICop}', '${UICop}', true));
        this.addCustomAnalyzer('${analyzerFolder}BusinessCentral.LinterCop.dll');
        this.analyzers.push(new CodeAnalyzerInfo('Compiler', 'Compiler', true));

        let alConfig = vscode.workspace.getConfiguration('al', undefined);
        let codeAnalyzersSetting = alConfig.get<string[]|undefined>("codeAnalyzers");
        if (codeAnalyzersSetting) {
            for (let i=0; i<codeAnalyzersSetting.length; i++) {
                let analyzerName = codeAnalyzersSetting[i].trim();
                if (analyzerName.startsWith('${')) {
                    let analyzerInfo = this.getAnalyzerInfo(analyzerName);
                    if (!analyzerInfo)
                        this.analyzers.push(new CodeAnalyzerInfo(analyzerName, analyzerName, true));
                } else {
                    this.analyzers.push(new CodeAnalyzerInfo(path.parse(codeAnalyzersSetting[i]).name,
                        codeAnalyzersSetting[i], true));
                }
            }
        }
    }

    protected getAnalyzerInfo(value: string): CodeAnalyzerInfo | undefined {
        value = value.toLowerCase();
        for (let i=0; i<this.analyzers.length; i++) {
            if (this.analyzers[i].value.toLowerCase() == value)
                return this.analyzers[i];
        }
        return undefined;
    }

    private addCustomAnalyzer(name: string) {
        if ((name.startsWith('${analyzerFolder}')) && (this._context.alLangProxy.extensionPath)) {
            let fileName = name.substring('${analyzerFolder}'.length);
            let fullPath = path.join(this._context.alLangProxy.extensionPath, 'bin', 'Analyzers', fileName);
            if (fs.existsSync(fullPath))
                this.analyzers.push(new CodeAnalyzerInfo(name, name, true));        
        }
    }

}