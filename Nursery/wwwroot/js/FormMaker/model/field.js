define(["require", "exports", "./db/tblValue", "./inputType"], function (require, exports, tblValue_1, inputType_1) {
    "use strict";
    exports.__esModule = true;
    exports.Field = void 0;
    var Field = /** @class */ (function () {
        function Field(data) {
            var _this = this;
            var _a, _b, _c, _d, _e, _f, _g, _h, _j, _k, _l, _m, _o, _p, _q, _r;
            this.data = data;
            var templateString = '';
            this.id = "I" + data.Label.split('').map(function (i) { return (i.charCodeAt(0) - 1700); }).join('') + "O";
            data.Options = data.Options.split(',');
            switch (data.Type) {
                case inputType_1.InputType.checkbox:
                    templateString =
                        "\n                <label class=\"fg-label uk-margin-auto-left row\" for=\"" + this.id + "\">" + data.Label + "\n                <input class=\"uk-checkbox\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"checkbox\">\n                </label>\n                   ";
                    break;
                case inputType_1.InputType.textarea:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <textarea class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_a = data.Validations[0]) === null || _a === void 0 ? void 0 : _a.Regex) + "\"></textarea>\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.color:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"color\">\n                   ";
                    break;
                case inputType_1.InputType.date:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"date\" placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_b = data.Validations[0]) === null || _b === void 0 ? void 0 : _b.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.dateTime:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"dateTime\" placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_c = data.Validations[0]) === null || _c === void 0 ? void 0 : _c.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.email:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"email\" placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_d = data.Validations[0]) === null || _d === void 0 ? void 0 : _d.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.file:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <label class=\"comp-upload\">\n                    " + this.data.Options.join(' ,') + "\n                    <input id=\"" + this.id + "\" type=\"file\" name=\"files\" accept=\"" + this.data.Options.map(function (i) { return '.' + i; }).join(',') + "\" />\n                </label>\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.hidden:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"hidden\">\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.image:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " alt=\"" + data.Label + "\" type=\"image\">\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.month:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"month\" placeholder=\"" + data.Placeholder + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.number:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"number\" placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_e = data.Validations[0]) === null || _e === void 0 ? void 0 : _e.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.password:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"password\" placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_f = data.Validations[0]) === null || _f === void 0 ? void 0 : _f.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.radio:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                " + ((_g = data.Options) === null || _g === void 0 ? void 0 : _g.map(function (o) { return "<label class=\"radio\"><span>" + o + "</span><input id=\"" + (data.Label + data.Options.indexOf(o)) + "\" name=\"" + _this.id + "\" class=\"uk-radio\" value=\"" + o + "\" type=\"radio\"></label>"; }).join('')) + "\n                    ";
                    break;
                case inputType_1.InputType.range:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"uk-range\" id=\"" + data.Label.toString() + "\" " + (data.IsRequired ? 'required' : '') + " min=\"" + data.Options[0] + "\" max=\"" + data.Options[1] + "\" type=\"range\" placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_h = data.Validations[0]) === null || _h === void 0 ? void 0 : _h.Regex) + "\">\n                   ";
                    break;
                case inputType_1.InputType.reset:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" type=\"reset\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_j = data.Validations[0]) === null || _j === void 0 ? void 0 : _j.Regex) + "\">\n                   ";
                    break;
                case inputType_1.InputType.search:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" type=\"search\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_k = data.Validations[0]) === null || _k === void 0 ? void 0 : _k.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.submit:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" type=\"submit\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_l = data.Validations[0]) === null || _l === void 0 ? void 0 : _l.Regex) + "\">\n                   ";
                    break;
                case inputType_1.InputType.tel:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" type=\"tel\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_m = data.Validations[0]) === null || _m === void 0 ? void 0 : _m.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.text:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" type=\"text\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_o = data.Validations[0]) === null || _o === void 0 ? void 0 : _o.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.time:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" type=\"time\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_p = data.Validations[0]) === null || _p === void 0 ? void 0 : _p.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.url:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" type=\"url\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_q = data.Validations[0]) === null || _q === void 0 ? void 0 : _q.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.week:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"week\" placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_r = data.Validations[0]) === null || _r === void 0 ? void 0 : _r.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                    break;
                case inputType_1.InputType.combo:
                    templateString =
                        "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <select class=\"entry\" id=\"" + this.id + "\">\n                    " + data.Options.map(function (o) { return ("<option>" + o + "</option>"); }).join('') + "\n                </select>\n                   ";
                    break;
                default:
                    console.log('type unknown');
                    break;
            }
            //fg-col
            templateString = "<div class=\"fg-col " + (data.IsRequired ? 'fg-required' : '') + "\">" + templateString.trim() + "</div>";
            var parser = new DOMParser();
            this.element = (parser.parseFromString(templateString, 'text/html').body);
            this.lblError = this.element.querySelector('.text-danger');
        }
        /** Gets the field's Value.*/
        Field.prototype.getVal = function () {
            var ans = new tblValue_1.TblValue();
            ans.Value = null;
            var input;
            switch (this.data.Type) {
                case inputType_1.InputType.checkbox:
                    input = document.querySelector("input#" + this.id + "[type=\"checkbox\"]");
                    ans.Value = input.checked;
                    break;
                case inputType_1.InputType.combo:
                    input = document.querySelector("select#" + this.id);
                    ans.Value = input.value;
                    break;
                case inputType_1.InputType.radio:
                    input = document.querySelector("input[name=\"" + this.id + "\"][type=\"radio\"]:checked");
                    ans.Value = input === null || input === void 0 ? void 0 : input.value;
                    break;
                case inputType_1.InputType.file:
                    input = document.querySelector("input#" + this.id + "[type=\"file\"]");
                    ans.Value = input.value;
                    break;
                case inputType_1.InputType.image:
                    input = document.querySelector("input#" + this.id + "[type=\"image\"]");
                    ans.Value = input.value;
                    break;
                default:
                    input = document.querySelector("#" + this.id);
                    ans.Value = input.value;
                    break;
            }
            return ans;
        };
        /** Validates the field's Value.
         * Shows the first error in it's validation list.
         * Clears the Errorprovider if there are no errors */
        Field.prototype.validate = function () {
            if (this.data.IsRequired) {
                if (this.getVal() === null
                    || this.getVal() === undefined
                    || this.getVal().Value === null
                    || this.getVal().Value === ''
                    || this.getVal().Value === undefined) {
                    if (this.lblError)
                        this.lblError.innerText = 'این فیلد اجباری است';
                    return false;
                }
            }
            for (var _i = 0, _a = this.data.Validations; _i < _a.length; _i++) {
                var validationRule = _a[_i];
                if (!validationRule.Regex)
                    continue;
                if (!validationRule.Regex.test)
                    continue;
                if (!validationRule.Regex.test(this.getVal().Value)) {
                    if (this.lblError)
                        this.lblError.innerText = validationRule.ValidationMessage;
                    return false;
                }
            }
            //- pass
            if (this.lblError)
                this.lblError.innerText = '';
            return true;
        };
        return Field;
    }());
    exports.Field = Field;
});
//# sourceMappingURL=field.js.map