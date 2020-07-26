export class ALTableWizardFieldData {
    pk: boolean;
    id: string;
    name: string;
    type: string;
    length: string;
    dataClassification: string;

    constructor(newPK: boolean, newId: string, newName: string, newType: string, newLength: string, newDataClassification: string) {
        this.pk = newPK;
        this.id = newId;
        this.name = newName;
        this.type = newType;
        this.length = newLength;
        this.dataClassification = newDataClassification;
    }

}