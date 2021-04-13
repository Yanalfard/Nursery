define(["require", "exports"], function (require, exports) {
    "use strict";
    exports.__esModule = true;
    exports.ConfirmType = exports.Direction = exports.Confirm = exports.ConfirmSettings = void 0;
    var attributeKey = 'popconfirm';
    var ConfirmSettings = /** @class */ (function () {
        function ConfirmSettings(title, description, type, okText, cancelText, direction) {
            if (title === void 0) { title = 'تایید'; }
            if (description === void 0) { description = 'آیا مطمئن هستید؟'; }
            if (type === void 0) { type = ConfirmType["default"]; }
            if (okText === void 0) { okText = 'بله'; }
            if (cancelText === void 0) { cancelText = 'خیر'; }
            if (direction === void 0) { direction = Direction.tc; }
            this.title = 'تایید';
            this.description = 'آیا مطمئن هستید؟';
            this.type = ConfirmType["default"];
            this.okText = 'بله';
            this.cancelText = 'خیر';
            this.direction = Direction.tc;
            this.title = title;
            this.description = description;
            this.type = type;
            this.okText = okText;
            this.cancelText = cancelText;
            this.direction = direction;
        }
        return ConfirmSettings;
    }());
    exports.ConfirmSettings = ConfirmSettings;
    var Confirm = /** @class */ (function () {
        function Confirm(parent, okEval, cancelEval, settings) {
            var _this = this;
            this.settings = new ConfirmSettings();
            this.settings.title = (settings === null || settings === void 0 ? void 0 : settings.title) || this.settings.title;
            this.settings.description = (settings === null || settings === void 0 ? void 0 : settings.description) || this.settings.description;
            this.settings.okText = (settings === null || settings === void 0 ? void 0 : settings.okText) || this.settings.okText;
            this.settings.direction = (settings === null || settings === void 0 ? void 0 : settings.direction) || this.settings.direction;
            this.settings.cancelText = (settings === null || settings === void 0 ? void 0 : settings.cancelText) || this.settings.cancelText;
            this.cancelEval = cancelEval;
            this.okEval = okEval;
            this.parent = parent;
            var template = "<div class=\"popconfirm\">\n                <div class=\"popconfirm-header\">\n                    " + this.settings.title + "\n                </div>\n                <div class=\"popconfirm-body\">\n                    <p class=\"popconfirm-description\">\n                        " + this.settings.description + "\n                    </p>\n                </div>\n                <div class=\"popconfirm-footer\">\n                    <button btnOk class=\"popconfirm-btn popconfirm-ok btn-primary\">" + this.settings.okText + "</button>\n                    <button btnCancel class=\"popconfirm-btn popconfirm-cancel btn-secondary\">" + this.settings.cancelText + "</button>\n                </div>\n            </div>";
            this.element = new DOMParser().parseFromString(template, 'text/html').querySelector('.popconfirm');
            this.btnOk = this.element.querySelector('[btnOk]');
            this.btnCancel = this.element.querySelector('[btnCancel]');
            this.btnOk.addEventListener('click', function () { _this.ok(); });
            this.btnCancel.addEventListener('click', function () { _this.cancel(); });
            var offet = 16;
            var left = 0;
            var top = 0;
            document.body.appendChild(this.element);
            var calculateDeltas = function () {
                switch (Direction[_this.settings.direction.toString()]) {
                    case Direction.bl:
                        top = parent.offsetTop + parent.offsetHeight + offet;
                        left = parent.offsetLeft;
                        break;
                    case Direction.br:
                        top = parent.offsetTop + parent.offsetHeight + offet;
                        left = parent.offsetLeft - (_this.element.offsetWidth - parent.offsetWidth);
                        break;
                    case Direction.lb:
                        top = (parent.offsetTop + parent.offsetHeight) - _this.element.offsetHeight;
                        left = parent.offsetLeft - _this.element.offsetWidth - offet;
                        break;
                    case Direction.lt:
                        top = parent.offsetTop;
                        left = parent.offsetLeft - _this.element.offsetWidth - offet;
                        break;
                    case Direction.tl:
                        top = parent.offsetTop - _this.element.offsetHeight - offet;
                        left = parent.offsetLeft;
                        break;
                    case Direction.tr:
                        top = parent.offsetTop - _this.element.offsetHeight - offet;
                        left = parent.offsetLeft - (_this.element.offsetWidth - parent.offsetWidth);
                        break;
                    case Direction.rt:
                        top = parent.offsetTop;
                        left = parent.offsetLeft + parent.offsetWidth + offet;
                        break;
                    case Direction.rb:
                        top = (parent.offsetTop + parent.offsetHeight) - _this.element.offsetHeight;
                        left = parent.offsetLeft + parent.offsetWidth + offet;
                        break;
                    case Direction.rc:
                        top = (parent.offsetTop + (parent.offsetHeight / 2)) - (_this.element.offsetHeight / 2);
                        left = parent.offsetLeft + parent.offsetWidth + offet;
                        break;
                    case Direction.tc:
                        top = parent.offsetTop - _this.element.offsetHeight - offet;
                        left = parent.offsetLeft + (parent.offsetWidth / 2) - (_this.element.offsetWidth / 2);
                        break;
                    case Direction.bc:
                        top = parent.offsetTop + parent.offsetHeight + offet;
                        left = parent.offsetLeft + (parent.offsetWidth / 2) - (_this.element.offsetWidth / 2);
                        break;
                    case Direction.lc:
                        top = (parent.offsetTop + (parent.offsetHeight / 2)) - (_this.element.offsetHeight / 2);
                        left = parent.offsetLeft - _this.element.offsetWidth - offet;
                        break;
                    default:
                        top = parent.offsetTop + _this.element.offsetHeight + offet;
                        left = parent.offsetLeft;
                        break;
                }
                _this.element.style.left = left + 'px';
                _this.element.style.top = top + 'px';
            };
            window.addEventListener('scroll', function () {
                calculateDeltas();
            });
            calculateDeltas();
            window.addEventListener('click', function (e) {
                if (!e.path.includes(_this.element) && !e.path.includes(_this.parent)) {
                    _this.hide();
                    if (_this.element.style.display == 'block') {
                        _this.cancel();
                    }
                }
                else
                    _this.show();
            });
            this.hide();
        }
        Confirm.prototype.show = function () {
            this.element.style.display = 'block';
            this.element.style.transition = 'all ease 0.3s';
            this.element.style.opacity = '1';
            // TODO Calculate deltas when shown!!!
        };
        Confirm.prototype.hide = function () {
            var _this = this;
            this.element.style.transition = 'all ease 0.3s';
            this.element.style.opacity = '0';
            setTimeout(function () {
                _this.element.style.display = 'none';
            }, 300);
        };
        Confirm.prototype.cancel = function () {
            var _this = this;
            setTimeout(function () {
                _this.hide();
                eval(_this.cancelEval);
            }, 1);
        };
        Confirm.prototype.ok = function () {
            var _this = this;
            setTimeout(function () {
                _this.hide();
                eval(_this.okEval);
            }, 1);
        };
        return Confirm;
    }());
    exports.Confirm = Confirm;
    var Direction;
    (function (Direction) {
        Direction[Direction["tl"] = 0] = "tl";
        Direction[Direction["tc"] = 1] = "tc";
        Direction[Direction["tr"] = 2] = "tr";
        Direction[Direction["rt"] = 3] = "rt";
        Direction[Direction["rc"] = 4] = "rc";
        Direction[Direction["rb"] = 5] = "rb";
        Direction[Direction["br"] = 6] = "br";
        Direction[Direction["bc"] = 7] = "bc";
        Direction[Direction["bl"] = 8] = "bl";
        Direction[Direction["lb"] = 9] = "lb";
        Direction[Direction["lc"] = 10] = "lc";
        Direction[Direction["lt"] = 11] = "lt";
    })(Direction = exports.Direction || (exports.Direction = {}));
    var ConfirmType;
    (function (ConfirmType) {
        ConfirmType[ConfirmType["default"] = 0] = "default";
        ConfirmType[ConfirmType["info"] = 1] = "info";
        ConfirmType[ConfirmType["warning"] = 2] = "warning";
        ConfirmType[ConfirmType["danger"] = 3] = "danger";
    })(ConfirmType = exports.ConfirmType || (exports.ConfirmType = {}));
    var confirmables = document.querySelectorAll("[" + attributeKey + "]");
    confirmables.forEach(function (parent) {
        var ok = parent.getAttribute('popok') || undefined;
        var cancel = parent.getAttribute('popcancel') || undefined;
        var settings = sanitizeAttribute(parent.getAttribute(attributeKey));
        var conf = new Confirm(parent, ok, cancel, settings);
    });
    /**
     *
     * @param {string} attribute
     */
    function sanitizeAttribute(attribute) {
        if (attribute.startsWith('{'))
            attribute = attribute.substring(1, attribute.length - 1);
        if (attribute.startsWith('}'))
            attribute = attribute.substring(0, attribute.length - 2);
        if (!attribute)
            return undefined;
        var keyValues = attribute.split(';');
        var obj = {};
        for (var _i = 0, keyValues_1 = keyValues; _i < keyValues_1.length; _i++) {
            var item = keyValues_1[_i];
            var split = item.split(':');
            obj[split[0]] = split[1];
        }
        return obj;
    }
});
//# sourceMappingURL=confirm.js.map