const colorArray = ['#48f', '#8f5', '#fe2', '#d8d'];

export class Timeline {
    element: HTMLElement;
    timelineHeader: HTMLElement;
    timelineEmpty: HTMLElement;
    timelineTimestamps: HTMLElement;
    timelineBody: HTMLElement;
    incrementMinutes: number = 15;
    timeRows: TimeRow[];

    constructor(element: HTMLElement, incrementMinutes: number = 15) {
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

        const blockCount: number = Math.floor(1440 / this.incrementMinutes);

        for (var i = 0; i < blockCount; i++) {
            const timeStamp = document.createElement('div');
            timeStamp.classList.add('timeline-timestamp');

            const hours = (i * incrementMinutes) / 60;

            const hour = Math.floor(hours);
            const minute = (i * incrementMinutes) % 60;

            timeStamp.innerText = `${hour}:${minute}`

            this.timelineTimestamps.appendChild(timeStamp);
        }

        element.appendChild(this.timelineHeader);
        this.timelineHeader.appendChild(this.timelineEmpty);
        this.timelineHeader.appendChild(this.timelineTimestamps);
        element.appendChild(this.timelineBody);
    }

    addRow(annotaion?: string, data?: any) {
        const rElement = document.createElement('div');
        rElement.classList.add('timeline-row');
        const row = new TimeRow(this, rElement, annotaion, data);
        this.timelineBody.appendChild(rElement);
    }
}

export class TimeRow {
    element: HTMLElement;
    timelineAnnotation: HTMLElement;
    timelineBlocks: HTMLElement;
    timeline: Timeline;
    annotation: string;
    data: any;

    constructor(timeline: Timeline, element: HTMLElement, annotation?: string, data?: any) {
        this.element = element;
        this.timeline = timeline;
        this.annotation = annotation || null
        this.data = data || null;

        this.timelineAnnotation = document.createElement('div');
        this.timelineAnnotation.classList.add('timeline-annotation');
        this.timelineBlocks = document.createElement('div');
        this.timelineBlocks.classList.add('timeline-blocks');

        this.element.appendChild(this.timelineAnnotation);
        this.element.appendChild(this.timelineBlocks);

        const blockCount: number = Math.floor(1440 / timeline.incrementMinutes);

        let repColorArray = colorArray;

        for (var i = 0; i < blockCount; i++) {
            let bElement = document.createElement('div');
            bElement.classList.add('timeline-block');

            let b = new Block(bElement, i * timeline.incrementMinutes, repColorArray[repColorArray.length - 1]);
            repColorArray.pop();
            if (repColorArray.length === 0) repColorArray = colorArray;

            this.timelineBlocks.appendChild(bElement);
        }
    }

    blocks: Block[]
}

export class Block {
    element: HTMLElement;
    isSelected: boolean;
    color: string;
    minute: number;

    constructor(element: HTMLElement, minute: number, color: string) {
        this.color = color;
        this.element = element;
        this.minute = minute;
    }

    toggleSelection() {
        this.isSelected = !this.isSelected;
        this.updateSelection();
    }

    select() {
        this.isSelected = true;
        this.updateSelection();
    }

    deselect() {
        this.isSelected = false;
        this.updateSelection();
    }

    private updateSelection() {
        if (this.isSelected) {
            // Select
            this.element.style.backgroundColor = this.color;
        }
        else if (!this.isSelected) {
            // Deselect
            this.element.style.backgroundColor = 'white';

        }
    }
}

class dataModel {
    userId: number;
    role: any;
    minuteStart: number;
    minuteEnd: number;
}

const timelineComponents = (document.querySelectorAll('[timeline]'));
timelineComponents.forEach(comp => {
    let timeline = new Timeline((comp as HTMLElement));
    const url = comp.getAttribute('timeline-data-url');

    if (url) {
        fetch(url).then(j => {
            j.json().then((data: dataModel[]) => {
                data.forEach(i => {
                    timeline.addRow(i.role.name, i.role);
                })
            });
        })
    }

});