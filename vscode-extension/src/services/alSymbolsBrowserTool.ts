'use strict';

import * as vscode from 'vscode';
import * as fs from 'fs';
import { ALProjectSymbolsLibrary } from '../symbollibraries/alProjectSymbolsLibrary';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { ToolsGetProjectSymbolLocationRequest } from '../langserver/toolsGetProjectSymbolLocationRequest';
import { ToolsGetALAppContentRequest } from '../langserver/toolsGetALAppContentRequest';

interface ALSymbolsBrowserToolInput {
    query?: string;
    symbolKind?: string;
    includeDependencies?: boolean;
    includeBody?: boolean;
    includeSourceCode?: boolean;
    maxResults?: number;
    workspaceFolderPath?: string;
}

const SYMBOL_KIND_MAP: { [key: string]: AZSymbolKind[] } = {
    'table': [AZSymbolKind.TableObject],
    'page': [AZSymbolKind.PageObject],
    'codeunit': [AZSymbolKind.CodeunitObject],
    'report': [AZSymbolKind.ReportObject],
    'query': [AZSymbolKind.QueryObject],
    'xmlport': [AZSymbolKind.XmlPortObject],
    'enum': [AZSymbolKind.EnumType],
    'interface': [AZSymbolKind.Interface],
    'tableExtension': [AZSymbolKind.TableExtensionObject],
    'pageExtension': [AZSymbolKind.PageExtensionObject],
    'enumExtension': [AZSymbolKind.EnumExtensionType],
    'reportExtension': [AZSymbolKind.ReportExtensionObject],
    'permissionSet': [AZSymbolKind.PermissionSet],
    'permissionSetExtension': [AZSymbolKind.PermissionSetExtension],
    'profile': [AZSymbolKind.ProfileObject],
    'pageCustomization': [AZSymbolKind.PageCustomizationObject],
    'controlAddIn': [AZSymbolKind.ControlAddInObject],
    'dotNetPackage': [AZSymbolKind.DotNetPackage],
    'entitlement': [AZSymbolKind.Entitlement],
};

export class ALSymbolsBrowserTool implements vscode.LanguageModelTool<ALSymbolsBrowserToolInput> {
    protected _context : DevToolsExtensionContext;

    constructor(context : DevToolsExtensionContext) {
        this._context = context;
    }

    async invoke(options : vscode.LanguageModelToolInvocationOptions<ALSymbolsBrowserToolInput>, token : vscode.CancellationToken) : Promise<vscode.LanguageModelToolResult> {
        let input = options.input;
        let includeDeps = input.includeDependencies !== false;
        let includeBody = input.includeBody === true;
        let maxResults = (input.maxResults) ? input.maxResults : 50;
        let query = (input.query) ? input.query.trim() : undefined;
        let kindFilter = (input.symbolKind) ? input.symbolKind.trim() : undefined;

        let workspacePath = (input.workspaceFolderPath) ? input.workspaceFolderPath : this._context.alLangProxy.getCurrentWorkspaceFolderPath();
        if (!workspacePath) {
            return new vscode.LanguageModelToolResult([
                new vscode.LanguageModelTextPart('No AL workspace folder is currently open.')
            ]);
        }

        let lib = new ALProjectSymbolsLibrary(this._context, includeDeps, workspacePath);
        let loaded = await lib.loadAsync(true);

        if (!loaded || !lib.rootSymbol) {
            return new vscode.LanguageModelToolResult([
                new vscode.LanguageModelTextPart('Failed to load project symbols. Make sure the AL Language extension is active and the project compiles.')
            ]);
        }

        if (token.isCancellationRequested) {
            return new vscode.LanguageModelToolResult([
                new vscode.LanguageModelTextPart('Operation cancelled.')
            ]);
        }

        //collect all AL objects from the symbol tree
        let allObjects : AZSymbolInformation[] = [];
        lib.rootSymbol.collectObjectSymbols(allObjects);

        //filter by kind
        if ((kindFilter) && (kindFilter !== 'all')) {
            let allowedKinds = SYMBOL_KIND_MAP[kindFilter];
            if (allowedKinds) {
                allObjects = allObjects.filter(s => allowedKinds.indexOf(s.kind) >= 0);
            }
        }

        //filter by name query (supports wildcards)
        if (query) {
            let pattern = this.wildcardToRegex(query);
            allObjects = allObjects.filter(s => pattern.test(s.name) || pattern.test(s.fullName));
        }

        //limit results
        let totalCount = allObjects.length;
        let truncated = allObjects.length > maxResults;
        if (truncated) {
            allObjects = allObjects.slice(0, maxResults);
        }

        if (allObjects.length === 0) {
            return new vscode.LanguageModelToolResult([
                new vscode.LanguageModelTextPart('No symbols found matching the given criteria.')
            ]);
        }

        //format output
        let lines : string[] = [];
        lines.push('Found ' + totalCount.toString() + ' symbol(s)' + (truncated ? ' (showing first ' + maxResults.toString() + ')' : '') + ':\n');
        for (let i=0; i<allObjects.length; i++) {
            lines.push(this.formatSymbol(allObjects[i], includeBody, 0));
        }

        //resolve and include source code if requested
        if (input.includeSourceCode) {
            let workspaceFolders = vscode.workspace.workspaceFolders;
            let workspaceFolder : vscode.WorkspaceFolder | undefined = undefined;
            if (workspaceFolders) {
                for (let i=0; i<workspaceFolders.length; i++) {
                    if (workspacePath.startsWith(workspaceFolders[i].uri.fsPath)) {
                        workspaceFolder = workspaceFolders[i];
                        break;
                    }
                }
                if (!workspaceFolder)
                    workspaceFolder = workspaceFolders[0];
            }

            let libraryUri = lib.getUri();
            let libraryPath : string | undefined = undefined;
            if (libraryUri)
                libraryPath = libraryUri.fsPath;

            for (let i=0; i<allObjects.length; i++) {
                let sourceCode = await this.resolveSourceCode(allObjects[i], workspacePath, libraryPath, workspaceFolder);
                if (sourceCode) {
                    let objId = allObjects[i].id ? ' ' + allObjects[i].id.toString() : '';
                    lines.push('\n--- Source: ' + allObjects[i].getObjectTypeName() + objId + ' "' + allObjects[i].name + '" ---');
                    lines.push(sourceCode);
                }
            }
        }

        return new vscode.LanguageModelToolResult([
            new vscode.LanguageModelTextPart(lines.join('\n'))
        ]);
    }

    async prepareInvocation(options : vscode.LanguageModelToolInvocationPrepareOptions<ALSymbolsBrowserToolInput>, _token : vscode.CancellationToken) : Promise<vscode.PreparedToolInvocation> {
        let input = options.input;
        let message = 'Browsing AL project symbols';
        if (input.query) {
            message += ' matching "' + input.query + '"';
        }
        if ((input.symbolKind) && (input.symbolKind !== 'all')) {
            message += ' (kind: ' + input.symbolKind + ')';
        }
        return { invocationMessage: message };
    }

    protected formatSymbol(symbol : AZSymbolInformation, includeChildren : boolean, indent : number) : string {
        let prefix = '  '.repeat(indent);
        let kindName = symbol.getObjectTypeName();
        let id = symbol.id ? ' ' + symbol.id.toString() : '';
        let ext = symbol.extends ? ' extends "' + symbol.extends + '"' : '';
        let src = symbol.source ? ' [' + symbol.source + ']' : '';
        let line = prefix + '- ' + kindName + id + ' "' + symbol.name + '"' + ext + src;
        if ((includeChildren) && (symbol.childSymbols)) {
            for (let i=0; i<symbol.childSymbols.length; i++) {
                line += '\n' + this.formatChildSymbol(symbol.childSymbols[i], indent + 1);
            }
        }
        return line;
    }

    protected formatChildSymbol(symbol : AZSymbolInformation, indent : number) : string {
        let prefix = '  '.repeat(indent);
        let kindLabel = this.getChildKindLabel(symbol);
        let line = prefix + '- [' + kindLabel + '] "' + symbol.name + '"';
        if (symbol.childSymbols) {
            for (let i=0; i<symbol.childSymbols.length; i++) {
                line += '\n' + this.formatChildSymbol(symbol.childSymbols[i], indent + 1);
            }
        }
        return line;
    }

    protected getChildKindLabel(symbol : AZSymbolInformation) : string {
        if (symbol.isMethod()) return 'Method';
        if (symbol.isTrigger()) return 'Trigger';
        switch (symbol.kind) {
            case AZSymbolKind.Field: return 'Field';
            case AZSymbolKind.Key: return 'Key';
            case AZSymbolKind.PrimaryKey: return 'PrimaryKey';
            case AZSymbolKind.FieldGroup: return 'FieldGroup';
            case AZSymbolKind.PageField: return 'PageField';
            case AZSymbolKind.PageAction: return 'Action';
            case AZSymbolKind.PageActionGroup: return 'ActionGroup';
            case AZSymbolKind.PageGroup: return 'Group';
            case AZSymbolKind.PageArea: return 'Area';
            case AZSymbolKind.PagePart: return 'Part';
            case AZSymbolKind.PageLayout: return 'Layout';
            case AZSymbolKind.PageActionList: return 'Actions';
            case AZSymbolKind.PageRepeater: return 'Repeater';
            case AZSymbolKind.ReportDataItem: return 'DataItem';
            case AZSymbolKind.ReportColumn: return 'Column';
            case AZSymbolKind.ReportLabel: return 'Label';
            case AZSymbolKind.ReportLabelMultilanguage: return 'Label';
            case AZSymbolKind.ReportDataSetSection: return 'DataSet';
            case AZSymbolKind.ReportLabelsSection: return 'Labels';
            case AZSymbolKind.QueryDataItem: return 'DataItem';
            case AZSymbolKind.QueryColumn: return 'Column';
            case AZSymbolKind.QueryFilter: return 'Filter';
            case AZSymbolKind.EnumValue: return 'Value';
            case AZSymbolKind.XmlPortTableElement: return 'TableElement';
            case AZSymbolKind.XmlPortFieldElement: return 'FieldElement';
            case AZSymbolKind.XmlPortFieldAttribute: return 'FieldAttribute';
            case AZSymbolKind.XmlPortTextElement: return 'TextElement';
            case AZSymbolKind.XmlPortTextAttribute: return 'TextAttribute';
            case AZSymbolKind.XmlPortSchema: return 'Schema';
            case AZSymbolKind.VariableDeclaration:
            case AZSymbolKind.VariableDeclarationName: return 'Variable';
            case AZSymbolKind.VarSection:
            case AZSymbolKind.GlobalVarSection: return 'Variables';
            case AZSymbolKind.Property: return 'Property';
            case AZSymbolKind.PropertyList: return 'Properties';
            case AZSymbolKind.Parameter: return 'Parameter';
            case AZSymbolKind.ParameterList: return 'Parameters';
            case AZSymbolKind.FieldList: return 'Fields';
            case AZSymbolKind.KeyList: return 'Keys';
            case AZSymbolKind.FieldGroupList: return 'FieldGroups';
            case AZSymbolKind.FieldExtensionList: return 'FieldChanges';
            case AZSymbolKind.PageViewList:
            case AZSymbolKind.PageExtensionViewList: return 'Views';
            case AZSymbolKind.PageActionArea:
            case AZSymbolKind.PageExtensionActionList: return 'ActionArea';
            case AZSymbolKind.RequestPage:
            case AZSymbolKind.RequestPageExtension: return 'RequestPage';
            case AZSymbolKind.Region: return 'Region';
            case AZSymbolKind.DotNetAssembly: return 'Assembly';
            case AZSymbolKind.DotNetTypeDeclaration: return 'Type';
            case AZSymbolKind.EventDeclaration: return 'Event';
            case AZSymbolKind.IntegrationEventDeclaration: return 'IntegrationEvent';
            case AZSymbolKind.InternalEventDeclaration: return 'InternalEvent';
            case AZSymbolKind.BusinessEventDeclaration: return 'BusinessEvent';
            case AZSymbolKind.ExternalBusinessEventDeclaration: return 'ExternalBusinessEvent';
            case AZSymbolKind.EventSubscriberDeclaration: return 'EventSubscriber';
            default: return AZSymbolKind[symbol.kind] || 'Symbol';
        }
    }

    protected async resolveSourceCode(symbol : AZSymbolInformation, workspacePath : string,
        libraryPath : string | undefined, workspaceFolder : vscode.WorkspaceFolder | undefined) : Promise<string | undefined> {
        if (!workspaceFolder)
            return undefined;

        let locationResponse = await this._context.toolsLangServerClient.getProjectSymbolLocation(
            new ToolsGetProjectSymbolLocationRequest(
                workspaceFolder.uri.fsPath, libraryPath, symbol.kind.toString(), symbol.name));

        if ((!locationResponse) || (!locationResponse.location))
            return undefined;

        let location = locationResponse.location;
        if ((!location.schema) || (!location.sourcePath))
            return undefined;

        if (location.schema === 'file') {
            try {
                let content = fs.readFileSync(location.sourcePath, 'utf8');
                return content;
            } catch {
                return undefined;
            }
        } else if (location.schema === 'alapp') {
            let pathParts = location.sourcePath.split('::');
            if (pathParts.length >= 2) {
                let appPath = pathParts[0];
                let filePath = pathParts.slice(1).join('::');
                let contentResponse = await this._context.toolsLangServerClient.getALAppContent(
                    new ToolsGetALAppContentRequest(appPath, filePath));
                if ((contentResponse) && (contentResponse.source))
                    return contentResponse.source;
            }
        } else if (location.schema === 'al-preview') {
            try {
                let previewUri = vscode.Uri.parse(
                    'al-preview://allang/' + workspaceFolder.name + '/' + encodeURIComponent(location.sourcePath));
                let doc = await vscode.workspace.openTextDocument(previewUri);
                return doc.getText();
            } catch {
                return undefined;
            }
        }

        return undefined;
    }

    protected wildcardToRegex(pattern : string) : RegExp {
        let escaped = pattern.replace(/([.+^${}()|[\]\\])/g, '\\$1');
        let regexStr = escaped.replace(/\*/g, '.*').replace(/\?/g, '.');
        return new RegExp('^' + regexStr + '$', 'i');
    }
}
