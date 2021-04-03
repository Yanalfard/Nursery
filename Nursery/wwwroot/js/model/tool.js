define(["require", "exports"], function (require, exports) {
    "use strict";
    exports.__esModule = true;
    exports.Select = exports.Regex = exports.Option = exports.Tool = void 0;
    var Tool = /** @class */ (function () {
        function Tool() {
            this.options = [];
            this.regexs = [];
            this.selects = [];
        }
        return Tool;
    }());
    exports.Tool = Tool;
    var Option = /** @class */ (function () {
        function Option() {
        }
        return Option;
    }());
    exports.Option = Option;
    var Regex = /** @class */ (function () {
        function Regex() {
        }
        return Regex;
    }());
    exports.Regex = Regex;
    var Select = /** @class */ (function () {
        function Select() {
        }
        return Select;
    }());
    exports.Select = Select;
});
//# sourceMappingURL=tool.js.map