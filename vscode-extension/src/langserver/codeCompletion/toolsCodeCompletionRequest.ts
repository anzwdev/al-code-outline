import { TextPosition } from "../../symbollibraries/textPosition";
import { ToolsCodeCompletionParameters } from "./toolsCodeCompletionParameters";

export class ToolsCodeCompletionRequest {
    
    position: TextPosition;
    path: string;    
    providers: string[];
    parameters: ToolsCodeCompletionParameters;

    constructor(newPosition: TextPosition, newPath: string, newProviders: string[], newParameters: ToolsCodeCompletionParameters) {
        this.position = newPosition;
        this.path = newPath;
        this.providers = newProviders;
        this.parameters = newParameters;
    }

}