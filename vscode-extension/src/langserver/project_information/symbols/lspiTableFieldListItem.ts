import { LSTableFieldClass } from "../../common_types/lsTableFieldClass";
import { LSPILabel } from "./lspiLabel";

export interface LSPITableFieldListItem {
    id: number;
    name?: string;
    displayString?: string;
    caption?: string;
    captionLabel?: LSPILabel;
    description?: string;
    dataType?: string;
    class?: LSTableFieldClass;
    toolTips?: LSPILabel[];
}