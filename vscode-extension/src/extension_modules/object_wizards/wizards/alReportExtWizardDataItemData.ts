import { LSPITableFieldListItem } from "../../../langserver/project_information/symbols/lspiTableFieldListItem";

export class ALReportExtWizardDataItemData {
    name : string;
    fields : LSPITableFieldListItem[];

    constructor(newName : string) {
        this.name = newName;
        this.fields = [];
    }

}