import { TextRange } from "../symbollibraries/textRange";

export interface ToolsWorkspaceCommandCodeAction {
    commandName: string | undefined;
    description: string | undefined;
    range: TextRange | undefined;
}