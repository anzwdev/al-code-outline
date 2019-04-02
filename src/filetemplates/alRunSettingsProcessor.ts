'use strict';

import * as vzFileTemplates from 'vz-file-templates';

export class ALRunSettingsProcessor implements vzFileTemplates.ITemplateRunSettingsProcessor {
    
    constructor() {
   }

    getName(): string {
        return "azALDevTools.ALTemplates";
    }

    processSettings(settings: vzFileTemplates.IProjectItemTemplateRunSettings): void {       
    }

}
