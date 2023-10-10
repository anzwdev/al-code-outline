import { SymbolWithIdInformation } from "./symbolWithIdInformation";

export class ObjectInformation extends SymbolWithIdInformation {
    type: string | undefined;
    inherentPermissions: string | undefined;
    fullInherentPermissions: boolean | undefined;
}