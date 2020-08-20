//Funcionamento do Drawer
let btn = document.getElementById('bread_menu');
btn.addEventListener('click', function () {
    let drawer = document.getElementById('drawer');
    (drawer.style.right === '0px') ? (drawer.style.right = '-350px') : (drawer.style.right = '0px');
});
