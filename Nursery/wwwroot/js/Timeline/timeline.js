define(["require", "exports"], function (require, exports) {
    "use strict";
    exports.__esModule = true;
    exports.Block = exports.TimeRow = exports.Timeline = void 0;
    var colorArray = ['#48f', '#8f5', '#fe2', '#d8d'];
    var Timeline = /** @class */ (function () {
        function Timeline(element, incrementMinutes) {
            if (incrementMinutes === void 0) { incrementMinutes = 15; }
            this.incrementMinutes = 15;
            this.element = element;
            this.element.classList.add('timeline');
            this.incrementMinutes = incrementMinutes;
            this.timelineHeader = document.createElement('div');
            this.timelineHeader.classList.add('timeline-header');
            this.timelineEmpty = document.createElement('div');
            this.timelineEmpty.classList.add('timeline-empty');
            this.timelineTimestamps = document.createElement('div');
            this.timelineTimestamps.classList.add('timeline-timestamps');
            this.timelineBody = document.createElement('div');
            this.timelineBody.classList.add('timeline-body');
            var blockCount = Math.floor(1440 / this.incrementMinutes);
            for (var i = 0; i < blockCount; i++) {
                var timeStamp = document.createElement('div');
                timeStamp.classList.add('timeline-timestamp');
                var hours = (i * incrementMinutes) / 60;
                var hour = Math.floor(hours);
                var minute = (i * incrementMinutes) % 60;
                timeStamp.innerText = hour + ":" + minute;
                this.timelineTimestamps.appendChild(timeStamp);
            }
            element.appendChild(this.timelineHeader);
            this.timelineHeader.appendChild(this.timelineEmpty);
            this.timelineHeader.appendChild(this.timelineTimestamps);
            element.appendChild(this.timelineBody);
        }
        Timeline.prototype.addRow = function (annotaion, data) {
            var rElement = document.createElement('div');
            rElement.classList.add('timeline-row');
            var row = new TimeRow(this, rElement, annotaion, data);
            this.timelineBody.appendChild(rElement);
        };
        return Timeline;
    }());
    exports.Timeline = Timeline;
    var TimeRow = /** @class */ (function () {
        function TimeRow(timeline, element, annotation, data) {
            this.element = element;
            this.timeline = timeline;
            this.annotation = annotation || null;
            this.data = data || null;
            this.timelineAnnotation = document.createElement('div');
            this.timelineAnnotation.classList.add('timeline-annotation');
            this.timelineBlocks = document.createElement('div');
            this.timelineBlocks.classList.add('timeline-blocks');
            this.element.appendChild(this.timelineAnnotation);
            this.element.appendChild(this.timelineBlocks);
            var blockCount = Math.floor(1440 / timeline.incrementMinutes);
            var repColorArray = colorArray;
            for (var i = 0; i < blockCount; i++) {
                var bElement = document.createElement('div');
                bElement.classList.add('timeline-block');
                var b = new Block(bElement, i * timeline.incrementMinutes, repColorArray[repColorArray.length - 1]);
                repColorArray.pop();
                if (repColorArray.length === 0)
                    repColorArray = colorArray;
                this.timelineBlocks.appendChild(bElement);
            }
        }
        return TimeRow;
    }());
    exports.TimeRow = TimeRow;
    var Block = /** @class */ (function () {
        function Block(element, minute, color) {
            this.color = color;
            this.element = element;
            this.minute = minute;
        }
        Block.prototype.toggleSelection = function () {
            this.isSelected = !this.isSelected;
            this.updateSelection();
        };
        Block.prototype.select = function () {
            this.isSelected = true;
            this.updateSelection();
        };
        Block.prototype.deselect = function () {
            this.isSelected = false;
            this.updateSelection();
        };
        Block.prototype.updateSelection = function () {
            if (this.isSelected) {
                // Select
                this.element.style.backgroundColor = this.color;
            }
            else if (!this.isSelected) {
                // Deselect
                this.element.style.backgroundColor = 'white';
            }
        };
        return Block;
    }());
    exports.Block = Block;
    var dataModel = /** @class */ (function () {
        function dataModel() {
        }
        return dataModel;
    }());
    var timelineComponents = (document.querySelectorAll('[timeline]'));
    timelineComponents.forEach(function (comp) {
        var timeline = new Timeline(comp);
        var url = comp.getAttribute('timeline-data-url');
        if (url) {
            fetch(url).then(function (j) {
                j.json().then(function (data) {
                    console.log(data);
                    data.forEach(function (i) {
                        timeline.addRow(i.role.name, i.role);
                    });
                });
            });
        }
    });
});
//# sourceMappingURL=timeline.js.map