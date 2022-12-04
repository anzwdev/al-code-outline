import { TableFieldClass } from "./tableFieldClass";
import { TableFieldInformation } from "./tableFieldInformation";

export class TableFieldInformationHelper {

    static getFieldUIDesc(field: TableFieldInformation) : string {
        let value = "(";
        if (field.dataType)
            value = value + field.dataType + ", ";
        value = value + TableFieldInformationHelper.getFieldClassName(field) + ")";
        return value;
    }

    static getFieldClassName(field: TableFieldInformation) : string {
        switch (field.fieldClass) {
            case TableFieldClass.FlowField: return "FlowField";
            case TableFieldClass.FlowFilter: return "FlowFilter";
            default: return "Normal";
        }
    }

}