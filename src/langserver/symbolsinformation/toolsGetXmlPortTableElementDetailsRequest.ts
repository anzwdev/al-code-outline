import { ToolsSymbolInformationRequest } from "./toolsSymbolInformationRequest";

export class ToolsGetXmlPortTableElementDetailsRequest extends ToolsSymbolInformationRequest {
    xmlPortName: string;
    name: string;
    getXmlPortTableFields: boolean;
    getAvailableFields: boolean;

    constructor(newPath: string | undefined, newXmlPortName: string, newName: string, newGetXmlPortTableFields: boolean, newGetAvailableFields: boolean) {
        super(newPath);
        this.xmlPortName = newXmlPortName;
        this.name = newName;
        this.getXmlPortTableFields = newGetXmlPortTableFields;
        this.getAvailableFields = newGetAvailableFields;
    }

}