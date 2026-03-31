import { LSPIObjectIdentifier } from "../../../langserver/project_information/symbols/lspiObjectIdentifier";
import { LSPIObjectListItem } from "../../../langserver/project_information/symbols/lspiObjectListItem";
import { ALTableBasedWizardData } from "./alTableBasedWizardData";

export class ALCodeunitWizardData extends ALTableBasedWizardData {
    interfaceList? : LSPIObjectListItem[];
    interface?: LSPIObjectIdentifier;

    constructor() {
        super();
        this.interfaceList = undefined;
        this.interface = undefined;
    }

}