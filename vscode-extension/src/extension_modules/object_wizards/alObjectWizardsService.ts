import * as vscode from 'vscode';
import { DevToolsExtensionService } from '../devToolsExtensionService';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALObjectWizard } from './wizards/alObjectWizard';
import { ALTableWizard } from './wizards/alTableWizard';
import { ALTableExtWizard } from './wizards/alTableExtWizard';
import { ALPageWizard } from './wizards/alPageWizard';
import { ALPageExtWizard } from './wizards/alPageExtWizard';
import { ALCodeunitWizard } from './wizards/alCodeunitWizard';
import { ALInterfaceWizard } from './wizards/alInterfaceWizard';
import { ALXmlPortWizard } from './wizards/alXmlPortWizard';
import { ALReportWizard } from './wizards/alReportWizard';
import { ALReportExtWizard } from './wizards/alReportExtWizard';
import { ALQueryWizard } from './wizards/alQueryWizard';
import { ALEnumWizard } from './wizards/alEnumWizard';
import { ALEnumExtWizard } from './wizards/alEnumExtWizard';
import { ALPermissionSetWizard } from './wizards/alPermissionSetWizard';
import { ALPermissionSetExtensionWizard } from './wizards/alPermissionSetExtensionWizard';
import { UriHelper } from '../../core/uriHelper';
import { ALObjectWizardSettings } from './wizards/alObjectWizardSettings';
import { ALObjectWizardsConst } from './alObjectWizardsConst';

export class ALObjectWizardsService extends DevToolsExtensionService {

    protected _wizards: ALObjectWizard[];

    constructor(context: DevToolsExtensionContext) {
        super(context);
        
        this._wizards = [];

        this.initWizards(context);
        this.initCommands();
    }

    private initWizards(context: DevToolsExtensionContext) {
        this._wizards.push(new ALTableWizard(context, 'Table', 'New AL Table Wizard', 'Allows to select table name and enter list of fields'));
        this._wizards.push(new ALTableExtWizard(context, 'Table Extension', 'New AL Table Extension Wizard', 'Allows to add list of fields to existing table'));
        this._wizards.push(new ALPageWizard(context, 'Page', 'New AL Page Wizard', 'Allows to select page type, fast tabs, source table and fields.'));
        this._wizards.push(new ALPageExtWizard(context, 'Page Extension', 'New AL Page Extension Wizard', 'Allows to add layout and action controls to existing page'));

        this._wizards.push(new ALCodeunitWizard(context, 'Codeunit', 'New AL Codeunit Wizard', 'Allows to create simple codeunits and interface implementations'));

        this._wizards.push(new ALInterfaceWizard(context, 'Interface', 'New AL Interface Wizard', 'Allows to create a new interface and copy public procedures from a codeunit'));

        this._wizards.push(new ALXmlPortWizard(context, 'XmlPort', 'New AL XmlPort Wizard', 'Allows to select source table and fields'));
        this._wizards.push(new ALReportWizard(context, 'Report', 'New AL Report Wizard', 'Allows to select source table and fields'));
        this._wizards.push(new ALReportExtWizard(context, 'Report Extension', 'New AL Report Extension Wizard', 'Allows to add dataitems and columns to existing reports'));
        this._wizards.push(new ALQueryWizard(context, 'Query', 'New AL Query Wizard', 'Allows to select query type, source table and fields'));
        this._wizards.push(new ALEnumWizard(context, 'Enum', 'New AL Enum Wizard', 'Allows to select list of enum values and captions'));
        this._wizards.push(new ALEnumExtWizard(context, 'Enum Extension', 'New AL Enum Extension Wizard', 'Allows to add list of enum values and captions to existing enum'));        

        this._wizards.push(new ALPermissionSetWizard(context, 'PermissionSet', 'New AL PermissionSet Wizard', 'Allows to create permission set for extension objects'));
        this._wizards.push(new ALPermissionSetExtensionWizard(context, 'PermissionSetExtension', 'New AL PermissionSetExtension Wizard', 'Allows to create permission set extension for extension objects'));
    }

    private initCommands() {
        //register commands
        this.subscriptions.push(
            vscode.commands.registerCommand(
                ALObjectWizardsConst.cmdNewALFile,
                (fileUri) => {
                    this.runALWizards(fileUri);
                }
            )
        );
    }

    protected async runALWizards(uri: vscode.Uri|undefined) {
        uri = this.getDirectoryUri(uri);
        if (!uri) {
            await vscode.window.showErrorMessage('File cannot be created. Cannot detect destination folder.');
            return;
        }

        //select wizard
        let wizard = await vscode.window.showQuickPick(this._wizards, {
            placeHolder: 'Select wizard type'
        });

        let settings: ALObjectWizardSettings = new ALObjectWizardSettings(uri);
        settings.projectProfile = await this.context.projectInformationService.getProjectProfile(uri);

        if (wizard) {
            wizard.run(settings);
        }
    }

    private getDirectoryUri(uri: vscode.Uri|undefined): vscode.Uri | undefined {
        uri = uri ?? this.getActiveUri();
        if (!uri) {
            return undefined;
        }
        return UriHelper.getDirectoryUriIfFile(uri);
    }

    private getActiveUri(): vscode.Uri | undefined {
        if ((vscode.window.activeTextEditor) && (vscode.window.activeTextEditor.document) && (vscode.window.activeTextEditor.document.uri)) {
            return vscode.window.activeTextEditor.document.uri;
        }

        return this.context.msalLanguageServerProxyService.getCurrentALWorkspaceUri();
    }


}