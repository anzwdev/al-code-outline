class ObjectBrowser {

    constructor() {
        let that = this;

        this._vscode = acquireVsCodeApi();

        this._objTree = new TreeControl('objects', 'objidsbtn', true);
        this._symTree = new TreeControl('symbols', 'symidsbtn', false);

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
        this._objTree.onNodeSelected = function(node) { 
            that.onNodeSelected(node); 
        };
        this._objTree.onShowIdsChanged = function(newVal) {
            this.sendMessage({
                command: 'objTreeShowIdsChanged',
                value: newVal 
            });
        };

        this._symTree.onShowIdsChanged = function(newVal) {
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

        $('#objects').contextMenu({
            selector: '.shead', 
            callback: function(key, options) {
                browser.sendMessage({
                    command : key,
                    path : browser._objTree.getNodePath(this),
                    selpaths : browser._objTree.getSelectedPaths(this),
                    uid : $(this).data('uid'),
                    kind : $(this).data('kind')
                });
            },
            items: {
                "definition": {name: "Go to Definition"},
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
        this._objTree.filterData($('#searchid').val(), $('#searchname').val());
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

