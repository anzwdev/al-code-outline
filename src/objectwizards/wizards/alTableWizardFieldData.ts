export class ALTableWizardFieldData {
    id: string;
    name: string;
    type: string;
    length: string;
    dataClassification: string;

    constructor(newId: string, newName: string, newType: string, newLength: string, newDataClassification: string) {
        this.id = newId;
        this.name = newName;
        this.type = newType;
        this.length = newLength;
        this.dataClassification = newDataClassification;
    }

}