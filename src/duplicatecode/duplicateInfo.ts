import { DocumentTextRange } from "../symbollibraries/documentTextRange";
import { CodeBlockType } from "./codeBlockType";

export class DuplicateInfo {
    noOfStatements: number | undefined;
    codeBlockType: CodeBlockType | undefined;
    ranges: DocumentTextRange[] | undefined;
}