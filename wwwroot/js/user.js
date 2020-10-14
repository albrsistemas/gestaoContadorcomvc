//function validaSenhaUsuario(valor) {    
//    var senha = document.getElementById("usuario_senha").value;

//    if (senha != valor) {
//        document.getElementById("spanConfirmeSenhaUsuario").innerHTML = "Senha não confere";

//        return false;
//    }
//}

//function limpaValidacao() {
//    document.getElementById("spanConfirmeSenhaUsuario").innerHTML = "";
//}

function checkPermissoes(id) {
    console.log(id);
    let local = id.replace("List", "");
    //Facilitador de checagem para o usuário
    if (id.includes("List")) {
        if (!document.getElementById(local + "List").checked) {
            document.getElementById(local + "Create").checked = false;
            document.getElementById(local + "Edit").checked = false;
            document.getElementById(local + "Delete").checked = false;
        } else {
            document.getElementById(local + "Create").checked = true;
            document.getElementById(local + "Edit").checked = true;
            document.getElementById(local + "Delete").checked = true;
        }
    }
    

    //Varrengo os inputs e atribuindo valores ao campo usuario_permissoes
    //var strPermissoes = "";
    //var inputs = [];
    //inputs = document.querySelectorAll("input");

    //for (let x = 0; x < inputs.length; x++) {
    //    if (inputs[x].type == "checkbox" && inputs[x].checked) {
    //        strPermissoes += inputs[x].value + "|";
    //    }
    //}
    //document.getElementById("inputPermissao").value = strPermissoes;
    //console.log(document.getElementById("inputPermissao").value);
}