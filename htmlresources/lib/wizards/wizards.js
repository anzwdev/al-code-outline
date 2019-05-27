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
    }
}



