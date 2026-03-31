import { LSConnector } from "../lsConnector";
import { LSModuleClient } from "../lsModuleClient";
import { LSSTSGetSyntaxTreeSymbolsTreeViewRequest } from "./lsstsGetSyntaxTreeSymbolsTreeViewRequest";
import { LSSTSGetSyntaxTreeSymbolsTreeViewResponse } from "./lsstsGetSyntaxTreeSymbolsTreeViewResponse";

//"al/syntaxtreesymbolsview/gettreeview"
export class LSSyntaxTreeSymbolsClient extends LSModuleClient {

    constructor(connector: LSConnector) {
        super(connector);
    }

    public async getSymbols(params: LSSTSGetSyntaxTreeSymbolsTreeViewRequest) : Promise<LSSTSGetSyntaxTreeSymbolsTreeViewResponse | undefined> {
        return this.connector.sendRequest<LSSTSGetSyntaxTreeSymbolsTreeViewRequest, LSSTSGetSyntaxTreeSymbolsTreeViewResponse>(params, "al/syntaxtreesymbolsview/gettreeview");
    }

}
