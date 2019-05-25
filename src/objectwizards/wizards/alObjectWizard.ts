'use strict';

import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";

export class ALObjectWizard {
    description: string;
    detail: string;
    label: string;
    protected _toolsExtensionContext : DevToolsExtensionContext;
    
    constructor(newToolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        this._toolsExtensionContext = newToolsExtensionContext;
        this.label = newLabel;
        this.description = newDescription;
        this.detail = newDetails;
    }

    run(settings: ALObjectWizardSettings) {
    }

} 