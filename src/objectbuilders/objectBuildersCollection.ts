import { ReportBuilder } from "./reportBuilder";
import { PageBuilder } from "./pageBuilder";
import { QueryBuilder } from "./queryBuilder";
import { XmlPortBuilder } from "./xmlPortBuilder";

export class ObjectBuildersCollection {
    public reportBuilder = new ReportBuilder();
    public pageBuilder = new PageBuilder();
    public xmlPortBuilder = new XmlPortBuilder();
    public queryBuilder = new QueryBuilder();
}

