(function() {
  var template = Handlebars.template, templates = Handlebars.templates = Handlebars.templates || {};
templates['objectlist'] = template({"1":function(container,depth0,helpers,partials,data) {
    return "            <th>Package</th>\r\n";
},"3":function(container,depth0,helpers,partials,data,blockParams,depths) {
    var stack1;

  return ((stack1 = helpers["if"].call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? depth0.visible : depth0),{"name":"if","hash":{},"fn":container.program(4, data, 0, blockParams, depths),"inverse":container.noop,"data":data})) != null ? stack1 : "");
},"4":function(container,depth0,helpers,partials,data,blockParams,depths) {
    var stack1;

  return ((stack1 = helpers.each.call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? depth0.childSymbols : depth0),{"name":"each","hash":{},"fn":container.program(5, data, 0, blockParams, depths),"inverse":container.noop,"data":data})) != null ? stack1 : "");
},"5":function(container,depth0,helpers,partials,data,blockParams,depths) {
    var stack1;

  return ((stack1 = helpers["if"].call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? depth0.objvisible : depth0),{"name":"if","hash":{},"fn":container.program(6, data, 0, blockParams, depths),"inverse":container.noop,"data":data})) != null ? stack1 : "");
},"6":function(container,depth0,helpers,partials,data,blockParams,depths) {
    var stack1, helper, alias1=container.lambda, alias2=container.escapeExpression, alias3=depth0 != null ? depth0 : (container.nullContext || {}), alias4=helpers.helperMissing, alias5="function";

  return "                    <tr class=\"objrow\" data-objt=\""
    + alias2(alias1((depths[1] != null ? depths[1].objectType : depths[1]), depth0))
    + "\" data-objk="
    + alias2(((helper = (helper = helpers.kind || (depth0 != null ? depth0.kind : depth0)) != null ? helper : alias4),(typeof helper === alias5 ? helper.call(alias3,{"name":"kind","hash":{},"data":data}) : helper)))
    + " data-pkind="
    + alias2(alias1((depths[1] != null ? depths[1].kind : depths[1]), depth0))
    + " data-pidx="
    + alias2(alias1((depths[1] != null ? depths[1].idx : depths[1]), depth0))
    + " data-objidx="
    + alias2(((helper = (helper = helpers.idx || (depth0 != null ? depth0.idx : depth0)) != null ? helper : alias4),(typeof helper === alias5 ? helper.call(alias3,{"name":"idx","hash":{},"data":data}) : helper)))
    + " data-objid=\""
    + alias2(((helper = (helper = helpers.id || (depth0 != null ? depth0.id : depth0)) != null ? helper : alias4),(typeof helper === alias5 ? helper.call(alias3,{"name":"id","hash":{},"data":data}) : helper)))
    + "\" data-objname=\""
    + alias2(((helper = (helper = helpers.name || (depth0 != null ? depth0.name : depth0)) != null ? helper : alias4),(typeof helper === alias5 ? helper.call(alias3,{"name":"name","hash":{},"data":data}) : helper)))
    + "\" onclick=\"objclick(this)\" tabindex=\"0\">\r\n                        <td>"
    + alias2(alias1((depths[1] != null ? depths[1].objectType : depths[1]), depth0))
    + "</td><td>"
    + alias2(((helper = (helper = helpers.id || (depth0 != null ? depth0.id : depth0)) != null ? helper : alias4),(typeof helper === alias5 ? helper.call(alias3,{"name":"id","hash":{},"data":data}) : helper)))
    + "</td><td>"
    + alias2(((helper = (helper = helpers.name || (depth0 != null ? depth0.name : depth0)) != null ? helper : alias4),(typeof helper === alias5 ? helper.call(alias3,{"name":"name","hash":{},"data":data}) : helper)))
    + "</td>\r\n"
    + ((stack1 = helpers["if"].call(alias3,(depths[2] != null ? depths[2].showLibrary : depths[2]),{"name":"if","hash":{},"fn":container.program(7, data, 0, blockParams, depths),"inverse":container.noop,"data":data})) != null ? stack1 : "")
    + "                    </tr>\r\n";
},"7":function(container,depth0,helpers,partials,data) {
    var helper;

  return "                            <td>"
    + container.escapeExpression(((helper = (helper = helpers.library || (depth0 != null ? depth0.library : depth0)) != null ? helper : helpers.helperMissing),(typeof helper === "function" ? helper.call(depth0 != null ? depth0 : (container.nullContext || {}),{"name":"library","hash":{},"data":data}) : helper)))
    + "</th>\r\n";
},"compiler":[7,">= 4.0.0"],"main":function(container,depth0,helpers,partials,data,blockParams,depths) {
    var stack1, alias1=depth0 != null ? depth0 : (container.nullContext || {});

  return "<table id=\"objlisttab\" data-rs-selectable data-rs-class=\"objsel\">\r\n    <tr id=\"objhead\" class=\"objhead\">\r\n        <th>Type</th><th>ID</th><th>Name</th>\r\n"
    + ((stack1 = helpers["if"].call(alias1,(depth0 != null ? depth0.showLibrary : depth0),{"name":"if","hash":{},"fn":container.program(1, data, 0, blockParams, depths),"inverse":container.noop,"data":data})) != null ? stack1 : "")
    + "    </tr>\r\n\r\n"
    + ((stack1 = helpers.each.call(alias1,(depth0 != null ? depth0.objectCollections : depth0),{"name":"each","hash":{},"fn":container.program(3, data, 0, blockParams, depths),"inverse":container.noop,"data":data})) != null ? stack1 : "")
    + "\r\n</table>\r\n";
},"useData":true,"useDepths":true});
})();