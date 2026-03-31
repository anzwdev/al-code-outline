import { LSConnector } from "../lsConnector";
import { LSModuleClient } from "../lsModuleClient";
import { LSSVCloseDocumentRequest } from "./lssvCloseDocumentRequest";
import { LSSVCloseDocumentResponse } from "./lssvCloseDocumentResponse";
import { LSSVGetObjectHeadersTreeRequest } from "./lssvGetObjectHeadersTreeRequest";
import { LSSVGetObjectHeadersTreeResponse } from "./lssvGetObjectHeadersTreeResponse";
import { LSSVGetObjectRequest } from "./lssvGetObjectRequest";
import { LSSVGetObjectResponse } from "./lssvGetObjectResponse";
import { LSSVGetSymbolLocationRequest } from "./lssvGetSymbolLocationRequest";
import { LSSVGetSymbolLocationResponse } from "./lssvGetSymbolLocationResponse";
import { LSSVOpenAppFileDocumentRequest } from "./lssvOpenAppFileDocumentRequest";
import { LSSVOpenAppFileDocumentResponse } from "./lssvOpenAppFileDocumentResponse";
import { LSSVOpenProjectDocumentRequest } from "./lssvOpenProjectDocumentRequest";
import { LSSVOpenProjectDocumentResponse } from "./lssvOpenProjectDocumentResponse";

export class LSSymbolsViewerClient extends LSModuleClient {

    constructor(connector: LSConnector) {
        super(connector);
    }

    async OpenAppFileDocument(params: LSSVOpenAppFileDocumentRequest) : Promise<LSSVOpenAppFileDocumentResponse | undefined> {
        return this.connector.sendRequest<LSSVOpenAppFileDocumentRequest, LSSVOpenAppFileDocumentResponse>(params, "al/symbolsviewer/openappfile");
    }

    async OpenProjectDocument(params: LSSVOpenProjectDocumentRequest) : Promise<LSSVOpenProjectDocumentResponse | undefined> {
        return this.connector.sendRequest<LSSVOpenProjectDocumentRequest, LSSVOpenProjectDocumentResponse>(params, "al/symbolsviewer/openproject");
    }

    async CloseDocument(params: LSSVCloseDocumentRequest) : Promise<LSSVCloseDocumentResponse | undefined> {
        return this.connector.sendRequest<LSSVCloseDocumentRequest, LSSVCloseDocumentResponse>(params, "al/symbolsviewer/closedocument");
    }

    async GetObjectHeadersTree(params: LSSVGetObjectHeadersTreeRequest) : Promise<LSSVGetObjectHeadersTreeResponse | undefined> {
        return this.connector.sendRequest<LSSVGetObjectHeadersTreeRequest, LSSVGetObjectHeadersTreeResponse>(params, "al/symbolsviewer/getobjectheaderstree");
    }

    async GetObject(params: LSSVGetObjectRequest) : Promise<LSSVGetObjectResponse | undefined> {
        return this.connector.sendRequest<LSSVGetObjectRequest, LSSVGetObjectResponse>(params, "al/symbolsviewer/getobject");
    }

    async getSymbolLocation(params: LSSVGetSymbolLocationRequest) : Promise<LSSVGetSymbolLocationResponse | undefined> {
        return this.connector.sendRequest<LSSVGetSymbolLocationRequest, LSSVGetSymbolLocationResponse>(params, "al/symbolsviewer/getsymbollocation");
    }


}