var vscodeContext;

$(function() {
    //initialize api
    vscodeContext = acquireVsCodeApi();

    window.addEventListener('message', event => {
        processMessage(event.data);
    });
    
    vscodeContext.postMessage({
        command: 'documentLoaded'
    });

    //reload data
    //setMode('Table', false);
});

var dataMode = '';
var pivotType = '';
var pivotId = 0;
var objdata = {};

var filterType = [];
var filterTypeActive = false;
var filterId = undefined;
var filterIdExpr = undefined;
var filterIdActive = false;
var filterName = undefined;
var filterNameExpr = undefined;
var filterNameActive = false;
var filterPackage = undefined;
var filterPackageExpr = undefined;
var filterPackageActive = false;
var filterActive = false;

function processMessage(msg_data) {
    if (msg_data.msgtype == 'objectsLoaded') {
        processObjectsLoadedMessage(msg_data);
    }
    else if (msg_data.msgtype == 'filterObjects') {
        processFilterObjectsMessage(msg_data);
    }
}

function processObjectsLoadedMessage(msg_data) {
    objdata = msg_data.data;
    let name = 'AL Objects Browser';
    if (objdata.name)
        name = name + ' - ' + objdata.name;
    $('#headtitle').html(name);

    //process data
    if ((objdata) && (objdata.objectCollections)) {
        for (let i=0; i<objdata.objectCollections.length; i++) {
            objdata.objectCollections[i].objectType = symbolKindToObjectType(objdata.objectCollections[i].kind);
            sortData(objdata.objectCollections[i]);
        }
    }

    setMode('Table', true);
}

function sortData(data) {       
    if (data && data.childSymbols && (data.childSymbols.length > 1)) {
        data.childSymbols.sort(function(a,b) {
            if (a.id < b.id)
                return -1;
            if (a.id > b.id)
                return 1;
            if (a.fullName < b.fullName)
                return -1;
            if (a.fullName > b.fullName)
                return 1;
            return 0;                    
        });
    }
}

function processFilterObjectsMessage(msg_data) {
    var column = msg_data.column;
    if (column == 'Type') {
        filterType = msg_data.filterSet;
    }
    else if (column == 'ID') {
        if (!(msg_data.filterExpr)) {
            filterId = undefined;
            filterIdExpr = undefined;
        }
        else {
            filterIdExpr = msg_data.filterExpr;
            try {
                filterId = compileFilter('int', filterIdExpr);
            }
            catch (e) {
                vscodeContext.postMessage({
                    command    : 'errorInFilter',
                    message : filterIdExpr});

                filterId = undefined;
                filterIdExpr = undefined;
            }
        }
    }
    else if (column == 'Name') {
        if (!(msg_data.filterExpr)) {
            filterName = undefined;
            filterNameExpr = undefined;
        }
        else {
            filterNameExpr = msg_data.filterExpr;
            try {
                filterName = compileFilter('text', filterNameExpr);
            }
            catch (e) {
                vscodeContext.postMessage({
                    command    : 'errorInFilter',
                    message : filterNameExpr});
                
                filterName = undefined;
                filterNameExpr = undefined;
            }
        }
    }
    else if (column == 'Package') {
        if (!(msg_data.filterExpr)) {
            filterPackage = undefined;
            filterPackageExpr = undefined;
        }
        else {
            filterPackageExpr = msg_data.filterExpr;
            try {
                filterPackage = compileFilter('text', filterPackageExpr);
            }
            catch (e) {
                vscodeContext.postMessage({
                    command    : 'errorInFilter',
                    message : filterPackageExpr});
                
                filterPackage = undefined;
                filterPackageExpr = undefined;
            }
        }
    }

    filterTypeActive = (filterType !== undefined && filterType.length > 0);
    filterIdActive = filterId !== undefined;
    filterNameActive = filterName !== undefined;
    filterPackageActive = filterPackage !== undefined;
    filterActive = filterTypeActive || filterIdActive || filterNameActive || filterPackageActive;

    applyObjectFilters();

    renderData();
}

function symbolKindToObjectType(kind) {
    switch (kind)
    {
        case ALSymbolKind.TableObjectList:
            return "Table";
        case ALSymbolKind.PageObjectList:
            return "Page";
        case ALSymbolKind.ReportObjectList:
            return "Report";
        case ALSymbolKind.XmlPortObjectList:
            return "XmlPort";
        case ALSymbolKind.QueryObjectList:
            return "Query";
        case ALSymbolKind.CodeunitObjectList:
            return "Codeunit";
        case ALSymbolKind.ControlAddInObjectList:
            return "ControlAddIn";
        case ALSymbolKind.PageExtensionObjectList:
            return "PageExtension";
        case ALSymbolKind.TableExtensionObjectList:
            return "TableExtension";
        case ALSymbolKind.ProfileObjectList:
            return "Profile";
        case ALSymbolKind.PageCustomizationObjectList:
            return "PageCustomization";
        case ALSymbolKind.EnumTypeList:
            return "Enum";
        case ALSymbolKind.DotNetPackageList:
            return "DotNetPackage";
        default:
            return "Undefined";
    }
}

function applyObjectFilters() {
    for (var i=0; i<objdata.objectCollections.length; i++) {
        // First, check if the object was visible based on the datamode (or otherwise was filtered out by the current selection).
        var inMode = ((dataMode === 'All') || (dataMode == objdata.objectCollections[i].objectType));
        if (!inMode) {
            objdata.objectCollections[i].visible = false;
            continue;
        }

        // Case: No filters active, so we can set everything from this collection to be visible:
        if (!filterActive) {
            objdata.objectCollections[i].visible = true;
            var objects = objdata.objectCollections[i].childSymbols;
            for (var j=0; j<objects.length; j++) {
                objects[j].objvisible = true;
            }
            continue;
        }

        // Next, check if there is a filter on the 'Type' column, and whether the object is contained in it.
        var inTypeFilter = !filterTypeActive || filterType.includes(objdata.objectCollections[i].objectType);
        if (!inTypeFilter) {
            objdata.objectCollections[i].visible = false;
            continue;
        }

        // If the above all holds, then the object should be visible in the browser.
        objdata.objectCollections[i].visible = true;

        // Now check per object.
        var objects = objdata.objectCollections[i].childSymbols;
        for (var j=0; j<objects.length; j++) {
            var inIdFilter = !filterIdActive || filterId({INT: objects[j].id});
            if (!inIdFilter) {
                objects[j].objvisible = false;
                continue;
            }

            var inNameFilter = !filterNameActive || filterName({TEXT: objects[j].name});
            if (!inNameFilter) {
                objects[j].objvisible = false;
                continue;
            }

            var inPackageFilter = !filterPackageActive || filterPackage({TEXT: objects[j].library});
            if (!inPackageFilter) {
                objects[j].objvisible = false;
                continue;
            }

            objects[j].objvisible = true;
        }
    }
}

function clearObjectFilters() {
    filterType = [];
    filterId = undefined;
    filterIdExpr = undefined;
    filterName = undefined;
    filterNameExpr = undefined;
    filterPackage = undefined;
    filterPackageExpr = undefined;

    filterTypeActive = false;
    filterIdActive = false;
    filterNameActive = false;
    filterPackageActive = false;
    filterActive = false;

    applyObjectFilters();
    
    renderData();
}

function setMode(newMode, reload) {
    if ((dataMode == newMode) && (!reload))
        return;

    if (dataMode)
        $("div[data-mode='" + dataMode + "']").attr('class', 'btn btnstd');

    dataMode = newMode;

    applyObjectFilters();

    if (dataMode)
        $("div[data-mode='" + dataMode + "']").attr('class', 'btn btnsel');

    renderData();
}

function renderData() {
    $('#objects').html(Handlebars.templates.objectlist(objdata));
    $("tr[data-objt='" + pivotType + "'][data-objid='" + pivotId + "']").attr('class', 'objsel');

    initContextMenus();
}

function initContextMenus() {
    $(function(){
        $('#objhead').contextMenu({
            selector: 'th',
            callback: function(key, options) {
                execFilterCommand($(this).text(), key);
            },
            items: {
                "filterObjects": {
                    name: "Filter on Column"
                },
                "clearFilters": {
                    name: "Clear All Filters",
                    disabled: function(key, opt) {
                        return !filterActive;
                    }
                }
            }
        })
    });

    $(function(){
        $('#objects').contextMenu({
            selector: 'tr', 
            callback: function(key, options) {
                execObjCommand(key, $(this)[0], $(this).data("objt"));
            },
            items: {
                "definition": {name: "Go to Definition"},
                "sep1": "---------",
                "runinwebclient": {
                    name: "Run in Web Client",
                    disabled: function(key, opt) {
                        var objt = $(this).data("objt"); 
                        return ((objt != "Table") && (objt != "Page") && (objt != "Report"));
                    }},
                "sep2": "---------",
                // New Base Objects
                "newcardpage": {
                    name: "New Card Page",
                    disabled: function(key, opt) {
                        return ($(this).data("objt") != "Table");
                    }},
                "newlistpage": {
                    name: "New List Page",
                    disabled: function(key, opt) {
                        return ($(this).data("objt") != "Table");
                    }},
                "newreport": {
                    name: "New Report",
                    disabled: function(key, opt) {
                        return ($(this).data("objt") != "Table");
                    }},
                "newxmlport": {
                    name: "New Xml Port",
                    disabled: function(key, opt) {
                        return ($(this).data("objt") != "Table");
                    }},
                "newquery": {
                    name: "New Query",
                    disabled: function(key, opt) {
                        return ($(this).data("objt") != "Table");
                    }},
                "sep3": "---------",
                // New Extension Objects
                "extendtable": {
                    name: "New Table Extension",
                    disabled: function(key, opt) {
                        return ($(this).data("objt") != "Table");
                    }},
                "extendpage": {
                    name: "New Page Extension",
                    disabled: function(key, opt) {
                     return ($(this).data("objt") != "Page");
                    }},
            }
        });
    });
}

function modeBtnClick(btn) {
    setMode(btn.dataset.mode, false);
}

function arraysEq(array1, array2) {
    if (array1.length != array2.length)
        return false;
    for (let i=0; i<array1.length;i++) {
        if (array1[i] != array2[i])
            return false;
    }
    return true;
}

function containsPath(pathlist, path) {
    for (let i=0; i<pathlist.length; i++) {
        if (arraysEq(path, pathlist[i]))
            return true;
    }
    return false;
}

function execObjCommand(cmdname, selectedrow, objtype) {
    if (vscodeContext) {
        //merge path with selectedpaths
        let path = getSymbolRef(selectedrow);
        let selpaths = getSelectedObjectIds(objtype);
        if (!selpaths)
            selpaths = [];
        if ((path) && (!containsPath(selpaths, path)))
            selpaths.push(path);
  
        vscodeContext.postMessage({
            command : cmdname,
            path : path,
            selpaths: selpaths});
    }
}

function execFilterCommand(headColumn, cmdname) {
    if (vscodeContext) {
        if (cmdname == "filterObjects") {
            vscodeContext.postMessage({
                command    : "execFilterCommand",
                headColumn : headColumn,
                currentIdFilter: filterIdExpr,
                currentNameFilter: filterNameExpr});
        }
        else if (cmdname == "clearFilters") {
            clearObjectFilters();
        }
    }
}

function updatePivotObject(path) {
    if (vscodeContext) {
        vscodeContext.postMessage({
            command : "updatePivotObj",
            path : path
        });
    }
}

function getSymbolRef(selectedrow) {
    let path = [
        selectedrow.dataset.objidx,
        selectedrow.dataset.pidx
    ];
    return path;
}

function getSelectedObjectIds(objtype) {
    var selectedIds = [];
    $('#objlisttab').selectedrows().each(function (idx, selectedrow)
    {
        if (selectedrow.dataset.objt === objtype) {
            selectedIds.push(getSymbolRef(selectedrow));
        }
    });
    return selectedIds;
}

function selectObject(newSelType, newSelId, path) {
    if ((newSelType != pivotType) || (newSelId != pivotId)) {
        // update view
        pivotType = newSelType;
        pivotId = newSelId;
        // notify vscode extension (to view the corresponding outline)
        updatePivotObject(path);
    }
}

function objclick(item) {
    selectObject(item.dataset.objt, item.dataset.objid, getSymbolRef(item));
}

function treeViewClick() {
    vscodeContext.postMessage({
        command: 'showTreeView'
    });    
}

$(document).keydown(function(evt)
{
    // Disable text-select from ctrl+A.
	if (evt.ctrlKey) {          
		if (evt.keyCode == 65) {                         
			evt.preventDefault();
			return false;
		}            
	}
});
