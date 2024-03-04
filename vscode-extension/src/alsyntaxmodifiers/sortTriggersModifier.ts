import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SortProceduresOrTriggersModifier } from './sortProceduresOrTriggersModifier';

export class SortTriggersModifier extends SortProceduresOrTriggersModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Sort Triggers", "sortTriggers");
    }

}