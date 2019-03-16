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
var selType = '';
var selId = 0;
var objdata = {};

var filterType = [];
var filterTypeActive = false;
var filterId = undefined;
var filterIdExpr = undefined;
var filterIdActive = false;
var filterName = undefined;
var filterNameExpr = undefined;
var filterNameActive = false;
var filterActive = false;

function processMessage(msg_data) {
    if (event.data.msgtype == 'objectsLoaded') {
        processObjectsLoadedMessage(msg_data);
    }
    else if (event.data.msgtype == 'filterObjects') {
        processFilterObjectsMessage(msg_data);
    }
}

function processObjectsLoadedMessage(msg_data) {
    objdata = msg_data.data;

    setMode('Table', true);
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

    filterTypeActive = (filterType !== undefined && filterType.length > 0);
    filterIdActive = filterId !== undefined;
    filterNameActive = filterName !== undefined;
    filterActive = filterTypeActive || filterIdActive || filterNameActive;

    applyObjectFilters();

    renderData();
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
            var objects = objdata.objectCollections[i].objects;
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
        var objects = objdata.objectCollections[i].objects;
        for (var j=0; j<objects.length; j++) {
            var inIdFilter = !filterIdActive || filterId({INT: objects[j].OId});
            if (!inIdFilter) {
                objects[j].objvisible = false;
                continue;
            }

            var inNameFilter = !filterNameActive || filterName({TEXT: objects[j].Name});
            if (!inNameFilter) {
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

    filterTypeActive = false;
    filterIdActive = false;
    filterNameActive = false;
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
    $("tr[data-objt='" + selType + "'][data-objid='" + selId + "']").attr('class', 'objsel');

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
                execObjCommand($(this).data("objt"), $(this).data("objid"), key);
            },
            items: {
                /*
                "definition": {name: "Go to Definition"},
                "sep1": "---------",
                */
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

function execObjCommand(objtype, objid, cmdname) {
    if (vscodeContext) {
        vscodeContext.postMessage({
            command : 'execObjCommand',
            objtype : objtype,
            objid : objid,
            cmdname : cmdname});
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

function selectObject(newSelType, newSelId) {
    if ((newSelType != selType) || (newSelId != selId)) {
        // //update view
        // $("tr[data-objt='" + selType + "'][data-objid='" + selId + "']").attr('class', '');
        // selType = newSelType;
        // selId = newSelId;
        // $("tr[data-objt='" + selType + "'][data-objid='" + selId + "']").attr('class', 'objsel');
        // //notify vscode extension
        // execObjCommand(selType, selId, "selected");
    }
}

function objclick(item) {
    selectObject(item.dataset.objt, item.dataset.objid);
}