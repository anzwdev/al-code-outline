var vscodeContext;

$(function() {
    //initialize api
    vscodeContext = acquireVsCodeApi();

    window.addEventListener('message', event => {
        processMessage(event.data);
    });

    //initialize context menu
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
                /*
                !!! TO-DO !!!
                "extend": {
                    name: "New Table Extension",
                    disabled: function(key, opt) {
                        return ($(this).data("objt") != "Table");
                    }},
                */
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
            }
        });
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

function processMessage(data) {
    objdata = data.data;

    setMode('Table', true);
}

function setMode(newMode, reload) {
    if ((dataMode == newMode) && (!reload))
        return;

    if (dataMode)
        $("div[data-mode='" + dataMode + "']").attr('class', 'btn btnstd');

    for (var i=0; i<objdata.objectCollections.length; i++) {
        objdata.objectCollections[i].visible = ((newMode == objdata.objectCollections[i].objectType) || (newMode === 'All'));
    }

    dataMode = newMode;
    if (dataMode)
        $("div[data-mode='" + dataMode + "']").attr('class', 'btn btnsel');

    renderData();
}

function renderData() {
    $('#objects').html(Handlebars.templates.objectlist(objdata));
    $("tr[data-objt='" + selType + "'][data-objid='" + selId + "']").attr('class', 'objsel');
}

function modeBtnClick(btn) {
    setMode(btn.dataset.mode, false);
}

function execObjCommand(objtype, objid, cmdname) {
    if (vscodeContext) {
        vscodeContext.postMessage({
            objtype : objtype,
            objid : objid,
            cmdname : cmdname});
    }
}

function selectObject(newSelType, newSelId) {
    if ((newSelType != selType) || (newSelId != selId)) {
        //update view
        $("tr[data-objt='" + selType + "'][data-objid='" + selId + "']").attr('class', '');
        selType = newSelType;
        selId = newSelId;
        $("tr[data-objt='" + selType + "'][data-objid='" + selId + "']").attr('class', 'objsel');
        //notify vscode extension
        execObjCommand(selType, selId, "selected");
    }
}

function objclick(item) {
    selectObject(item.dataset.objt, item.dataset.objid);
}
