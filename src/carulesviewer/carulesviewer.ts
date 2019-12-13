import * as path from 'path';
import { BaseWebViewEditor } from "../webviews/baseWebViewEditor";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsGetCodeAnalyzersRulesRequest } from '../langserver/toolsGetCodeAnalyzersRulesRequest';
import { window } from 'vscode';

export class CARulesViewer extends BaseWebViewEditor {
    protected _devToolsContext: DevToolsExtensionContext;

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

        this.sendMessage({
            command: 'setAnalyzers',
            data: analyzers
        });

    }

    protected async onAnalyzerSelected(name: string) {
        let request: ToolsGetCodeAnalyzersRulesRequest = new ToolsGetCodeAnalyzersRulesRequest(name);
        let response = await this._devToolsContext.toolsLangServerClient.getCodeAnalyzersRules(request);
        if (response) {
            this.sendMessage({
                command: 'setRules',
                data: response.rules
            });
        }

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
            }
        }

        return false;
    }

    protected newRuleSet(rules: string[]) {
        window.showInformationMessage('New rule set ' + rules.length.toString());
    }

    protected copyRules(rules: string[]) {

    }

}