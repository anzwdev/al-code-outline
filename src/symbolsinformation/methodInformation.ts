import { AZSymbolAccessModifier } from "../symbollibraries/azSymbolAccessModifier";

export class MethodInformation {
    name: string | undefined;
    header: string | undefined;
    accessModifier: AZSymbolAccessModifier | undefined;
}