import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AppJsonSyntaxProvider } from './appJsonSyntaxProvider';
import { RulesetSyntaxProvider } from './rulesetSyntaxProvider';
import { SyntaxProvider } from './syntaxProvider';

export class SyntaxFilesContentProvider implements vscode.TextDocumentContentProvider {
    onDidChange?: vscode.Event<vscode.Uri> | undefined;
    protected _onDidChangeEmitter: vscode.EventEmitter<vscode.Uri>;
    protected _context: DevToolsExtensionContext;
    protected _providers: SyntaxProvider[]; 
    scheme: string;
    
    constructor(context: DevToolsExtensionContext) {        
        this._context = context;        
        this._onDidChangeEmitter = new vscode.EventEmitter<vscode.Uri>();
        this.onDidChange = this._onDidChangeEmitter.event;
        this._providers = [];
        this.scheme = "aloutlinesyntax";
        this.createProviders();
    }

    private createProviders() {
        this._providers.push(new AppJsonSyntaxProvider(this._context));
        this._providers.push(new RulesetSyntaxProvider(this._context));
    }

    private findProvider(name: string): SyntaxProvider | undefined {
        for (let i=0; i<this._providers.length; i++)
            if (this._providers[i].name == name)
                return this._providers[i];
        return undefined;
    }

    async provideTextDocumentContent(uri: vscode.Uri, token: vscode.CancellationToken): Promise<string> {
        let provider = this.findProvider(uri.path);
        if (provider)
            return await provider.provideTextDocumentContent(uri, token);
        return "{}";
    }

}