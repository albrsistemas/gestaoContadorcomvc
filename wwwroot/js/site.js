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

function boxAccordionToggle(id) {
    var status = document.getElementById(id).parentElement.children[1].style.display;
    if (status == "" || status == null || status == "block") {
        document.getElementById(id).parentElement.children[1].style.display = "none";
    } else {
        document.getElementById(id).parentElement.children[1].style.display = "block";
    }
}

$(".delete").click(function () {
    var id = $(this).attr("data-id");    
    $("#modal").load("Delete?id=" + id, function () {
        $("#modal").modal('show');
    })
});

$(".EditPassword").click(function () {
    var id = $(this).attr("data-id");
    $("#modal").load("EditPassword?id=" + id, function () {
        $("#modal").modal('show');
    })
});

$(".createBco").click(function () {
    //var id = $(this).attr("data-id");    
    $("#modal").load("CreateCxBanco" , function () {
        $("#modal").modal('show');
    })
});

$(".deleteBco").click(function () {
    var id = $(this).attr("data-id");
    var descricao = $(this).attr("data-descricao");
    $("#modal").load("DeleteCxBanco?id=" + id + "&descricao=" + encodeURIComponent(descricao), function () {
        $("#modal").modal('show');
    })
});

$(".create").click(function () {
    var id = $(this).attr("data-id");
    $("#modal").load("Create?id=" + id, function () {
        $("#modal").modal('show');
    })
});

$(document).ready(function () {
    $('#Esconder').delay(3000).fadeOut();
});
