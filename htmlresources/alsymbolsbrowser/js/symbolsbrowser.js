class ObjectBrowser {

    constructor() {
        let that = this;

        this._vscode = acquireVsCodeApi();

        this._objTree = new SymbolsTreeControl('objects', 'objidsbtn', true);
        this._symTree = new SymbolsTreeControl('symbols', 'symidsbtn', false);
        this._symTree.enableSimpleFilter('selobjfilter', 'selobjfilterbtn');

        //prevent standard Ctrl+A inside tree elements
        $('body').on('keydown', '.symbolscont', function(evt)
        {
            if ((!evt.ctrlKey) || (evt.keyCode !== 65))
                return;
            evt.preventDefault();
            return false;
        });
        // Prevent text select from shift + click inside tree elements 
        $('body').on('mousedown', '.symbolscont', function(evt)
        {
            if (evt.shiftKey || evt.ctrlKey) {
                evt.preventDefault();
            }
        });

        this._objTree.emptyContent = 'There is nothing to show.';
        this._objTree.nodeSelected = function(node) { 
            that.onNodeSelected(node); 
        };
        this._objTree.showIdsChanged = function(newVal) {
            this.sendMessage({
                command: 'objTreeShowIdsChanged',
                value: newVal 
            });
        };

        this._symTree.showIdsChanged = function(newVal) {
            this.sendMessage({
                command: 'symTreeShowIdsChanged',
                value: newVal 
            });
        }

        this.initializeContextMenu();

        //initialize splitter
        Split(['#olpanel', '#odpanel'], {
            minSize: 0,
            gutter: function (index, direction) {
                var gutter = document.createElement('div')
                gutter.className = 'gutter gutter-' + direction
                return gutter
            },
            gutterSize: 6
        })

        // Handle messages sent from the extension to the webview
        var me = this;
        window.addEventListener('message', event => {
            me.onMessage(event.data);
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

        document.getElementById('listbtn').addEventListener('click', event => {
            me.sendMessage({
                command: 'showlist'
            });
        });

        this.sendMessage({
            command: 'documentLoaded'
        });
    }

    initializeContextMenu() {
        let browser = this;
        
        //object list context menu
        $('#objects').contextMenu({
            selector: '.shead', 
            callback: function(key, options) {
                if (key === "expandcollapse") {
                    browser._objTree.toggleChildNodes(this);
                }
                else {
                    browser.sendMessage({
                        command : key,
                        path : browser._objTree.getNodePath(this),
                        selpaths : browser._objTree.getSelectedPaths(this),
                        uid : $(this).data('uid'),
                        kind : $(this).data('kind')
                    });
                }
            },
            items: {
                "definition": {name: "Go to definition"},
                "shownewtab": {name: "Open symbol in new tab"},
                "sep1": "---------",
                "runinwebclient": {
                    name: "Run in Web Client",
                    disabled: function(key, opt) {
                        let kind = Number($(this).data("kind")); 
                        return ((kind != ALSymbolKind.TableObject) && (kind != ALSymbolKind.PageObject) && (kind != ALSymbolKind.ReportObject));
                    }},
                "sep2": "---------",
                "newcardpage": {
                    name: "New Card Page",
                    disabled: function(key, opt) {
                        return (Number($(this).data("kind")) != ALSymbolKind.TableObject);
                    }},
                "newlistpage": {
                    name: "New List Page",
                    disabled: function(key, opt) {
                        return (Number($(this).data("kind")) != ALSymbolKind.TableObject);
                    }},
                "newreport": {
                    name: "New Report",
                    disabled: function(key, opt) {
                        return (Number($(this).data("kind")) != ALSymbolKind.TableObject);
                    }},
                "newxmlport": {
                    name: "New Xml Port",
                    disabled: function(key, opt) {
                        return (Number($(this).data("kind")) != ALSymbolKind.TableObject);
                    }},
                "newquery": {
                    name: "New Query",
                    disabled: function(key, opt) {
                        return (Number($(this).data("kind")) != ALSymbolKind.TableObject);
                    }},
                "sep3": "---------",
                // New Extension Objects
                "extendtable": {
                    name: "New Table Extension",
                    disabled: function(key, opt) {
                        return (Number($(this).data("kind")) != ALSymbolKind.TableObject);
                    }},
                "extendpage": {
                    name: "New Page Extension",
                    disabled: function(key, opt) {
                        return (Number($(this).data("kind")) != ALSymbolKind.PageObject);
                    }},
                "sep4": "---------",
                "expandcollapse": {name: "Expand/collapse child nodes"},
                }
        });

        //object symbols context menu
        $('#symbols').contextMenu({
            selector: '.shead', 
            callback: function(key, options) {
                if (key === "expandcollapse") {
                    browser._symTree.toggleChildNodes(this);
                }
            },
            items: {
                "expandcollapse": {name: "Expand/collapse child nodes"},
            }
        });


    }

    onMessage(message) {     
        switch (message.command) {
            case 'setSearch':
                $('#searchname').val(message.data);
                break;
            case 'setData':
                this._objTree.setData(message.data);
                this._symTree.setData(undefined);
                break;
            case 'setSelObjData':
                this._symTree.setData(message.data);
                break;
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);    
    }

    onNodeSelected(node) {        
        if (node) {
            var uid = $(node).data('uid');
            if ((uid === undefined) || (Number(uid) < 0))
                this._symTree.setData(undefined);
            else
                this.sendMessage({
                    command: 'objselected',
                    path: this.getNodePath(node),
                    uid: uid
                });
        }
    }

    getNodePath(node) {
        let items = $(node).parents('.treeitem');
        let path = [];
        if ((items) && (items.length)) {
            for (let pid = 0; pid<items.length;pid++) {
                path.push($(items[pid]).data('idx'));
            }
        }
        return path;
    }

    search() {        
        let typeList = [];
        let typeElem = document.getElementById('searchtype');
        let options = typeElem.options;
        for (let i=0; i<options.length; i++) {
            if (options[i].selected)
                typeList.push(this.objectTypeToSymbolKind(options[i].value));
        }


        this._objTree.filterData(typeList, $('#searchid').val(), $('#searchname').val());
    }

    objectTypeToSymbolKind(name) {
        switch (name)
        {
            case "Table":
                return ALSymbolKind.TableObject;
            case "Page":
                return ALSymbolKind.PageObject;
            case "Report":
                return ALSymbolKind.ReportObject;
            case "XmlPort":
                return ALSymbolKind.XmlPortObject;
            case "Query":
                return ALSymbolKind.QueryObject;
            case "Codeunit":
                return ALSymbolKind.CodeunitObject;
            case "ControlAddIn":
                return ALSymbolKind.ControlAddInObject;
            case "PageExtension":
                return ALSymbolKind.PageExtensionObject;
            case "TableExtension":
                return ALSymbolKind.TableExtensionObject;
            case "Profile":
                return ALSymbolKind.ProfileObject;
            case "PageCustomization":
                return ALSymbolKind.PageCustomizationObject;
            case "Enum":
                return ALSymbolKind.EnumType;
            case "DotNetPackage":
                return ALSymbolKind.DotNetPackage;
            default:
            	return ALSymbolKind.Undefined;
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

}

