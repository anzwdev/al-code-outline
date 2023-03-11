import { ALObsoleteState } from "../symbollibraries/alObsoleteState";

export class ToolsFindDuplicateCodeRequest {
    minNoOfStatements: number;
    skipObsoleteCodeLevel: ALObsoleteState;
    path: string | undefined;

    constructor(newMinNoOfStatements: number, newSkipObsoleteCodeLevel: ALObsoleteState, newPath: string | undefined) {
        this.minNoOfStatements = newMinNoOfStatements;
        this.skipObsoleteCodeLevel = newSkipObsoleteCodeLevel;
        this.path = newPath;
    }

}