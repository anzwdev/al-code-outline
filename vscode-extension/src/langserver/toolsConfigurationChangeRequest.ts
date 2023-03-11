import { ToolsALProjectSource } from "./toolsALProjectSource";

export class ToolsConfigurationChangeRequest {
    updatedProjects: ToolsALProjectSource[] | undefined;

    constructor(newUpdatedProjects: ToolsALProjectSource[] | undefined) {
        this.updatedProjects = newUpdatedProjects;
    }

}