import { LabelInformation } from "./labelInformation";
import { SymbolWithIdInformation } from "./symbolWithIdInformation";

export class TableFieldInformation extends SymbolWithIdInformation {
    captionLabel: LabelInformation | undefined;
}