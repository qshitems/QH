layui.define(['jquery'], function (exports) {
    "use strict";

    var MOD_NAME = 'common';
    var $ = layui.jquery;

    var common = {

        val: function (filter, formdate) {
            var element = $('div[lay-filter=' + filter + ']');
            if (!!formdate) {
                for (var key in formdate) {
                    if (key == "enabled") {
                        debugger;
                    }
                    var $id = element.find('#' + key);
                    var value = $.trim(formdate[key]).replace(/&nbsp;/g, '');
                    var type = $id.attr('type');
                    switch (type) {
                        case "checkbox":
                          
                            if (value == "true") {
                                $id.attr("checked", 'checked');
                            } else {
                                $id.removeAttr("checked");
                            }
                            break;
                        case "select":
                            $id.val(value).trigger("change");
                            break;
                        default:
                            $id.val(value);
                            break;
                    }
                };
                return false;
            }
          
        },


        request: function (name) {
            var search = location.search.slice(1);
            var arr = search.split("&");
            for (var i = 0; i < arr.length; i++) {
                var ar = arr[i].split("=");
                if (ar[0] == name) {
                    if (unescape(ar[1]) == 'undefined') {
                        return "";
                    } else {
                        return unescape(ar[1]);
                    }
                }
            }
            return "";
        }
    };
    exports(MOD_NAME, common);
});




