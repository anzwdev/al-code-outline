export class NumberHelper {

    static zeroIfNotDef(value: number | undefined): number {
        if (value)
            return value;
        return 0;
    }

}