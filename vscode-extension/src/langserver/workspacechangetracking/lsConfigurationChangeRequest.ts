import { LSALProjectSource } from "./lsALProjectSource";

export class LSConfigurationChangeRequest {
    updatedProjects: LSALProjectSource[] | undefined;

    constructor(newUpdatedProjects: LSALProjectSource[] | undefined) {
        this.updatedProjects = newUpdatedProjects;
    }

}