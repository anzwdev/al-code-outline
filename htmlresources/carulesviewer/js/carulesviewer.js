class CARulesViewer {

    constructor() {
        this._vscode = acquireVsCodeApi();
        this._rulesTab = new AZGridView('rules', [
            {name:'id', caption:'Id', style: 'width:100px;'},
            {name:'title', caption:'Title' },
            {name:'defaultSeverity', caption:'Severity', style: 'width:100px' }
        ]);
        this._analyzersSel = document.getElementById('analyzers');

        // Handle messages sent from the extension to the webview
        window.addEventListener('message', event => {
            this.onMessage(event.data);
        });

        document.getElementById('analyzers').addEventListener('change', event => {
            this.onAnalyzerChanged();
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
                this._analyzersSel.appendChild(option);
            }

            if (data.length > 0) {
                this._analyzersSel.value = data[0].value;
                this.onAnalyzerChanged();
            }
        }
    }

    setRules(rules) {
        if (!rules)
            rules = [];
        for (let i=0; i<rules.length; i++) {
            rules[i].idx = i;
        }
        this._rulesTab.setData(rules);
    }

    onAnalyzerChanged() {
        this.sendMessage({
            command: 'analyzerselected',
            name: this._analyzersSel.value
        });
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



}