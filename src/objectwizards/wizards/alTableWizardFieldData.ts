export class ALTableWizardFieldData {
    id: string;
    name: string;
    type: string;
    length: string;

    constructor(newId: string, newName: string, newType: string, newLength: string) {
        this.id = newId;
        this.name = newName;
        this.type = newType;
        this.length = newLength;
    }

}