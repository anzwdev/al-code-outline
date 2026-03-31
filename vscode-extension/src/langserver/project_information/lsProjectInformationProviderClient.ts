import { LSConnector } from "../lsConnector";
import { LSModuleClient } from "../lsModuleClient";
import { LSPIGetNamespaceAndUsingsResponse } from "./lspiGetNamespaceAndUsingsReponse";
import { LSPIGetNamespaceAndUsingsRequest } from "./lspiGetNamespaceAndUsingsRequest";
import { LSPIGetNextObjectIdRequest } from "./lspiGetNextObjectIdRequest";
import { LSPIGetNextObjectIdResponse } from "./lspiGetNextObjectIdResponse";
import { LSPIGetObjectMethodsRequest } from "./lspiGetObjectMethodsRequest";
import { LSPIGetObjectMethodsResponse } from "./lspiGetObjectMethodsResponse";
import { LSPIGetObjectsListRequest } from "./lspiGetObjectsListRequest";
import { LSPIGetObjectsListResponse } from "./lspiGetObjectsListResponse";
import { LSPIGetProjectProfileRequest } from "./lspiGetProjectProfileRequest";
import { LSPIGetProjectProfileResponse } from "./lspiGetProjectProfileResponse";
import { LSPIGetTableFieldsRequest } from "./lspiGetTableFieldsRequest";
import { LSPIGetTableFieldsResponse } from "./lspiGetTableFieldsResponse";

export class LSProjectInformationProviderClient extends LSModuleClient {

    constructor(connector: LSConnector) {
        super(connector);
    }

    async getNextObjectId(params: LSPIGetNextObjectIdRequest) : Promise<LSPIGetNextObjectIdResponse | undefined> {
        return this.connector.sendRequest<LSPIGetNextObjectIdRequest, LSPIGetNextObjectIdResponse>(params, "al/projectinformation/getnextobjectid");
    }

    async getObjectsList(params: LSPIGetObjectsListRequest) : Promise<LSPIGetObjectsListResponse | undefined> {
        return this.connector.sendRequest<LSPIGetObjectsListRequest, LSPIGetObjectsListResponse>(params, "al/projectinformation/getobjectslist");
    }

    async getTableFieldsList(params: LSPIGetTableFieldsRequest) : Promise<LSPIGetTableFieldsResponse | undefined> {
        return this.connector.sendRequest<LSPIGetTableFieldsRequest, LSPIGetTableFieldsResponse>(params, "al/projectinformation/gettablefieldslist");
    }

    async getProjectProfile(params: LSPIGetProjectProfileRequest) : Promise<LSPIGetProjectProfileResponse | undefined> {
        return this.connector.sendRequest<LSPIGetProjectProfileRequest, LSPIGetProjectProfileResponse>(params, "al/projectinformation/getprojectprofile");
    }

    async getNamespaceAndUsings(params: LSPIGetNamespaceAndUsingsRequest) : Promise<LSPIGetNamespaceAndUsingsResponse | undefined> {
        return this.connector.sendRequest<LSPIGetNamespaceAndUsingsRequest, LSPIGetNamespaceAndUsingsResponse>(params, "al/projectinformation/getnamespaceandusings");
    }

    async getObjectMethods(params: LSPIGetObjectMethodsRequest) : Promise<LSPIGetObjectMethodsResponse | undefined> {
        return this.connector.sendRequest<LSPIGetObjectMethodsRequest, LSPIGetObjectMethodsResponse>(params, "al/projectinformation/getobjectmethods");
    }

}
