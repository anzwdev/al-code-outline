import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALCodeCommand } from '../alCodeCommand';

export class ALBaseSortCodeCommand extends ALCodeCommand {
    constructor(context : DevToolsExtensionContext, commandName: string) {
        super(context, commandName);
    }
}
