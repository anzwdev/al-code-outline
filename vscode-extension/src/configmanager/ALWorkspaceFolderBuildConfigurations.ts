import * as vscode from 'vscode';
import * as path from 'path';
import { ALBuildConfiguration } from './ALBuildConfiguration';
import { ALBuildConfigurationNaming } from './ALBuildConfigurationNaming';
import { SettingsConst } from '../const/settingsConst';
import { StringHelper } from '../tools/stringHelper';

export class ALWorkspaceFolderBuildConfigurations {
    uri: vscode.Uri;
    active: ALBuildConfiguration | undefined = undefined;
    configurations: ALBuildConfiguration[] = [];
    private _configurationNaming: ALBuildConfigurationNaming = ALBuildConfigurationNaming.nameAppJson;

    constructor(uri: vscode.Uri) {
        this.uri = uri;
    }

    async initAsync() {
        this.loadSettings();
        await this.loadConfigurationsAsync();
    }

    async setActiveConfigurationAsync(config: ALBuildConfiguration | undefined) {
        this.active = config;
        if (this.active) {
            await this.active.copyToAppJsonAsync();            
        }
        await this.saveActiveConfigurationToSettingsAsync();
    }

    private async loadConfigurationsAsync() {
        this.configurations = [];

        if (this._configurationNaming !== ALBuildConfigurationNaming.none) {
            //get all files from the folder
            let files = await vscode.workspace.findFiles(new vscode.RelativePattern(this.uri.fsPath, '*.*'));
            for (let i = 0; i < files.length; i++) {
                let configName = this.getConfigurationName(files[i]);
                if ((configName !== undefined) && (configName.length > 0)) {
                    let config = new ALBuildConfiguration(this.uri, files[i], configName);
                    this.configurations.push(config);
                }
            }
        }

        //select active configuration
        await this.loadActiveConfigurationFromSettingsAsync();
    }

    private async loadActiveConfigurationFromSettingsAsync() {
        let settings = vscode.workspace.getConfiguration(SettingsConst.ExtensionSettingsSection, this.uri);
        let activeConfigName = settings.get<string>(SettingsConst.ActiveBuildConfiguration);
        let activeConfig = this.findConfigurationByName(activeConfigName);
        if ((activeConfig === undefined) && (this.configurations.length > 0)) {
            activeConfig = this.configurations[0];
        }
        await this.setActiveConfigurationAsync(activeConfig);
    }

    private async saveActiveConfigurationToSettingsAsync() {
        let settings = vscode.workspace.getConfiguration(SettingsConst.ExtensionSettingsSection, this.uri);
        let activeConfigName = StringHelper.emptyIfNotDef(settings.get<string>(SettingsConst.ActiveBuildConfiguration));
        let newName = StringHelper.emptyIfNotDef(this.active?.name);
        if (activeConfigName !== newName) {
            await settings.update(SettingsConst.ActiveBuildConfiguration, newName, vscode.ConfigurationTarget.WorkspaceFolder);
        }
    }

    private loadSettings(): boolean {
        let config = vscode.workspace.getConfiguration(SettingsConst.ExtensionSettingsSection, this.uri);
               
        let naming = config.get<string>(SettingsConst.BuildConfigurationNaming);
        let newNaming = ALBuildConfigurationNaming.none;
        if (naming) {
            switch (naming.toLowerCase()) {
                case 'appnamejson':
                    newNaming = ALBuildConfigurationNaming.appNameJson;
                    break;
                case 'nameappjson':
                    newNaming = ALBuildConfigurationNaming.nameAppJson;
                    break;
                case 'appjsonname':
                    newNaming = ALBuildConfigurationNaming.appJsonName;
                    break;
            }
        }

        let reloadRequired = (newNaming !== this._configurationNaming);
        this._configurationNaming = newNaming;

        return reloadRequired;
    }

    async onFileCreated(uri: vscode.Uri) {
        if (this._configurationNaming !== ALBuildConfigurationNaming.none) {
            let configName = this.getConfigurationName(uri);
            if ((configName !== undefined) && (configName.length > 0)) {
                let config = new ALBuildConfiguration(this.uri, uri, configName);
                this.configurations.push(config);
                await config.initFromAppJsonAsync();
                if (this.configurations.length === 1) {
                    await this.setActiveConfigurationAsync(config);
                }
            }
        }
    }

    async onFileSavedAsync(uri: vscode.Uri) {
        if (this._configurationNaming !== ALBuildConfigurationNaming.none) {
            if ((this.active) && (this.active.appJsonUri.toString() === uri.toString())) {
                await this.active.copyFromAppJsonAsync();
            }
        }
    }

    async onFileDeletedAsync(uri: vscode.Uri) {
        if (this._configurationNaming !== ALBuildConfigurationNaming.none) {
            let configIndex = this.findConfigurationIndex(uri);
            if (configIndex >= 0) {
                let config = this.configurations[configIndex];
                this.configurations.splice(configIndex, 1);
                if (this.active === config) {
                    if (this.configurations.length > 0) {
                        await this.setActiveConfigurationAsync(this.configurations[0]);
                    } else {
                        await this.setActiveConfigurationAsync(undefined);
                    }
                }
            }
        }
    }

    async onDidChangeConfigurationAsync(e: vscode.ConfigurationChangeEvent) {
        let reloadRequired = this.loadSettings();
        if (reloadRequired) {
            await this.loadConfigurationsAsync();
        }
    }

    private findConfigurationIndex(uri: vscode.Uri) : number {
        for (let i=0; i<this.configurations.length; i++) {
            if (this.configurations[i].uri.toString() === uri.toString()) {
                return i;
            }
        }
        return -1;
    }

    private findConfigurationByName(name: string | undefined) : ALBuildConfiguration | undefined {
        if (name !== undefined) {
            for (let i=0; i<this.configurations.length; i++) {
                if (this.configurations[i].name === name) {
                    return this.configurations[i];
                }
            }
        }
        return undefined;
    }

    private getConfigurationName(uri: vscode.Uri) : string | undefined {
        let filePath = path.parse(uri.fsPath);
        let name = filePath.base;
        let lcaseName = name.toLowerCase();

        switch (this._configurationNaming) {
            case ALBuildConfigurationNaming.appNameJson:
                if (lcaseName.startsWith('app.') && lcaseName.endsWith('.json')) {
                    return name.substring(4, name.length - 5).trim();
                }
                break;
            case ALBuildConfigurationNaming.nameAppJson:
                if (lcaseName.endsWith('.app.json')) {
                    return name.substring(0, name.length - 9).trim();
                }
                break;
            case ALBuildConfigurationNaming.appJsonName:
                if (lcaseName.startsWith('app.json.')) {
                    return name.substring(9).trim();
                }
                break;
        }
        return undefined;
    }

}