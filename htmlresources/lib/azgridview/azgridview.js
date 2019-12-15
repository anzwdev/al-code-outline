class AZGridView {

    constructor(elementId, columns, leftElementId, rightElementId, loadingText, editable) {
        this._elementId = elementId;
        this._columns = columns;
        this._container = document.getElementById(this._elementId);
        this._container.className = 'azgridviewcont';
        this._selStyle = 'azgridviewrowsel';
        this._currRowStyle = this._selStyle + ' azgridviewrowactive'; 

        this._defaultCellStyle = '';
        this._selCellStyle = 'azgridviewselcell'; 

        this._leftElementId = leftElementId;
        this._rightElementId = rightElementId;
        this._loadingText = loadingText;
        this._editable = editable;
        this._inEditMode = false;
        this._headerCreated - false;

        this._currRow = -1;
        this._currColumn = -1;
        this._currDataIdx = -1;
        this._minRowIndex = 1;

        this.renderTable();
        this.initEditor();

        let that = this;
        this._mouseMoveFunction = function(e) { that.onMouseMove(e); };
        this._table.addEventListener('pointerdown', function(e) { that.onMouseDown(e); }, true);
        this._container.addEventListener('keydown', function(e) { that.onKeyDown(e); });
        this._table.addEventListener('pointerup', function(e) { that.onMouseUp(e); });
    }

    initEditor() {
        if (this._editable) {
            this._editor = document.createElement('input');
            this._editor.type = 'text';
            this._editor.className = 'azgridvieweditor';
            //init editor autocomplete
            let me = this;
            let allowedChars = new RegExp(/^[a-zA-Z\s]+$/);
            this._autocomplete = autocomplete({
                input: this._editor,
                disableOpenOnKeyDown: true,
                minLength: -1,
                onSelect: function (item, inputfield) {
                    inputfield.value = item
                },
                fetch: function (text, callback) {
                    var match = text.toLowerCase();
                    if ((me._currColumn >= 0) && 
                        (me._currColumn < me._columns.length) &&
                        (me._columns[me._currColumn].autocomplete)) {

                        callback(me._columns[me._currColumn].autocomplete.filter(function(n) { return n.toLowerCase().startsWith(match); }));
                    }
                },
                render: function(item, value) {
                    var itemElement = document.createElement("div");
                    if (allowedChars.test(value)) {
                        var regex = new RegExp(value, 'gi');
                        var inner = item.replace(regex, function(match) { return "<strong>" + match + "</strong>" });
                        itemElement.innerHTML = inner;
                    } else {
                        itemElement.textContent = item;
                    }
                    return itemElement;
                },
                //emptyMsg: "No  found",
                customize: function(input, inputRect, container, maxHeight) {
                    if (maxHeight < 100) {
                        container.style.top = "";
                        container.style.bottom = (window.innerHeight - inputRect.bottom + input.offsetHeight) + "px";
                        container.style.maxHeight = "140px";
                    }
                }
            });
        }        
    }

    setData(data) {        
        if (this._loadingDiv) {
            this._container.removeChild(this._loadingDiv);
            this._loadingDiv = undefined;
        }
        this._data = data;
        this.renderTableHeader();
        this.renderData();
    }

    getData() {
        return this._data;
    }

    renderData() {
        let newIdx = -1;
        let rowCount = 0;

        let tbody = document.createElement('tbody');
        for (let i=0; i<this._data.length; i++) {
            if (this.checkRowFilter(i)) {
                rowCount++;
                tbody.appendChild(this.renderRow(i));
                if (i == this._currDataIdx)
                    newIdx = rowCount + this._minRowIndex - 1;
            }
        }

        //append empty row if editable
        if (this._editable) {
            rowCount++;
            tbody.appendChild(this.renderEmptyRow());
        }

        if ((newIdx < 0) && (rowCount > 0))
            newIdx = this._minRowIndex;

        if (this._tabBody)
            this._table.replaceChild(tbody, this._tabBody);
        else
            this._table.appendChild(tbody);
        this._tabBody = tbody;

        this._currRow = -1;
        this.setCurrCell(newIdx, 0, false, false, true, true);
    }

    renderEmptyRow() {
        let row = document.createElement('tr');
        for (let i=0; i<this._columns.length; i++) {
            if (!this._columns[i].hidden) {
                let col = document.createElement('td');
                row.appendChild(col);
            }
        }
        return row;
    }

    renderRow(index) {
        let row = document.createElement('tr');
        this.renderRowCells(row, index);
        return row;
    }

    renderRowCells(row, index) {
        row.tabData = this._data[index];
        for (let i=0; i<this._columns.length; i++) {
            if (!this._columns[i].hidden) {
                let value = this._data[index][this._columns[i].name];
                if (value === undefined)
                    value = '';
                let col = document.createElement('td');
                col.innerText = value;
                row.appendChild(col);
                if (this._columns[i].data)
                    row.dataset[this._columns[i].data] = value;
            }
        }
    }

    renderTable() {
        //clear container element
        while (this._container.firstChild) {
            this._container.removeChild(this._container.firstChild);
        }
        //create table
        this._table = document.createElement('table');
        this._table.className = 'azgridview';
        //create headers
        this._tabHead = document.createElement('thead');                
        this._table.appendChild(this._tabHead);
        this._container.appendChild(this._table);

        if (this._loadingText) {
            this._loadingDiv = document.createElement('div');
            this._loadingDiv.className = 'loading';
            this._loadingDiv.innerText = this._loadingText;
            this._container.appendChild(this._loadingDiv);
        }
    }

    renderTableHeader() {
        if (this._headerCreated)
            return;
        this._headerCreated = true;

        let row = document.createElement('tr');
        for (let i=0; i<this._columns.length; i++) {
            if (!this._columns[i].hidden) {
                let col = document.createElement('th');
                col.innerText = this._columns[i].caption;
                if (this._columns[i].style)
                    col.style = this._columns[i].style;
                row.appendChild(col);
            }
        }
        this._tabHead.appendChild(row);
    }

    setCurrCell(rowIndex, columnIndex, clearSel, invertSel, setStart, enableEditor) {
        if ((this._inEditMode) && (this.rowIndex >= this._table.rows.length) && (this._editor.value))
            this.addDataRow();
        
        if (rowIndex < this._minRowIndex)
            rowIndex = this._minRowIndex;
        if (rowIndex >= this._table.rows.length)
            rowIndex = this._table.rows.length - 1;
        if (columnIndex < 0)
            columnIndex = 0;
        if ((this._columns) && (columnIndex >= this._columns.length))
            columnIndex = this._columns.length - 1;

        if ((setStart) || (!this._startRow))
            this._startRow = rowIndex;

        if ((this._currRow != rowIndex) || (invertSel)) {
            if (setStart) {
                this.selectRange(rowIndex, rowIndex, invertSel, clearSel);
            } else
                this.selectRange(Math.min(this._startRow, rowIndex), Math.max(this._startRow, rowIndex), invertSel, clearSel);
            
            if (this._currRow != rowIndex) {
                this.endEdit();
                this.refreshRowStyle(this._currRow, false);
                this.refreshCellStyle(this._currRow, this._currColumn, false);
                this._currRow = rowIndex;
                this._currColumn = columnIndex;
                this.refreshRowStyle(this._currRow, true);
                this.refreshCellStyle(this._currRow, this._currColumn, true);
                if (enableEditor)
                    this.startEdit();
            }
        }

        //change column
        if ((this._currColumn != columnIndex)) {
            this.endEdit();
            this.refreshCellStyle(this._currRow, this._currColumn, false);
            this._currColumn = columnIndex;
            this.refreshCellStyle(this._currRow, this._currColumn, true);
            if (enableEditor)
                this.startEdit();
        }

        if (this._currRow >= 0)
            this.scrollToRowElement(this._table.rows[this._currRow]);        

        if (this.onCurrRowChanged)
            this.onCurrRowChanged(this._table.rows[this._currRow].tabData);
    }

    refreshRowStyle(rowIndex, setCurrentStyle) {
        if ((rowIndex >= this._minRowIndex) && (rowIndex < this._table.rows.length)) {
            if (setCurrentStyle)
                this._table.rows[rowIndex].classList = this._currRowStyle;
            else if (this._table.rows[rowIndex].classList == this._currRowStyle)
                this._table.rows[rowIndex].classList = this._selStyle;
        }
    }

    refreshCellStyle(rowIndex, columnIndex, selected) {
        if (!this._editable)
            return;
        
        if ((rowIndex >= this._minRowIndex) && (rowIndex < this._table.rows.length) &&
            (columnIndex >= 0) && (columnIndex < this._columns.length)) {
            let row = this._table.rows[rowIndex]; 
            
            if (selected)
                row.cells[columnIndex].classList = this._selCellStyle;
            else if (row.cells[columnIndex].classList == this._selCellStyle)
                row.cells[columnIndex].classList = this._defaultCellStyle;
        }
    }


    selectRange(startIndex, endIndex, invertSel, clearSel) {
        if (startIndex < this._minRowIndex)
            startIndex = this._minRowIndex;
        if (endIndex >= this._table.rows.length)
            endIndex = this._tabBody.rows.length - 1;
        for (let i=startIndex; i<=endIndex; i++) {
            let row = this._table.rows[i];
            if ((invertSel) && ((row.className == this._selStyle) || (row.className == this._currRowStyle)))
                row.className = '';
            else if (i == this._currRow)
                row.className = this._currRowStyle;
            else
                row.className = this._selStyle;

            row.dataSelected = (row.className != '');
        }
        if (clearSel) {
            for (let i=0; i<startIndex; i++) {
                this._table.rows[i].className = '';
                this._table.rows[i].dataSelected = false;
            }
            for (let i=endIndex+1; i<this._table.rows.length; i++) {
                this._table.rows[i].className = '';
                this._table.rows[i].dataSelected = false;
            }
        }

    }

    selectAll() {
        this.selectRange(this._minRowIndex, this._table.rows.length - 1, false, false);
    }
    
    nextPageRowIndex() {
        if ((this._currRow < this._minRowIndex) || (this._currRow >= this._table.rows.length))
            return this._minRowIndex;
        let row = this._table.rows[this._currRow];
        let targetTop = row.offsetTop - row.offsetHeight + this._container.clientHeight;
        for (let i = this._currRow + 1; i<this._table.rows.length; i++) {
            if (this._table.rows[i].offsetTop >= targetTop)
                return i;
        }
        return this._table.rows.length - 1;
    }

    prevPageRowIndex() {
        if ((this._currRow < this._minRowIndex) || (this._currRow >= this._table.rows.length))
            return this._minRowIndex;
        let row = this._table.rows[this._currRow];
        let targetTop = row.offsetTop + row.offsetHeight - this._container.clientHeight;
        for (let i = this._currRow - 1; i >= this._minRowIndex; i--) {
            if (this._table.rows[i].offsetTop <= targetTop)
                return i;
        }
        return this._minRowIndex;
    }

    editPrevCell(clearSel, invSel, setSel) {
        if (this._currColumn > 0)
            this.setCurrCell(this._currRow, this._currColumn - 1, clearSel, invSel, setSel, true);
        else
            this.setCurrCell(this._currRow - 1, this._columns.length - 1, clearSel, invSel, setSel, true);
    }

    editNextCell(clearSel, invSel, setSel) {       
        if (this._currColumn < this._columns.length - 1)
            this.setCurrCell(this._currRow, this._currColumn + 1, clearSel, invSel, setSel, true);
        else
            this.setCurrCell(this._currRow + 1, 0, clearSel, invSel, setSel, true);
    }

    onMouseDown(e) {
        if (e.button == 0) {
            let node = e.target;
            let cell = node.closest('td');
            let row = node.closest('tr');
            if (row) {
                let columnIdx = 0;
                if (cell)
                    columnIdx = cell.cellIndex;

                if ((this._inEditMode) && (row.rowIndex == this._currRow) && (columnIdx == this._currColumn))
                    return;

                this.endEdit();
                this.setCurrCell(row.rowIndex, columnIdx, !e.ctrlKey, ((e.ctrlKey) && (!e.shiftKey)), !e.shiftKey, false);
            }
            this._table.setPointerCapture(e.pointerId);
            this._table.addEventListener('pointermove', this._mouseMoveFunction);
        }
    }

    onMouseUp(e) {
        if ((e.button == 0) && (!this._inEditMode)) {
            this._table.removeEventListener('pointermove', this._mouseMoveFunction);
            this._table.releasePointerCapture(e.pointerId);
            this.startEdit();
        }
    }

    onMouseMove(e) {
        let rect = this._table.getBoundingClientRect();
        let posY = e.clientY - rect.top;
        let posX = e.clientX - rect.left;
        let row = this.findRowByYPos(posY);
    	    //scrollTop = window.pageYOffset || document.documentElement.scrollTop;
	        //return { top: rect.top + scrollTop, left: rect.left + scrollLeft }
        //!        let node = document.elementFromPoint(e.clientX, e.clientY);
        //let node = e.target;
        //!        let row = node.closest('tr');
        if (row) {
            let cell = this.findCellByXPos(row, posX);
            let columnIndex = this._currRow;
            if (cell)
                columnIndex = cell.cellIndex;            
            this.setCurrCell(row.rowIndex, columnIndex, !e.ctrlKey, false, false, false);
        }
        e.preventDefault();
    }

    onKeyDown(e) {    
        let handled = false;
        let nodeList;
        let clearSel = !e.ctrlKey;
        let invSel = ((e.ctrlKey) && (!e.shiftKey));
        let setSel = !e.shiftKey;

        switch (e.which) {
            case 65:    //A
                if ((e.ctrlKey) && 
                    ((!this._inEditMode) ||
                    ((this._editor.selectionStart == 0) && (this._editor.selectionEnd == this._editor.value.length))))                
                {
                    this.selectAll();
                    handled = true;
                }
                break;
            case 13:
                if (this._editable) {
                    //this.setCurrCell(this._currRow + 1, this._currColumn, true, false, true, true);
                    this.editNextCell(true, false, true);
                    handled = true;
                }
                break;
            case 9:
                if (this._editable) {
                    if (e.shiftKey)
                        this.editPrevCell(true, false, true);
                    else
                        this.editNextCell(true, false, true);
                    handled = true;
                }
                break;
            case 37:    //left
                if (this._editable) {
                    if ((!this._inEditMode) ||
                        (this._editor.selectionStart != 0) || 
                        ((this._editor.selectionEnd != 0) && (this._editor.selectionEnd != this._editor.value.length)))
                        return true;

                    this.editPrevCell(clearSel, invSel, setSel);
                    handled = true;
                } else if (this._leftElementId) {
                    document.getElementById(this._leftElementId).focus();
                    handled = true;
                }
                break;
            case 39:    //right
                if (this._editable) {
                    if ((!this._inEditMode) ||
                        (this._editor.selectionEnd != this._editor.value.length) ||
                        ((this._editor.selectionStart != 0) && (this._editor.selectionStart != this._editor.selectionEnd)))
                        return true;
        
                    this.editNextCell(clearSel, invSel, setSel);
                    handled = true;
                } else if (this._rightElementId) {
                    document.getElementById(this._rightElementId).focus();
                    handled = true;
                }
                break;
            case 38:    //up
                this.setCurrCell(this._currRow - 1, this._currColumn, clearSel, invSel, setSel, true);
                handled = true;
                break;
            case 40:    //down
                this.setCurrCell(this._currRow + 1, this._currColumn,clearSel, invSel, setSel, true);
                handled = true;
                break;
            case 33:    //page up
                this.setCurrCell(this.prevPageRowIndex(), this._currColumn,clearSel, invSel, setSel, true);
                handled = true;
                break;
            case 34:    //page down
                this.setCurrCell(this.nextPageRowIndex(), this._currColumn,clearSel, invSel, setSel, true);
                handled = true;
                break;
            case 36:    //home
                if ((!this._inEditMode) ||
                    (this._editor.selectionStart != 0) || 
                    ((this._editor.selectionEnd != 0) && (this._editor.selectionEnd != this._editor.value.length)))
                    return true;

                if (e.ctrlKey)
                    this.setCurrCell(0, 0, clearSel, invSel, setSel, true);
                else
                    this.setCurrCell(this._currRow, 0, clearSel, invSel, setSel, true);
                handled = true;
                break;
            case 35:    //end
                if ((!this._inEditMode) ||
                    (this._editor.selectionEnd != this._editor.value.length) ||
                    ((this._editor.selectionStart != 0) && (this._editor.selectionStart != this._editor.selectionEnd)))
                    return true;

                if (e.ctrlKey)
                    this.setCurrCell(this._table.rows.length - 1, this._columns.length-1, clearSel, invSel, setSel, true);
                else
                    this.setCurrCell(this._currRow, this._currColumn.length-1, clearSel, invSel, setSel, true);
                handled = true;
                break;
            case 27: //Esc
                if (this._inEditMode) {
                    this._editor.value = this._originalValue;
                    this._editor.select();
                    handled = true;
                }
                break;
            case 113: //F2
                if ((this._inEditMode)) {
                    let editorValLen = this._editor.value.length;                        
                    if (this._editor.selectionEnd == 0)
                        this._editor.setSelectionRange(editorValLen, editorValLen)
                    else
                        this._editor.setSelectionRange(0, 0);
                    handled = true;
                }
                break;
            case 119: //F8
                if ((this._inEditMode) && (this._data) && (this._currRow > this._minRowIndex) && (this._currColumn >= 0) && (this._currColumn < this._columns.length)) {
                    let prevRowValue = this._data[this._currRow - this._minRowIndex - 1][this._columns[this._currColumn].name];
                    if (prevRowValue === undefined)
                        prevRowValue = '';
                    else
                        prevRowValue = prevRowValue.toString();
                    this._editor.value = prevRowValue;
                    this._editor.setSelectionRange(0, prevRowValue.length);
                    handled = true;
                }
                break;
            case 46: //delete
                if ((this._editable) && (e.ctrlKey)) {
                    this.deleteRows();
                    handled = true;
                }
                break;
            case 45: //insert
                if ((this._editable) && (e.ctrlKey)) {
                    this.insertRow();
                    handled = true;
                }
                break;
        }

        if (handled) {
            e.preventDefault();
            e.stopPropagation();
            return false;
        }

        return true;
    }

    scrollToRowElement(row) {
        let mainTop = 0; //this._container.offsetTop;
        let viewTop = this._container.scrollTop;
        let viewBottom = viewTop + this._container.clientHeight;
        let elementTop = row.offsetTop - mainTop;
        let elementBottom = elementTop + row.offsetHeight;

        if (elementTop < viewTop)
            row.scrollIntoView(true);
        else if (elementBottom > viewBottom)
            row.scrollIntoView(false);
        else if ((this._editable) && (this._currColumn >= 0)) {
            let cell = row.cells[this._currColumn];
            let mainLeft = 0; //this._container.offsetTop;
            let viewLeft = this._container.scrollLeft;
            let viewRight = viewLeft + this._container.clientWidth;
            let elementLeft = cell.offsetLeft - mainLeft;
            let elementRight = elementLeft + cell.offsetWidth;
            if ((elementLeft < viewLeft) || (elementRight > viewRight)) 
            //    cell.scrollIntoView(true);
            //else if (elementRight > viewRight)
                cell.scrollIntoView(false);
        }
    }

    getSelected() {
        let sel = [];
        for (let i=this._minRowIndex; i<this._table.rows.length; i++) {
            //if ((this._table.rows[i].className == this._selStyle) || (this._table.rows[i].className == this._currRowStyle))
            if (this._table.rows[i].dataSelected)
                sel.push(this._table.rows[i].tabData);
        }
        return sel;
    }

    findRowByYPos(yPos) {
        let min = this._minRowIndex;
        let max = this._table.rows.length - 1;

        while (min < max) {
            let mid = Math.trunc((min + max) / 2);
            let row = this._table.rows[mid];
            if (row.offsetTop <= yPos) {
                if (row.offsetTop + row.offsetHeight > yPos)
                    return row;
                min = mid + 1;

            } else
                max = mid - 1;
        }

        if (min == max)
            return this._table.rows[min];
        return undefined;
    }

    findCellByXPos(row, xPos) {
        let min = 0;
        let max = row.cells.length;
        while (min < max) {
            let mid = Math.trunc((min + max) / 2);
            let cell = row.cells[mid];
            if (cell.offsetLeft <= xPos) {
                if (cell.offsetLeft + cell.offsetWidth > xPos)
                    return cell;
                min = mid + 1;

            } else
                max = mid - 1;
        }

        if (min == max)
            return row.cells[min];
        return undefined;
    }

    checkRowFilter(index) {
        for (let i=0; i<this._columns.length; i++) {
            if (!this.checkColFilter(index, i))
                return false;
        }
        return true;
    }

    checkColFilter(index, column) {
        if ((!this._columns[column].appFilter) && 
            (!this._columns[column].userFilter) && 
            ((!this._columns[column].userFilterArray) || (this._columns[column].userFilterArray.length == 0)))
            return true;
        let value = this._data[index][this._columns[column].name];
        
        if ((this._columns[column].appFilter) && (value != this._columns[column].appFilter))
            return false;
        
        if (this._columns[column].compiledFilter) {
            if (this._columns[column].isNumVal) {
                if (!this._columns[column].compiledFilter({INT: value}))
                    return false;
            } else {
                if (!this._columns[column].compiledFilter({TEXT: value}))
                    return false;
            }
        }

        if ((this._columns[column].userFilterArray) && (this._columns[column].userFilterArray.length > 0)) {
            if (this._columns[column].userFilterArray.indexOf(value) < 0)
                return false;
        }

        return true;
    }

    compileFilters() {
        for (let i=0; i<this._columns.length; i++) {
            if (this._columns[i].userFilter != this._columns[i].applUserFilter) {
                if (this._columns[i].userFilter) {
                    try {
                        if (this._columns[i].isNumVal)
                            this._columns[i].compiledFilter = compileFilter('int', this._columns[i].userFilter);
                        else    
                            this._columns[i].compiledFilter = compileFilter('text', this._columns[i].userFilter);
                    }
                    catch (e) {                        
                    }
                } else {
                    this._columns[i].compiledFilter = undefined;
                }
                this._columns[i].applUserFilter = this._columns[i].userFilter;
            }
        }
    }

    startEdit() {
        if ((!this._inEditMode) && (this.validCellSelected())) {
            let cell = this._table.rows[this._currRow].cells[this._currColumn];
            if (cell) {
                this._originalValue = cell.innerText;
                this._editor.value = this._originalValue;                                                            
                while (cell.firstChild) {
                    cell.removeChild(cell.firstChild);
                }

                if ((this._autocomplete) && (this._autocomplete.containerDisplayed()))
                    this._autocomplete.clear();

                cell.appendChild(this._editor);
                this._editor.select();
                this._editor.focus();
                this._inEditMode = true;
            }
        }
    }

    endEdit() {
        if ((this._inEditMode) && (this.validCellSelected())) {            
            let row = this._table.rows[this._currRow];
            let cell = row.cells[this._currColumn];
            let value = this.validateAutocomplete(this._editor.value);
            let updateData = true;

            if (this._currRow == this._table.rows.length - 1) {
                if (!value)
                    updateData = false;
                else
                    this.addDataRow();
            }

            while (cell.firstChild) {
                cell.removeChild(cell.firstChild);
            }
            cell.innerText = value;

            if ((value != this._originalValue) && (row.tabData) && (updateData))
                row.tabData[this._columns[cell.cellIndex].name] = value;
            
            this._inEditMode = false;
        }
    }

    validateAutocomplete(value) {
        if ((value) && (this._columns[this._currColumn].autocomplete)) {            
            let autocompl = this._columns[this._currColumn].autocomplete;
            value = value.toLowerCase();

            for (let i=0; i<autocompl.length; i++) {
                if (autocompl[i].toLowerCase() == value)
                    return autocompl[i];
            }

            for (let i=0; i<autocompl.length; i++) {
                if (autocompl[i].toLowerCase().startsWith(value))
                    return autocompl[i];
            }

            return this._originalValue;
        }

        return value;
    }

    createDataEntry() {
        let item = {};
        for (let i=0; i<this._columns.length; i++) {
            item[this._columns[i].name] = '';
        }
        return item;
    }

    addDataRow() {
        //insert last row data
        let rowIndex = this._table.rows.length - 1; 
        let row = this._table.rows[rowIndex];
        let item = this.createDataEntry();
        this._data.push(item);
        row.tabData = item;

        this._tabBody.appendChild(this.renderEmptyRow());
    }

    insertRow() {
        if ((this._editable) && (this._currRow >= this._minRowIndex) && (this._currRow < this._table.rows.length)) {
            let idx = this._currRow - this._minRowIndex;
            let item = this.createDataEntry();
            this._data.splice(idx, 0, item);
            let row = this._table.insertRow(this._currRow);
            this.renderRowCells(row, idx);
            this._currRow++;
            this.setCurrCell(this._currRow-1, 0, true, false, true, true);
        }
    }

    deleteRows() {
        if (this._editable) {
            let idx = this._minRowIndex;
            let newRowSel = this._currRow;
            this.endEdit();

            while (idx < this._table.rows.length - 1) {
                if ((this._table.rows[idx].className == this._selStyle) || (this._table.rows[idx].className == this._currRowStyle)) {
                    this._data.splice(idx-this._minRowIndex, 1);
                    this._table.deleteRow(idx);
                    if (newRowSel >= idx)
                        newRowSel--;
                } else 
                    idx++;
            }

            this.setCurrCell(newRowSel, this._currColumn, true, false, true, false);
            this.startEdit();
        }

    }

    validCellSelected() {
        return (this._editable && 
            (this._currRow >= this._minRowIndex) && 
            (this._currRow < this._table.rows.length) &&
            (this._currColumn >= 0) &&
            (this._currColumn < this._columns.length));    
    }

}