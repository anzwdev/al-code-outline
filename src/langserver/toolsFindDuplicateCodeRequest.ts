export class ToolsFindDuplicateCodeRequest {
    minNoOfStatements: number;
    path: string | undefined;

    constructor(newMinNoOfStatements: number, newPath: string | undefined) {
        this.minNoOfStatements = newMinNoOfStatements;
        this.path = newPath;
    }

}