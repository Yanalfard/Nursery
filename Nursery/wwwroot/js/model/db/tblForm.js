define(["require", "exports"], function (require, exports) {
    "use strict";
    exports.__esModule = true;
    exports.TblForm = void 0;
    var TblForm = /** @class */ (function () {
        function TblForm() {
            this.FormId = -1;
            this.Name = '';
            /** A text to put on forms subtitle */
            this.Body = '';
            this.IsDeleted = false;
            //-
            this.Fields = [];
        }
        return TblForm;
    }());
    exports.TblForm = TblForm;
});
//# sourceMappingURL=tblForm.js.map