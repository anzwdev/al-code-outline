export class Version {
    version : string;
    parts : number[];
    major : number;
    minor : number;

    static create(versionText: string): Version {
        let version = new Version();
        version.parse(versionText);
        return version;
    }

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

    isLower(version: Version): boolean {
        return (this.compare(version) < 0);
    }

    isGreater(version: Version): boolean {
        return (this.compare(version) > 0);
    }

    isEqual(version: Version): boolean {
        return (this.compare(version) == 0);
    }

    isGreaterOrEqual(version: Version): boolean {
        return (this.compare(version) >= 0);
    }

    isLowerOrEqual(version: Version): boolean {
        return (this.compare(version) <= 0);
    }

    compare(version: Version) {
        let length = Math.min(this.parts.length, version.parts.length);
        for (let i=0; i<length; i++) {
            if (this.parts[i] < version.parts[i])
                return -1;
            if (this.parts[i] > version.parts[i])
                return 1;
        }
        if (this.parts.length < version.parts.length)
            return -1;
        if (this.parts.length > version.parts.length)
            return 1;
        return 0;
    }

}