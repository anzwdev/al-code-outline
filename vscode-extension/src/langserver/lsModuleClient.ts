import { LSConnector } from "./lsConnector";

export class LSModuleClient {
    protected connector: LSConnector;

    constructor(connector: LSConnector) {
        this.connector = connector;
    }
}