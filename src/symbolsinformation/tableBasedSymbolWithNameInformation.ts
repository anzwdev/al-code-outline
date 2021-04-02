import { SymbolWithNameInformation } from "./smbolWithNameInformation";
import { TableFieldInformation } from "./tableFieldInformation";

export class TableBasedSymbolWithNameInformation extends SymbolWithNameInformation {
    source: string | undefined;
    existingTableFields: TableFieldInformation[] | undefined;
    availableTableFields: TableFieldInformation[] | undefined;
}