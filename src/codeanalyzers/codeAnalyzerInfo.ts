export class CodeAnalyzerInfo {
    label: string;
    value: string;
    selected: boolean;

    constructor(newLabel: string, newValue: string, newSelected: boolean) {
        this.label = newLabel;
        this.value = newValue;
        this.selected = newSelected;
    }

}