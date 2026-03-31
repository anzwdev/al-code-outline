export class ALObjectWizardData {
    objectId : number;
    objectNamespace?: string;
    objectUsings?: string[];

    idResProviders?: string[];
    idResProviderName?: string;
    idResObjectType: string;

    constructor() {
        this.objectId = 0;
        this.idResObjectType = "";
    }

}