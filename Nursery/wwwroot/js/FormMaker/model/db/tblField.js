define(["require", "exports", "../inputType"], function (require, exports, inputType_1) {
    "use strict";
    exports.__esModule = true;
    exports.TblField = void 0;
    var TblField = /** @class */ (function () {
        function TblField() {
            this.FieldId = -1;
            this.FormId = -1;
            this.Label = '';
            this.Type = inputType_1.InputType.text;
            this.IsRequired = false;
            this.Options = '';
            this.Placeholder = '';
            this.Tooltip = '';
            this.IsDeleted = false;
            //-
            this.Validations = [];
        }
        return TblField;
    }());
    exports.TblField = TblField;
});
//# sourceMappingURL=tblField.js.map