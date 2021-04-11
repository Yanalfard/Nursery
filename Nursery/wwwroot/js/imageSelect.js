const previewKey = 'preview';
const instances = document.querySelectorAll(`[${previewKey}]`);

for (let label of instances) {
    const input = label.querySelector('input[type=file]');

    const options = sanitizeAttribute(label.getAttribute(previewKey));

    input.addEventListener('change', () => imageSelected(label, input, options));
}

/**
 * 
 * @param {any} label
 * @param {any} input
 * @param {Object} options
 * @param {number} options.maxSize
 */
function imageSelected(label, input, options) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        if (options) {

            if (input.files[0].size > options.maxSize) {
                notify('فایل انتخاب شده بزرگ تر از ' + options.maxSize + ' می باشد','danger');
                return;
            }

        }

        reader.onload = function (e) {

            var img = new Image();
            img.src = e.target.result.toString();

            label.style.backgroundImage = `url(${img.src})`;
        }

        reader.readAsDataURL(input.files[0]); // convert to base64 string
    }
}

