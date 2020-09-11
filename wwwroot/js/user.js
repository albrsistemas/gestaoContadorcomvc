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

    //Facilitador de checagem para o usuário
    if (id == "usuarioList") {
        if (!document.getElementById("usuarioList").checked) {            
            document.getElementById("usuarioInsert").checked = false;
            document.getElementById("usuarioEdit").checked = false;
            document.getElementById("usuarioDelete").checked = false;
        } else {            
            document.getElementById("usuarioInsert").checked = true;
            document.getElementById("usuarioEdit").checked = true;
            document.getElementById("usuarioDelete").checked = true;
        }
    }



    //Varrengo os inputs e atribuindo valores ao campo usuario_permissoes
    var strPermissoes = "";
    var inputs = [];
    inputs = document.querySelectorAll("input");

    for (let x = 0; x < inputs.length; x++) {
        if (inputs[x].type == "checkbox" && inputs[x].checked) {
            strPermissoes += inputs[x].value + "|";            
        }
    }    
    document.getElementById("inputPermissao").value = strPermissoes;
    console.log(document.getElementById("inputPermissao").value);
}