import { ALTableBasedWizardData } from "./alTableBasedWizardData";

export class ALCodeunitWizardData extends ALTableBasedWizardData {
    interfaceList : string[] | undefined;
    interfaceName: string;

    constructor() {
        super();
        this.interfaceList = undefined;
        this.interfaceName = "";
    }

}