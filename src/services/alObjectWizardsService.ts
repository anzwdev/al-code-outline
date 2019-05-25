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

export class ALObjectWizardsService {
    protected _context: DevToolsExtensionContext;
    protected _wizards: ALObjectWizard[];

    constructor(context: DevToolsExtensionContext) {
        //initialize
        this._context = context;
        //create list of wizards
        this._wizards = [];
        this._wizards.push(new ALPageWizard(context, 'Page Wizard', 'Page Wizard', ''));
        this._wizards.push(new ALXmlPortWizard(context, 'XmlPort Wizard', 'XmlPort Wizard', ''));
        this._wizards.push(new ALReportWizard(context, 'Report Wizard', 'Report Wizard', ''));
        this._wizards.push(new ALQueryWizard(context, 'Query Wizard', 'Query Wizard', ''));
        this._wizards.push(new ALEnumWizard(context, 'Enum Wizard', 'Enum Wizard', ''));
        this._wizards.push(new ALEnumExtWizard(context, 'Enum Extension Wizard', 'Enum Extension Wizard', ''));
        
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
            placeHolder: 'Select object wizard'
        });
        if (!wizard)
            return;

        //run wizard
        wizard.run(settings);
    }

}