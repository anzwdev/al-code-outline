class SymbolsTreeControl {

    constructor(controlId, idsBtnId, collapsed) {
        //initialize variables
        this._controlId = controlId;
        this._controlSelector = '#' + this._controlId;
        this._collapsed = collapsed;
        this._selNode = undefined;
        this._selSymbol = undefined;
        this._idsBtnId = idsBtnId;
        this._showIds = true;
        this.showIcons = true;
        this.sortNodes = true;
        this.emptyContent = '';
        this.resetCollapsedState = true;
        this.nodeSelected = undefined;
        this.showIdsChanged = undefined;
        this.nodeDefaultAction = undefined;
        this.syntaxTreeMode = false;

        this._multiselect = false;
        this._idFilterText = undefined;
        this._idFilter = undefined;
        this._nameFilterText = undefined;
        this._nameFilter = undefined;
        this._fullNameFilterText = undefined;
        this._fullNameFilter = undefined;

        this._visibleSymbolList = undefined;
        this._alNodes = true;

        //initialize event listeners
        let element = document.getElementById(controlId);
        let that = this;
        element.addEventListener('click', function(e) { that.onClick(e); }, true);
        element.addEventListener('keydown', function(e) { that.onKeyDown(e); });

        if (this._idsBtnId) {
            let idsBtn = document.getElementById(this._idsBtnId);
            idsBtn.addEventListener('click', function(e) { that.onIdsBtnClick(e); });
        }
    }

    enableSimpleFilter(filterId, filterBtnId) {
        this._filterId = filterId;
        this._filterBtnId = filterBtnId;
        let that = this;
        let element = document.getElementById(this._filterBtnId);
        if (element)
            element.addEventListener('click', function(e) { that.onSimpleFilter(e); });
        element = document.getElementById(this._filterId);
        if (element)
            element.addEventListener('keydown', function(e) { that.onSimpleFilterKeyDown(e); });
        }

    setShowIds(newShowIds) {        
        this._showIds = newShowIds;
        if (this._idsBtnId) {
            if (this._showIds)
                document.getElementById(this._idsBtnId).innerText = "Hide Ids";
            else
                document.getElementById(this._idsBtnId).innerText = "Show Ids";
        }
        this.setData(this._data);
        if (this.showIdsChanged)
            this.showIdsChanged(this._showIds);
    }

    setData(data) {
        this._alNodes = true;
        this._selNode = undefined;
        this._selSymbol = undefined;
        this._data = data;
        if (this._data) {
            this._alNodes = this.hasALNodes(data);
            this._data.showIds = this._showIds;
            if (this.sortNodes)
                this.sortData(this._data);
            //this.resetFilter(this._data);
            this._data.parent = undefined;
            this.prepareData(this._data);
        }

        this.refreshFilter();
    }

    hasALNodes(node) {
        if (this.isObjectSymbol(node))
            return true;
        if (node.childSymbols) {
            for (let i=0; i < node.childSymbols.length; i++)
                if (this.hasALNodes(node.childSymbols[i]))
                    return true;
        }
        return false;
    }

    prepareData(data) {
        if (this.resetCollapsedState)
            data.collapsed = this._collapsed;
        data.selected = false;
        if (data.childSymbols) {
            for (let i=0; i<data.childSymbols.length; i++) {
                data.childSymbols[i].parent = data;
                this.prepareData(data.childSymbols[i]);
            }
        }
    }

    //#region Rendering

    renderData() {
        let element = document.getElementById(this._controlId);
        while (element.firstChild) {
            element.removeChild(element.firstChild);
        }
        this._visibleSymbolList = [];
        if (this._data)
            this.appendDataHtml(element, this._data);
        else
            element.innerText = this.emptyContent;
    }

    appendDataHtml(parent, data) {
        if (data) {
            if (data.visible) {            
                let main = document.createElement('div');
                main.className = "treeitem";
                main.dataset.idx = data.idx;

                let shead = document.createElement('div');
                shead.className = this.getSymbolCssClass(data);
                shead.dataset.uid = data.uid;
                shead.dataset.kind = data.kind;
                shead.alsymbolnode = data;
                data.htmlNode = shead;
                data.visualidx = this._visibleSymbolList.length;
                this._visibleSymbolList.push(data);

                if (this.showIcons) {
                    let icon = document.createElement('div');
                    icon.className = "ico ico-" + data.icon;
                    shead.appendChild(icon);
                }

                let cap = document.createElement('div');
                cap.className = "cap";
                if ((this._showIds) && (data.id))
                    cap.innerText  = data.id + ' ' + data.fullName;
                else
                    cap.innerText = data.fullName;
                shead.appendChild(cap);

                main.appendChild(shead);
                
                let treeList = document.createElement('div');
                treeList.className = "treelist";
                if (data.collapsed)
                    treeList.style = 'display:none';

                if (data.childSymbols) {
                    for (let i=0; i<data.childSymbols.length; i++) {
                        this.appendDataHtml(treeList, data.childSymbols[i]);
                    }
                }
                main.appendChild(treeList);
                
                parent.appendChild(main);
            } else {
                this.detachHtmlElement(data);
            }
        }
    }

    getSymbolCssClass(symbol) {
        if ((!symbol) || (!symbol.childSymbols) || (symbol.childSymbols.length == 0))
            return  "shead sheadnoexp";
        if (symbol.collapsed)
            return "shead sheadcol";
        return "shead sheadexp";        
    }

    detachHtmlElement(data) {
        data.htmlNode = undefined;
        if (data.childSymbols) {
            for (let i=0; i<data.childSymbols.length; i++) {
                this.detachHtmlElement(data.childSymbols[i]);
            }                
        }
    }    

    //#endregion

    isObjectSymbol(symbol) {
        return ((symbol) && ((symbol.kind == ALSymbolKind.TableObject) ||
            (symbol.kind == ALSymbolKind.CodeunitObject) ||
            (symbol.kind == ALSymbolKind.PageObject) ||
            (symbol.kind == ALSymbolKind.ReportObject) ||
            (symbol.kind == ALSymbolKind.QueryObject) ||
            (symbol.kind == ALSymbolKind.XmlPortObject) ||
            (symbol.kind == ALSymbolKind.TableExtensionObject) ||
            (symbol.kind == ALSymbolKind.PageExtensionObject) ||
            (symbol.kind == ALSymbolKind.ControlAddInObject) ||
            (symbol.kind == ALSymbolKind.ProfileObject) ||
            (symbol.kind == ALSymbolKind.PageCustomizationObject) ||
            (symbol.kind == ALSymbolKind.EnumType) ||
            (symbol.kind == ALSymbolKind.EnumExtensionType) ||
            (symbol.kind == ALSymbolKind.DotNetPackage) ||
            (symbol.kind == ALSymbolKind.Interface)));
    }

    //#region Node scrolling

    scrollToNode(node) {
        let mainElement = document.getElementById(this._controlId);
        let mainTop = mainElement.offsetTop;
        let viewTop = mainElement.scrollTop;
        let viewBottom = viewTop + mainElement.clientHeight;
        let nodeTop = node.offsetTop - mainTop;
        let nodeBottom = nodeTop + node.offsetHeight;

        if (nodeTop < viewTop)
            node.scrollIntoView(true);
        else if (nodeBottom > viewBottom)
            node.scrollIntoView(false);
    }

    //#endregion

    //#region Sorting

    sortData(data) {       
        if (data && data.childSymbols && (!this.syntaxTreeMode)) {
            //only table objects can have sortable child elements
            if ((this.isObjectSymbol(data)) && (data.kind != ALSymbolKind.Table))
                return;
            
            //check if node child items are sortable - only objects and table fields can be sorted by name or id
            if ((data.childSymbols.length > 1) && ((this.isObjectSymbol(data.childSymbols[0])) || (data.childSymbols[0].kind == ALSymbolKind.Field))) {
                //sort child symbols            
                if (this._showIds)
                    data.childSymbols.sort(function(a,b) {
                        if (a.id < b.id)
                            return -1;
                        if (a.id > b.id)
                            return 1;
                        if (a.fullName < b.fullName)
                            return -1;
                        if (a.fullName > b.fullName)
                            return 1;
                        return 0;                    
                    });
                else
                    data.childSymbols.sort(function(a,b) {
                        if (a.fullName < b.fullName)
                            return -1;
                        if (a.fullName > b.fullName)
                            return 1;
                        return 0;
                    });                  
            }

            //sort each child symbol child nodes
            for (var i=0; i<data.childSymbols.length; i++)
                this.sortData(data.childSymbols[i]);
        }

    }

    //#endregion

    //#region Filtering

    onSimpleFilter(e) {
        let element = document.getElementById(this._filterId);
        let filterValue = undefined;
        if (element)
            filterValue = element.value;
        if (filterValue)
            filterValue = filterValue.replace(/"/g, "");
        this.filterDataAdv(undefined, undefined, undefined, filterValue);
    }

    onSimpleFilterKeyDown(e) {
        if (e.which == 13) {
            this.onSimpleFilter(undefined);
            e.preventDefault();
            return false;
        }
        return true;
    }

    refreshFilter() {
        this.filterDataAdv(this._typeFilter, this._idFilterText, this._nameFilterText, this._fullNameFilterText);
    }

    filterData(typeList, idFilter, nameFilter) {
        this.filterDataAdv(typeList, idFilter, nameFilter, undefined)
    }

    filterDataAdv(typeList, idFilter, nameFilter, fullNameFilter) {
        //set type filter
        this._typeFilter = typeList;        
        //compile id filter
        if (this._idFilterText != idFilter) {
            this._idFilterText = idFilter;
            this._idFilter = undefined;
            if (this._idFilterText) {
                try {
                    this._idFilter = compileFilter('int', this._idFilterText);
                }
                catch (e) {
                    //vscodeContext.postMessage({
                    //    command    : 'errorInFilter',
                    //    message : filterNameExpr});                    
                    //filterName = undefined;
                    //filterNameExpr = undefined;
                }    
            }
        }
        //compile name filter
        if (this._nameFilterText != nameFilter) {
            this._nameFilterText = nameFilter;
            this._nameFilter = undefined;
            if (this._nameFilterText) {
                try {
                    this._nameFilter = compileFilter('text', this._nameFilterText);
                }
                catch (e) {
                    //vscodeContext.postMessage({
                    //    command    : 'errorInFilter',
                    //    message : filterNameExpr});                    
                    //filterName = undefined;
                    //filterNameExpr = undefined;
                }    
            }
        }
        //compile full name filter
        if (this._fullNameFilterText != fullNameFilter) {
            this._fullNameFilterText = fullNameFilter;
            this._fullNameFilter = undefined;
            if (this._fullNameFilterText) {
                try {
                    this._fullNameFilter = compileFilter('text', this._fullNameFilterText);
                }
                catch (e) {
                    //vscodeContext.postMessage({
                    //    command    : 'errorInFilter',
                    //    message : filterNameExpr});                    
                    //filterName = undefined;
                    //filterNameExpr = undefined;
                }    
            }
        }

        //apply filters
        if (this._data) {        
            this.resetFilter(this._data);

            if (!this.syntaxTreeMode) {
                if ((this._typeFilter) && (this._typeFilter.length > 0))
                    this.applyTypeFilter(this._data);
                if (this._idFilter)
                    this.applyIdFilter(this._data);
                if (this._nameFilter)
                    this.applyNameFilter(this._data);
                if (this._fullNameFilter)
                    this.applyFullNameFilter(this._data);

                this.hideEmptyGroups(this._data);
            }
            
            this._data.visible = true;
        }

        this.renderData();
    }

    resetFilter(data) {
        if (data) {
            data.visible = true;
            if (data.childSymbols) {
                for (let i=0; i<data.childSymbols.length;i++)
                    this.resetFilter(data.childSymbols[i]);
            }
        }
    }

    applyTypeFilter(data) {
        if ((data) && (!this.syntaxTreeMode) && (this._alNodes)) {
            if ((this.isObjectSymbol(data)) && (data.kind)) {
                if (this._typeFilter.indexOf(data.kind) < 0)
                    data.visible = false;
            }
            if (data.childSymbols) {
                for (let i=0; i<data.childSymbols.length;i++)
                    this.applyTypeFilter(data.childSymbols[i]);
            }
        }
    }

    applyIdFilter(data) {
        if ((data) && (!this.syntaxTreeMode) && (this._alNodes)) {
            if ((this.isObjectSymbol(data)) && (data.id)) {
                if (!this._idFilter({INT: data.id}))
                    data.visible = false;
            }
            if (data.childSymbols) {
                for (let i=0; i<data.childSymbols.length;i++)
                    this.applyIdFilter(data.childSymbols[i]);
            }
        }
    }

    applyNameFilter(data) {
        if ((data) && (!this.syntaxTreeMode) && (this._alNodes)) {
            if (this.isObjectSymbol(data)) {
                if (!this._nameFilter({TEXT: data.name}))
                    data.visible = false;
            }
            if (data.childSymbols) {
                for (let i=0; i<data.childSymbols.length;i++)
                    this.applyNameFilter(data.childSymbols[i]);
            }
        }
    }

    applyFullNameFilter(data) {
        let visible = false;
        if (data) {
            if (data.childSymbols) {
                for (let i=0; i<data.childSymbols.length;i++) {
                    if (this.applyFullNameFilter(data.childSymbols[i]))
                        visible = true;
                }
            }
        
            if ((!visible) && (this._fullNameFilter({TEXT: data.fullName.replace(/"/g, "")})))
                visible = true;
            
            data.visible = visible;
        }
        return visible;
    }

    hideEmptyGroups(data) {
        if ((data) && (!this.isObjectSymbol(data))) {
            let visibleChild = false;
            if (data.childSymbols) {
                for (let i=0; i<data.childSymbols.length;i++) {
                    this.hideEmptyGroups(data.childSymbols[i]);
                    if (data.childSymbols[i].visible)
                        visibleChild = true;
                }                
            } else if (!this._alNodes)
                visibleChild = data.visible;
            data.visible = visibleChild;
        }

    }

    //#endregion

    //#region Expand/Collapse

    isNodeCollapsed(node) {
        let element = $(node)[0];
        let symbol = element.alsymbolnode;
        if (!symbol || !symbol.collapsed) {
            return false;
        }
        return true;
    }

    toggleChildNodes(node) {
        // Retrieve all child-nodes that can be expanded/collapsed
        let nodeList = $(node).next('.treelist').children('.treeitem').children('.shead').not('.sheadnoexp');
        if (nodeList.length === 0) {
            return;
        }
        
        // Check whether we need to expand or collapse based on the first child
        let firstChildNode = nodeList[0];
        let isFirstChildCollapsed = this.isNodeCollapsed(firstChildNode);

        for (let i = 0; i < nodeList.length; i++) {
            this.expandOrCollapseNode(nodeList[i], !isFirstChildCollapsed);
        }
    }

    toggleNode(node) {
        let isCollapsed = this.isNodeCollapsed(node);
        this.expandOrCollapseNode(node, !isCollapsed);
    }

    expandOrCollapseNode(node, collapse) {
        let element = $(node)[0];
        let symbol = element.alsymbolnode;
        if (symbol) {
            if ((symbol.childSymbols) && (symbol.childSymbols.length > 0)) {
                symbol.collapsed = collapse;
                if (symbol.collapsed)
                    $(node).next('.treelist').hide();
                else
                    $(node).next('.treelist').show();
                node.className = this.getSymbolCssClass(symbol);
            }
        } 
        else {
            $(node).next('.treelist').toggle();
        }
    }

    //#endregion

    //#region Selection

    getVisualIdx(node) {
        let symbol = this.getNodeSymbol(node);
        if (symbol)
            return symbol.visualidx;
        return -1;
    }

    getNodeSymbol(node) {
        if (node)
            return $(node)[0].alsymbolnode;
        return undefined;
    }

    setSelectionState(symbol, selected) {
        if (symbol.selected != selected) {        
            symbol.selected = selected;
            let hasClass = $(symbol.htmlNode).hasClass('selected');        
            if ((selected) && (!hasClass))
                $(symbol.htmlNode).addClass('selected');
            else if ((!selected) && (hasClass))
                $(symbol.htmlNode).removeClass('selected');
        }
    }

    clearSelection(skipMain) {
        let skipIdx = -1;
        if ((skipMain) && (this._selNode))
            skipIdx = this._selNode.visualidx;
        for (let i=0; i<this._visibleSymbolList.length; i++) {
            if (i != skipIdx)
                this.setSelectionState(this._visibleSymbolList[i], false);    
        }
    }

    selectAll() {
        for (let i=0; i<this._visibleSymbolList.length; i++) {
            this.setSelectionState(this._visibleSymbolList[i], true);
        }
    }

    selectRange(fromIdx, toIdx, addToSelection) {
        let maxIdx = this._visibleSymbolList.length - 1;
        if (fromIdx > toIdx) {
            let tmpIdx = fromIdx;
            fromIdx = toIdx;
            toIdx = tmpIdx;
        }
        fromIdx = Math.max(0, Math.min(fromIdx, maxIdx));
        toIdx = Math.max(0, Math.min(toIdx, maxIdx));

        if (addToSelection) {
            for (let i=fromIdx; i<=toIdx; i++) {
                this.setSelectionState(this._visibleSymbolList[i], true);
            }
        } else {
            for (let i=0; i<=this._visibleSymbolList.length; i++) {
                this.setSelectionState(this._visibleSymbolList[i], ((i>=fromIdx) && (i<=toIdx)));
            }
        }
    }

    selectNode(node, ctrlKey, shiftKey, keyboardEvent) {
        let toggleMode = ((ctrlKey) && (!shiftKey));
        let rangeMode = ((!ctrlKey) && (shiftKey));

        let symbol = this.getNodeSymbol(node);

        if ((this._selSymbol) && (symbol) && (this._selSymbol.visualidx == symbol.visualidx))
            return;

        if (rangeMode) {
            if (symbol) {
                let fromIdx = 0;
                if (this._selSymbol)
                    fromIdx = this._selSymbol.visualidx;
                this.selectRange(fromIdx, symbol.visualidx, true);
                if (keyboardEvent) {
                    this._selSymbol = symbol;
                    this._selNode = node;   
                }
            }
        } else {
            this._selSymbol = symbol;
            this._selNode = node;   

            if (!toggleMode)
                this.clearSelection(true);

            if (this._selSymbol)
                this.setSelectionState(this._selSymbol, true);
        }

        if (node)
            this.scrollToNode(node);

        if (this.nodeSelected)
            this.nodeSelected(this._selNode);
    }

    selectSingleNode(node) {
        let element = $(node)[0];
        let symbol = element.alsymbolnode;

        //remove previous selection
        if (this._selSymbol) {
            if (this._selSymbol.visualidx == symbol.visualidx)
                return;
            $(this._selSymbol.htmlNode).removeClass('selected');
        }

        //update current symbol
        this._selSymbol = symbol;
        this._selNode = node;

        //select new symbol
        $(this._selNode).addClass('selected');
    }

    selectNodeByPath(path) {
        if ((path) && (path.length > 0) && (this._data)) {            
            let node = this._data;
            //expand parents and select node
            for (let i=0; ((node) && (i<path.length)); i++) {
                if (node.collapsed)
                    this.toggleNode(node);

                let idx = path.length - (i + 1);

                if ((node.childSymbols) && (path[idx] < node.childSymbols.length) && (path[idx] >= 0))
                    node = node.childSymbols[path[idx]];
                else
                    node = undefined;

                if ((i == (path.length - 1)) && (node) && (node.htmlNode)) {
                    this.selectNode(node.htmlNode, false, false, false);
                    return;
                }
            }
        }
    }

    //#endregion

    //#region Path function

    getNodePath(node) {
        return this.getSymbolPath(this.getNodeSymbol(node));
    }

    getSymbolPath(symbol) {
        let path = [];
        while (symbol) {
            path.push(symbol.idx);
            symbol = symbol.parent;
        }
        return path;
    }

    getSelectedPaths(inclNode) {
        let list = [];
        for (let i=0; i<this._visibleSymbolList.length; i++) {
            if (this._visibleSymbolList[i].selected)
                list.push(this.getSymbolPath(this._visibleSymbolList[i]));
        }
        if (inclNode) {
            let inclSymbol = this.getNodeSymbol(inclNode);
            if ((inclSymbol) && (!inclSymbol.selected))
                list.push(this.getSymbolPath(inclSymbol));
        }
        return list;
    }

    //#endregion

    //#region Node browsing functions

    parentNode(node) {
        if (node) {
            let nodeList = $(node).parent('.treeitem').parents('.treeitem:first').children('.shead');
            if (nodeList.length > 0)
                return nodeList[0];
            return node;
        }
        return this.firstNode();
    }

    prevNode(node) {
        if (node) {
            //prev sibling
            let nodeList = $(node).parent('.treeitem').prev('.treeitem').find('.shead:visible:last');
            if (nodeList.length > 0)
                return nodeList[0];
            else {
                //parent node
                nodeList = $(node).parent('.treeitem').parents('.treeitem:first').children('.shead');
                if (nodeList.length > 0)
                    return nodeList[0];
            }
            return node;
        }
        return this.firstNode();
    }

    nextNode(node) {
        if (node) {
            //first visible child node
            let nodeList = $(node).next('.treelist:visible').children('.treeitem:first').children('.shead');
            if (nodeList.length > 0)
                return nodeList[0];            
            
            //next sibling
            nodeList = $(node).parent('.treeitem').next('.treeitem').children('.shead');
            if (nodeList.length > 0)
                return nodeList[0];

            //next parent
            nodeList = $(node).parent('.treeitem').parents('.treeitem:first');
            let siblingNodeList = nodeList.next('.treeitem');

            while ((nodeList.length > 0) && (siblingNodeList.length == 0)) {
                nodeList = nodeList.parents('.treeitem:first');
                siblingNodeList = nodeList.next('.treeitem');
            }

            if (siblingNodeList.length > 0) {
                nodeList = siblingNodeList.children('.shead');
                if (nodeList.length > 0)
                    return nodeList[0];
            }

            return node;
        }
        return this.firstNode();
    }

    firstNode() {
        let nodeList = $(this._controlSelector).find('.shead:first');
        if (nodeList.length > 0)
            return nodeList[0];
        return undefined;
    }

    lastNode() {
        let nodeList = $(this._controlSelector).find('.shead:visible:last');
        if (nodeList.length > 0)
            return nodeList[0];
        return undefined;
    }

    nextPageNode(node) {
        let mainElement = document.getElementById(this._controlId);
        let targetTop = node.offsetTop - node.offsetHeight + mainElement.clientHeight;
        do {
            let currIdx = node.alsymbolnode.visualidx;
            node = this.nextNode(node);
            if (node.alsymbolnode.visualidx == currIdx)
                return node;
        } while (node.offsetTop < targetTop);
        return node;
    }

    prevPageNode(node) {
        let mainElement = document.getElementById(this._controlId);
        let targetTop = node.offsetTop + node.offsetHeight - mainElement.clientHeight;
        do {
            let currIdx = node.alsymbolnode.visualidx;
            node = this.prevNode(node);
            if (node.alsymbolnode.visualidx == currIdx)
                return node;
        } while (node.offsetTop > targetTop);
        return node;
    }

    //#endregion

    //#region Event handlers

    onIdsBtnClick(e) {
        this.setShowIds(!this._showIds);
    }

    onNodeClick(node, ctrlKey, shiftKey) {
        if ((!ctrlKey) && (!shiftKey))
            this.toggleNode(node);
        this.selectNode(node, ctrlKey, shiftKey, false);
    }

    onClick(e) {
        let node = e.target;
        let nodeList = $(node).closest('.shead');
        if (nodeList.length > 0)
            this.onNodeClick(nodeList[0], e.ctrlKey, e.shiftKey);
    }

    onKeyDown(e) {
        let handled = false;
        let nodeList;

        switch (e.which) {
            case 65:    //A
                if (e.ctrlKey) {
                    this.selectAll();
                    handled = true;
                }
                break;
            case 37:    //left
                //has visible child
                if (this._selNode) {
                    nodeList = $(this._selNode).next('.treelist:visible');
                    if (nodeList.length > 0)
                        nodeList.hide();
                    else 
                        this.selectNode(this.parentNode(this._selNode), e.ctrlKey, e.shiftKey, true);
                } else
                    this.selectNode(this.firstNode(), e.ctrlKey, e.shiftKey, true);
                handled = true;
                break;
            case 39:    //right
                if (this._selNode) {
                    nodeList = $(this._selNode).next('.treelist:hidden');
                    if (nodeList.length > 0)
                        nodeList.show();
                }
                handled = true;
                break;
            case 38:    //up
                this.selectNode(this.prevNode(this._selNode), e.ctrlKey, e.shiftKey, true);
                handled = true;
                break;
            case 40:    //down
                this.selectNode(this.nextNode(this._selNode), e.ctrlKey, e.shiftKey, true);
                handled = true;
                break;
            case 33:    //page up
                this.selectNode(this.prevPageNode(this._selNode), e.ctrlKey, e.shiftKey, true);
                handled = true;
                break;
            case 34:    //page down
                this.selectNode(this.nextPageNode(this._selNode), e.ctrlKey, e.shiftKey, true);
                handled = true;
                break;
            case 36:    //home
                this.selectNode(this.firstNode(), e.ctrlKey, e.shiftKey, true);
                handled = true;
                break;
            case 35:    //end
                this.selectNode(this.lastNode(), e.ctrlKey, e.shiftKey, true);
                handled = true;
                break;
            case 13:    //enter
                if ((this._selNode) && (this.nodeDefaultAction)) {
                    this.nodeDefaultAction(this._selNode);
                    handled = true;
                }
                break;
        }

        if (handled) {
            e.preventDefault();
            return false;
        }

        return true;
    }

    //#endregion

    //#region Merge data stat

    applyTreeItemState(data) {
        if ((data) && (this._data))
            this.applyTreeItemStateInternal(data, this._data);
    }

    applyTreeItemStateInternal(data, stateSource) {
        if (stateSource.collapsed)
            data.collapsed = stateSource.collapsed;
        if ((data.childSymbols) && (stateSource.childSymbols)) {
            for (let i=0; i<data.childSymbols.length; i++) {
                let childStateSource = this.findChildStateSource(data.childSymbols[i].fullName, stateSource);
                if (childStateSource) {
                    childStateSource.processed = true;
                    this.applyTreeItemStateInternal(data.childSymbols[i], childStateSource);
                }
            }
        }
    }

    findChildStateSource(fullName, stateSource) {
        for (let i=0; i<stateSource.childSymbols.length; i++) {
            if ((!stateSource.childSymbols[i].processed) && (stateSource.childSymbols[i].fullName == fullName))
                return stateSource.childSymbols[i];
        } 
        return undefined;
    }

    //#endregion

}