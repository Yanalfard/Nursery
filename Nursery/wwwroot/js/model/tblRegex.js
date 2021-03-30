"use strict";
exports.__esModule = true;
exports.tblRegex = void 0;
var tblRegex = /** @class */ (function () {
    function tblRegex(regex, validationMessage, name) {
        this.RegexId = -1;
        this.Name = '';
        this.Regex = new RegExp('');
        this.ValidationMessage = '';
        this.IsDeleted = false;
        this.Regex = regex;
        this.ValidationMessage = validationMessage !== null && validationMessage !== void 0 ? validationMessage : '';
        this.Name = name;
    }
    return tblRegex;
}());
exports.tblRegex = tblRegex;
//# sourceMappingURL=tblRegex.js.map