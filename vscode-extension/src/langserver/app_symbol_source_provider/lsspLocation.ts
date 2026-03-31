import { LSTextRange } from "../common_types/lsTextRange";

export interface LSSPLocation {
    schema?: string;
    containerPath?: string;
    sourcePath?: string;
    range?: LSTextRange;
}
