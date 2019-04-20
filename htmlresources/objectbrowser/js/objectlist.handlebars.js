(function() {
  var template = Handlebars.template, templates = Handlebars.templates = Handlebars.templates || {};
templates['objectlist'] = template({"1":function(container,depth0,helpers,partials,data,blockParams,depths) {
    var stack1;

  return ((stack1 = helpers["if"].call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? depth0.visible : depth0),{"name":"if","hash":{},"fn":container.program(2, data, 0, blockParams, depths),"inverse":container.noop,"data":data})) != null ? stack1 : "");
},"2":function(container,depth0,helpers,partials,data,blockParams,depths) {
    var stack1;

  return ((stack1 = helpers.each.call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? depth0.childSymbols : depth0),{"name":"each","hash":{},"fn":container.program(3, data, 0, blockParams, depths),"inverse":container.noop,"data":data})) != null ? stack1 : "");
},"3":function(container,depth0,helpers,partials,data,blockParams,depths) {
    var stack1;

  return ((stack1 = helpers["if"].call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? depth0.objvisible : depth0),{"name":"if","hash":{},"fn":container.program(4, data, 0, blockParams, depths),"inverse":container.noop,"data":data})) != null ? stack1 : "");
},"4":function(container,depth0,helpers,partials,data,blockParams,depths) {
    var helper, alias1=container.lambda, alias2=container.escapeExpression, alias3=depth0 != null ? depth0 : (container.nullContext || {}), alias4=helpers.helperMissing, alias5="function";

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
    + "\" onclick=\"objclick(this)\" tabindex=\"0\"><td>"
    + alias2(alias1((depths[1] != null ? depths[1].objectType : depths[1]), depth0))
    + "</td><td>"
    + alias2(((helper = (helper = helpers.id || (depth0 != null ? depth0.id : depth0)) != null ? helper : alias4),(typeof helper === alias5 ? helper.call(alias3,{"name":"id","hash":{},"data":data}) : helper)))
    + "</td><td>"
    + alias2(((helper = (helper = helpers.name || (depth0 != null ? depth0.name : depth0)) != null ? helper : alias4),(typeof helper === alias5 ? helper.call(alias3,{"name":"name","hash":{},"data":data}) : helper)))
    + "</td></tr>\r\n";
},"compiler":[7,">= 4.0.0"],"main":function(container,depth0,helpers,partials,data,blockParams,depths) {
    var stack1;

  return "<table id=\"objlisttab\" data-rs-selectable data-rs-class=\"objsel\">\r\n    <tr id=\"objhead\" class=\"objhead\"><th>Type</th><th>ID</th><th>Name</th></tr>\r\n\r\n"
    + ((stack1 = helpers.each.call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? depth0.objectCollections : depth0),{"name":"each","hash":{},"fn":container.program(1, data, 0, blockParams, depths),"inverse":container.noop,"data":data})) != null ? stack1 : "")
    + "\r\n</table>\r\n";
},"useData":true,"useDepths":true});
})();