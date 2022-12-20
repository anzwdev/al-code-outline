import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { ALAddPageFieldsCodeCommand } from './addFields/alAddPageFieldsCodeCommand';
import { ALAddQueryFieldsCodeCommand } from './addFields/alAddQueryFieldsCodeCommand';
import { ALAddReportFieldsCodeCommand } from './addFields/alAddReportFieldsCodeCommand';
import { ALAddXmlPortFieldsCodeCommand } from './addFields/alAddXmlPortFieldsCodeCommand';
import { ALSortProceduresCodeCommand } from './sortSymbols/alSortProceduresCodeCommand';
import { ALSortReportColumnsCommand } from './sortSymbols/alSortReportColumnsCommand';
import { ALSortPropertiesCommand } from './sortSymbols/alSortPropertiesCommand';
import { ALCreateInterfaceCodeCommand } from './createObjects/alCreateInterfaceCodeCommand';
import { AZDocumentSymbolsLibrary } from '../symbollibraries/azDocumentSymbolsLibrary';
import { ALCodeAction } from './alCodeAction';
import { ALSortVariablesCommand } from './sortSymbols/alSortVariablesCommand';
import { ALCodeCopFixAA0008 } from './codeFixes/alCodeCopFixAA0008';
import { ALCodeCopFixAA0137 } from './codeFixes/alCodeCopFixAA0137';
import { ALCodeCopFixAA0139 } from './codeFixes/alCodeCopFixAA0139';
import { ALSortTableFieldsCommand } from './sortSymbols/alSortTableFieldsCommand';
import { ALSortPermissionsCommand } from './sortSymbols/alSortPermissionsCommand';
import { ALAddPermissionsCodeCommand } from './alAddPermissionsCodeCommand';
import { ALSortPermissionSetListCommand } from './sortSymbols/alSortPermissionSetListCommand';
import { ALReuseToolTipCodeCommand } from './alReuseToolTipCodeCommand';
import { ALSortCustomizationsCommand } from './sortSymbols/alSortCustomizationsCommand';
import { ALXmlPortHeadersCodeCommand } from './alXmlPortHeadersCodeCommand';

export class ALCodeActionsProvider implements vscode.CodeActionProvider {
    protected _toolsExtensionContext : DevToolsExtensionContext;
    protected _codeCommands: ALCodeAction[];

    constructor(context : DevToolsExtensionContext) {
        this._toolsExtensionContext = context;
        this._codeCommands = [
            //code actions
            new ALAddPageFieldsCodeCommand(this._toolsExtensionContext),
            new ALAddQueryFieldsCodeCommand(this._toolsExtensionContext),
            new ALAddReportFieldsCodeCommand(this._toolsExtensionContext),
            new ALAddXmlPortFieldsCodeCommand(this._toolsExtensionContext, 'fieldelement', 'Add multiple field elements (AZ AL Dev Tools)'),
            new ALAddXmlPortFieldsCodeCommand(this._toolsExtensionContext, 'fieldattribute', 'Add multiple field attributes (AZ AL Dev Tools)'),

            new ALAddPermissionsCodeCommand(this._toolsExtensionContext),
            new ALReuseToolTipCodeCommand(this._toolsExtensionContext),
            new ALXmlPortHeadersCodeCommand(this._toolsExtensionContext),

            //sorting
            new ALSortTableFieldsCommand(this._toolsExtensionContext),
            new ALSortVariablesCommand(this._toolsExtensionContext),
            new ALSortReportColumnsCommand(this._toolsExtensionContext),
            new ALSortPropertiesCommand(this._toolsExtensionContext),
            new ALSortProceduresCodeCommand(this._toolsExtensionContext),
            new ALSortPermissionsCommand(this._toolsExtensionContext),
            new ALSortPermissionSetListCommand(this._toolsExtensionContext),
            new ALSortCustomizationsCommand(this._toolsExtensionContext),
            
            new ALCreateInterfaceCodeCommand(this._toolsExtensionContext),
            
            //diagnostics fixes
            new ALCodeCopFixAA0008(this._toolsExtensionContext),
            new ALCodeCopFixAA0137(this._toolsExtensionContext),
            new ALCodeCopFixAA0139(this._toolsExtensionContext)];
    }

    provideCodeActions(document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, token: vscode.CancellationToken): vscode.ProviderResult<(vscode.Command | vscode.CodeAction)[]> {
        //load diagnostics only if CodeCop fixes are enabled
        let settings =  vscode.workspace.getConfiguration('alOutline', document.uri);
        let enableCodeCopFixes = !!settings.get<boolean>('enableCodeCopFixes');
        let diagnostics: vscode.Diagnostic[];
        if (enableCodeCopFixes)
            diagnostics = vscode.languages.getDiagnostics(document.uri);
        else
            diagnostics = [];
        
        //collect code actions
        return this.collectCodeActions(document,range, diagnostics);
    }

    protected async collectCodeActions(document: vscode.TextDocument, range: vscode.Range | vscode.Selection, diagnostic: vscode.Diagnostic[]): Promise<vscode.CodeAction[]> {
        let actions: vscode.CodeAction[] = []; 

        if (this._toolsExtensionContext.alLangProxy.version.major < 1)
            return actions;

        let docSymbols = await this.getDocumentSymbolsAsync(document);
        let symbol = docSymbols.findSymbolInRange(range);

        for (let i=0; i<this._codeCommands.length; i++) {
            this._codeCommands[i].collectCodeActions(docSymbols, symbol, document, range, diagnostic, actions);
        }

        //create OnSave action
        let configuration = vscode.workspace.getConfiguration('alOutline', document.uri);
        let actionsList = configuration.get<string[]>('codeActionsOnSave');
        if ((actionsList) && (actionsList.length > 0)) {
            //check if document can run actions on save
            if (ALCodeActionsProvider.canRunOnSaveOnFile(configuration, document)) {
                let actionKind = vscode.CodeActionKind.SourceFixAll.append('al');
                let action = new vscode.CodeAction("Fix document on save (AZ AL Dev Tools)", actionKind);
                action.command = {
                    title: "Fix document on save",
                    command: "azALDevTools.fixDocumentOnSave",
                    arguments: [
                        document
                    ]
                };
                actions.push(action);
            }
        }

        return actions;
    }

    public static canRunOnSaveOnFile(configuration: vscode.WorkspaceConfiguration, document: vscode.TextDocument) : boolean {
        let ignorePatterns = configuration.get<string[]>('codeActionsOnSaveIgnoreFiles');

        let wsFolder = vscode.workspace.getWorkspaceFolder(document.uri);
        if (!wsFolder)
            return false;        

        if ((ignorePatterns) && (ignorePatterns.length > 0)) {
            let selectors = ignorePatterns.map(pattern => {
                if ((!pattern) || (pattern.startsWith("**")))
                    return {   
                        language: 'al',                 
                        pattern: pattern
                    };
                if (pattern.startsWith("./"))
                    pattern = pattern.substring(2);
                return {   
                    language: 'al',                 
                    pattern: new vscode.RelativePattern(wsFolder!, pattern)
                };
            });

            let matchValue =vscode.languages.match(selectors, document);
            if (matchValue > 0)
                return false;
        }
        return true;
    }

    protected async getDocumentSymbolsAsync(document: vscode.TextDocument): Promise<AZDocumentSymbolsLibrary> {
        let docUri = this._toolsExtensionContext.activeDocumentSymbols.getDocUri();
        
        if ((docUri) && (docUri.fsPath == document.uri.fsPath))
            return this._toolsExtensionContext.activeDocumentSymbols;
        
        //parse document
        let source = document.getText();

        //get document symbols
        let library = new AZDocumentSymbolsLibrary(this._toolsExtensionContext, document.uri, document);
        await library.loadAsync(false);

        return library;
    }

}