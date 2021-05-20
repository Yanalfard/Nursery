import { TblField } from "./db/tblField";
import { TblForm } from "./db/tblForm";
import { TblValue } from "./db/tblValue";
import { Field } from "./field";
import { InputType } from "./inputType";

export class Form {
    dform: HTMLElement;
    element: HTMLFormElement;
    header: HTMLElement;
    body: HTMLElement;
    footer: HTMLElement;
    submit: HTMLButtonElement;
    public sendto: string;
    public goto: string;
    public uploadto: string;

    public readonly data: TblForm;

    public Fields: Field[] = [];
    public Uploadables: Field[] = [];

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
        this.submit.addEventListener('click', (e) => { this.submitClick(e) });
        this.footer.appendChild(this.submit);

        this.element.appendChild(this.header);
        this.element.appendChild(this.body);
        this.element.appendChild(this.footer);
    }

    addField(tblField: TblField): void {
        const newField = new Field(tblField);
        if (tblField.Type == InputType.file || tblField.Type == InputType.image) {
            this.Uploadables.push(newField);
        }
        else {
            this.Fields.push(newField);
        }
        newField.element.childNodes.forEach(node => this.body.appendChild(node));
        return;
    };

    attachForm(element: HTMLElement) { return element.appendChild(this.element) }

    validate(): boolean {

        let ans: boolean = true;

        for (var field of this.Fields)
            if (!field.validate()) ans = false;

        return ans;
    }

    submitClick(e) {

        //e.preventDefault(); return null;
        e.preventDefault();
        if (!this.validate()) return null;

        let res: TblValue[] = [];

        this.Fields.forEach(i => {
            let val = i.getVal();
            val.FormFieldId = i.data.FieldId;
            val.FormId = this.data.FormId;
            res.push(val);
        });

        eval('LoadingRun();');

        const formData = new FormData(this.element);
        console.log(formData.get('files'));

        try {
            // Upload files first
            fetch(this.uploadto, {
                method: 'POST',
                body: formData
            }).then(response => {
                (response.json().then((filenames: TblValue[]) => {
                    // Recieve file names

                    // Put FormId and FieldId back to values
                    for (var file = 0; file < filenames.length; file++) {
                        filenames[file].FormFieldId = this.Uploadables[file].data.FieldId;
                        filenames[file].FormId = this.Uploadables[file].data.FormId;
                    }

                    // Combine values to send and fileName
                    res = [...res, ...filenames];

                    // Then send data
                    fetch(this.sendto, {
                        method: 'post',
                        mode: 'cors',
                        cache: 'no-cache',
                        credentials: 'same-origin',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(res)
                    }).then(response => {
                        console.log('send data response', response);
                        // Go to wherever after submission
                        window.location.href = this.element.getAttribute('goto');
                    }).catch(() => {
                        eval('LoadingEnd();');
                    })
                }));
            }).catch()

        } catch (error) {
            console.error('Error:', error);
        }

        //this.element.submit();
    }

}
