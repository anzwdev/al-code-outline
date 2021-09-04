import { ReportDataItemInformation } from "./reportDataItemInformation";
import { SymbolWithIdInformation } from "./symbolWithIdInformation";

export class ReportInformation extends SymbolWithIdInformation {
    dataItems: ReportDataItemInformation[] | undefined;
}
