'use strict';

import * as vzFileTemplates from 'vz-file-templates';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

export class ALAbstractWizard implements vzFileTemplates.IProjectItemWizard {
    protected _toolsExtensionContext : DevToolsExtensionContext;
    protected _name : string;
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, name : string) {
        this._toolsExtensionContext = toolsExtensionContext;
        this._name = name;
    }

    getName() : string {
        return this._name;
    }

    run(template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings) {
    }


} 