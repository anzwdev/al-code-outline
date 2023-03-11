import { TextPosition } from "../../symbollibraries/textPosition";

export class ToolsCodeCompletionRequest {
    
    position: TextPosition;
    path: string;
    providers: string[];

    constructor(newPosition: TextPosition, newPath: string, newProviders: string[]) {
        this.position = newPosition;
        this.path = newPath;
        this.providers = newProviders;
    }

}