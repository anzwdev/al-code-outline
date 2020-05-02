export class StringHelper {

    static emptyIfNotDef(value: string | undefined) {
        if (value)
            return value;
        return "";
    }

}