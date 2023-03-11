import { DuplicateInfo } from "../duplicatecode/duplicateInfo";

export class ToolsFindDuplicateCodeResponse {
    duplicates: DuplicateInfo[] | undefined;
    isError: boolean | undefined;
    message: string | undefined;
}
