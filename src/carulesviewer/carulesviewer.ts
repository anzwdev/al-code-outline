import * as vscode from 'vscode';
import * as path from 'path';
import { BaseWebViewEditor } from "../webviews/baseWebViewEditor";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsGetCodeAnalyzersRulesRequest } from '../langserver/toolsGetCodeAnalyzersRulesRequest';
import { TextEditorHelper } from '../tools/textEditorHelper';

export class CARulesViewer extends BaseWebViewEditor {
    protected _devToolsContext: DevToolsExtensionContext;
    protected _rules: any;

    constructor(devToolsContext : DevToolsExtensionContext) {
        super(devToolsContext.vscodeExtensionContext, "Code Analyzers");
        this._devToolsContext = devToolsContext;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'carulesviewer', 'carulesviewer.html');
    }

    protected getViewType() : string {
        return 'azALDevTools.CARulesViewer';
    }

    protected async onDocumentLoaded() {
        let analyzers = [
            {
                label: '${AppSourceCop}',
                value: '${AppSourceCop}'
            }, 
            {
                label:'${CodeCop}',
                value:'${CodeCop}'
            }, 
            {
                label:'${PerTenantExtensionCop}',
                value:'${PerTenantExtensionCop}'
            }, 
            {
                label:'${UICop}',
                value:'${UICop}'
            }];
            
        //detect other code analyzers
        let alConfig = vscode.workspace.getConfiguration('al', undefined);
        let codeAnalyzersSetting = alConfig.get<string[]|undefined>("codeAnalyzers");
        if (codeAnalyzersSetting) {
            for (let i=0; i<codeAnalyzersSetting.length; i++) {
                if (!codeAnalyzersSetting[i].startsWith('${')) {
                    analyzers.push({
                        label: path.parse(codeAnalyzersSetting[i]).name,
                        value: codeAnalyzersSetting[i]
                    });
                }
            }
        }

        //send list of analyzers to the webview
        this.sendMessage({
            command: 'setAnalyzers',
            data: analyzers
        });

    }

    protected async onAnalyzerSelected(name: string) {
        let request: ToolsGetCodeAnalyzersRulesRequest = new ToolsGetCodeAnalyzersRulesRequest(name);
        let response = await this._devToolsContext.toolsLangServerClient.getCodeAnalyzersRules(request);
        if (response) {
            this._rules = response.rules;
            this.sendMessage({
                command: 'setRules',
                data: response.rules
            });
        } else
            this._rules = undefined;

    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;        

        if (message) {
            switch (message.command) {
                case 'analyzerselected':
                    if (message.name)
                        this.onAnalyzerSelected(message.name);
                    return true;
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
        let ruleSetText = '{' + 
            '\n    "name": "Name",' +
            '\n    "description": "Description",' +
            '\n    "rules": [' +
            this.getRulesAsString(rulesIndexes, '        ') +
            '\n    ],' +
            '\n}'; 

        TextEditorHelper.showNewDocument(ruleSetText, 'json');
    }

    protected copyRules(rulesIndexes: number[]) {
        let rulesText = this.getRulesAsString(rulesIndexes, '');
        vscode.env.clipboard.writeText(rulesText);
    }

    protected copyTable(rulesIndexes: number[]) {
        let rulesText = 'Id\tTitle\tDefault Severity';
        for (let i=0; i<rulesIndexes.length; i++) {
            let rule = this._rules[rulesIndexes[i]];
            rulesText += ('\n' + rule.id + '\t' + rule.title + '\t' + rule.defaultSeverity);
        }
        vscode.env.clipboard.writeText(rulesText);
    }

    protected getRulesAsString(rulesIndexes: number[], indentText: string) : string {
        let rules = '';
        for (let i=0; i<rulesIndexes.length; i++) {
            let ruleDef = this._rules[rulesIndexes[i]];
            if (i > 0)
                rules += ',';
            rules += ('\n// Rule: ' + ruleDef.title);
            rules += ('\n//       Default action: ' + ruleDef.defaultSeverity)
            rules += ('\n' + JSON.stringify({
                id: ruleDef.id,
                action: ruleDef.defaultSeverity,
                justification: 'Justification'
            }, undefined, 4));

        }

        if (indentText.length > 0)
        rules = rules.replace(/\n/g, '\n' + indentText);

        return rules;
    }

    protected onPanelClosed() {
        super.onPanelClosed();
        this._devToolsContext.codeAnalyzersService.onCodeAnalyzersViewerClosed();   
    }
}