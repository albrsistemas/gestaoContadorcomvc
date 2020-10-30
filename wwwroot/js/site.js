//Onload página layout
function Page() {

    //Atribuindo valor padrão a campos da tela novo lançamento contábil
    let Element_vlr = document.getElementById("lancamento_valor_create");
    let Element_data = document.getElementById("lancamento_data_create");
    if (Element_vlr) {
        Element_vlr.value = (0).toFixed(2);
    }
    if (Element_data) {
        let data = new Date();
        Element_data.value = data.toLocaleDateString();
    }  

    //Tela balancete contábil
    let Elemet_data_inicial = document.getElementById("data_inicial");
    if (Elemet_data_inicial) {
        let data = new Date();
        document.getElementById("data_inicial").value = '01/' + (data.getMonth() + 1) + "/" + data.getFullYear();

        var ultimoDia = new Date(data.getFullYear(), data.getMonth() + 1, 0);
        document.getElementById('data_final').value = ultimoDia.toLocaleDateString();
    }

    //Tela create Participante
    let tipo_Pessoa = document.getElementById("participante_tipoPessoa");
    if (tipo_Pessoa) {
        tipoPessoa(tipo_Pessoa.value);
    }
}

function ValidaRegistro(id) {
    if (id == "pj") {
        document.getElementById()
    }
}

function openNav(escopo) {

    if (escopo == "aberto") {        
        var estado = document.getElementById("mySidenav").style.width;

        if (estado != "250px") {
            document.getElementById("mySidenav").style.width = "250px";
            document.getElementById("corpo").style.marginLeft = "250px";
        } else {
            document.getElementById("mySidenav").style.width = "0";
            document.getElementById("corpo").style.marginLeft = "0";
        }
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

$(".edit").click(function () {
    var id = $(this).attr("data-id");
    $("#modal").load("Edit?id=" + id, function () {
        $("#modal").modal('show');
    })
});

$(".createConta").click(function () {
    var id = $(this).attr("data-id");
    $("#modal").load("Create?id=" + id, function () {
        $("#modal").modal('show');
    })
});


$(document).ready(function () {
    $('#Esconder').delay(4000).fadeOut();
});

//Categoria
$(".createGrupoCategoria").click(function () {
    var escopo = $(this).attr("data-escopo");
    $("#modal").load("CreateGrupoCategoria?escopo=" + escopo, function () {
        $("#modal").modal('show');
    })
});

$(".createCategoria").click(function () {
    var grupo = $(this).attr("data-grupo");
    var escopo = $(this).attr("data-escopo");
    $("#modal").load("Create?grupo=" + grupo + "&escopo=" + escopo, function () {
        $("#modal").modal('show');
    })
});

$(".editCategoria").click(function () {
    var id = $(this).attr("data-id");
    var tipo = $(this).attr("data-tipo");
    $("#modal").load("Edit?id=" + id + "&tipo=" + tipo, function () {
        $("#modal").modal('show');
    })
});

$(".deleteCategoria").click(function () {
    var id = $(this).attr("data-id");
    var tipo = $(this).attr("data-tipo");
    $("#modal").load("Delete?id=" + id + "&tipo=" + tipo, function () {
        $("#modal").modal('show');
    })
});

$(".copiarPlanoCategorias").click(function () {    
    $("#modal").load("copiarPlanoCategorias", function () {
        $("#modal").modal('show');
    })
});

function mask_classificaoCategoriaGrupo(valor, id, escopo) {    
    let tamanho = valor.length;
    if (tamanho == 1) {
        document.getElementById(id).value = valor + ".";
    }
    if (tamanho > 3) {
        document.getElementById(id).value = valor.substring(0, 3);
    }
}

function montaClassificacao(valor, grupo) {
    document.getElementById("categoria_classificacao").value = grupo + "." + valor;
}

//Bread alteração de empresa selecionada pelo contador;
$(".SelectClient").click(function () {
    //var id = $(this).attr("data-id");
    var url = window.location.href;
    $("#modal").load("/Contabilidade/Clientes/SelectCliente?url=" + url, function () {
        $("#modal").modal('show');
    })
});

//Configurações Contábes
function addCont() {
    let cnpj = document.getElementById("cc_cnpj").value;    
    if (cnpj.length >= 11) {
        let retorno = "";

        $("#modal").load("/Configuracoes/AddContabilidade?dctoContador=" + cnpj, function () {
            $("#modal").modal('show');
        });
        
    } else {
        document.getElementById("labelCNPJCont").innerText = "Digite o CNPJ ou CPF da contabilidade corretamente";
    }
}
function limpaValidacaoCNPJCont() {
    document.getElementById("labelCNPJCont").innerText = "";
}
$(".desvincularContador").click(function () {        
    $("#modal").load("DeleteContabilidade", function () {        
        $("#modal").modal('show');
    })
});

//Contas contábeis
function nivelConta(vlr) {    
    var tamanho = vlr.length;

    if (tamanho == 2) {        
        document.getElementById('ccontabil_classificacao').value = vlr + ".";
        document.getElementById('ccontabil_nivel').value = 1;
    }
    if (tamanho == 4) {
        document.getElementById('ccontabil_classificacao').value = vlr + ".";
        document.getElementById('ccontabil_nivel').value = 2;
    }
    if (tamanho == 6) {
        document.getElementById('ccontabil_classificacao').value = vlr + ".";
        document.getElementById('ccontabil_nivel').value = 3;
    }
    if (tamanho == 9) {
        document.getElementById('ccontabil_classificacao').value = vlr + ".";
        document.getElementById('ccontabil_nivel').value = 4;
    }   
    if (tamanho == 13) {        
        document.getElementById('ccontabil_nivel').value = 5;
    }
    if (tamanho > 13) {
        document.getElementById('ccontabil_classificacao').value = vlr.substring(0, 13);        
    }
}

function removeDot(vlr) {
    let nivel = document.getElementById('ccontabil_nivel').value;

    if (nivel != "5") {
        console.log(vlr);
        console.log((vlr.length - 1));
        console.log(vlr.substring((vlr.length - 1), 1));
        document.getElementById('ccontabil_classificacao').value = vlr.toString().substring(0, (vlr.length - 1));
    }
}

$(".editContaContabil").click(function () {
    var ccontabil_id = $(this).attr("data-ccontabil_id");
    var plano_id = $(this).attr("data-plano_id");
    $("#modal").load("Edit?ccontabil_id=" + ccontabil_id + "&plano_id=" + plano_id, function () {
        $("#modal").modal('show');
    })
});

$(".deleteContaContabil").click(function () {
    var ccontabil_id = $(this).attr("data-ccontabil_id");
    var plano_id = $(this).attr("data-plano_id");
    $("#modal").load("Delete?ccontabil_id=" + ccontabil_id + "&plano_id=" + plano_id, function () {
        $("#modal").modal('show');
    })
});

$(document).ready(function () {
    $(".toast").toast('show');
});

$(document).ready(function () {
    $('.js_example_basic_single').select2({
        width: 'resolve'
    });
});

function contabilizacao(vlr) {
    let cb = document.getElementById("sw_ccc_pref_contabilizacao");   

    if (!cb.disabled) {        
        if (!cb.checked) {
            $('#_ccc_planoContasVigente option').each(function () {
                //Removendo os options selected
                $(this).removeAttr('selected');
                console.log(this);
            });
            $('#_ccc_planoContasVigente option').each(function () {
                if (this.value == 0) {
                    this.setAttribute("selected", "selected");

                    $('#_ccc_planoContasVigente').val("");
                }
            });

            document.getElementById("_ccc_planoContasVigente").disabled = true;
            document.getElementById("_ccc_planoContasVigente-error").innerHTML = "";
        } else {
            document.getElementById("_ccc_planoContasVigente").disabled = false;
        }
    }
}

//Vinculo conta on line categorias cliente visão contador
$(".createCategoria_contaonline").click(function () {
    var categoria_id = $(this).attr("data-categoria_id");
    var plano_id = $(this).attr("data-plano_id");
    var local = $(this).attr("data-local");
    console.log(plano_id);
    $("#modal").load("/Contabilidade/CCO/Create?categoria_id=" + categoria_id + "&plano_id=" + plano_id + "&local=" + local, function () {
        $("#modal").modal('show');
    })
});

$(".DetailsCCO").click(function () {
    var id = $(this).attr("data-id");
    $("#modal").load("/Contabilidade/CCO/Details?id=" + id, function () {
        $("#modal").modal('show');
    })
});

$(".SelectPlano").click(function () {
    var pc_id = $(this).attr("data-pc_id");
    $("#modal").load("/Contabilidade/CategoriasPlano/SelectPlano?pc_id=" + pc_id, function () {
        $("#modal").modal('show');
    })
});


//Plano de Categorias
$(".createGrupoCategoriaPlano").click(function () {
    var escopo = $(this).attr("data-escopo");
    var planoCategorias_id = $(this).attr("data-planoCategorias_id");
    var planoContas_id = $(this).attr("data-planoContas_id");
    $("#modal").load("CreateGrupoCategoria?escopo=" + escopo + "&planoCategorias_id=" + planoCategorias_id + "&planoContas_id=" + planoContas_id, function () {
        $("#modal").modal('show');
    })
});

$(".createCategoriaPlano").click(function () {
    var grupo = $(this).attr("data-grupo");
    var escopo = $(this).attr("data-escopo");
    var planoCategorias_id = $(this).attr("data-planoCategorias_id");
    var planoContas_id = $(this).attr("data-planoContas_id");
    $("#modal").load("/Contabilidade/CategoriasPlano/Create?grupo=" + grupo + "&escopo=" + escopo + "&planoCategorias_id=" + planoCategorias_id + "&planoContas_id=" + planoContas_id, function () {
        $("#modal").modal('show');
    })
});

$(".editCategoriaPlano").click(function () {
    var id = $(this).attr("data-id");
    var tipo = $(this).attr("data-tipo");
    var planoCategorias_id = $(this).attr("data-planoCategorias_id");
    var planoContas_id = $(this).attr("data-planoContas_id");
    $("#modal").load("/Contabilidade/CategoriasPlano/Edit?id=" + id + "&tipo=" + tipo + "&planoCategorias_id=" + planoCategorias_id + "&planoContas_id=" + planoContas_id, function () {
        $("#modal").modal('show');
    })
});

$(".deleteCategoriaPlano").click(function () {
    var id = $(this).attr("data-id");
    var tipo = $(this).attr("data-tipo");
    var planoCategorias_id = $(this).attr("data-planoCategorias_id");
    var planoContas_id = $(this).attr("data-planoContas_id");
    $("#modal").load("/Contabilidade/CategoriasPlano/Delete?id=" + id + "&tipo=" + tipo + "&planoCategorias_id=" + planoCategorias_id + "&planoContas_id=" + planoContas_id, function () {
        $("#modal").modal('show');
    })
});

//Vinculo conta on line categorias cliente visão contador
$(".createCategoria_contaonlinePlano").click(function () {
    var categoria_id = $(this).attr("data-categoria_id");    
    var planoCategorias_id = $(this).attr("data-planoCategorias_id");
    var planoContas_id = $(this).attr("data-planoContas_id");    
    $("#modal").load("/Contabilidade/CCOPlanoCategorias/Create?categoria_id=" + categoria_id + "&planoContas_id=" + planoContas_id + "&planoCategorias_id=" + planoCategorias_id, function () {
        $("#modal").modal('show');
    })
});

$(".DetailsCCOPlano").click(function () {
    var id = $(this).attr("data-id");
    $("#modal").load("/Contabilidade/CCOPlanoCategorias/Details?id=" + id, function () {
        $("#modal").modal('show');
    })
});

$(function () {
    $(".datepicker").datepicker({
        buttonImageOnly: true,
        dateFormat: 'dd-mm-yyyy',
        changeMonth: false,
        changeYear: false,
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez']
    });
});

//Tela de lançamento contábil
function consultaContasContabeis(id, tipo, termo) {    
    let id_campo = "#" + id;
    $(id_campo).autocomplete({        
        source: function (request, response) {            
            $.ajax({
                url: "/Contabilidade/ContaContabil/consultaContasContabeis",
                data: { termo: request.term },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("erro");
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    var results = JSON.parse(data);                    
                    var autocompleteObjects = [];
                    for (var i = 0; i < results.length; i++) {
                        var object = {
                            // Used by jQuery Autocomplete to show
                            // autocomplete suggestions as well as
                            // the text in yourInputTextBox upon selection.
                            // Assign them to a value that you want the user to see.
                            value: results[i].ccontabil_classificacao + " - " + results[i].ccontabil_nome,
                            label: results[i].ccontabil_classificacao + " - " + results[i].ccontabil_nome,

                            // Put our own custom id here.
                            // If you want to, you can even put the result object.
                            id: results[i].ccontabil_id,
                            nivel: results[i].ccontabil_nivel
                        };

                        autocompleteObjects.push(object);
                    }

                    // Invoke the response callback.
                    response(autocompleteObjects);
                }
            });
        },
        minLength: 3,
        select: function (event, ui) {
            // Retrieve your id here and do something with it.  
            if (tipo == 'debito') {
                document.getElementById('lancamento_debito_conta_id').value = ui.item.id;
                document.getElementById('conta_debito_nivel').value = ui.item.nivel;
                document.getElementById('nivel_debito').innerHTML = ui.item.nivel;
            }
            if (tipo == 'credito') {
                document.getElementById('lancamento_credito_conta_id').value = ui.item.id;
                document.getElementById('conta_credito_nivel').value = ui.item.nivel;
                document.getElementById('nivel_credito').innerHTML = ui.item.nivel;
            }
                        

        }
    });
}

function formatValor(id, vlr, decimais) {    
    vlr = vlr.replace(",", ".");
    vlr = (parseFloat(vlr) * 1);
    document.getElementById(id).value = vlr.toFixed(2);    
}

function gerarDataFinal(vlr) {
    data_inicial = vlr.substring(6, 10) + "-" + vlr.substring(3, 5) + "-" + (vlr.substring(0, 2) + 1);        
    let data = new Date(data_inicial);    
    var ultimoDia = new Date(data.getFullYear(), data.getMonth() + 1, 0);
    document.getElementById('data_final').value = ultimoDia.toLocaleDateString();
}

//Manipulação do validados de formulários
$.validator.setDefaults({ ignore: '' }); //Setar informação para validação verificar campos hidden

//Fim Manipulação do validados de formulários

//Participante
function tipoPessoa(vlr) {
    if (vlr == 1) {
        document.getElementById('label_cnpj_cpf').innerHTML = "CPF";
        document.getElementById('grupo_regime').style.display = 'none';
        document.getElementById('grupo_rg').style.display = 'block';
        document.getElementById('grupo_orgaoEmissor').style.display = 'block';
        document.getElementById('grupo_contribuinte').style.display = 'block';
        document.getElementById('grupo_ie').style.display = 'block';
        document.getElementById('grupo_im').style.display = 'block';
    }
    if (vlr == 2) {
        document.getElementById('label_cnpj_cpf').innerHTML = "CNPJ";
        document.getElementById('grupo_regime').style.display = 'block';
        document.getElementById('grupo_rg').style.display = 'none';
        document.getElementById('grupo_orgaoEmissor').style.display = 'none';
        document.getElementById('grupo_contribuinte').style.display = 'block';
        document.getElementById('grupo_ie').style.display = 'block';
        document.getElementById('grupo_im').style.display = 'block';
    }
    if (vlr == 3) {
        document.getElementById('label_cnpj_cpf').innerHTML = "Identificação Estrangeiro";
        document.getElementById('grupo_regime').style.display = 'none';
        document.getElementById('grupo_rg').style.display = 'none';
        document.getElementById('grupo_orgaoEmissor').style.display = 'none';
        document.getElementById('grupo_contribuinte').style.display = 'none';
        document.getElementById('grupo_ie').style.display = 'none';
        document.getElementById('grupo_im').style.display = 'none';
    }
}

function mascaraCNPJ_cpf(id, vlr) {
    let tipo = document.getElementById('participante_tipoPessoa');
    let campo = document.getElementById(id);

    let tamanho = vlr.length;

    if (tipo.value == 1) {
        if (tamanho == 3) {
            campo.value = vlr + ".";
        }
        if (tamanho == 7) {
            campo.value = vlr + ".";
        }
        if (tamanho == 11) {
            campo.value = vlr + "-";
        }
        if (tamanho > 14) {
            campo.value = vlr.substring(0, 14);
            alert("Quantidade de dígitos do CPF inválido");
        }
    }

    if (tipo.value == 2) {
        if (tamanho == 2) {
            campo.value = vlr + ".";
        }
        if (tamanho == 6) {
            campo.value = vlr + ".";
        }
        if (tamanho == 10) {
            campo.value = vlr + "/";
        }
        if (tamanho == 15) {
            campo.value = vlr + "-";
        }
        if (tamanho > 18) {
            campo.value = vlr.substring(0, 18);
            alert("Quantidade de dígitos do CNPJ inválido");
        }
    }
}
//Participante fim
//Produtos
function mascaraNCM(id, vlr) {    
    let campo = document.getElementById(id);
    let tamanho = vlr.length;

    if (tamanho == 4) {
        campo.value = vlr + ".";
    }
    if (tamanho == 7) {
        campo.value = vlr + ".";
    }
    if (tamanho > 10) {
        campo.value = vlr.substring(0, 10);
        alert("Quantidade de dígitos da NCM inválido");
    }   
}

function mascaraCEST(id, vlr) {
    let campo = document.getElementById(id);
    let tamanho = vlr.length;

    if (tamanho == 2) {
        campo.value = vlr + ".";
    }
    if (tamanho == 6) {
        campo.value = vlr + ".";
    }
    if (tamanho > 9) {
        campo.value = vlr.substring(0, 9);
        alert("Quantidade de dígitos do CEST inválido");
    }
}
//Produtos fim

//Geral
function tamanhoDigitado(id, vlr, limit, msg) {
    let tamanho = vlr.length;

    if (tamanho <= limit) {
        document.getElementById(msg).innerHTML = tamanho + ' de ' + limit;
    } else {
        document.getElementById(id).value = vlr.substring(0, limit);
        alert('Quantidade de caractes execido!');
    }
}

function decimal(id, vlr, limit) {
    let matriz = vlr.split(",");    

    if (matriz.length > 1) {
        let tamanho = matriz[1].length;

        if (tamanho > limit) {
            alert('Permitido até ' + limit + ' dígitos nas casas decimais!');
            let cd = matriz[1].substring(0, limit);
            let valor = matriz[0] + ',' + cd;
            document.getElementById(id).value = valor;
        }

        if (tamanho < 2) {
            document.getElementById(id).value = ((vlr.toString().replace(",",".")) * 1).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
        }

    } else {
        document.getElementById(id).value = (vlr * 1).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
    }
}

function mascaraCNPJ(id, vlr) {    
    let campo = document.getElementById(id);

    let tamanho = vlr.length;

    if (tamanho == 2) {
        campo.value = vlr + ".";
    }
    if (tamanho == 6) {
        campo.value = vlr + ".";
    }
    if (tamanho == 10) {
        campo.value = vlr + "/";
    }
    if (tamanho == 15) {
        campo.value = vlr + "-";
    }
    if (tamanho > 18) {
        campo.value = vlr.substring(0, 18);
        alert("Quantidade de dígitos do CNPJ inválido");
    }
}

function mascaraCPF(id, vlr) {
    let campo = document.getElementById(id);

    let tamanho = vlr.length;

    if (tamanho == 3) {
        campo.value = vlr + ".";
    }
    if (tamanho == 7) {
        campo.value = vlr + ".";
    }
    if (tamanho == 11) {
        campo.value = vlr + "-";
    }
    if (tamanho > 14) {
        campo.value = vlr.substring(0, 14);
        alert("Quantidade de dígitos do CPF inválido");
    }
}

//Forma de pagamento
function baixaAutomatica(id) {
    let cb = document.getElementById(id);
    let meioPgto = document.getElementById('fp_meio_pgto_nfe');
    let identificacao = document.getElementById('fp_identificacao');

    if (identificacao.value == "Recebimento") {
        if (meioPgto.value == '03' || meioPgto.value == '04') {
            if (!cb.checked) {
                alert('Recebimento no cartão é obrigatório a baixa automática!');
                cb.checked = true;
            }
        }

        if (meioPgto.value == '02' || meioPgto.value == '05' || meioPgto.value == '15') {
            if (cb.checked) {
                alert('O meio de pagamento selecionado não permite baixa automática!');
                cb.checked = false;
            }
        }

    }

    if (identificacao.value == "Pagamento") {
        if (meioPgto.value == '03' || meioPgto.value == '02' || meioPgto.value == '05' || meioPgto.value == '15') {
            if (cb.checked) {
                alert('O meio de pagamento selecionado não permite baixa automática!');
                cb.checked = false;
            }
        }

        if (meioPgto.value == '04') {
            if (!cb.checked) {
                alert('O meio de pagamento selecionado é obrigatória a baixa automática!');
                cb.checked = true;
            }
        }
    }
}

function meioPgto(vlr) {
    let identificacao = document.getElementById('fp_identificacao');
    let cb = document.getElementById('fp_baixa_automatica');

    let diaFecha = document.getElementById('fp_dia_fechamento_cartao');
    let diaVenc = document.getElementById('fp_dia_vencimento_cartao');

    if (identificacao.value == 'Pagamento') {

        if (vlr == '03') {
            document.getElementById('legenda_cartoes').style.display = 'block';
            document.getElementById('grupo_cartoes_pagamento').style.display = 'block';
            document.getElementById('grupo_cartoes_recebimento').style.display = 'none';
            diaFecha.removeAttribute("disabled");
            diaVenc.removeAttribute("disabled");            
        } else {
            document.getElementById('legenda_cartoes').style.display = 'none';
            document.getElementById('grupo_cartoes_pagamento').style.display = 'none';
            document.getElementById('grupo_cartoes_recebimento').style.display = 'none';
            diaFecha.setAttribute("disabled", "disabled");
            diaVenc.setAttribute("disabled", "disabled");
        }

        if (vlr == '02' || vlr == '03' || vlr == '05' || vlr == '15') {
            cb.checked = false;
        }

        if (vlr == '04') {
            cb.checked = true;
        }
    }

    if (identificacao.value == 'Recebimento') {
        if (vlr == '03' || vlr == '04') {
            document.getElementById('legenda_cartoes').style.display = 'block';
            document.getElementById('grupo_cartoes_recebimento').style.display = 'block';
            document.getElementById('grupo_cartoes_pagamento').style.display = 'none';
            cb.checked = true;
        } else {
            document.getElementById('legenda_cartoes').style.display = 'none';
            document.getElementById('grupo_cartoes_recebimento').style.display = 'none';
            document.getElementById('grupo_cartoes_pagamento').style.display = 'none';            
        }

        if (vlr == '02' || vlr == '05' || vlr == '15') {
            cb.checked = false;
        }

        if (vlr == '03' || vlr == '04') {
            cb.checked = true;
        }


    }

    carregarSelectCC(identificacao.value, vlr);

}

function identificacaoPgto(vlr) {
    let meioPagto = document.getElementById('fp_meio_pgto_nfe');
    let diaFecha = document.getElementById('fp_dia_fechamento_cartao');
    let diaVenc = document.getElementById('fp_dia_vencimento_cartao');
    let cb = document.getElementById('fp_baixa_automatica');

    if (vlr == 'Pagamento') {

        if (meioPagto.value == '03') {
            document.getElementById('legenda_cartoes').style.display = 'block';            
            document.getElementById('grupo_cartoes_pagamento').style.display = 'block';
            document.getElementById('grupo_cartoes_recebimento').style.display = 'none';
            diaFecha.removeAttribute("disabled");
            diaVenc.removeAttribute("disabled");
        } else {
            document.getElementById('legenda_cartoes').style.display = 'none';            
            document.getElementById('grupo_cartoes_pagamento').style.display = 'none';            
            document.getElementById('grupo_cartoes_recebimento').style.display = 'none';
            diaFecha.removeAttribute("disabled");
            diaVenc.removeAttribute("disabled");
        }

        if (meioPagto.value == '02' || meioPagto.value == '03' || meioPagto.value == '05' || meioPagto.value == '15') {
            cb.checked = false;
        }

        if (meioPagto.value == '04') {
            cb.checked = true;
        }
    }

    if (vlr == 'Recebimento') {
        if (meioPagto.value == '03' || meioPagto.value == '04') {
            document.getElementById('legenda_cartoes').style.display = 'block';
            document.getElementById('grupo_cartoes_recebimento').style.display = 'block';
            document.getElementById('grupo_cartoes_pagamento').style.display = 'none';
            diaFecha.setAttribute("disabled", "disabled");
            diaVenc.setAttribute("disabled", "disabled");
        } else {
            document.getElementById('legenda_cartoes').style.display = 'none';
            document.getElementById('grupo_cartoes_recebimento').style.display = 'none';
            document.getElementById('grupo_cartoes_pagamento').style.display = 'none';
            diaFecha.setAttribute("disabled", "disabled");
            diaVenc.setAttribute("disabled", "disabled");
        }

        if (meioPagto.value == '02' || meioPagto.value == '05' || meioPagto.value == '15') {
            cb.checked = false;
        }

        if (meioPagto.value == '03' || meioPagto.value == '04') {
            cb.checked = true;
        }
    }

    carregarSelectCC(vlr, meioPagto.value);


}

function carregarSelectCC(identificacao, meioPgto) {    
    $.ajax({
        url: "/FormaPagamento/gerarSelectMeioPgto",
        data: { __RequestVerificationToken: gettoken(), identificacao: identificacao, meioPgto: meioPgto },
        type: 'POST',
        dataType: 'json',
        beforeSend: function (XMLHttpRequest) {
            document.getElementById('text_ccorrente').innerHTML = "Gerando lista de contas corrente";
            document.getElementById('sub').setAttribute("disabled", "disabled");
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("erro");
        },
        success: function (data, textStatus, XMLHttpRequest) {
            var results = JSON.parse(data);            
            $('#fp_vinc_conta_corrente').children('option').remove(); //Remove todos os itens do select

            for (i = 0; i < results.length; i++) { //Adiciona os item recebidos no results no select
                $('#fp_vinc_conta_corrente').append($("<option></option>").attr("value", results[i].value).text(results[i].text));
            }

            if (results[0].value == "0") {
                document.getElementById('fp_vinc_conta_corrente').setAttribute("disabled", "disabled");
            } else {
                document.getElementById('fp_vinc_conta_corrente').removeAttribute("disabled");
            }

            document.getElementById('text_ccorrente').innerHTML = "";
            document.getElementById('sub').removeAttribute("disabled");
        }
    });    
}

function gettoken() {
    var token = $('[name=__RequestVerificationToken]').val();    
    return token;
};


//Forma de pagamento fim

//Ajax com token: https://www.codigoexpresso.com.br/Home/Postagem/78
/*
 <script>
    // Gera o token
    function gettoken() {
        var token = $('[name=__RequestVerificationToken]').val();
        console.log(token);
        return token;
    };


    // Exibe os dados do aluno gerando a chave de validação token
    function DisplayAluno(idaluno) {

        $.ajax({
            type: 'POST',
            url: '@Url.Action("DisplayAluno","Home")',
            data: { __RequestVerificationToken: gettoken(), 'idaluno': idaluno },
            dataType: 'html',
            cache: false,
            async: true,
            success: function (data) {
                $('#divDisplayAluno').html(data);
            }
        });

    };
</script>
 */


