import { TableFieldInformation } from "../../symbolsinformation/tableFieldInformation";

export class ALReportExtWizardDataItemData {
    name : string;
    fields : TableFieldInformation[];

    constructor(newName : string) {
        this.name = newName;
        this.fields = [];
    }

}