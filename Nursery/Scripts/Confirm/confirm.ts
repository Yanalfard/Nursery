const attributeKey = 'popconfirm';

export class ConfirmSettings {
    title: string = 'تایید';
    description: string = 'آیا مطمئن هستید؟';
    type: ConfirmType = ConfirmType.default;
    okText: string = 'بله';
    cancelText: string = 'خیر';
    floating: boolean = false;
    direction: Direction = Direction.tc;

    constructor(
        title: string = 'تایید',
        description: string = 'آیا مطمئن هستید؟',
        type: ConfirmType = ConfirmType.default,
        okText: string = 'بله',
        cancelText: string = 'خیر',
        floating: boolean = false,
        direction: Direction = Direction.tc) {
        this.title = title;
        this.description = description;
        this.type = type;
        this.okText = okText;
        this.cancelText = cancelText;
        this.direction = direction;
        this.floating = floating;
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
    private arrow: HTMLElement;

    constructor(parent: HTMLElement, okEval: string, cancelEval?: string, settings?: ConfirmSettings) {

        this.settings = new ConfirmSettings();
        this.settings.title = settings?.title || this.settings.title;
        this.settings.description = settings?.description || this.settings.description;
        this.settings.okText = settings?.okText || this.settings.okText;
        this.settings.floating = settings?.floating || this.settings.floating;
        this.settings.cancelText = settings?.cancelText || this.settings.cancelText;
        this.settings.direction = settings?.direction || this.settings.direction || Direction.tc as any;
        this.settings.type = settings?.type || this.settings.type || ConfirmType.default;

        this.cancelEval = cancelEval;
        this.okEval = okEval;

        this.parent = parent;

        let template =
            `<div class="popconfirm">
                <div class="popconfirm-arrow"></div>
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
        this.arrow = this.element.querySelector('.popconfirm-arrow');

        this.btnOk.innerText = this.settings.okText;
        this.btnCancel.innerText = this.settings.cancelText;

        this.btnOk.addEventListener('click', () => { this.ok() });
        this.btnCancel.addEventListener('click', () => { this.cancel() });

        this.settings.type = ConfirmType[this.settings.type] as any || ConfirmType.default;

        this.btnOk.classList.remove('btn-primary');
        switch (this.settings.type) {
            case ConfirmType.danger:
                this.btnOk.classList.add('btn-danger');
                break;
            case ConfirmType.default:
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

        window.addEventListener('scroll', () => {
            if (this.element.style.display == 'block') {
                this.cancel();
            }
        });
        window.addEventListener('resize', () => {
            if (this.element.style.display == 'block') {
                this.cancel();
            }
        });

        document.body.appendChild(this.element);

        this.calculateDeltas();

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

        this.element.style.display = 'none';
        this.hide();
    }

    // Calculate where the element should be positioned relative to its parent
    // Calculate where the arrow should be relative to the element
    calculateDeltas() {
        let left: number = 0;
        let top: number = 0;
        let parent = this.parent;
        const offet = 16;
        //- arrow
        let atop: number | string = '';
        let abottom: number | string = '';
        let aleft: number | string = '';
        let aright: number | string = '';

        const rect = parent.getBoundingClientRect();
        const ptop = (this.settings.floating) ? rect.top : parent.offsetTop;
        const pleft = (this.settings.floating) ? rect.left : parent.offsetLeft;

        switch (Direction[this.settings.direction.toString()]) {
            case Direction.bl:
                top = ptop + parent.offsetHeight + offet;
                left = pleft;
                atop = -10;
                aleft = 16;
                break;
            case Direction.br:
                top = ptop + parent.offsetHeight + offet;
                left = pleft - (this.element.offsetWidth - parent.offsetWidth);
                atop = -10;
                aright = 16;
                break;
            case Direction.lb:
                top = (ptop + parent.offsetHeight) - this.element.offsetHeight
                left = pleft - this.element.offsetWidth - offet;
                aright = -10;
                abottom = 16;
                break;
            case Direction.lt:
                top = ptop;
                left = pleft - this.element.offsetWidth - offet;
                aright = -10;
                atop = 16;
                break;
            case Direction.tl:
                top = ptop - this.element.offsetHeight - offet;
                left = pleft;
                abottom = -10;
                aleft = 16;
                break;
            case Direction.tr:
                top = ptop - this.element.offsetHeight - offet;
                left = pleft - (this.element.offsetWidth - parent.offsetWidth);
                abottom = -10;
                aright = 16;
                break;
            case Direction.rt:
                top = ptop;
                left = pleft + parent.offsetWidth + offet;
                aleft = -10;
                atop = 16;
                break;
            case Direction.rb:
                top = (ptop + parent.offsetHeight) - this.element.offsetHeight;
                left = pleft + parent.offsetWidth + offet;
                aleft = -10;
                abottom = 16;
                break;
            case Direction.rc:
                top = (ptop + (parent.offsetHeight / 2)) - (this.element.offsetHeight / 2);
                left = pleft + parent.offsetWidth + offet;
                aleft = -10;
                atop = abottom = (this.element.offsetHeight - this.arrow.offsetHeight) / 2;
                break;
            case Direction.lc:
                top = (ptop + (parent.offsetHeight / 2)) - (this.element.offsetHeight / 2);
                left = pleft - this.element.offsetWidth - offet;
                aright = -10;
                atop = abottom = (this.element.offsetHeight - this.arrow.offsetHeight) / 2;
                break;
            case Direction.tc:
                top = ptop - this.element.offsetHeight - offet;
                left = pleft + (parent.offsetWidth / 2) - (this.element.offsetWidth / 2);
                abottom = -10;
                aleft = aright = (this.element.offsetWidth - this.arrow.offsetHeight) / 2;
                break;
            case Direction.bc:
                top = ptop + parent.offsetHeight + offet;
                left = pleft + (parent.offsetWidth / 2) - (this.element.offsetWidth / 2);
                atop = -10;
                aleft = aright = (this.element.offsetWidth - this.arrow.offsetHeight) / 2;
                break;
            default:
                top = ptop - this.element.offsetHeight - offet;
                left = pleft + (parent.offsetWidth / 2) - (this.element.offsetWidth / 2);
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
    }

    show() {
        this.element.style.display = 'block';
        this.element.style.transition = 'all ease 0.3s';
        this.element.style.transitionProperty = 'opacity';
        this.element.style.opacity = '1';
        this.calculateDeltas();
    }

    hide() {
        this.element.style.transition = 'all ease 0.2s ';
        this.element.style.transitionProperty = 'opacity';
        this.element.style.opacity = '0';
        setTimeout(() => {
            this.element.style.display = 'none';
        }, 200);
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
    default, success, warning, danger
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
        split[1] = (split[1] == 'true') ? true : (split[1] == 'false') ? false : split[1];
        obj[split[0]] = split[1];
    }

    return obj;
}

function isElementInViewport(element: HTMLElement) {
    const rect = element.getBoundingClientRect();
    return rect.top >= 0 &&
        rect.left >= 0 &&
        rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
        rect.right <= (window.innerWidth || document.documentElement.clientWidth);
}