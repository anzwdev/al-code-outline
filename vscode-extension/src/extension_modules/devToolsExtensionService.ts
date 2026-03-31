import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { DevToolsExtensionDisposable } from './devToolsExtensionDisposable';

export class DevToolsExtensionService extends DevToolsExtensionDisposable {
    constructor(context: DevToolsExtensionContext) {
        super(context);
    }
}
