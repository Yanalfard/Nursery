class tblForm {
    Id: number;
    Name: string;

    constructor() { }
}

class tblRegex {
    Id: number;
    Regex: RegExp;
    Label: string;

    constructor(regex: RegExp, label?: string) {
        this.Regex = regex;
        this.Label = label ?? '';
    }
}

class tblField {
    Id: number;
    FormId: number;
    Label: string;
    Type: Type;
    Options: string[] = [];

    validations: tblRegex[] = [];

    constructor() { }
};

class tblValue {
    Id: number;
    UserId: number;
    Value: string;

    constructor() { }
}

enum Type {
    combo,
    checkbox,
    color,
    date,
    dateTime,
    email,
    file,
    hidden,
    image,
    month,
    number,
    password,
    radio,
    range,
    reset,
    search,
    submit,
    tel,
    text,
    time,
    url,
    week,
}

//#region Data Definition

const form1 = new tblForm();
form1.Id = 0;
form1.Name = 'این نام فرم می باشد';

const exp = new RegExp("[0-9]{11}");

const tblField1 = new tblField();
tblField1.Id = 0;
tblField1.FormId = 0;
tblField1.Type = Type.checkbox;
tblField1.Label = "مرا به خاطر بسپار";

const tblField2 = new tblField();
tblField2.Id = 1;
tblField2.FormId = 0;
tblField2.Type = Type.tel;
tblField2.validations.push(new tblRegex(exp));
tblField2.Label = 'شماره تلفن';

const tblField3 = new tblField();
tblField3.Id = 2;
tblField3.FormId = 0;
tblField3.Type = Type.radio;
tblField3.Options = ['مرد', 'زن', 'ترجیح میدم نگم'];
tblField3.Label = 'جنسیت';

const tblField4 = new tblField();
tblField4.Id = 4;
tblField4.FormId = 0;
tblField4.Type = Type.combo;
tblField4.Options = ['سیب', 'موز', 'هلو', 'آلبالو'];
tblField4.Label = 'میوه مورد علاقه';

const tblFields = [tblField1, tblField2, tblField3, tblField4];
//#endregion

class Form {
    element: HTMLFormElement;
    header: HTMLElement;
    body: HTMLElement;
    footer: HTMLElement;
    submit: HTMLButtonElement;

    private data: tblForm;

    public Fields: Field[] = [];

    constructor(tblForm: tblForm) {
        this.data = tblForm;

        this.element = document.createElement('form');
        this.element.id = 'form';
        this.element.classList.add('form');
        //- header
        this.header = document.createElement('div');
        this.header.classList.add('form-header');
        this.header.innerHTML += `<h3>${this.data.Name}</h3>`;
        //- body
        this.body = document.createElement('div');
        this.body.classList.add('form-body');
        //- footer
        this.footer = document.createElement('div');
        this.footer.classList.add('form-footer');
        //- submit
        this.submit = document.createElement('button');
        this.submit.classList.add('btn');
        this.submit.classList.add('btn-primary');
        this.submit.innerText = 'ثبت';
        this.submit.addEventListener('click', (e) => { e.preventDefault(); return null; });
        this.footer.appendChild(this.submit);

        this.element.appendChild(this.header);
        this.element.appendChild(this.body);
        this.element.appendChild(this.footer);
    }

    addField(tblField: tblField): void {
        const newField = new Field(tblField);
        this.Fields.push(newField);
        this.body.innerHTML += newField.element;
        return;
    }

    attachForm(element: HTMLElement) { return element.appendChild(this.element) }

    getInputValues(): tblValue[] {

        const ans: tblValue[] = [];
        for (const f of this.Fields) {
            ans.push(f.getVal());
        }

        //return this.Fields.map(f => f.getVal());
        return ans;
    }
}

class Field {
    private data: tblField;
    element: string;

    constructor(data: tblField) {
        this.data = data;

        switch (data.Type) {
            case Type.checkbox:
                this.element =
                    `<div class="fg-col">
                        <label class="fg-label" for="${data.Label}">${data.Label}</label>
                        <input class="entry" name="${data.Label}" type="checkbox">
                        <span class="text-danger"></span>
                    </div>`
                break;
            case Type.color:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="color">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.date:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="date" placeholder="${data.Label}" pattern="${data.validations[0].Regex}">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.dateTime:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="dateTime" placeholder="${data.Label}" pattern="${data.validations[0].Regex}">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.email:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="email" placeholder="${data.Label}" pattern="${data.validations[0].Regex}">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.file:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="file">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.hidden:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="hidden">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.image:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="image">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.month:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="month" placeholder="${data.Label}">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.number:
                this.element =
                    ` <div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="number" placeholder="${data.Label}" pattern="${data.validations[0].Regex}">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.password:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="password" placeholder="${data.Label}" pattern="${data.validations[0].Regex}">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.radio:
                this.element =
                    `<div class="fg-col">
                        <label class="fg-label" for="${data.Label}">${data.Label}</label>
                        ${data.Options.map(o => `<label class="radio"><span>${o}</span><input name="${data.Label}" value="${o}" type="radio"></label>`).join('')}
                        <span class="text-danger"></span>
                    </div>`
                break;
            case Type.range:
                this.element =
                    `<div class="fg-col">
                        <label class="fg-label" for="${data.Label}">${data.Label}</label>
                        <input class="entry" name="${data.Label}" type="range" placeholder="${data.Label}" pattern="${data.validations[0].Regex}">
                        <span class="text-danger"></span>
                    </div>`
                break;
            case Type.reset:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="reset" placeholder="${data.Label}" pattern="${data.validations[0].Regex}">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.search:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="search" placeholder="${data.Label}" pattern="${data.validations[0].Regex}">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.submit:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="submit" placeholder="${data.Label}" pattern="${data.validations[0].Regex}">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.tel:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="tel" placeholder="${data.Label}" pattern="${data.validations[0].Regex}">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.text:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="text" placeholder="${data.Label}" pattern="${data.validations[0].Regex}">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.time:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="time" placeholder="${data.Label}" pattern="${data.validations[0].Regex}">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.url:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="url" placeholder="${data.Label}" pattern="${data.validations[0].Regex}">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.week:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <input class="entry" name="${data.Label}" type="week" placeholder="${data.Label}" pattern="${data.validations[0].Regex}">
                <span class="text-danger"></span>
            </div>`
                break;
            case Type.combo:
                this.element =
                    `<div class="fg-col">
                <label class="fg-label" for="${data.Label}">${data.Label}</label>
                <select class="entry" name="${data.Label}">
                    ${data.Options.map(o => (`<option>${o}</option>`)).join('')}
                </select>
                <span class="text-danger"></span>
            </div>`;
                break;
            default:
                console.log('type unknown');
                break;
        }
    }

    getVal(): tblValue {
        const ans = new tblValue();
        let input: any;

        switch (this.data.Type) {
            case Type.checkbox:
                input = document.querySelector(`input[name="${this.data.Label}"][type="checkbox"]`);
                ans.Value = input.checked;
                console.log(input);
                break;
            case Type.combo:
                input = document.querySelector(`select[name="${this.data.Label}"]`);
                ans.Value = input.value;
                console.log(input);
                break;
            case Type.radio:
                input = document.querySelector(`input[name="${this.data.Label}"][type="radio"]`);
                ans.Value = input.value;
                console.log(input);
                break;
            case Type.file:
                input = document.querySelector(`input[name="${this.data.Label}"][type="file"]`);
                ans.Value = input.value;
                console.log(input);
                break;
            case Type.image:
                input = document.querySelector(`input[name="${this.data.Label}"][type="image"]`);
                ans.Value = input.value;
                console.log(input);
                break;
            default:
        }

        return
    }
}

let oi = new Form(form1);
tblFields.forEach(f => oi.addField(f));
oi.attachForm(document.getElementById('container'));
oi.submit.addEventListener('click', () => { oi.Fields.forEach(i => console.log(i.getVal())) })