export class ALConditionalCompilationSection {
    start: number;
    end: number;
    childSections: ALConditionalCompilationSection[];
    parent: ALConditionalCompilationSection | undefined;
    enabled: boolean;

    constructor(parentSection: ALConditionalCompilationSection | undefined, startLine: number) {
        this.start = startLine;
        this.end = -1;
        this.childSections = [];
        this.enabled = true;
        this.parent = parentSection;
    }

}