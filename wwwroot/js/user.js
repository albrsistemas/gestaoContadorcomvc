function checkPermissoes(id) {
    let chave = "";
    if (id.includes("List")) {
        chave = id.replace("List", "");
    }
    if (id.includes("Create")) {
        chave = id.replace("Create", "");
    }
    if (id.includes("Edit")) {
        chave = id.replace("Edit", "");
    }
    if (id.includes("Delete")) {
        chave = id.replace("Delete", "");
    }

    let idList = chave + "List";

    if (idList == id) {
        if (document.getElementById(idList).checked) {
            document.getElementById(chave + "Create").removeAttribute("disabled");
            document.getElementById(chave + "Edit").removeAttribute("disabled");
            document.getElementById(chave + "Delete").removeAttribute("disabled");

            document.getElementById(chave + "Create").checked = true;
            document.getElementById(chave + "Edit").checked = true;
            document.getElementById(chave + "Delete").checked = true;
        } else {
            document.getElementById(chave + "Create").checked = false;
            document.getElementById(chave + "Edit").checked = false;
            document.getElementById(chave + "Delete").checked = false;

            document.getElementById(chave + "Create").setAttribute("disabled", "disabled");
            document.getElementById(chave + "Edit").setAttribute("disabled", "disabled");
            document.getElementById(chave + "Delete").setAttribute("disabled", "disabled");
        }
    } else {
        if (document.getElementById(idList).checked) {
            document.getElementById(chave + "Delete").removeAttribute("disabled");
        } else {
            document.getElementById(id).checked = false;
            document.getElementById(chave + "Create").setAttribute("disabled", "disabled");
            document.getElementById(chave + "Edit").setAttribute("disabled", "disabled");
            document.getElementById(chave + "Delete").setAttribute("disabled", "disabled");
        }
    }
}