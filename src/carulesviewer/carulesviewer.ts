import * as vscode from 'vscode';
import * as path from 'path';
import * as fs from 'fs';
import { BaseWebViewEditor } from "../webviews/baseWebViewEditor";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsGetCodeAnalyzersRulesRequest } from '../langserver/toolsGetCodeAnalyzersRulesRequest';
import { TextEditorHelper } from '../tools/textEditorHelper';
import { CodeAnalyzerInfo } from './codeAnalyzerInfo';
import { CARuleInfo } from '../langserver/caRuleInfo';
import { StringHelper } from '../tools/stringHelper';
import { CARulesCollection } from './caRulesCollection';

export class CARulesViewer extends BaseWebViewEditor {
    protected _devToolsContext: DevToolsExtensionContext;
    protected _rulesCollection: CARulesCollection;

    constructor(devToolsContext : DevToolsExtensionContext) {
        super(devToolsContext.vscodeExtensionContext, "Code Analyzers");
        this._devToolsContext = devToolsContext;
        this._rulesCollection = new CARulesCollection(devToolsContext);
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'carulesviewer', 'carulesviewer.html');
    }

    protected getViewType() : string {
        return 'azALDevTools.CARulesViewer';
    }

    protected async loadRules() {
        await this._rulesCollection.loadRules();

        this.sendMessage({
            command: 'setRules',
            data: this._rulesCollection.rules});
    }

    protected async onDocumentLoaded() {            
        //send list of analyzers to the webview
        this.sendMessage({
            command: 'setAnalyzers',
            data: this._rulesCollection.analyzers
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
        if (this._rulesCollection.rules) {
            for (let i=0; i<rulesIndexes.length; i++) {
                let rule = this._rulesCollection.rules[rulesIndexes[i]];
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
        if (this._rulesCollection.rules) {
            for (let i=0; i<rulesIndexes.length; i++) {
                let ruleDef = this._rulesCollection.rules[rulesIndexes[i]];
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