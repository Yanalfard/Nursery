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
            this.settings.cancelText = (settings === null || settings === void 0 ? void 0 : settings.cancelText) || this.settings.cancelText;
            this.settings.direction = (settings === null || settings === void 0 ? void 0 : settings.direction) || this.settings.direction || Direction.tc;
            this.settings.type = (settings === null || settings === void 0 ? void 0 : settings.type) || this.settings.type || ConfirmType["default"];
            this.cancelEval = cancelEval;
            this.okEval = okEval;
            this.parent = parent;
            var template = "<div class=\"popconfirm\">\n                <div class=\"popconfirm-arrow\"></div>\n                <div class=\"popconfirm-header\">\n                    " + this.settings.title + "\n                </div>\n                <div class=\"popconfirm-body\">\n                    <p class=\"popconfirm-description\">\n                        " + this.settings.description + "\n                    </p>\n                </div>\n                <div class=\"popconfirm-footer\">\n                    <button btnOk class=\"popconfirm-btn popconfirm-ok btn-primary\">" + this.settings.okText + "</button>\n                    <button btnCancel class=\"popconfirm-btn popconfirm-cancel btn-secondary\">" + this.settings.cancelText + "</button>\n                </div>\n            </div>";
            this.element = new DOMParser().parseFromString(template, 'text/html').querySelector('.popconfirm');
            this.btnOk = this.element.querySelector('[btnOk]');
            this.btnCancel = this.element.querySelector('[btnCancel]');
            this.arrow = this.element.querySelector('.popconfirm-arrow');
            this.btnOk.innerText = this.settings.okText;
            this.btnCancel.innerText = this.settings.cancelText;
            this.btnOk.addEventListener('click', function () { _this.ok(); });
            this.btnCancel.addEventListener('click', function () { _this.cancel(); });
            this.settings.type = ConfirmType[this.settings.type] || ConfirmType["default"];
            this.btnOk.classList.remove('btn-primary');
            switch (this.settings.type) {
                case ConfirmType.danger:
                    this.btnOk.classList.add('btn-danger');
                    break;
                case ConfirmType["default"]:
                    this.btnOk.classList.add('btn-primary');
                    break;
                case ConfirmType.success:
                    this.btnOk.classList.add('btn-success');
                    break;
                case ConfirmType.warning:
                    this.btnOk.classList.add('btn-warning');
                    break;
                default:
                    this.btnOk.classList.add('btn-primary');
                    break;
            }
            window.addEventListener('scroll', function () {
                if (_this.element.style.display == 'block') {
                    _this.cancel();
                }
            });
            window.addEventListener('resize', function () {
                if (_this.element.style.display == 'block') {
                    _this.cancel();
                }
            });
            document.body.appendChild(this.element);
            this.calculateDeltas();
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
            this.element.style.display = 'none';
            this.hide();
        }
        // Calculate where the element should be positioned relative to its parent
        // Calculate where the arrow should be relative to the element
        Confirm.prototype.calculateDeltas = function () {
            var left = 0;
            var top = 0;
            var parent = this.parent;
            var offet = 16;
            //- arrow
            var atop = '';
            var abottom = '';
            var aleft = '';
            var aright = '';
            switch (Direction[this.settings.direction.toString()]) {
                case Direction.bl:
                    top = parent.offsetTop + parent.offsetHeight + offet;
                    left = parent.offsetLeft;
                    atop = -10;
                    aleft = 16;
                    break;
                case Direction.br:
                    top = parent.offsetTop + parent.offsetHeight + offet;
                    left = parent.offsetLeft - (this.element.offsetWidth - parent.offsetWidth);
                    atop = -10;
                    aright = 16;
                    break;
                case Direction.lb:
                    top = (parent.offsetTop + parent.offsetHeight) - this.element.offsetHeight;
                    left = parent.offsetLeft - this.element.offsetWidth - offet;
                    aright = -10;
                    abottom = 16;
                    break;
                case Direction.lt:
                    top = parent.offsetTop;
                    left = parent.offsetLeft - this.element.offsetWidth - offet;
                    aright = -10;
                    atop = 16;
                    break;
                case Direction.tl:
                    top = parent.offsetTop - this.element.offsetHeight - offet;
                    left = parent.offsetLeft;
                    abottom = -10;
                    aleft = 16;
                    break;
                case Direction.tr:
                    top = parent.offsetTop - this.element.offsetHeight - offet;
                    left = parent.offsetLeft - (this.element.offsetWidth - parent.offsetWidth);
                    abottom = -10;
                    aright = 16;
                    break;
                case Direction.rt:
                    top = parent.offsetTop;
                    left = parent.offsetLeft + parent.offsetWidth + offet;
                    aleft = -10;
                    atop = 16;
                    break;
                case Direction.rb:
                    top = (parent.offsetTop + parent.offsetHeight) - this.element.offsetHeight;
                    left = parent.offsetLeft + parent.offsetWidth + offet;
                    aleft = -10;
                    abottom = 16;
                    break;
                case Direction.rc:
                    top = (parent.offsetTop + (parent.offsetHeight / 2)) - (this.element.offsetHeight / 2);
                    left = parent.offsetLeft + parent.offsetWidth + offet;
                    aleft = -10;
                    atop = abottom = (this.element.offsetHeight - this.arrow.offsetHeight) / 2;
                    break;
                case Direction.lc:
                    top = (parent.offsetTop + (parent.offsetHeight / 2)) - (this.element.offsetHeight / 2);
                    left = parent.offsetLeft - this.element.offsetWidth - offet;
                    aright = -10;
                    atop = abottom = (this.element.offsetHeight - this.arrow.offsetHeight) / 2;
                    break;
                case Direction.tc:
                    top = parent.offsetTop - this.element.offsetHeight - offet;
                    left = parent.offsetLeft + (parent.offsetWidth / 2) - (this.element.offsetWidth / 2);
                    abottom = -10;
                    aleft = aright = (this.element.offsetWidth - this.arrow.offsetHeight) / 2;
                    break;
                case Direction.bc:
                    top = parent.offsetTop + parent.offsetHeight + offet;
                    left = parent.offsetLeft + (parent.offsetWidth / 2) - (this.element.offsetWidth / 2);
                    atop = -10;
                    aleft = aright = (this.element.offsetWidth - this.arrow.offsetHeight) / 2;
                    break;
                default:
                    top = parent.offsetTop - this.element.offsetHeight - offet;
                    left = parent.offsetLeft + (parent.offsetWidth / 2) - (this.element.offsetWidth / 2);
                    abottom = -10;
                    aleft = aright = (this.element.offsetWidth - this.arrow.offsetHeight) / 2;
                    break;
            }
            this.element.style.left = left + 'px';
            this.element.style.top = top + 'px';
            this.arrow.style.left = aleft + 'px';
            this.arrow.style.right = aright + 'px';
            this.arrow.style.top = atop + 'px';
            this.arrow.style.bottom = abottom + 'px';
        };
        Confirm.prototype.show = function () {
            this.element.style.display = 'block';
            this.element.style.transition = 'all ease 0.3s';
            this.element.style.transitionProperty = 'opacity';
            this.element.style.opacity = '1';
            this.calculateDeltas();
        };
        Confirm.prototype.hide = function () {
            var _this = this;
            this.element.style.transition = 'all ease 0.2s ';
            this.element.style.transitionProperty = 'opacity';
            this.element.style.opacity = '0';
            setTimeout(function () {
                _this.element.style.display = 'none';
            }, 200);
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
        ConfirmType[ConfirmType["success"] = 1] = "success";
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
    function isElementInViewport(element) {
        var rect = element.getBoundingClientRect();
        return rect.top >= 0 &&
            rect.left >= 0 &&
            rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
            rect.right <= (window.innerWidth || document.documentElement.clientWidth);
    }
});
//# sourceMappingURL=confirm.js.map