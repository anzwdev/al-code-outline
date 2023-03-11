import { TextRange } from "../symbollibraries/textRange";
import { WarningDirectiveInfoKind } from "./warningDirectiveInfoKind";

export class WarningDirectiveInfo {
    title?: string;
    kind?: WarningDirectiveInfoKind;
    fullPath?: string;
    description?: string;
    range?: TextRange;
    disabled?: boolean;
    childItems?: WarningDirectiveInfo[];
}