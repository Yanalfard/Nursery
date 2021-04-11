define(["require", "exports"], function (require, exports) {
    "use strict";
    exports.__esModule = true;
    exports.TblRegex = void 0;
    var TblRegex = /** @class */ (function () {
        function TblRegex(regex, validationMessage, name) {
            this.RegexId = -1;
            this.Name = '';
            this.Regex = new RegExp('');
            this.ValidationMessage = '';
            this.IsDeleted = false;
            this.Regex = regex;
            this.ValidationMessage = validationMessage !== null && validationMessage !== void 0 ? validationMessage : '';
            this.Name = name;
        }
        return TblRegex;
    }());
    exports.TblRegex = TblRegex;
});
//# sourceMappingURL=tblRegex.js.map