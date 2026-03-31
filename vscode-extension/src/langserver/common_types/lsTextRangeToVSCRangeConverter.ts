import * as vscode from 'vscode';
import { LSTextRange } from "./lsTextRange";

export class LSTextRangeToVSCRangeConverter {

    public static convert(range: LSTextRange | undefined): vscode.Range | undefined {
        if (!range) {
            return undefined;
        }

        return new vscode.Range(
            new vscode.Position(range.start?.line ?? 0, range.start?.character ?? 0),
            new vscode.Position(range.end?.line ?? 0, range.end?.character ?? 0)
        );
    }

}