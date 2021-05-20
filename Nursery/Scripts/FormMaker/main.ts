import { TblField } from "./model/db/tblField";
import { TblForm } from "./model/db/tblForm";
import { Form } from "./model/form";

const forms: any = document.querySelectorAll('dform');

for (var item of forms as HTMLFormElement[]) {

    const stringModel: string = item.getAttribute('model');
    const goto: string = item.getAttribute('goto');
    const sendto: string = item.getAttribute('sendto');
    const uploadto: string = item.getAttribute('uploadto');

    if (!sendto) {
        throw "attribute [sendto] was not set";
    }

    if (!uploadto) {
        throw 'attribute [uploadto] was not set';
    }

    if (!goto) {
        console.warn('attribute [goto] was not set, no redirection will be made after form submission');
    }


    const model = JSON.parse(stringModel) as TblForm;

    let form = new Form(model);
    form.goto = goto;
    form.sendto = sendto;
    form.uploadto = uploadto;

    for (var field of model.Fields) {
        form.addField(field);
    }

    form.element.enctype = 'multipart/form-data';
    form.element.method = 'post';
    form.attachForm(item);

    console.log(form);

    form.submitClick

}

