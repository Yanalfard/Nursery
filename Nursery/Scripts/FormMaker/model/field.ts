import { TblField } from "./db/tblField";
import { TblValue } from "./db/tblValue";
import { InputType } from "./inputType";

export class Field {
    public readonly data: TblField;
    element: HTMLElement;
    lblError: HTMLLabelElement;
    id: string;

    constructor(data: TblField) {
        this.data = data;
        let templateString: string = '';

        this.id = "I" + data.Label.split('').map(i => (i.charCodeAt(0) - 1700)).join('') + "O";

        data.Options = (data.Options as string).split(',');

        switch (data.Type) {
            case InputType.checkbox:
                templateString =
                    `
                <label class="fg-label uk-margin-auto-left row" for="${this.id}">${data.Label}
                <input class="uk-checkbox" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="checkbox">
                </label>
                   `;
                break;
            case InputType.textarea:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <textarea class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}"></textarea>
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.color:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="color">
                   `;
                break;
            case InputType.date:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="date" placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.dateTime:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="dateTime" placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.email:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="email" placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.file:
                console.log('hello world');
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <label class="comp-upload" preview="">
                    ${(this.data.Options as string[]).join(' ,')}
                    <input id="${this.id}" type="file" name="files" accept="${(this.data.Options as string[]).map(i => '.' + i).join(',')}" />
                </label>
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.hidden:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="hidden">
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.image:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} alt="${data.Label}" type="image">
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.month:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="month" placeholder="${data.Placeholder}">
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.number:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="number" placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.password:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="password" placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.radio:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                ${data.Options?.map(o => `<label class="radio"><span>${o}</span><input id="${data.Label + data.Options.indexOf(o)}" name="${this.id}" class="uk-radio" value="${o}" type="radio"></label>`).join('')}
                    `;
                break;
            case InputType.range:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="uk-range" id="${data.Label.toString()}" ${data.IsRequired ? 'required' : ''} min="${data.Options[0]}" max="${data.Options[1]}" type="range" placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}">
                   `;
                break;
            case InputType.reset:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" type="reset" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}">
                   `;
                break;
            case InputType.search:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" type="search" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.submit:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" type="submit" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}">
                   `;
                break;
            case InputType.tel:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" type="tel" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.text:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" type="text" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.time:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" type="time" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.url:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" type="url" ${data.IsRequired ? 'required' : ''} placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.week:
                templateString =
                    `
                <label class="fg-label" for="${this.id}">${data.Label}</label>
                <input class="entry" id="${this.id}" ${data.IsRequired ? 'required' : ''} type="week" placeholder="${data.Placeholder}" pattern="${data.Validations[0]?.Regex}">
                <span class="text-danger"></span>
                   `;
                break;
            case InputType.combo:
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
        templateString = `<div class="fg-col ${data.IsRequired ? 'fg-required' : ''}">${templateString.trim()}</div>`

        const parser = new DOMParser();
        this.element = (parser.parseFromString(templateString, 'text/html').body);
        this.lblError = this.element.querySelector('.text-danger');
    }

    /** Gets the field's Value.*/
    getVal(): TblValue {
        const ans = new TblValue();
        ans.Value = null;
        let input: any;

        switch (this.data.Type) {
            case InputType.checkbox:
                input = document.querySelector(`input#${this.id}[type="checkbox"]`);
                ans.Value = input.checked;
                break;
            case InputType.combo:
                input = document.querySelector(`select#${this.id}`);
                ans.Value = input.value;
                break;
            case InputType.radio:
                input = document.querySelector(`input[name="${this.id}"][type="radio"]:checked`);
                ans.Value = input?.value;
                break;
            case InputType.file:
                input = document.querySelector(`input#${this.id}[type="file"]`);
                ans.Value = input.value;
                break;
            case InputType.image:
                input = document.querySelector(`input#${this.id}[type="image"]`);
                ans.Value = input.value;
                break;
            default:
                input = document.querySelector(`#${this.id}`);
                ans.Value = input.value;
                break;
        }

        console.log(ans)

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

        for (const validationRule of this.data.Validations) {
            if (!validationRule.Regex) continue;
            if (!validationRule.Regex.test) continue;
            if (!validationRule.Regex.test(this.getVal().Value)) {
                if (this.lblError) this.lblError.innerText = validationRule.ValidationMessage;
                return false;
            }
        }

        //- pass
        if (this.lblError) this.lblError.innerText = '';
        return true;
    }
}
