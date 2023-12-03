import { ToolsWorkspaceCommandCodeAction } from "./toolsWorkspaceCommandCodeAction";

export class ToolsCollectWorkspaceCodeActionsResponse {
    codeActions: ToolsWorkspaceCommandCodeAction[] | undefined;
    error: boolean | undefined;
    errorMessage: string | undefined;
}