import * as vscode from 'vscode';
import { CrsIntegrationConst } from './crsIntegrationConst';
import { CrsTypeConverters } from './crsTypeConverters';
import { CrsOnSaveAlFileAction } from './crsOnSaveAlFileAction';
import { SettingsWrapper } from '../../core/settingsWrapper';

export class CrsSettings extends SettingsWrapper {

    constructor(uri: vscode.Uri | undefined) {
        super(CrsIntegrationConst.cfgConfigurationSection, uri);
    }

    public getFileNamePattern() : string | undefined {
        return this.configuration.get<string>(CrsIntegrationConst.cfgFileNamePattern);
    }

    public getExtensionFileNamePattern() : string | undefined {
        return this.configuration.get<string>(CrsIntegrationConst.cfgFileNamePatternExtensions);
    }

    public getExtensionObjectNamePattern() : string | undefined {
        return this.configuration.get<string>(CrsIntegrationConst.cfgExtensionObjectNamePattern);
    }

    public getOnSaveAlFileAction() : CrsOnSaveAlFileAction {
        return CrsTypeConverters.parseCrsOnSaveAlFileAction(this.configuration.get<string>(CrsIntegrationConst.cfgOnSaveAlFileAction));
    }

    public getAlSubFolderName() : string | undefined {
        return this.configuration.get<string>(CrsIntegrationConst.cfgAlSubFolderName);
    }

    public hasCrsFileNamePattern() : boolean {
        let patternText = this.getFileNamePattern();
        return ((!!patternText) && (patternText !== ""));
    }

    public hasCrsExtensionFileNamePattern() : boolean {
        let patternText = this.getExtensionFileNamePattern();
        return ((!!patternText) && (patternText !== ""));
    }

    public hasCrsExtensionObjectNamePattern() {
        let patternText = this.getExtensionObjectNamePattern();
        return ((!!patternText) && (patternText !== ""));
    }

}