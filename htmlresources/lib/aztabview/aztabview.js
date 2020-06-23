class AZTabView {

    constructor(elementId) {
        this._tabs = [];
        this._elementId = elementId;
        this._mainElement = document.getElementById(elementId);
        this.initContainers();
    }

    initContainers() {
        this._tabsContainer = document.createElement('div');
        this._tabsContainer.className = 'tabvtabs';
        this._mainElement.appendChild(this._tabsContainer);
    }

    addTab(caption) {
        let tab = {
            caption: caption,
            tabButton: document.createElement('button'),
            tabContent: document.createElement('div')
        }

        tab.tabButton.className = 'tabvtabbtn';
        tab.tabButton.innerText = caption;
        tab.index = this._tabs.length;
        this._tabsContainer.appendChild(tab.tabButton);

        tab.tabContent.className = 'tabvcontent';
        this._mainElement.appendChild(tab.tabContent);

        this._tabs.push(tab);
        this._lastTab = tab;

        tab.tabButton.addEventListener('click', event => {
            this.selectTab(tab.index);
        });

        return tab.tabContent;
    }

    selectTab(idx) {
        if (idx >= this._tabs.length)
            idx = this._tabs.length - 1;
        if (idx < 0)
            idx = 0;

        for (let i=0; i<this._tabs.length; i++) {
            if (i != idx) {
                this._tabs[i].tabContent.className = 'tabvcontent';
                this._tabs[i].tabButton.className = 'tabvtabbtn';
                this._tabs[i].active = false;
            }
        }        
        this._tabs[idx].tabContent.className = 'tabvcontent tabvcactive';
        this._tabs[idx].tabButton.className = 'tabvtabbtn tabvtbactive';
        this._tabs[idx].active = true;
    }

    appendContentChild(element) {
        if (!this._lastTab)
            this.addTab('General');
        this._lastTab.tabContent.appendChild(element);
    }

}