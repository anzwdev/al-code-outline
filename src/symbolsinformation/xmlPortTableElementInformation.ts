import { SymbolWithNameInformation } from "./smbolWithNameInformation";
import { TableFieldInformation } from "./tableFieldInformation";

export class XmlPortTableElementInformation extends SymbolWithNameInformation {
    source: string | undefined;
    xmlPortTableFields: TableFieldInformation[] | undefined;
    availableTableFields: TableFieldInformation[] | undefined;
}