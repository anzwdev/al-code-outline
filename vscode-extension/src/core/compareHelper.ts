export class CompareHelper {

    static compareValues(a: any, b: any): number {
        if (a < b) {
            return -1;
        }
        if (a > b) {
            return 1;
        }
        return 0;                               
    }

}