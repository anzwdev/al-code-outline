import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SortProceduresOrTriggersModifier } from './sortProceduresOrTriggersModifier';

export class SortProceduresModifier extends SortProceduresOrTriggersModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Sort Procedures", "sortProcedures");
    }

}