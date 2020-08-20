//Funcionamento do Drawer
let btn = document.getElementById('bread_menu');
btn.addEventListener('click', function () {
    let drawer = document.getElementById('drawer');
    (drawer.style.right === '0px') ? (drawer.style.right = '-350px') : (drawer.style.right = '0px');
});

//Onload página layout
function Page() {

    //Renderizando as informações do Drawer conforme a página específica:
    let page = window.location.href;    
    switch (page) {
        case 'https://localhost:44339/ContasPagar':
            document.getElementById('drawer').innerHTML = "";
            document.getElementById('drawer').innerHTML = "Página Contas a Pagar";
            break;
        case 'https://localhost:44339/':
            document.getElementById('drawer').style.display = 'none';            
            document.getElementById('bread_menu').style.display = 'none';            

            break;
        default:
            document.getElementById('drawer').innerHTML = "";                        
    }
}
