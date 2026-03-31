import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { CodeOutlineTreeDocumentState } from "./codeOutlineTreeDocumentState";
import { CodeOutlineTreeItem } from "./codeOutlineTreeItem";

export interface CodeOutlineTreeLoader {
    getTreeId(): string | undefined;
    loadTree(context: DevToolsExtensionContext, state: CodeOutlineTreeDocumentState): Promise<CodeOutlineTreeItem | undefined>;
}