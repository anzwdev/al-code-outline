import { ToolsSymbolReference } from "./symbolsinformation/toolsSymbolReference";

export class ToolsGetNewFileRequiredInterfacesRequest {
    useFolderStructure: boolean;
    path: string;
    rootNamespace: string | undefined;
    referencedObjects: ToolsSymbolReference[] | undefined;

    constructor(useFolderStructure: boolean, path: string, rootNamespace: string | undefined, referencedObjects: ToolsSymbolReference[] | undefined) {
        this.useFolderStructure = useFolderStructure;
        this.path = path;
        this.rootNamespace = rootNamespace;
        this.referencedObjects = referencedObjects;
    }

}