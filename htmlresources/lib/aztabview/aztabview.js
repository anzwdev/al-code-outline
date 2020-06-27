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
        
        this._tabsContainer.tabIndex = 1;
        this._tabsContainer.addEventListener('keydown', event => { 
            this.onKeyDown(event); 
        });
    }

    addTab(caption) {
        let tab = {
            caption: caption,
            tabButton: document.createElement('div'),
            tabContent: document.createElement('div')
        }

        tab.index = this._tabs.length;

        tab.tabButton.className = 'tabvtabbtn';
        tab.tabButton.innerText = caption;

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

    appendContentChild(element) {
        if (!this._lastTab)
            this.addTab('General');
        this._lastTab.tabContent.appendChild(element);
    }

    selectTab(idx) {
        if (idx >= this._tabs.length)
            idx = this._tabs.length - 1;
        if (idx < 0)
            idx = 0;

        if ((!this._activeTab) || (this._activeTab.index != idx)) { 
            if (this._activeTab) {
                this._activeTab.tabContent.className = 'tabvcontent';
                this._activeTab.tabButton.className = 'tabvtabbtn';
                this._activeTab.active = false;
            }

            this._activeTab = this._tabs[idx]; 
            this._activeTab.tabContent.className = 'tabvcontent tabvcactive';
            this._activeTab.tabButton.className = 'tabvtabbtn tabvtbactive';
            this._activeTab.active = true;
        }

    }

    goToTab() {
        if ((this._activeTab) && (this.tabContentFocused)) {
            this.tabContentFocused(this._activeTab.index);
        }
    }

    onKeyDown(e, idx) {
        let handled = false;
        switch (e.which) {
            case 37:    //left
                if (this._activeTab)                
                    this.selectTab(this._activeTab.index - 1);
                else
                    this.selectTab(0);
                handled = true;
                break;
            case 39:    //right
                if (this._activeTab)                
                    this.selectTab(this._activeTab.index + 1);
                else
                    this.selectTab(this._tabs.length - 1);
                handled = true;
                break;
            case 13:    //enter
            case 40:    //down
                this.goToTab();
                handled = true;
                break;
        }
        if (handled) {
            e.preventDefault();
            e.stopPropagation();
            return false;
        }

        return true;
    }

    focus() {
        this._tabsContainer.focus();
    }

    noOfTabs() {
        return this._tabs.length;
    }

}