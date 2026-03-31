import { LSPIObjectIdentifier } from "../../../langserver/project_information/symbols/lspiObjectIdentifier";
import { LSPIObjectListItem } from "../../../langserver/project_information/symbols/lspiObjectListItem";
import { ALEnumWizardData } from "./alEnumWizardData";

export class ALEnumExtWizardData extends ALEnumWizardData {
    firstValueId : number;
    baseEnum? : LSPIObjectIdentifier;
    baseEnumList? : LSPIObjectListItem[];
    
    constructor() {
        super();
        this.firstValueId = 0;
        this.baseEnum = undefined;
        this.baseEnumList = [];
    }

} 