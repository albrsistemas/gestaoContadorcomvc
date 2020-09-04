//Onload página layout
function Page() {

    //Renderizando as informações do Drawer conforme a página específica:
    //let page = window.location.href;    
    //switch (page) {
    //    case 'https://localhost:44339/ContasPagar':
    //        document.getElementById('drawer').innerHTML = "";
    //        document.getElementById('drawer').innerHTML = "Página Contas a Pagar";
    //        break;
    //    case 'https://localhost:44339/':
    //        document.getElementById('drawer').style.display = 'none';            
    //        document.getElementById('bread_menu').style.display = 'none';            

    //        break;
    //    default:
    //        document.getElementById('drawer').innerHTML = "";                        
    //}
}

function ValidaRegistro(id) {
    if (id == "pj") {
        document.getElementById()
    }
}

function openNav(local) {
    var estado = document.getElementById("mySidenav").style.width;   
    
    if (estado != "250px") {
        document.getElementById("mySidenav").style.width = "250px";
        document.getElementById("corpo").style.marginLeft = "250px";
    } else {
        document.getElementById("mySidenav").style.width = "0";
        document.getElementById("corpo").style.marginLeft = "0";
    }
}
