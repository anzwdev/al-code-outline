import { TextRange } from "./textRange";

export class ALSymbolSourceLocation {
    schema: string | undefined;
    sourcePath: string | undefined;
    range: TextRange | undefined;
}