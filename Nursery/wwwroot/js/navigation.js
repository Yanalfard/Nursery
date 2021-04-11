var accordions = document.querySelectorAll('[accordion]')
var navi = document.querySelector('.m-aside')

for (let acc of accordions) {

    const trigger = acc.querySelector('[accordion-trigger]');
    let ul = acc.querySelector('ul');
    let closed = true;
    trigger.addEventListener('click', () => {
        ul.classList.toggle('open');
    });
    navi.addEventListener("mouseleave", () => { ul.classList.remove('open') })
}
