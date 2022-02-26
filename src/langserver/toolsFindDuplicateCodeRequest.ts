export class ToolsFindDuplicateCodeRequest {
    minNoOfStatements: number;

    constructor(newMinNoOfStatements: number) {
        this.minNoOfStatements = newMinNoOfStatements;
    }

}