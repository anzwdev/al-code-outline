import { DevToolsExtensionContext } from "../devToolsExtensionContext";

export class DevToolsExtensionService {
    protected _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;
    }

}