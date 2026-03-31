import { LSConnector } from "../lsConnector";
import { LSModuleClient } from "../lsModuleClient";
import { LSSPGetAppFileSymbolSourceRequest } from "./lsspGetAppFileSymbolSourceRequest";
import { LSSPGetAppFileSymbolSourceResponse } from "./lsspGetAppFileSymbolSourceResponse";

export class LSSPSymbolSourceProviderClient extends LSModuleClient {

    constructor(connector: LSConnector) {
        super(connector);
    }

    async getAppFileSymbolSource(params: LSSPGetAppFileSymbolSourceRequest) : Promise<LSSPGetAppFileSymbolSourceResponse | undefined> {
        return this.connector.sendRequest<LSSPGetAppFileSymbolSourceRequest, LSSPGetAppFileSymbolSourceResponse>(params, "al/symbolssourceprovider/getappfilesymbolsource");
    }

}