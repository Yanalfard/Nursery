define(["require", "exports"], function (require, exports) {
    "use strict";
    exports.__esModule = true;
    exports.Select = exports.Regex = exports.Option = exports.OptionType = exports.Tool = void 0;
    var Tool = /** @class */ (function () {
        function Tool() {
            this.options = [];
            this.regexs = [];
            this.selects = [];
        }
        return Tool;
    }());
    exports.Tool = Tool;
    var OptionType;
    (function (OptionType) {
        OptionType[OptionType["input"] = 0] = "input";
        OptionType[OptionType["select"] = 1] = "select";
        OptionType[OptionType["checkbox"] = 2] = "checkbox";
        OptionType[OptionType["doubleInput"] = 3] = "doubleInput";
    })(OptionType = exports.OptionType || (exports.OptionType = {}));
    var Option = /** @class */ (function () {
        function Option(element, optionType, name, value) {
            this.element = element;
            this.optionType = optionType;
            this.name = name;
            this.value = value;
            switch (+optionType) {
                case OptionType.input:
                    var input_1 = element.querySelector('input');
                    input_1.addEventListener('input', function () {
                        value = input_1.value;
                    });
                    break;
                case OptionType.select:
                    var select_1 = element.querySelector('select');
                    select_1.addEventListener('change', function () {
                        value = select_1.value;
                    });
                    break;
                case OptionType.checkbox:
                    var checkbox_1 = element.querySelector('input[type=checkbox]');
                    checkbox_1.addEventListener('input', function () {
                        value = checkbox_1.checked ? 'true' : 'false';
                    });
                    break;
                case OptionType.doubleInput:
                    var from_1 = element.querySelector('[input-from]');
                    var to_1 = element.querySelector('[input-to]');
                    //-
                    var change = function () {
                        value = from_1.value + ',' + to_1.value;
                    };
                    from_1.addEventListener('input', change);
                    to_1.addEventListener('input', change);
                    break;
                default:
                    console.error('Unknown Option Type');
                    break;
            }
        }
        return Option;
    }());
    exports.Option = Option;
    var Regex = /** @class */ (function () {
        function Regex(tblRegex) {
            this.tblRegex = tblRegex;
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