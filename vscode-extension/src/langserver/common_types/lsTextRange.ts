import { LSTextPosition } from "./lsTextPosition";

export interface LSTextRange {
    start?: LSTextPosition;
    end?: LSTextPosition;
    isEmpty: boolean;
    isSingleLine: boolean;
}