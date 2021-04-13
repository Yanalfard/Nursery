const attributeKey = 'popconfirm';

export class ConfirmSettings {
    title: string = 'تایید';
    description: string = 'آیا مطمئن هستید؟';
    type: ConfirmType = ConfirmType.default;
    okText: string = 'بله';
    cancelText: string = 'خیر';
    direction: Direction = Direction.tc;

    constructor(
        title: string = 'تایید',
        description: string = 'آیا مطمئن هستید؟',
        type: ConfirmType = ConfirmType.default,
        okText: string = 'بله',
        cancelText: string = 'خیر',
        direction: Direction = Direction.tc) {
        this.title = title;
        this.description = description;
        this.type = type;
        this.okText = okText;
        this.cancelText = cancelText;
        this.direction = direction;
    }
}

export class Confirm {
    okEval: string;
    cancelEval: string;
    settings: ConfirmSettings;
    //-
    parent: HTMLElement;
    element: HTMLElement;
    btnOk: HTMLButtonElement;
    btnCancel: HTMLButtonElement;

    constructor(parent: HTMLElement, okEval: string, cancelEval?: string, settings?: ConfirmSettings) {

        this.settings = new ConfirmSettings();
        this.settings.title = settings?.title || this.settings.title;
        this.settings.description = settings?.description || this.settings.description;
        this.settings.okText = settings?.okText || this.settings.okText;
        this.settings.direction = settings?.direction || this.settings.direction;
        this.settings.cancelText = settings?.cancelText || this.settings.cancelText;

        this.cancelEval = cancelEval;
        this.okEval = okEval;

        this.parent = parent;

        let template =
            `<div class="popconfirm">
                <div class="popconfirm-header">
                    ${this.settings.title}
                </div>
                <div class="popconfirm-body">
                    <p class="popconfirm-description">
                        ${this.settings.description}
                    </p>
                </div>
                <div class="popconfirm-footer">
                    <button btnOk class="popconfirm-btn popconfirm-ok btn-primary">${this.settings.okText}</button>
                    <button btnCancel class="popconfirm-btn popconfirm-cancel btn-secondary">${this.settings.cancelText}</button>
                </div>
            </div>`

        this.element = new DOMParser().parseFromString(template, 'text/html').querySelector('.popconfirm');
        this.btnOk = this.element.querySelector('[btnOk]');
        this.btnCancel = this.element.querySelector('[btnCancel]');

        this.btnOk.addEventListener('click', () => { this.ok() });
        this.btnCancel.addEventListener('click', () => { this.cancel() });

        const offet = 16;

        let left: number = 0;
        let top: number = 0;

        document.body.appendChild(this.element);

        let calculateDeltas = () => {
            switch (Direction[this.settings.direction.toString()]) {
                case Direction.bl:
                    top = parent.offsetTop + parent.offsetHeight + offet;
                    left = parent.offsetLeft;

                    break;
                case Direction.br:
                    top = parent.offsetTop + parent.offsetHeight + offet;
                    left = parent.offsetLeft - (this.element.offsetWidth - parent.offsetWidth);

                    break;
                case Direction.lb:
                    top = (parent.offsetTop + parent.offsetHeight) - this.element.offsetHeight
                    left = parent.offsetLeft - this.element.offsetWidth - offet;

                    break;
                case Direction.lt:
                    top = parent.offsetTop;
                    left = parent.offsetLeft - this.element.offsetWidth - offet;

                    break;
                case Direction.tl:
                    top = parent.offsetTop - this.element.offsetHeight - offet;
                    left = parent.offsetLeft;
                    break;
                case Direction.tr:
                    top = parent.offsetTop - this.element.offsetHeight - offet;
                    left = parent.offsetLeft - (this.element.offsetWidth - parent.offsetWidth);

                    break;
                case Direction.rt:
                    top = parent.offsetTop;
                    left = parent.offsetLeft + parent.offsetWidth + offet;

                    break;
                case Direction.rb:
                    top = (parent.offsetTop + parent.offsetHeight) - this.element.offsetHeight;
                    left = parent.offsetLeft + parent.offsetWidth + offet;

                    break;
                case Direction.rc:
                    top = (parent.offsetTop + (parent.offsetHeight / 2)) - (this.element.offsetHeight / 2);
                    left = parent.offsetLeft + parent.offsetWidth + offet;

                    break;
                case Direction.tc:
                    top = parent.offsetTop - this.element.offsetHeight - offet;
                    left = parent.offsetLeft + (parent.offsetWidth / 2) - (this.element.offsetWidth / 2);

                    break;
                case Direction.bc:
                    top = parent.offsetTop + parent.offsetHeight + offet;
                    left = parent.offsetLeft + (parent.offsetWidth / 2) - (this.element.offsetWidth / 2);

                    break;
                case Direction.lc:
                    top = (parent.offsetTop + (parent.offsetHeight / 2)) - (this.element.offsetHeight / 2);
                    left = parent.offsetLeft - this.element.offsetWidth - offet;

                    break;
                default:
                    top = parent.offsetTop + this.element.offsetHeight + offet;
                    left = parent.offsetLeft;

                    break;
            }
            this.element.style.left = left + 'px';
            this.element.style.top = top + 'px';
        }

        window.addEventListener('scroll', () => {
            calculateDeltas();
        });
        calculateDeltas();

        window.addEventListener('click', (e: any) => {
            if (!e.path.includes(this.element) && !e.path.includes(this.parent)) {
                this.hide();
                if (this.element.style.display == 'block') {
                    this.cancel();
                }
            }
            else
                this.show();
        });

        this.hide();
    }

    show() {
        this.element.style.display = 'block';
        this.element.style.transition = 'all ease 0.3s';
        this.element.style.opacity = '1';
        // TODO Calculate deltas when shown!!!
    }

    hide() {
        this.element.style.transition = 'all ease 0.3s';
        this.element.style.opacity = '0';
        setTimeout(() => {
            this.element.style.display = 'none';
        }, 300);
    }

    cancel() {
        setTimeout(() => {
            this.hide();
            eval(this.cancelEval);
        }, 1)
    }

    ok() {
        setTimeout(() => {
            this.hide();
            eval(this.okEval);
        }, 1)
    }

}

export enum Direction {
    tl, tc, tr, rt, rc, rb, br, bc, bl, lb, lc, lt
}

export enum ConfirmType {
    default, info, warning, danger
}

const confirmables = document.querySelectorAll(`[${attributeKey}]`);
confirmables.forEach(parent => {
    const ok = parent.getAttribute('popok') || undefined;
    const cancel = parent.getAttribute('popcancel') || undefined;

    let settings = sanitizeAttribute(parent.getAttribute(attributeKey)) as ConfirmSettings;

    let conf = new Confirm(parent as HTMLElement, ok, cancel, settings);
})

/**
 * 
 * @param {string} attribute
 */
function sanitizeAttribute(attribute) {

    if (attribute.startsWith('{')) attribute = attribute.substring(1, attribute.length - 1);
    if (attribute.startsWith('}')) attribute = attribute.substring(0, attribute.length - 2);

    if (!attribute)
        return undefined;

    const keyValues = attribute.split(';');


    let obj = {};

    for (let item of keyValues) {

        const split = item.split(':');
        obj[split[0]] = split[1];
    }

    return obj;
}
