/**
 * 
 * @param {string} attribute
 */
function sanitizeAttribute(attribute) {

    if (attribute.startsWith('{')) attribute = attribute.substring(1, attribute.length - 1);
    if (attribute.startsWith('}')) attribute = attribute.substring(0, attribute.length - 2);

    const keyValues = attribute.split(';');
    if (keyValues.length === 1)
        return undefined;

    let obj = {};

    for (let item of keyValues) {

        const split = item.split(':');
        obj[split[0]] = split[1];
    }

    return obj;
}

//#region Gauge

//const circles = document.getElementsByClassName('progress-ring__circle');
//let counter = 0;
//for (let circle of circles) {

//    const container = document.getElementsByClassName('progress-ring-container')[counter];
//    const svg = document.getElementsByClassName('progress-ring')[counter];


//    const percentage = container.getAttribute("percentage");
//    const size = Number.parseFloat(container.getAttribute("size"));

//    container.style.width = `${size}px`;
//    container.style.height = `${size}px`;

//    svg.setAttribute('width', size);
//    svg.setAttribute('height', size);

//    circle.setAttribute('cx', size / 2);
//    circle.setAttribute('cy', size / 2);
//    circle.setAttribute('r', (size / 2) / 1.071428571428571);

//    var radius = circle.r.baseVal.value;
//    var circumference = radius * 2 * Math.PI;

//    circle.style.strokeDasharray = `${circumference} ${circumference}`;
//    circle.style.strokeDashoffset = `${circumference}`;

//    function setProgress(percent) {
//        const offset = circumference - percent / 100 * circumference;
//        circle.style.strokeDashoffset = offset;
//    }



//    setProgress(parseInt(percentage));

//    counter++;
//}

//#endregion

const gauges = document.querySelectorAll('[gauge]');

for (let gauge of gauges) {

    options = sanitizeAttribute(gauge.getAttribute('gauge'));
    options.size = Number.parseInt(options.size);
    options.percent = Number.parseInt(options.percent);
    options.thickness = options.thickness ? options.thickness : 3;

    const circumference = Number.parseInt((options.size - (2 * options.thickness)) * Math.PI);
    const offset = Number.parseInt(circumference - options.percent / 100 * circumference);

    const svg = `
    <svg width="${options.size}" height="${options.size}" class="gauge">
        <circle cx="${options.size / 2}" cy="${options.size / 2}" r="${(options.size / 2) - (options.thickness)}"
            class="gauge-progress"
            stroke-width="${options.thickness}"
            stroke-dasharray="${circumference} ${circumference}"
            stroke-dashoffset="${offset}"/>
        <circle cx="${options.size / 2}" cy="${options.size / 2}" r="${(options.size / 2) - (options.thickness)}"
            stroke-width="${options.thickness}"
            class="gauge-background"/>
        <div class="gauge-content">${gauge.innerHTML}</div>
    </svg>`;

    gauge.innerHTML = svg;

    gauge.style.width = `${options.size}px`;
    gauge.style.height = `${options.size}px`;

    const progress = gauge.querySelector('.gauge-progress');
    progress.style.strokeDashoffset = circumference;
    setTimeout(() => {
        progress.style.strokeDashoffset = offset;
    }, 100);
}