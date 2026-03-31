import { LSConnector } from "../lsConnector";
import { LSModuleClient } from "../lsModuleClient";
import { LSRSTVGetSyntaxTreeViewerTreeNodePropertiesRequest } from "./lsrstvGetSyntaxTreeViewerTreeNodePropertiesRequest";
import { LSRSTVGetSyntaxTreeViewerTreeNodePropertiesResponse } from "./lsrstvGetSyntaxTreeViewerTreeNodePropertiesResponse";
import { LSRSTVGetSyntaxTreeViewerTreeViewRequest } from "./lsrstvGetSyntaxTreeViewerTreeViewRequest";
import { LSRSTVGetSyntaxTreeViewerTreeViewResponse } from "./lsrstvGetSyntaxTreeViewerTreeViewResponse";

export class LSRSTVRawSyntaxTreeViewerClient extends LSModuleClient {

    constructor(connector: LSConnector) {
        super(connector);
    }

    async getSyntaxTreeViewerTreeView(params: LSRSTVGetSyntaxTreeViewerTreeViewRequest) : Promise<LSRSTVGetSyntaxTreeViewerTreeViewResponse | undefined> {
        return this.connector.sendRequest<LSRSTVGetSyntaxTreeViewerTreeViewRequest, LSRSTVGetSyntaxTreeViewerTreeViewResponse>(params, "al/syntaxtreeviewer/gettreeview");
    }

    async getSyntaxTreeViewerTreeNodeProperties(params: LSRSTVGetSyntaxTreeViewerTreeNodePropertiesRequest) : Promise<LSRSTVGetSyntaxTreeViewerTreeNodePropertiesResponse | undefined> {
        return this.connector.sendRequest<LSRSTVGetSyntaxTreeViewerTreeNodePropertiesRequest, LSRSTVGetSyntaxTreeViewerTreeNodePropertiesResponse>(params, "al/syntaxtreeviewer/gettreenodeproperties");
    }

}