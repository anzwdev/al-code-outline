class CARulesViewer {

    constructor() {
        this._vscode = acquireVsCodeApi();
        this._rulesTab = new AZGridView('rules', [
            {name:'id', caption:'Id', style: 'width:100px;'},
            {name:'title', caption:'Title' },
            {name:'defaultSeverity', caption:'Severity', style: 'width:100px' },
            {name:'analyzer', caption:'Analyzer', style: 'width:100px' }
        ]);
        this._rulesTab.clipboardEnabled = true;
        this._rulesTab.onClipboardCopy = (() => {
            this.execRuleCommand('copytable', undefined);
        });

        this._analyzersSel = document.getElementById('searchanalyzers');
        this._severitySel = document.getElementById('searchseverity');

        $('#searchseverity').multiselect({
            selectAll: true,
            maxPlaceholderOpts: 1,
            texts: {
                placeholder: 'Severity filter',
                selectedOptions: ' selected'
            }
        });      

        // Handle messages sent from the extension to the webview
        window.addEventListener('message', event => {
            this.onMessage(event.data);
        });

        document.getElementById('searchbtn').addEventListener('click', event => {
            this.search(true);
        });

        document.getElementById('searchanalyzers').addEventListener('keydown', event => {
            this.onSearchKeyDown(event);
        });
        document.getElementById('searchseverity').addEventListener('keydown', event => {
            this.onSearchKeyDown(event);
        });
        document.getElementById('searchid').addEventListener('keydown', event => {
            this.onSearchKeyDown(event);
        });
        document.getElementById('searchtitle').addEventListener('keydown', event => {
            this.onSearchKeyDown(event);
        });

        this.initContextMenu();

        this.sendMessage({
            command: 'documentLoaded'
        }); 
    }

    onMessage(message) {     
        switch (message.command) {
            case 'setAnalyzers':
                this.setAnalyzers(message.data);
                break;
            case 'setRules':
                this.setRules(message.data);
                break;
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);    
    }

    setAnalyzers(data) {
        while (this._analyzersSel.firstChild) {
            this._analyzersSel.removeChild(this._analyzersSel.firstChild);
        }

        if (data) {
            for (let i=0; i<data.length; i++) {
                let option = document.createElement('option');
                option.label = data[i].label;
                option.value = data[i].value;
                option.innerText = data[i].label;
                option.selected = data[i].selected
                this._analyzersSel.appendChild(option);
            }
        }

        $('#searchanalyzers').multiselect({
            selectAll: true,
            maxPlaceholderOpts: 1,
            texts: {
                placeholder: 'Analyzer filter',
                selectedOptions: ' selected'
            }
        });

    }

    setRules(rules) {
        if (!rules)
            rules = [];
        //keep item source index
        for (let i=0; i<rules.length; i++) {
            rules[i].idx = i;
        }
        //sort items
        rules.sort((a,b) => {
            if (a.id < b.id) return -1;
            if (a.id > b.id) return 1;
            return 0;
        });
        //filter items
        this.search(false);
        this._rulesTab.setData(rules);
    }

    initContextMenu() {  
        let me = this; 
        $('#rules').contextMenu({
            selector: 'tr', 
            callback: (key, options) => {
                this.execRuleCommand(key, $(this)[0]);
            },
            items: {
                "newruleset": {name: "New Ruleset File"},
                "copyrules": {name: "Copy as Rules"},
                "copytable": {name: "Copy as Table"}
            }
        });
    }    

    execRuleCommand(cmdname, selectedrow) {
        //merge path with selectedpaths
        let selFound = false;        
        let mainItem = undefined;
        
        let items = this._rulesTab.getSelected();
        let selRules = [];
        if (selectedrow)
            mainItem = selectedrow.tabData;        

        if ((items) && (items.length > 0)) {
            if (!mainItem)
                mainItem = items[0];

            for (let i=0; i<items.length; i++) {
                if (items[i].idx == mainItem.idx)
                    selFound = true;
                selRules.push(items[i].idx);
            }            
        }    

        if (mainItem) {
            if (!selFound)
                selRules.push(mainItem.idx);

            this.sendMessage({
                command : cmdname,
                mainrule : mainItem.idx,
                selrules: selRules});        
        }
    }

    onSearchKeyDown(e) {
        let handled = false;

        switch (e.which) {
            case 13:
                this.search(true);
                handled = true;
                break;
        }

        if (handled) {
            e.preventDefault();
            return false;
        }
    }

    search(render) {
        let sevList = this.getSelOptionsList('searchseverity');
        let analyzersList = this.getSelOptionsList('searchanalyzers');

        this._rulesTab._columns[0].userFilter = document.getElementById('searchid').value;
        this._rulesTab._columns[1].userFilter = document.getElementById('searchtitle').value;
        this._rulesTab._columns[2].userFilterArray = sevList;
        this._rulesTab._columns[3].userFilterArray = analyzersList;
        this._rulesTab.compileFilters();
        if (render)
            this._rulesTab.renderData();
    }

    getSelOptionsList(id) {
        let selList = [];
        let selElem = document.getElementById(id);
        let options = selElem.options;
        for (let i=0; i<options.length; i++) {
            if (options[i].selected)
                selList.push(options[i].value);
        }
        return selList;
    }

}