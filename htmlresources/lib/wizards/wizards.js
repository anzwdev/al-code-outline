var wizardHelper = {
    setElementOptions : function (selector, values, indexAsValue) {
        $(selector).html("");
        if (values) {
            var options = [];
            for (var i=0; i<values.length;i++) {
                if (indexAsValue) {
                    options.push($('<option>', {
                    value: i,
                    text : values[i]
                    }));    
                } else {
                    options.push($('<option>', {
                    value: values[i],
                    text : values[i]
                    }));                    
                }
            }
            $(selector).append(options);
        }
    },

    getSelectedElementOptions : function(selector) {
        var data = [];
        $(selector + " option:selected").each(function() {
            data.push($(this).val());
        });
        return data;
    },

    getAllElementOptions : function(selector) {
        var data = [];
        $(selector + " option").each(function() {
            data.push($(this).val());
        });
        return data;
    },
    
    moveSelectedOptions : function(srcList, destList) {
        var selectedOpts = $(srcList + ' option:selected');
        if (selectedOpts.length == 0) {
            return;
        }
        $(destList).append($(selectedOpts).clone());
        $(selectedOpts).remove();
    },

    moveAllOptions : function(srcList, destList) {
        var selectedOpts = $(srcList + ' option');
        if (selectedOpts.length == 0) {
            return;
        }
        $(destList).append($(selectedOpts).clone());
        $(selectedOpts).remove();
    }
}



