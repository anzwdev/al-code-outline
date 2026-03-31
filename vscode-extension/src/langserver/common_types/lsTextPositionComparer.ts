import { CompareHelper } from "../../core/compareHelper";
import { LSTextPosition } from "./lsTextPosition";

export class LSTextPositionComparer {

    public static compare(position: LSTextPosition | undefined, otherPosition: LSTextPosition | undefined): number {
        let compareResult = CompareHelper.compareValues(position?.line, otherPosition?.line);
        if (compareResult !== 0) {
            return compareResult;
        }
        return CompareHelper.compareValues(position?.character, otherPosition?.character);
    }


}