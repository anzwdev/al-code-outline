import * as vscode from 'vscode';
import * as fs from 'fs';
import * as path from 'path';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { ALObjectWizard } from '../objectwizards/wizards/alObjectWizard';
import { ALPageWizard } from '../objectwizards/wizards/alPageWizard';
import { ALXmlPortWizard } from '../objectwizards/wizards/alXmlPortWizard';
import { ALReportWizard } from '../objectwizards/wizards/alReportWizard';
import { ALQueryWizard } from '../objectwizards/wizards/alQueryWizard';
import { ALEnumWizard } from '../objectwizards/wizards/alEnumWizard';
import { ALEnumExtWizard } from '../objectwizards/wizards/alEnumExtWizard';
import { ALObjectWizardSettings } from '../objectwizards/wizards/alObjectWizardSettings';
import { ALTableWizard } from '../objectwizards/wizards/alTableWizard';
import { ALCodeunitWizard } from '../objectwizards/wizards/alCodeunitWizard';

export class ALObjectWizardsService {
    protected _context: DevToolsExtensionContext;
    protected _wizards: ALObjectWizard[];

    constructor(context: DevToolsExtensionContext) {
        //initialize
        this._context = context;
        //create list of wizards
        this._wizards = [];
        this._wizards.push(new ALTableWizard(context, 'Table', 'New AL Table Wizard', 'Allows to select table name and enter list of fields'));
        this._wizards.push(new ALPageWizard(context, 'Page', 'New AL Page Wizard', 'Allows to select page type, fast tabs, source table and fields.'));
        
        this._wizards.push(new ALCodeunitWizard(context, 'Codeunit', 'New AL Codeunit Wizard', 'Allows to create simple codeunits and interface implementations'));

        this._wizards.push(new ALXmlPortWizard(context, 'XmlPort', 'New AL XmlPort Wizard', 'Allows to select source table and fields'));
        this._wizards.push(new ALReportWizard(context, 'Report', 'New AL Report Wizard', 'Allows to select source table and fields'));
        this._wizards.push(new ALQueryWizard(context, 'Query', 'New AL Query Wizard', 'Allows to select query type, source table and fields'));
        this._wizards.push(new ALEnumWizard(context, 'Enum', 'New AL Enum Wizard', 'Allows to select list of enum values and captions'));
        this._wizards.push(new ALEnumExtWizard(context, 'Enum Extension', 'New AL Enum Extension Wizard', 'Allows to add list of enum values and captions to existing enum'));        

        //register commands
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.newALFile',
                (fileUri) => {
                    this.runALWizards(fileUri);
                }
            )
        );
    
    }

    protected async runALWizards(fileUri: vscode.Uri|undefined) {
        let settings: ALObjectWizardSettings = new ALObjectWizardSettings();           
        if (fileUri) {
            let fullPath: string = fileUri.fsPath;
            if (fs.lstatSync(fullPath).isDirectory()) {
                settings.destDirectoryPath = fullPath;
            } else {
                let parsedPath = path.parse(fullPath);
                settings.destDirectoryPath = parsedPath.dir;
            }
        }

        //select wizard
        let wizard = await vscode.window.showQuickPick(this._wizards, {
            placeHolder: 'Select wizard type'
        });
        if (!wizard)
            return;

        //run wizard
        wizard.run(settings);
    }

}