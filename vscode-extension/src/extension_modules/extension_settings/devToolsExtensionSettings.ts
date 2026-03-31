import * as vscode from 'vscode';
import { SettingsWrapper } from "../../core/settingsWrapper";
import { DevToolsExtensionSettingsConst } from "./devToolsExtensionSettingsConst";
import { ALAppAreaMode } from './alAppAreaMode';
import { LSPIProjectPlatformCapabilities } from '../../langserver/project_information/profile/lspiProjectPlatformCapabilities';
import { ALFieldToolTipsLocation } from './alFieldToolTipsLocation';

export class DevToolsExtensionSettings extends SettingsWrapper{

    constructor(resourceUri: vscode.Uri | undefined) {
        super(DevToolsExtensionSettingsConst.cfgConfigurationSection, resourceUri);
    }

    public getOpenDefinitionInNewTab() : boolean {
        return this.configuration.get<boolean>(DevToolsExtensionSettingsConst.cfgOpenDefinitionInNewTab) ?? false;
    }

    public getAppAreaMode(projectCapabilities: LSPIProjectPlatformCapabilities | undefined) : ALAppAreaMode {
        if (!projectCapabilities?.appAreasInheritance) {
            return ALAppAreaMode.addToAllControls;
        }

        let appAreaModeValue = this.configuration.get<string>(DevToolsExtensionSettingsConst.cfgAppAreaMode);
        if ((appAreaModeValue) && (appAreaModeValue !== '')) {
            let type = (<any>ALAppAreaMode)[appAreaModeValue];
            if (type !== undefined) {
                return type;
            }
        }
       
        return ALAppAreaMode.inheritFromMainObject;
    }

    public getDefaultAppArea() : string | undefined {
        return this.configuration.get<string>(DevToolsExtensionSettingsConst.cfgDefaultAppArea);
    }


    public getFieldToolTipsLocation(projectCapabilities: LSPIProjectPlatformCapabilities | undefined) : ALFieldToolTipsLocation {
        let locationValue = this.configuration.get<string>(DevToolsExtensionSettingsConst.cfgFieldToolTipsLocation);
        let location = ALFieldToolTipsLocation.page;
        if ((locationValue) && (locationValue === 'table') && (projectCapabilities) && (projectCapabilities.tableToolTips)) {
            location = ALFieldToolTipsLocation.table;
        }
        return location;
    }

    public getAddToolTipsToPageFields() : boolean {
        return this.configuration.get<boolean>(DevToolsExtensionSettingsConst.cfgAddToolTipsToPageFields) ?? false;
    }

    public getDoNotReuseToolTipsFromOtherPages() : boolean {
        return this.configuration.get<boolean>(DevToolsExtensionSettingsConst.cfgDoNotReuseToolTipsFromOtherPages) ?? false;
    }

    public getReuseToolTipsFromDependencies() : string[] | undefined {
        return this.configuration.get<string[]>(DevToolsExtensionSettingsConst.cfgReuseToolTipsFromDependencies);
    }

    public getPageFieldToolTip() : string | undefined {
        return this.configuration.get<string>(DevToolsExtensionSettingsConst.cfgPageFieldToolTip);
    }

    public getPageFieldToolTipComment() : string | undefined {
        return this.configuration.get<string>(DevToolsExtensionSettingsConst.cfgPageFieldToolTipComment);
    }

    public getUseTableFieldDescriptionAsToolTip() : boolean {
        return this.configuration.get<boolean>(DevToolsExtensionSettingsConst.cfgUseTableFieldDescriptionAsToolTip) ?? false;        
    }

    
    
    public getNoEmptyLinesAtTheEndOfWizardGeneratedFiles() : boolean {
        return this.configuration.get<boolean>(DevToolsExtensionSettingsConst.cfgNoEmptyLinesAtTheEndOfWizardGeneratedFiles) ?? false;
    }

    

    public getCreateApiFieldsCaptions() : boolean {
        return this.configuration.get<boolean>(DevToolsExtensionSettingsConst.cfgCreateApiFieldsCaptions) ?? false;
    }
        
    public getApiFieldNamesConversion() : any[] | undefined {
        return this.configuration.get<any[]>(DevToolsExtensionSettingsConst.cfgApiFieldNamesConversion);
    }
    
    public getDefaultApiPublisher() : string | undefined {
        return this.configuration.get<string>(DevToolsExtensionSettingsConst.cfgDefaultApiPublisher);
    }

    public getDefaultApiGroup() : string | undefined {
        return this.configuration.get<string>(DevToolsExtensionSettingsConst.cfgDefaultApiGroup);
    }

    public getDefaultApiVersion() : string | undefined {
        return this.configuration.get<string>(DevToolsExtensionSettingsConst.cfgDefaultApiVersion);
    }

}