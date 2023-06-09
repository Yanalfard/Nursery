﻿var accordions = document.querySelectorAll('[accordion]');
var navi = document.querySelector('.m-aside');
var btnHamburger = document.getElementById('hamburger');

for (let acc of accordions) {

    const trigger = acc.querySelector('[accordion-trigger]');
    let ul = acc.querySelector('ul');
    let closed = true;
    trigger.addEventListener('click', () => {
        ul.classList.toggle('open');
    });
    navi.addEventListener("mouseleave", () => { ul.classList.remove('open') })
}

// btn hamburger clicked
btnHamburger.addEventListener('click', () => {
    navi.style.transition = 'all ease 0.4s';
    if (navi.style.marginRight == '0px') {
        // hide
        navi.style.marginRight = '-222px'
    }
    else {
        // show
        navi.style.marginRight = '0px';
    }
});

window.addEventListener('resize', () => {
    if (window.innerWidth > 512) {
        navi.style.marginRight = '0px';
    }
})

window.addEventListener('click', (e) => {
    if (window.innerWidth < 512) {
        if (!e.path.includes(navi) && !e.path.includes(btnHamburger)) {
            if (navi.style.marginRight == '0px') {
                navi.style.marginRight = '-222px';
            }
        }
    }
})