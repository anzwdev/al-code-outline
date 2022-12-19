export class ApiFieldNameConversion {
    searchRegExp: RegExp;
    newValue: string;

    constructor(searchString: string, newValueString: string) {
        this.searchRegExp = new RegExp(searchString);
        this.newValue = newValueString;
    }
}