class ImageBrowser {

    constructor() {
        let that = this;

        this._contextMenuInitialized = false;
        this._stylesInitialized = false;
        this._selectedIndex = -1;
        this._selectedDataIndex = -1;
        this._itemStyle = 'image';
        this._selItemStyle = 'image imagesel';
        this._vscode = acquireVsCodeApi();
        this._controlId = 'content';
        this._content = document.getElementById(this._controlId);

        // Handle messages sent from the extension to the webview
        window.addEventListener('message', event => {
            this.onMessage(event.data);
        });

        document.getElementById('searchname').addEventListener('keydown', event => {
            this.onSearchKeyDown(event);
        });

        document.getElementById('searchbtn').addEventListener('click', event => {
            this.search();
        });

        this._content.addEventListener('pointerdown', event => { 
            this.onMouseDown(event); 
        }, true);

        this._content.addEventListener('keydown', event => {
            this.onKeyDown(event); 
        });

        this.sendMessage({
            command: 'documentLoaded'
        });
    }

    initializeContextMenu() {
        let items;
        if (this._withActions)
            items = {
                "copyname": {name: "Copy name"},
                "copyaction": {name: "Copy as action"},
                "copypromotedaction": {name: "Copy as promoted action"}
            };
        else
            items = {
                "copyname": {name: "Copy name"}
            };

        let browser = this;
        $('#' + this._controlId).contextMenu({
            selector: '.image', 
            callback: function(key, options) {
                let idx = $(this).data('idx');                
                let name = browser._data[idx].name;
                browser.sendMessage({
                    command : key,
                    name: name,
                    withui: false
                });
            },
            items: items
        });
        this._contextMenuInitialized = true;
    }

    initializeStyles() {
        this._itemStyle = 'image ' + this._imageStyleType;
        this._selItemStyle = 'image imagesel ' + this._imageStyleType;
        this._stylesInitialized = true;
    }

    onMessage(message) {     
        switch (message.command) {
            case 'setData':
                this.setData(message.data, message.withActions, message.imageStyleType);
                break;
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);    
    }

    setData(data, withActions, imageStyleType) {
        this._selectedIndex = -1;
        this._selectedDataIndex = -1;
        this._data = data;
        this._withActions = withActions;
        this._imageStyleType = imageStyleType;
        if (!this._contextMenuInitialized)
            this.initializeContextMenu();
        if (!this._stylesInitialized)
            this.initializeStyles();
        this.renderData();
    }

    renderData() {
        while (this._content.firstChild) {
            this._content.removeChild(this._content.firstChild);
        }

        this._selectedIndex = -1;
        if ((this._data) && (this._data.length > 0)) {
            let newSelectedIndex = 0;
            let visualIdx = -1;
            for (let i=0; i<this._data.length; i++) {
                if (this.validItem(this._data[i])) {
                    visualIdx++;
                    let item = document.createElement('div');
                    item.className = this._itemStyle;
                    item.dataset['idx'] = i;
                    item.dataset['visidx'] = visualIdx;
                    if (i == this._selectedDataIndex)
                        newSelectedIndex = visualIdx;

                    let img = document.createElement('img');
                    img.src = this._data[i].content;
                    item.appendChild(img);

                    let label = document.createElement('div');
                    label.className = 'name';
                    label.innerText = this._data[i].name;
                    item.appendChild(label);

                    this._content.appendChild(item);
                }
            }
            this.setCurrItem(newSelectedIndex);
        }

    }

    setCurrItem(index) {
        let maxIndex = this._content.children.length - 1;
        if (index < 0)
            index = 0;
        if (index >= maxIndex)
            index = maxIndex;            

        if ((this._selectedIndex >= 0) && (this._selectedIndex <= maxIndex))
            this._content.children[this._selectedIndex].className = this._itemStyle;

        this._selectedIndex = index;

        if ((this._selectedIndex >= 0) && (this._selectedIndex <= maxIndex)) {
            let item = this._content.children[this._selectedIndex];             
            this._selectedDataIndex = parseInt(item.dataset['idx']);
            item.className = this._selItemStyle;
            this.scrollToElement(item);
        } else
            this._selectedDataIndex = -1;
        
    }

    nextPageOrLineItemIndex(nextLineOnly) {
        if (this._selectedIndex < 0)
            return 0;
        if (this._selectedIndex >= (this._content.children.length - 1))
            return this._content.children.length - 1;

        let item = this._content.children[this._selectedIndex];
        let idx = this._selectedIndex;
        
        let targetTop = item.offsetTop - item.offsetHeight + this._content.clientHeight;
        for (let i = this._selectedIndex + 1; i<this._content.children.length; i++) {
            if (this._content.children[i].offsetLeft == item.offsetLeft) {
                idx = i;
                if ((nextLineOnly) || (this._content.children[i].offsetTop >= targetTop))
                    return i;
            }
        }
        return idx;
    }

    prevPageOrLineItemIndex(prevLineOnly) {
        if (this._selectedIndex <= 0)
            return 0;
        if (this._selectedIndex >= this._content.children.length)
            return this._content.children.length - 1;

        let item = this._content.children[this._selectedIndex];
        let idx = this._selectedIndex;

        let targetTop = item.offsetTop + item.offsetHeight - this._content.clientHeight;
        for (let i = this._selectedIndex - 1; i >= 0; i--) {
            if (this._content.children[i].offsetLeft == item.offsetLeft) {
                idx = i;
                if ((prevLineOnly) || (this._content.children[i].offsetTop <= targetTop))
                    return i;
            }
        }
        return idx;
    }


    compileFilters() {
        let nameFilterText = document.getElementById('searchname').value;
        if (nameFilterText)
            this._nameFilter = compileFilter('text', nameFilterText);
        else
            this._nameFilter = undefined;
    }

    validItem(item) {
        if ((this._nameFilter) && (!this._nameFilter({TEXT: item.name})))
            return false;
        return true;
    }

    search() {
        this.compileFilters();
        this.renderData();
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

    onMouseDown(e) {
        //if (e.button == 0) {
            let node = e.target;
            let item = node.closest('.image');
            if (item) {
                this.setCurrItem(parseInt(item.dataset['visidx']));
            }
        //}
    }

    onKeyDown(e) {    
        let handled = false;

        switch (e.which) {
            case 13:    //Enter
                this.copyName();
                handled = true;
                break;
            case 37:    //left
                this.setCurrItem(this._selectedIndex - 1);
                handled = true;
                break;
            case 39:    //right
                this.setCurrItem(this._selectedIndex + 1);
                handled = true;
                break;
            case 38:    //up
                this.setCurrItem(this.prevPageOrLineItemIndex(true));
                handled = true;
                break;
            case 40:    //down
                this.setCurrItem(this.nextPageOrLineItemIndex(true));
                handled = true;
                break;
           case 33:    //page up
                this.setCurrItem(this.prevPageOrLineItemIndex(false));
                handled = true;
                break;
            case 34:    //page down
                this.setCurrItem(this.nextPageOrLineItemIndex(false));
                handled = true;
                break;
            case 36:    //home
                this.setCurrItem(0);
                handled = true;
                break;
            case 35:    //end
                this.setCurrItem(this._content.children.length - 1);
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

    scrollToElement(element) {
        let mainTop = this._content.offsetTop;
        let viewTop = this._content.scrollTop;
        let viewBottom = viewTop + this._content.clientHeight;
        let elementTop = element.offsetTop - mainTop;
        let elementBottom = elementTop + element.offsetHeight;

        if (elementTop < viewTop)
            element.scrollIntoView(true);
        else if (elementBottom > viewBottom)
            element.scrollIntoView(false);
    }

    copyName() {
        if ((this._selectedIndex >= 0) && (this._selectedIndex < this._content.children.length)) {
            let item = this._content.children[this._selectedIndex];
            let idx = parseInt(item.dataset['idx']);
            this.sendMessage({
                command : 'copyname',
                name: this._data[idx].name,
                withui: true
            });                
        }
    }
}