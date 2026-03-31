import { LSPIObjectIdentifier } from "../../../langserver/project_information/symbols/lspiObjectIdentifier";
import { LSPIObjectListItem } from "../../../langserver/project_information/symbols/lspiObjectListItem";
import { ALObjectWizardData } from "./alObjectWizardData";

export class ALPageExtWizardData extends ALObjectWizardData {
    objectName : string;
    pageList : LSPIObjectListItem[] | undefined;
    basePage?: LSPIObjectIdentifier;

    constructor() {
        super();
        this.objectName = '';
        this.pageList = undefined;
        this.basePage = undefined;
    }

}