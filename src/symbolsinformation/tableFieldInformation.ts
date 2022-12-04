import { LabelInformation } from "./labelInformation";
import { SymbolWithIdInformation } from "./symbolWithIdInformation";
import { TableFieldClass } from "./tableFieldClass";
import { TableFieldState } from "./tableFieldState";

export class TableFieldInformation extends SymbolWithIdInformation {
    dataType: string | undefined;
    fieldClass: TableFieldClass | undefined;
    state: TableFieldState | undefined;
    captionLabel: LabelInformation | undefined;
    description: string | undefined;
    toolTips: string[] | undefined;
    uiDesc: string | undefined;
}