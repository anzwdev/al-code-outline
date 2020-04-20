class ObjectBrowser {

    constructor() {
        this._mode = 'Table';
        this._vscode = acquireVsCodeApi();
        this._objList = new AZGridView('objects', [
            {name:'type', caption:'Type', appFilter:this._mode, data:'objt', style: 'width:100px;'},
            {name:'id', caption:'Id', isNumVal:true, style: 'width:100px;' },
            {name:'name', caption:'Name', style: 'width:auto;'},
            {name:'library', caption:'Library', style: 'width:auto;'}
        ], 'tableModeBtn', '', 'Loading objects, please wait...');
        
        this._objList.onClipboardCopy = (() => {
            me.copySelected();
        });

        this._objList.setOnDblClickHandler(() => {
            let sel = this._objList.getSelected();
            if ((sel) && (sel.length > 0))
                this.sendMessage({
                    command : 'definition',
                    path : sel[0].path,
                    selpaths: [sel[0].path]}); 
        });

        this.updateModeButtonStyle(true);
        
        // Handle messages sent from the extension to the webview
        var me = this;
        window.addEventListener('message', event => {
            me.onMessage(event.data);
        });

        //prevent standard Ctrl+A inside tree elements
        $('body').on('keydown', '#objects', function(evt)
        {
            if (evt.ctrlKey) {
                switch (evt.keyCode) {
                    case 65:
                        evt.preventDefault();
                        me._objList.selectAll();
                        return false;
                    case 67:
                        evt.preventDefault();
                        me.copySelected();
                        return false;
                }
            }
            return;
        });

        this.initModeBtnEventHandlers('tableModeBtn', 'Table', '', 'pageModeBtn');
        this.initModeBtnEventHandlers('pageModeBtn', 'Page', 'tableModeBtn', 'reportModeBtn');
        this.initModeBtnEventHandlers('reportModeBtn', 'Report', 'pageModeBtn', 'xmlportModeBtn');
        this.initModeBtnEventHandlers('xmlportModeBtn', 'XmlPort', 'reportModeBtn', 'queryModeBtn');
        this.initModeBtnEventHandlers('queryModeBtn', 'Query', 'xmlportModeBtn', 'codeunitModeBtn');
        this.initModeBtnEventHandlers('codeunitModeBtn', 'Codeunit', 'queryModeBtn', 'contoladdinModeBtn');
        this.initModeBtnEventHandlers('contoladdinModeBtn', 'ControlAddIn', 'codeunitModeBtn', 'pageextensionModeBtn');
        this.initModeBtnEventHandlers('pageextensionModeBtn', 'PageExtension', 'contoladdinModeBtn', 'tableextensionModeBtn');
        this.initModeBtnEventHandlers('tableextensionModeBtn', 'TableExtension', 'pageextensionModeBtn', 'profileModeBtn');
        this.initModeBtnEventHandlers('profileModeBtn', 'Profile', 'tableextensionModeBtn', 'pagecustomizationModeBtn');
        this.initModeBtnEventHandlers('pagecustomizationModeBtn', 'PageCustomization', 'profileModeBtn', 'enumModeBtn');
        this.initModeBtnEventHandlers('enumModeBtn', 'Enum', 'pagecustomizationModeBtn', 'dotnetpackageModeBtn');
        this.initModeBtnEventHandlers('dotnetpackageModeBtn', 'DotNetPackage', 'enumModeBtn', 'interfaceModeBtn');
        this.initModeBtnEventHandlers('interfaceModeBtn', 'Interface', 'dotnetpackageModeBtn', 'allModeBtn');
        this.initModeBtnEventHandlers('allModeBtn', 'All', 'interfaceModeBtn', '');        

        document.getElementById('searchtype').addEventListener('keydown', event => {
            me.onSearchKeyDown(event);
        });

        document.getElementById('searchid').addEventListener('keydown', event => {
            me.onSearchKeyDown(event);
        });

        document.getElementById('searchname').addEventListener('keydown', event => {
            me.onSearchKeyDown(event);
        });

        document.getElementById('searchbtn').addEventListener('click', event => {
            me.search();
        });

        document.getElementById('treebtn').addEventListener('click', event => {
            me.sendMessage({
                command: 'showTreeView'
            });
        });

        this._objList.onCurrRowChanged = function(data) {
            me.sendMessage({
                command: 'currRowChanged',
                path: data.path
            });
        }

        this.initContextMenus();

        this.sendMessage({
            command: 'documentLoaded'
        });
    }

    copySelected() {
        this.execObjCommand("copysel", undefined, ALSymbolKind.Undefined);
    }

    initModeBtnEventHandlers(buttonId, mode, prevId, nextId) {
        let me = this;
        
        document.getElementById(buttonId).addEventListener('keydown', event => {
            me.onModeBtnKeyDown(event, mode, prevId, nextId);
        });

        document.getElementById(buttonId).addEventListener('click', event => {
            me.setMode(mode);
        });


    }

    onMessage(message) {     
        switch (message.command) {
            case 'setData':
                this.setData(message.data, message.showLibraries);
                break;
        }
    }

    setMode(newMode) {
        if (this._mode != newMode) {
            this.updateModeButtonStyle(false);
            this._mode = newMode;
            this.updateModeButtonStyle(true)

            if (this._mode == "All")
                this._objList._columns[0].appFilter = undefined;
            else
                this._objList._columns[0].appFilter = this._mode;
            this._objList.renderData();
        }
    }

    updateModeButtonStyle(selected) {
        if (this._mode) {
            if (selected)
                $("div[data-mode='" + this._mode + "']").attr('class', 'btn btnsel');    
            else
                $("div[data-mode='" + this._mode + "']").attr('class', 'btn btnstd');
        }
    }

    setData(data, showLibraries) {
        this._data = data;
        if (this._data) {
            for (let i=0; i<this._data.length; i++) {
                this._data[i].dataidx = i;
                this._data[i].type = this.symbolKindToObjectType(this._data[i].kind);
            }
            this.sortData();
        }

        this._objList._columns[3].hidden = !showLibraries; 
        //this._objList.renderTable();

        this._objList.setData(this._data);
    }

    sortData() {
        //init sorting
        let typeSort = [];
        typeSort[ALSymbolKind.TableObject] = 1;
        typeSort[ALSymbolKind.PageObject] = 2;
        typeSort[ALSymbolKind.ReportObject] = 3;
        typeSort[ALSymbolKind.XmlPortObject] = 4;
        typeSort[ALSymbolKind.QueryObject] = 5;
        typeSort[ALSymbolKind.CodeunitObject] = 6;
        typeSort[ALSymbolKind.ControlAddInObject] = 7;
        typeSort[ALSymbolKind.PageExtensionObject] = 8;
        typeSort[ALSymbolKind.TableExtensionObject] = 9;
        typeSort[ALSymbolKind.ProfileObject] = 10;
        typeSort[ALSymbolKind.PageCustomizationObject] = 11;
        typeSort[ALSymbolKind.EnumType] = 12;
        typeSort[ALSymbolKind.DotNetPackage] = 13;
        typeSort[ALSymbolKind.Interface] = 14;

        if ((this._data) && (this._data.length > 1)) {
            this._data.sort(function(a,b) {
                if (typeSort[a.kind] < typeSort[b.kind])
                    return -1;
                if (typeSort[a.kind] > typeSort[b.kind])
                    return 1;
                if (a.id < b.id)
                    return -1;
                if (a.id > b.id)
                    return 1;
                if (a.name < b.name)
                    return -1;
                if (a.name > b.name)
                    return 1;
                return 0;                    
            });
        }
    }    

    symbolKindToObjectType(kind) {
        switch (kind)
        {
            case ALSymbolKind.TableObject:
                return "Table";
            case ALSymbolKind.PageObject:
                return "Page";
            case ALSymbolKind.ReportObject:
                return "Report";
            case ALSymbolKind.XmlPortObject:
                return "XmlPort";
            case ALSymbolKind.QueryObject:
                return "Query";
            case ALSymbolKind.CodeunitObject:
                return "Codeunit";
            case ALSymbolKind.ControlAddInObject:
                return "ControlAddIn";
            case ALSymbolKind.PageExtensionObject:
                return "PageExtension";
            case ALSymbolKind.TableExtensionObject:
                return "TableExtension";
            case ALSymbolKind.ProfileObject:
                return "Profile";
            case ALSymbolKind.PageCustomizationObject:
                return "PageCustomization";
            case ALSymbolKind.EnumType:
                return "Enum";
            case ALSymbolKind.DotNetPackage:
                return "DotNetPackage";
            case ALSymbolKind.Interface:
                return "Interface";
            default:
                return "Undefined";
        }
    }

    initContextMenus() {  
        let me = this; 
        $(function(){
            $('#objects').contextMenu({
                selector: 'tr', 
                callback: function(key, options) {
                    let kind = $(this).data("objt");
                    if (key == "copysel")
                        kind = ALSymbolKind.Undefined;
                    me.execObjCommand(key, $(this)[0], kind);
                },
                items: {
                    "definition": {name: "Go to definition"},
                    "shownewtab": {name: "Open symbol in new Tab"},
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
                    "sep4": "---------",
                    "copysel": {
                        name: "Copy Selected"
                    }
                }
            });
        });
    }    

    modeBtnClick(btn) {
        this.setMode(btn.dataset.mode, false);
    }

    sendMessage(data) {
        this._vscode.postMessage(data);    
    }

    search() {
        let typeList = [];
        let typeElem = document.getElementById('searchtype');
        let options = typeElem.options;
        for (let i=0; i<options.length; i++) {
            if (options[i].selected)
                typeList.push(options[i].value);
        }

        this._objList._columns[0].userFilterArray = typeList;
        this._objList._columns[1].userFilter = $('#searchid').val();
        this._objList._columns[2].userFilter = $('#searchname').val();
        this._objList.compileFilters();
        this._objList.renderData();
    }

    onModeBtnKeyDown(e, mode, prevId, nextId) {
        let handled = false;

        switch (e.which) {
            case 32:
            case 13:
                this.setMode(mode);
                handled = true;
                break;
            case 39:
                document.getElementById('objects').focus();
                handled = true;
                break;
            case 38:    //up
                if (prevId)
                    document.getElementById(prevId).focus();
                handled = true;
                break;
            case 40:    //down
                if (nextId)
                    document.getElementById(nextId).focus();
                handled = true;
                break;
        }

        if (handled) {
            e.preventDefault();
            return false;
        }
        
    }

    onSearchKeyDown(e) {
        let handled = false;

        switch (e.which) {
            case 13:
                this.search();
                handled = true;
                break;
        }

        if (handled) {
            e.preventDefault();
            return false;
        }
    }

    execObjCommand(cmdname, selectedrow, objtype) {
        //merge path with selectedpaths
        let selFound = false;        
        let mainItem = undefined;
        let items = this._objList.getSelected();
        let selPaths = [];
        if (selectedrow)
            mainItem = selectedrow.tabData;        

        if ((items) && (items.length > 0)) {
            if (!mainItem)
                mainItem = items[0];

            for (let i=0; i<items.length; i++) {
                if ((items[i].type == objtype) || (objtype == ALSymbolKind.Undefined)) {
                    if (items[i].dataidx == mainItem.dataidx)
                        selFound = true;
                    selPaths.push(items[i].path);
                }
            }            
        }    

        if (mainItem) {
            if (!selFound)
                selPaths.push(mainItem.path);

            this.sendMessage({
                command : cmdname,
                path : mainItem.path,
                selpaths: selPaths});        
        }
    }

}

var objectBrowser;

window.onload = function() {
    $('#searchtype').multiselect({
        selectAll: true,
        maxPlaceholderOpts: 1,
        texts: {
            placeholder: 'Object type filter',
            selectedOptions: ' types selected'
        }
    });

    objectBrowser = new ObjectBrowser();
};
