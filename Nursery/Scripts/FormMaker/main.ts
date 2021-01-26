class tblForm {
    Id: number = -1;
    Name: string = '';
    Body: string = '';
    DateCreated: Date;
    IsDeleted: boolean = false;

    constructor() { }
}

class tblRegex {
    Id: number = -1;
    Regex: RegExp = new RegExp('');
    Label: string = '';
    IsDeleted: boolean = false;

    constructor(regex: RegExp, label?: string) {
        this.Regex = regex;
        this.Label = label ?? '';
    }
}

class tblField {
    Id: number = -1;
    FormId: number = -1;
    Label: string = '';
    Type: Type = Type.text;
    IsRequired: boolean = false;
    Options: string[] = [];
    Placeholder: string = '';
    Tooltip: string = '';
    IsDeleted: boolean = false;

    validations: tblRegex[] = [];

    constructor() { }
}

class tblValue {
    Id: number = -1;
    UserId: number = -1;
    Value: string = '';
    IsDeleted: boolean = false;

    constructor() { }
}

enum Type {
    combo,
    textarea,
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
tblField1.IsRequired = true;
tblField1.Type = Type.checkbox;
tblField1.Label = "مرا به خاطر بسپار";

const tblField2 = new tblField();
tblField2.Id = 1;
tblField2.FormId = 0;
tblField2.IsRequired = false;
tblField2.Type = Type.tel;
tblField2.validations.push(new tblRegex(exp, 'شماره تلفن صحیح نمی باشد'));
tblField2.Label = 'شماره تلفن';

const tblField3 = new tblField();
tblField3.Id = 2;
tblField3.FormId = 0;
tblField3.IsRequired = true;
tblField3.Type = Type.radio;
tblField3.Options = ['مرد', 'زن', 'ترجیح میدم نگم'];
tblField3.Label = 'جنسیت';

const tblField4 = new tblField();
tblField4.Id = 4;
tblField4.FormId = 0;
tblField4.IsRequired = false;
tblField4.Type = Type.combo;
tblField4.Options = ['سیب', 'موز', 'هلو', 'آلبالو'];
tblField4.Label = 'میوه مورد علاقه';

const tblField5 = new tblField();
tblField5.Id = 5;
tblField5.FormId = 0;
tblField5.IsRequired = true;
tblField5.Type = Type.text;
tblField5.Label = 'فیلد ضروری';

const tblFields = [tblField1, tblField2, tblField3, tblField4, tblField5];
//const tblFields = [];

//for (var i = 0; i < 21; i++) {
//    const f = new tblField();
//    f.Id = i;
//    f.FormId = 0;
//    f.Type = i;
//    f.Options = ['سیب', 'موز', 'هلو', 'آلبالو'];
//    f.Label = Type[i];
//    f.validations.push(new tblRegex(exp, 'Error'));
//    tblFields.push(f);
//}

//#endregion

class Form {
    element: HTMLFormElement;
    header: HTMLElement;
    body: HTMLElement;
    footer: HTMLElement;
    submit: HTMLButtonElement;

    public readonly data: tblForm;

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
        newField.element.childNodes.forEach(node => this.body.appendChild(node));
        return;
    }

    attachForm(element: HTMLElement) { return element.appendChild(this.element) }

    validate(): boolean {

        let ans: boolean = true;

        for (var field of this.Fields)
            if (!field.validate()) ans = false;

        return ans;
    }
}

class Field {
    public readonly data: tblField;
    element: HTMLElement;
    lblError: HTMLLabelElement;
    id: string;

    constructor(data: tblField) {
        this.data = data;
        let templateString: string = '';

        this.id = "I" + data.Label.split('').map(i => (i.charCodeAt(0) - 1700)).join('') + "O";

        switch (data.Type) {
            case Type.checkbox:
                templateString =
                    `
                <label class="fg-label uk-margin-auto-left row" for="${this.id}">${data.Label}
                <input class="uk-checkbox" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="checkbox">
                </label>
                   `;
                break;
            case Type.textarea:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <textarea class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}"></textarea>
                <span class="text-danger"></span>
                   `;
                break;
            case Type.color:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="color">
                   `;
                break;
            case Type.date:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="date" placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.dateTime:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="dateTime" placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.email:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="email" placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.file:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="file">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.hidden:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="hidden">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.image:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} alt="${data.Label}" type="image">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.month:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="month" placeholder="${data.Placeholder}">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.number:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="number" placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.password:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="password" placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.radio:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                ${data.Options.map(o => `<label class="radio"><span>${o}</span><input id="${data.Label + data.Options.indexOf(o)}" name="${this.id}" class="uk-radio" value="${o}" type="radio"></label>`).join('')}
                    `;
                break;
            case Type.range:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="uk-range" id="${data.Label.toString()}" ${data.IsRequired ? 'required' : ''} min="${data.Options[0]}" max="${data.Options[1]}" type="range" placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}">
                   `;
                break;
            case Type.reset:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" type="reset" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}">
                   `;
                break;
            case Type.search:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" type="search" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.submit:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" type="submit" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}">
                   `;
                break;
            case Type.tel:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" type="tel" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.text:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" type="text" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.time:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" type="time" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.url:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" type="url" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.week:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="week" placeholder="${data.Placeholder}" pattern="${data.validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case Type.combo:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <select class="entry" id="${this.id}">
                    ${data.Options.map(o => (`<option>${o}</option>`)).join('')}
                </select>
                   `;
                break;
            default:
                console.log('type unknown');
                break;
        }

        //fg-col
        templateString = `<div class="fg-col ${data.IsRequired ? 'fg-required' : ''}">${templateString}</div>`

        const parser = new DOMParser();
        this.element = (parser.parseFromString(templateString, 'text/html').body);
        this.lblError = this.element.querySelector('.text-danger');
    }

    /** Gets the field's Value.*/
    getVal(): tblValue {
        const ans = new tblValue();
        ans.Value = null;
        let input: any;

        switch (this.data.Type) {
            case Type.checkbox:
                input = document.querySelector(`input#${this.id}[type="checkbox"]`);
                ans.Value = input.checked;
                break;
            case Type.combo:
                input = document.querySelector(`select#${this.id}`);
                ans.Value = input.value;
                break;
            case Type.radio:
                input = document.querySelector(`input[name="${this.id}"][type="radio"]:checked`);
                ans.Value = input?.value;
                break;
            case Type.file:
                input = document.querySelector(`input#${this.id}[type="file"]`);
                ans.Value = input.value;
                break;
            case Type.image:
                input = document.querySelector(`input#${this.id}[type="image"]`);
                ans.Value = input.value;
                break;
            default:
                input = document.querySelector(`#${this.id}`);
                ans.Value = input.value;
                break;
        }

        return ans;
    }

    /** Validates the field's Value.
     * Shows the first error in it's validation list.
     * Clears the Errorprovider if there are no errors */
    validate(): boolean {

        if (this.data.IsRequired) {
            if (this.getVal() === null
                || this.getVal() === undefined
                || this.getVal().Value === null
                || this.getVal().Value === ''
                || this.getVal().Value === undefined) {
                if (this.lblError) this.lblError.innerText = 'این فیلد اجباری است';
                return false;
            }
        }

        for (const validationRule of this.data.validations) {
            if (!validationRule.Regex.test(this.getVal().Value)) {
                if (this.lblError) this.lblError.innerText = validationRule.Label;
                return false;
            }
        }

        if (this.lblError) this.lblError.innerText = '';
        return true;
    }

}

let oi = new Form(form1);
tblFields.forEach(f => oi.addField(f));
oi.attachForm(document.getElementById('container'));
oi.submit.addEventListener('click', submit);

function submit() {
    //oi.Fields.forEach(i => console.log(i.getVal()))
    //oi.validate();

    if (!oi.validate()) return;
    oi.Fields.forEach(i => console.log(i.getVal()));

    //console.log(oi.Fields[0].getVal());
}

