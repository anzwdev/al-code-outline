import { SymbolWithIdInformation } from "./symbolWithIdInformation";
import { TableFieldInformation } from "./tableFieldInformation";

export class PageInformation extends SymbolWithIdInformation {
    source: string | undefined;
    pageTableFields: TableFieldInformation[] | undefined;
    availableTableFields: TableFieldInformation[] | undefined;
}
