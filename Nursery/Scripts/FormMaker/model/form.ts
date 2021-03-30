import { TblField } from "./db/tblField";
import { TblForm } from "./db/tblForm";
import { Field } from "./field";

export class Form {
    element: HTMLFormElement;
    header: HTMLElement;
    body: HTMLElement;
    footer: HTMLElement;
    submit: HTMLButtonElement;

    public readonly data: TblForm;

    public Fields: Field[] = [];

    constructor(tblForm: TblForm) {
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
        this.submit.innerHTML = '<span>ثبت</span>';
        this.submit.addEventListener('click', (e) => { e.preventDefault(); return null; });
        this.footer.appendChild(this.submit);

        this.element.appendChild(this.header);
        this.element.appendChild(this.body);
        this.element.appendChild(this.footer);
    }

    addField(tblField: TblField): void {
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
