import * as vscode from 'vscode';
import * as path from 'path';
import { BaseWebViewEditor } from "../webviews/baseWebViewEditor";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsGetCodeAnalyzersRulesRequest } from '../langserver/toolsGetCodeAnalyzersRulesRequest';
import { TextEditorHelper } from '../tools/textEditorHelper';
import { CodeAnalyzerInfo } from './codeAnalyzerInfo';
import { CARuleInfo } from '../langserver/caRuleInfo';
import { StringHelper } from '../tools/stringHelper';

export class CARulesViewer extends BaseWebViewEditor {
    protected _devToolsContext: DevToolsExtensionContext;
    protected _rules: CARuleInfo[] | undefined;
    protected _analyzers: CodeAnalyzerInfo[];

    constructor(devToolsContext : DevToolsExtensionContext) {
        super(devToolsContext.vscodeExtensionContext, "Code Analyzers");
        this._devToolsContext = devToolsContext;
        this._analyzers = [];
        this.loadCodeAnalyzers();
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'carulesviewer', 'carulesviewer.html');
    }

    protected getViewType() : string {
        return 'azALDevTools.CARulesViewer';
    }

    protected getAnalyzerInfo(value: string): CodeAnalyzerInfo | undefined {
        value = value.toLowerCase();
        for (let i=0; i<this._analyzers.length; i++) {
            if (this._analyzers[i].value.toLowerCase() == value)
                return this._analyzers[i];
        }
        return undefined;
    }

    protected loadCodeAnalyzers() {
        this._analyzers.push(new CodeAnalyzerInfo('${AppSourceCop}', '${AppSourceCop}', false));
        this._analyzers.push(new CodeAnalyzerInfo('${CodeCop}', '${CodeCop}', false));
        this._analyzers.push(new CodeAnalyzerInfo('${PerTenantExtensionCop}', '${PerTenantExtensionCop}', false));
        this._analyzers.push(new CodeAnalyzerInfo('${UICop}', '${UICop}', false));
        this._analyzers.push(new CodeAnalyzerInfo('Compiler', 'Compiler', false));

        let alConfig = vscode.workspace.getConfiguration('al', undefined);
        let codeAnalyzersSetting = alConfig.get<string[]|undefined>("codeAnalyzers");
        if (codeAnalyzersSetting) {
            for (let i=0; i<codeAnalyzersSetting.length; i++) {
                if (codeAnalyzersSetting[i].startsWith('${')) {
                    let analyzerInfo = this.getAnalyzerInfo(codeAnalyzersSetting[i].trim());
                    if (analyzerInfo)
                        analyzerInfo.selected = true;
                } else {
                    this._analyzers.push(new CodeAnalyzerInfo(path.parse(codeAnalyzersSetting[i]).name,
                        codeAnalyzersSetting[i], true));
                }
            }
        }
    }

    protected async loadRules() {
        this._rules = [];
        for (let analyzerIdx=0; analyzerIdx<this._analyzers.length; analyzerIdx++) {
            let request: ToolsGetCodeAnalyzersRulesRequest = 
                new ToolsGetCodeAnalyzersRulesRequest(this._analyzers[analyzerIdx].value);
            let response = 
                await this._devToolsContext.toolsLangServerClient.getCodeAnalyzersRules(request);
            if ((response) && (response.rules)) {
                for (let ruleIdx = 0; ruleIdx < response.rules.length; ruleIdx++) {
                    response.rules[ruleIdx].analyzer = this._analyzers[analyzerIdx].label;
                    this._rules.push(response.rules[ruleIdx]);
                }
            }
        }

        this.sendMessage({
            command: 'setRules',
            data: this._rules});
    }

    protected async onDocumentLoaded() {            
        //send list of analyzers to the webview
        this.sendMessage({
            command: 'setAnalyzers',
            data: this._analyzers
        });
        //send analyzers rules to the web view
        await this.loadRules();
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;        

        if (message) {
            switch (message.command) {
                case 'newruleset':
                    this.newRuleSet(message.selrules);
                    return true;
                case 'copyrules':
                    this.copyRules(message.selrules);
                    return true;
                case 'copytable':
                    this.copyTable(message.selrules);
                    return true;
            }
        }

        return false;
    }

    protected newRuleSet(rulesIndexes: number[]) {
        let eol = StringHelper.getDefaultEndOfLine(undefined);
        let ruleSetText = '{' + 
            eol + '    "name": "Name",' +
            eol + '    "description": "Description",' +
            eol + '    "rules": [' +
            this.getRulesAsString(rulesIndexes, '        ') +
            eol + '    ],' +
            eol + '}';

        TextEditorHelper.showNewDocument(ruleSetText, 'json');
    }

    protected copyRules(rulesIndexes: number[]) {
        let rulesText = this.getRulesAsString(rulesIndexes, '');
        vscode.env.clipboard.writeText(rulesText);
    }

    protected copyTable(rulesIndexes: number[]) {
        let eol = StringHelper.getDefaultEndOfLine(undefined);
        let rulesText = 'Id\tTitle\tDefault Severity\tAnalyzer';
        if (this._rules) {
            for (let i=0; i<rulesIndexes.length; i++) {
                let rule = this._rules[rulesIndexes[i]];
                rulesText += (eol + rule.id + '\t' +
                    rule.title + '\t' + 
                    rule.defaultSeverity + '\t' +
                    rule.analyzer);
            }
        }
        vscode.env.clipboard.writeText(rulesText);
    }

    protected getRulesAsString(rulesIndexes: number[], indentText: string) : string {
        let eol = StringHelper.getDefaultEndOfLine(undefined);
        let rules = '';
        if (this._rules) {
            for (let i=0; i<rulesIndexes.length; i++) {
                let ruleDef = this._rules[rulesIndexes[i]];
                if (i > 0)
                    rules += ',';
                rules += (eol + '// Rule: ' + ruleDef.title);
                rules += (eol + '//       Default action: ' + ruleDef.defaultSeverity)
                rules += (eol + JSON.stringify({
                    id: ruleDef.id,
                    action: ruleDef.defaultSeverity,
                    justification: 'Justification'
                }, undefined, 4));
            }

            if (indentText.length > 0) {
                if (eol == '\n')
                    rules = rules.replace(/\n/g, eol + indentText);
                else
                    rules = rules.replace(/\r\n/g, eol + indentText);
            }
        }
        return rules;
    }

    protected onPanelClosed() {
        super.onPanelClosed();
        this._devToolsContext.codeAnalyzersService.onCodeAnalyzersViewerClosed();   
    }
}