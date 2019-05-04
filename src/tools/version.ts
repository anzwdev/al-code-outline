export class Version {
    version : string;
    parts : number[];
    major : number;
    minor : number;

    constructor() {
        this.version = '';
        this.parts = [];
        this.major = 0;
        this.minor = 0;
    }

    parse(versionText : string) {
        this.version = versionText;
        this.parts = [];
        let values = this.version.split('.');
        if (values) {
            for (let i=0; i<values.length; i++) {
                this.parts.push(Number.parseInt(values[i]));
            }
        }
        this.major = this.getPart(0);
        this.minor = this.getPart(1);
    }

    protected getPart(index : number) : number {
        if ((index < this.parts.length) && (this.parts[index]))
            return this.parts[index];
        return 0;
    }

}