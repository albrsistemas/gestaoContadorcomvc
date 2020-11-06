//Variaveis globais
var operacao = {
    operacao: {
        op_id: 0,
        op_tipo: '',
        op_data: '',
        op_previsao_entrega: '',
        op_data_saida: '',
        op_obs: '',
        op_numero_ordem: '',
        op_categoria_id: 0
    },
    participante: {
        op_part_id: 0,
        op_part_nome: '',
        op_part_tipo: '',
        op_part_cnpj_cpf: '',
        op_part_cep: '',
        op_part_cidade: '',
        op_part_bairro: '',
        op_part_logradouro: '',
        op_part_numero: '',
        op_part_complemento: '',
        op_paisesIBGE_codigo: 0,
        op_uf_ibge_codigo: 0,
        op_part_participante_id: 0,
    },
    itens: [],
    retencoes: {
        op_ret_id: 0,
        op_ret_pis: 0,
        op_ret_cofins: 0,
        op_ret_csll: 0,
        op_ret_irrf: 0,
        op_ret_inss: 0,
        op_ret_issqn: 0,
    },
    totais: {
        op_totais_id: 0,
        op_totais_preco_itens: 0,
        op_totais_frete: 0,
        op_totais_seguro: 0,
        op_totais_desp_aces: 0,
        op_totais_desconto: 0,
        op_totais_itens: 0,
        op_totais_qtd_itens: 0,
        op_totais_op_id: 0,
        op_totais_retencoes: 0,
        op_totais_total_op: 0,
        op_totais_ipi: 0,
        op_totais_icms_st: 0,
        op_totais_saldoLiquidacao: 0
    },
    parcelas: [],
    transportador: {
        op_transportador_id: 0,
        op_transportador_nome: '',
        op_transportador_cnpj_cpf: '',
        op_transportador_modalidade_frete: '',
        op_transportador_volume_qtd: 0,
        op_transportador_volume_peso_bruto: 0,
        op_transportador_participante_id: 0,
    },
};

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
        if (document.getElementById('label_cnpj_cpf')) {
            document.getElementById('label_cnpj_cpf').innerHTML = "CPF";
        }        
        if (document.getElementById('grupo_regime')) {
            document.getElementById('grupo_regime').style.display = 'none';
        }
        if (document.getElementById('grupo_rg')) {
            document.getElementById('grupo_rg').style.display = 'block';
        }
        if (document.getElementById('grupo_orgaoEmissor')) {
            document.getElementById('grupo_orgaoEmissor').style.display = 'block';
        }
        if (document.getElementById('grupo_contribuinte')) {
            document.getElementById('grupo_contribuinte').style.display = 'block';
        }
        if (document.getElementById('grupo_ie')) {
            document.getElementById('grupo_ie').style.display = 'block';
        }
        if (document.getElementById('grupo_im')) {
            document.getElementById('grupo_im').style.display = 'block';
        }
        
    }
    if (vlr == 2) {
        if (document.getElementById('label_cnpj_cpf')) {
            document.getElementById('label_cnpj_cpf').innerHTML = "CNPJ";
        }
        if (document.getElementById('grupo_regime')) {
            document.getElementById('grupo_regime').style.display = 'block';
        }
        if (document.getElementById('grupo_rg')) {
            document.getElementById('grupo_rg').style.display = 'none';
        }
        if (document.getElementById('grupo_orgaoEmissor')) {
            document.getElementById('grupo_orgaoEmissor').style.display = 'none';
        }
        if (document.getElementById('grupo_contribuinte')) {
            document.getElementById('grupo_contribuinte').style.display = 'block';
        }
        if (document.getElementById('grupo_ie')) {
            document.getElementById('grupo_ie').style.display = 'block';
        }
        if (document.getElementById('grupo_im')) {
            document.getElementById('grupo_im').style.display = 'block';
        }
    }
    if (vlr == 3) {
        if (document.getElementById('label_cnpj_cpf')) {
            document.getElementById('label_cnpj_cpf').innerHTML = "Identificação Estrangeiro";
        }
        if (document.getElementById('grupo_regime')) {
            document.getElementById('grupo_regime').style.display = 'none';
        }
        if (document.getElementById('grupo_rg')) {
            document.getElementById('grupo_rg').style.display = 'none';
        }
        if (document.getElementById('grupo_orgaoEmissor')) {
            document.getElementById('grupo_orgaoEmissor').style.display = 'none';
        }
        if (document.getElementById('grupo_contribuinte')) {
            document.getElementById('grupo_contribuinte').style.display = 'none';
        }
        if (document.getElementById('grupo_ie')) {
            document.getElementById('grupo_ie').style.display = 'none';
        }
        if (document.getElementById('grupo_im')) {
            document.getElementById('grupo_im').style.display = 'none';
        }
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

function contribICMS(vlr) {
    let ie = document.getElementById('participante_inscricaoEstadual');
    if (vlr == 9) {
        ie.value = "";
        ie.setAttribute("disabled", "disabled");
    } else {
        ie.removeAttribute("disabled");
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
    //console.log(vlr);
    //console.log(matriz);

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

        if (tamanho >= 2 && tamanho <= limit) {
            document.getElementById(id).value = ((vlr.toString().replace(",", ".")) * 1).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
        }


    } else {
        document.getElementById(id).value = ((vlr.toString().replace(",", ".")) * 1).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
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

//Autocomplete lista de participantes
function consultaParticipante(id) { //Parada
    let id_campo = "#" + id;
    $(id_campo).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Participante/consultaParticipante",
                data: { __RequestVerificationToken: gettoken(), termo: request.term },
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
                            value: results[i].participante_nome,
                            label: results[i].participante_nome + " - " + results[i].participante_cnpj_cpf,

                            id: results[i].participante_id,
                            nome: results[i].participante_nome,
                            tipo: results[i].participante_tipoPessoa,
                            cnpj_cpf: results[i].participante_cnpj_cpf,
                            cep: results[i].participante_cep,
                            rua: results[i].participante_logradouro,
                            numero: results[i].participante_numero,
                            complemento: results[i].participante_complemento,
                            bairro: results[i].participante_bairro,
                            cidade: results[i].participante_cidade,
                            uf: results[i].participante_uf,
                            pais: results[i].participante_pais,
                            categoria_id: results[i].participante_categoria,
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
            if (document.getElementById('nome')) {
                document.getElementById('nome').value = ui.item.nome;
            }
            if (document.getElementById('participante_tipoPessoa')) {
                document.getElementById('participante_tipoPessoa').value = ui.item.tipo;                
            }
            if (document.getElementById('op_part_cnpj_cpf')) {
                document.getElementById('op_part_cnpj_cpf').value = ui.item.cnpj_cpf;
            }
            if (document.getElementById('cep')) {
                document.getElementById('cep').value = ui.item.cep;
            }
            if (document.getElementById('rua')) {
                document.getElementById('rua').value = ui.item.rua;
            }
            if (document.getElementById('numero')) {
                document.getElementById('numero').value = ui.item.numero;
            }
            if (document.getElementById('complemento')) {
                document.getElementById('complemento').value = ui.item.complemento;
            }
            if (document.getElementById('bairro')) {
                document.getElementById('bairro').value = ui.item.bairro;
            }
            if (document.getElementById('cidade')) {
                document.getElementById('cidade').value = ui.item.cidade;
            }
            if (document.getElementById('uf')) {
                document.getElementById('uf').value = ui.item.uf;
            }
            if (document.getElementById('op_paisesIBGE_codigo')) {
                document.getElementById('op_paisesIBGE_codigo').value = ui.item.pais;
            }
            if (document.getElementById('op_categoria_id')) {
                document.getElementById('op_categoria_id').value = ui.item.categoria_id;
            }

            dadorParticipante(ui.item.id);
            modal_fornecedor
        }
    });
}

//Autocomplete lista de produtos
function consultaProdutos(id, contexto) { //Parada
    let id_campo = "#" + id;
    $(id_campo).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Produtos/consultaProdutos",
                data: { __RequestVerificationToken: gettoken(), termo: request.term },
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
                            value: results[i].produtos_nome,
                            label: results[i].produtos_nome + " - " + results[i].produtos_codigo,
                            id: results[i].produtos_id,
                            codigo: results[i].produtos_codigo,
                            valorCompra: results[i].produtos_estoque_preco_compra,
                            valorVenda: results[i].produtos_preco_venda,
                            unidade: results[i].produtos_unidade,
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
            if (document.getElementById('produto_id')) {
                document.getElementById('produto_id').value = ui.item.id;
            }
            if (document.getElementById('unidade')) {
                document.getElementById('unidade').value = ui.item.unidade;
            }
            if (document.getElementById('prod_codigo')) {
                document.getElementById('prod_codigo').value = ui.item.codigo;
            }            
            if (contexto == 'compra') {
                if (document.getElementById('prod_valor')) {                    
                    decimal('prod_valor', ui.item.valorCompra.toString().replace(".", ","), '6');
                    decimal('prod_valorTotal', ui.item.valorCompra.toString().replace(".", ","), '6');                    
                }
            }
            if (contexto == 'venda') {
                if (document.getElementById('prod_valor')) {
                    decimal('prod_valor', ui.item.valorVenda.toString().replace(".", ","), '6');
                    decimal('prod_valorTotal', ui.item.valorVenda.toString().replace(".", ","), '6');
                }
            }
            if (document.getElementById('prod_quantidade')) {
                decimal('prod_quantidade', '1', '6');
            }           
        }
    });
}

function incluir_item() {
    let id = document.getElementById('produto_id').value;
    let prod_descricao = document.getElementById('prod_descricao').value;
    let prod_codigo = document.getElementById('prod_codigo').value;
    let prod_valor = document.getElementById('prod_valor').value;
    let prod_quantidade = document.getElementById('prod_quantidade').value;
    let prod_valorTotal = document.getElementById('prod_valorTotal').value;
    let prod_unidade = document.getElementById('unidade').value;


    if (id > 0) {        
        document.getElementById('produto_id').value = 0;
        document.getElementById('prod_descricao').value = "";
        document.getElementById('prod_codigo').value = "";
        document.getElementById('prod_valor').value = "";
        document.getElementById('prod_quantidade').value = "";
        document.getElementById('prod_valorTotal').value = "";

        let numero_controle = id + Math.floor(Math.random() * 100000) + 1;

        let item_produto = {
            op_item_numero_controle: numero_controle, //id é o produto_id selecionado. Adicionaod ao objeto para fins de controle (localiza-lo no momento da edição dos dados);
            op_item_produto_id: id,
            op_item_id: 0,
            op_item_codigo: prod_codigo,
            op_item_nome: prod_descricao,
            op_item_unidade: prod_unidade,
            op_item_preco: prod_valor,
            op_item_gtin_ean: '',
            op_item_gtin_ean_trib: '',
            op_item_obs: '',
            op_item_qtd: prod_quantidade,
            op_item_frete: '0,00',
            op_item_seguros: '0,00',
            op_item_desp_aces: '0,00',
            op_item_desconto: '0,00',
            op_item_vlr_ipi: '0,00',
            op_item_vlr_icms_st: '0,00',
            op_item_cod_fornecedor: '',
            op_item_valor_total: prod_valorTotal,
            controleEdit: 'insert',
            op_item_id_banco: 0,
        };
        operacao.itens.push(item_produto);

        let item = "" +
            "<div id=\"item_" + id + "\" class=\"row item_" + id + "\">" +
            "<div class=\"col-10\" style=\"padding-right: 0px;\">" +
            "<input type =\"text\" class=\"include_item\" style=\"width: 40%\" id=\"prod_descricao_" + id + "\" readonly=\"readonly\" value=\"" + prod_descricao + "\" />" +
            "<input type =\"text\" class=\"include_item\" style=\"width: 15%\" id=\"prod_codigo_" + id + "\" readonly=\"readonly\" value=\"" + prod_codigo + "\"/>" +
            "<input type =\"text\" class=\"include_item\" style=\"width: 15%\" id=\"prod_valor_" + id + "\" readonly=\"readonly\" value=\"" + prod_valor + "\"/>" +
            "<input type =\"text\" class=\"include_item\" style=\"width: 15%\" id=\"prod_quantidade_" + id + "\" readonly=\"readonly\" value=\"" + prod_quantidade + "\"/>" +
            "<input type =\"text\" class=\"include_item\" style=\"width: 15%\" id=\"prod_valorTotal_" + id + "\" readonly=\"readonly\" value=\"" + prod_valorTotal + "\"/>" +
            "<input type =\"hidden\" id=\"produto_id_" + id + "\" value=\"\" />" +
            "<input type =\"hidden\" id=\"" + numero_controle + "\" value=\"\" />" +
            "</div>" +
            "<div class=\"col-2\" style=\"text-align: right;padding-top: 6px;padding-left: 0px;\">" +
            "<svg id=\"E" + numero_controle + "\" onclick=\"edit_item(this.id)\" width =\"1em\" height=\"1em\" viewBox=\"0 0 16 16\" class=\"bi bi-pencil\" fill=\"currentColor\" xmlns=\"http://www.w3.org/2000/svg\" style=\"cursor:pointer\">" +
            "<path fill - rule=\"evenodd\" d=\"M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168l10-10zM11.207 2.5L13.5 4.793 14.793 3.5 12.5 1.207 11.207 2.5zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293l6.5-6.5zm-9.761 5.175l-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325z\" />" +
            "</svg>" +
            "<svg id=\"D" + numero_controle + "\" onclick=\"delete_item(this.id, 'confirmação')\" width=\"1em\" height=\"1em\" viewBox=\"0 0 16 16\" class=\"bi bi-trash\" fill=\"currentColor\" xmlns=\"http://www.w3.org/2000/svg\" style=\"cursor:pointer\">" +
            "<path d =\"M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z\" />" +
            "<path fill - rule=\"evenodd\" d=\"M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4L4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z\" />" +
            "</svg>" +
            "</div>" +
            "</div>";

        $('#box_itens').append(item);

        document.getElementById('prod_descricao').focus();

        totaisOperacao();
    } else {
        document.getElementById('prod_descricao').focus();
    }    
}

function ajusta_item(id, vlr) {
    let qtd = document.getElementById('prod_quantidade').value;
    let vlrProd = document.getElementById('prod_valor').value;
    let total = qtd.toString().replace(",", ".") * vlrProd.toString().replace(",", ".");

    decimal('prod_valorTotal', total.toString().replace(".", ","), '6');
    decimal(id, vlr, '6');    
}

function changeItens(id, vlr, inputTotalizador) {
    let preco = document.getElementById('op_item_preco').value.toString().replace('.', '').replace(',', '.') * 1;
    let qtd = document.getElementById('op_item_qtd').value.toString().replace('.', '').replace(',', '.') * 1;
    let frete = document.getElementById('op_item_frete').value.toString().replace('.', '').replace(',', '.') * 1;
    let seguros = document.getElementById('op_item_seguros').value.toString().replace('.', '').replace(',', '.') * 1;
    let despesas = document.getElementById('op_item_desp_aces').value.toString().replace('.', '').replace(',', '.') * 1;
    let descontos = document.getElementById('op_item_desconto').value.toString().replace('.', '').replace(',', '.') * 1;
    
    let total = (preco * qtd);    

    decimal(id, vlr.toString().replace('.', ','), '6');
    decimal('item_valor_total', total.toString().replace('.',','),'6');
}

function edit_item(id) {
    let codigo = id.substring(1, 15);
    let item = operacao.itens.find(item => item.op_item_numero_controle == codigo);    

    document.getElementById('op_item_numero_controle_edicao_item').value = item.op_item_numero_controle;
    document.getElementById('op_item_nome').value = item.op_item_nome;
    document.getElementById('op_item_codigo').value = item.op_item_codigo;
    document.getElementById('op_item_cod_fornecedor').value = item.op_item_cod_fornecedor;
    document.getElementById('op_item_unidade').value = item.op_item_unidade;
    decimal('op_item_preco', item.op_item_preco.toString().replace('.',''), '6');
    decimal('op_item_qtd', item.op_item_qtd.toString().replace('.', ''), '6');
    decimal('item_valor_total', item.op_item_valor_total.toString().replace('.', ''), '6');
    decimal('op_item_frete', item.op_item_frete.toString().replace('.', ''), '6');
    decimal('op_item_seguros', item.op_item_seguros.toString().replace('.', ''), '6');
    decimal('op_item_desp_aces', item.op_item_desp_aces.toString().replace('.', ''), '6');
    decimal('op_item_desconto', item.op_item_desconto.toString().replace('.', ''), '6');
    decimal('op_item_vlr_ipi', item.op_item_vlr_ipi.toString().replace('.', ''), '6');
    decimal('op_item_vlr_icms_st', item.op_item_vlr_icms_st.toString().replace('.', ''), '6');

    $('#modal_item').modal('show');
}

function gravar_item() {
    let op_item_numero_controle = document.getElementById('op_item_numero_controle_edicao_item').value * 1;

    for (let i = 0; i < operacao.itens.length; i++) {
        if (operacao.itens[i].op_item_numero_controle == op_item_numero_controle) {
            //atualiza objeto
            operacao.itens[i].op_item_codigo = document.getElementById('op_item_codigo').value;
            operacao.itens[i].op_item_nome = document.getElementById('op_item_nome').value;
            operacao.itens[i].op_item_unidade = document.getElementById('op_item_unidade').value;
            operacao.itens[i].op_item_preco = document.getElementById('op_item_preco').value;
            operacao.itens[i].op_item_qtd = document.getElementById('op_item_qtd').value;
            operacao.itens[i].op_item_frete = document.getElementById('op_item_frete').value;
            operacao.itens[i].op_item_seguros = document.getElementById('op_item_seguros').value;
            operacao.itens[i].op_item_desp_aces = document.getElementById('op_item_desp_aces').value;
            operacao.itens[i].op_item_desconto = document.getElementById('op_item_desconto').value;
            operacao.itens[i].op_item_vlr_ipi = document.getElementById('op_item_vlr_ipi').value;
            operacao.itens[i].op_item_vlr_icms_st = document.getElementById('op_item_vlr_icms_st').value;
            operacao.itens[i].op_item_cod_fornecedor = document.getElementById('op_item_cod_fornecedor').value;
            operacao.itens[i].op_item_valor_total = document.getElementById('item_valor_total').value;
            //atualiza a view            
            document.getElementById('prod_descricao_' + operacao.itens[i].op_item_produto_id).value = operacao.itens[i].op_item_nome;
            document.getElementById('prod_codigo_' + operacao.itens[i].op_item_produto_id).value = operacao.itens[i].op_item_codigo;
            document.getElementById('prod_valor_' + operacao.itens[i].op_item_produto_id).value = operacao.itens[i].op_item_preco;
            document.getElementById('prod_quantidade_' + operacao.itens[i].op_item_produto_id).value = operacao.itens[i].op_item_qtd;
            document.getElementById('prod_valorTotal_' + operacao.itens[i].op_item_produto_id).value = operacao.itens[i].op_item_valor_total;
            //calcula os totais
            totaisOperacao();
            break;
        }                          
    }
    $('#modal_item').modal('hide');
}

function delete_item(id, confirma) {
    let codigo = id.substring(1, 15);
    
    if (confirma == 'confirmação') {
        document.getElementById('item_delete').value = codigo;

        $('#modal_item_delete').modal('show');
    }

    if (confirma == 'confirmado') {
        let codigo = document.getElementById('item_delete').value;
        $('#modal_item_delete').modal('hide');

        let item = operacao.itens.find(item => item.op_item_numero_controle == codigo);


        for (let i = 0; i < operacao.itens.length; i++) {
            if (operacao.itens[i].op_item_numero_controle == codigo) {
                if (operacao.itens[i].controleEdit == 'update') {
                    operacao.itens[i].controleEdit = 'delete';
                    //remove linha da view
                    let item_linha = 'item_' + item.op_item_produto_id;
                    document.getElementById(item_linha).remove();
                    totaisOperacao();
                } else {
                    //remove item do objeto operacao da matriz itens
                    operacao.itens.splice(i, 1);
                    //remove linha da view
                    let item_linha = 'item_' + item.op_item_produto_id;
                    document.getElementById(item_linha).remove();
                    totaisOperacao();
                }
            }
        }
    }
}

function totaisOperacao() {
    let op_totais_frete = 0;
    let op_totais_seguro = 0;
    let op_totais_desp_aces = 0;
    let op_totais_desconto = 0;
    let op_totais_preco_itens = 0;    
    let op_totais_icms_st = 0;    
    let op_totais_ipi = 0;    

    for (let i = 0; i < operacao.itens.length; i++) {
        if (operacao.itens[i].controleEdit != 'delele') {            
            op_totais_frete += ((operacao.itens[i].op_item_frete).toString().replace('.', '').replace(',', '.') * 1);
            op_totais_seguro += ((operacao.itens[i].op_item_seguros).toString().replace('.', '').replace(',', '.') * 1);
            op_totais_desp_aces += ((operacao.itens[i].op_item_desp_aces).toString().replace('.', '').replace(',', '.') * 1);
            op_totais_desconto += ((operacao.itens[i].op_item_desconto).toString().replace('.', '').replace(',', '.') * 1);
            op_totais_preco_itens += ((operacao.itens[i].op_item_valor_total).toString().replace('.', '').replace(',', '.') * 1);            
            op_totais_icms_st += ((operacao.itens[i].op_item_vlr_icms_st).toString().replace('.', '').replace(',', '.') * 1);            
            op_totais_ipi += ((operacao.itens[i].op_item_vlr_ipi).toString().replace('.', '').replace(',', '.') * 1);            
        }
    }

    let op_totais_total_op = op_totais_preco_itens + op_totais_frete + op_totais_seguro + op_totais_desp_aces - op_totais_desconto + op_totais_icms_st + op_totais_ipi;

    decimal('op_totais_frete', op_totais_frete.toString().replace('.',','), '6');
    decimal('op_totais_seguro', op_totais_seguro.toString().replace('.',','), '6');
    decimal('op_totais_desp_aces', op_totais_desp_aces.toString().replace('.',','), '6');
    decimal('op_totais_desconto', op_totais_desconto.toString().replace('.',','), '6');
    decimal('op_totais_preco_itens', op_totais_preco_itens.toString().replace('.',','), '6');
    decimal('op_totais_total_op', op_totais_total_op.toString().replace('.', ','), '6');
    decimal('op_totais_icms_st', op_totais_icms_st.toString().replace('.', ','), '6');
    decimal('op_totais_ipi', op_totais_ipi.toString().replace('.', ','), '6');

    operacao.totais.op_totais_preco_itens = document.getElementById('op_totais_preco_itens').value;
    operacao.totais.op_totais_frete = document.getElementById('op_totais_frete').value;
    operacao.totais.op_totais_seguro = document.getElementById('op_totais_seguro').value;
    operacao.totais.op_totais_desp_aces = document.getElementById('op_totais_desp_aces').value;
    operacao.totais.op_totais_desconto = document.getElementById('op_totais_desconto').value;
    operacao.totais.op_totais_total_op = document.getElementById('op_totais_total_op').value;
    operacao.totais.op_totais_icms_st = document.getElementById('op_totais_icms_st').value;
    operacao.totais.op_item_vlr_ipi = document.getElementById('op_totais_ipi').value;
}

function gerarParcela() {
    let formaPgto = document.getElementById('forma_pgto');
    let condicaoPgto = (document.getElementById('condicoes_pgto').value).split(',');
    let totalCompra = ((document.getElementById('op_totais_total_op').value).toString().replace('.', '').replace(',', '.')) * 1;
    let op_data = document.getElementById('op_data').value;
    let data = new Date(Date.parse(op_data.substring(6, 10) + "/" + (op_data.substring(3, 5)) + "/" + op_data.substring(0, 2)));     
    let s = document.querySelector('#forma_pgto'); //Forma de pagamento selecionada;        
    let optionsTxt = "";
    
    for (let x = 0; x < s.children.length; x++) {
        let item = s.children[x].outerHTML;
        let textoItem = s.children[x].innerText;
        let selecionado = s.selectedOptions[0].innerText;

        let txt = "";
        if (textoItem == selecionado) {
            txt = item.substring(0, 16) + "\" selected='selected'" + item.substring(17, 200);
        } else {
            txt = item;
        }      

        optionsTxt += txt;
    }
        
    if (totalCompra == 'NaN') {
        alert('Operação não possui valor');
    }

    if (op_data == "" || op_data == null || op_data == 'undefined') {
        alert('Informe a data da operação');        
    } else {
        for (let i = 0; i < condicaoPgto.length; i++) {
            let vencimento = new Date();
            vencimento = data;
            vencimento.setDate(vencimento.getDate() + parseInt(condicaoPgto[i]));

            let numero_controle = i + Math.floor(Math.random() * 100000) + 1;

            let Op_parcelas = {
                op_parcela_dias: condicaoPgto[i],
                op_parcela_vencimento: vencimento.toLocaleDateString(),
                op_parcela_fp_id: formaPgto.value,
                op_parcela_valor: (totalCompra / condicaoPgto.length).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" }),
                op_parcela_obs: '',
                op_parcela_saldo: '',
                op_parcela_op_id: '',
                op_parcela_id: '',
                controleEdit: 'insert',
                op_parcela_numero_controle: numero_controle,
            };

            operacao.parcelas.push(Op_parcelas);

            let parcela = "" +
                "<div class=\"row\" id=\"parcela_" + numero_controle + "\">" +               
                "<div class=\"col-10\" style=\"padding-right: 0px;\">" +
                "<input id=\"diasParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 10%;padding:0px;text-align:center\" value=\"" + Op_parcelas.op_parcela_dias + "\" />" +
                "<input id=\"vencParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" type =\"text\" class=\"include_item datepicker\" style=\"width: 25%\" value=\"" + Op_parcelas.op_parcela_vencimento + "\" />" +
                "<input id=\"vlrParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 25%\" value=\"" + Op_parcelas.op_parcela_valor + "\" />" +
                "<select id=\"fpParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" class=\"include_item\" style=\"width: 25%\">" + optionsTxt + "</select>" +
                "<input id=\"obsParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 15%\" />" +
                "" + 
                "</div>" +
                "<div class=\"col-2\" style=\"text-align: right;padding-top: 6px;padding-left: 0px;\">" +
                "<svg id=\"D" + numero_controle + "\" onclick=\"delete_parcela(this.id, 'confirmação')\" width =\"1em\" height=\"1em\" viewBox=\"0 0 16 16\" class=\"bi bi-trash\" fill=\"currentColor\" xmlns=\"http://www.w3.org/2000/svg\" style=\"cursor:pointer\">" +
                "<path d =\"M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z\" />" +
                "<path fill - rule=\"evenodd\" d=\"M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4L4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z\" />" +
                "</svg>" +
                "</div>" +
                "</div>";
           
            $('#parcelas').append(parcela);

            execDatapicker();            
        }
    }
}

function novaParcela(contexto) {    
    let numero_controle = Math.floor(Math.random() * 100000) + 1;
    let s = document.querySelector('#forma_pgto'); //Forma de pagamento selecionada;        
    let optionsTxt = "";

    for (let x = 0; x < s.children.length; x++) {
        let item = s.children[x].outerHTML;
        let textoItem = s.children[x].innerText;
        let selecionado = s.selectedOptions[0].innerText;

        let txt = "";
        if (textoItem == selecionado) {
            txt = item.substring(0, 16) + "\" selected='selected'" + item.substring(17, 200);
        } else {
            txt = item;
        }

        optionsTxt += txt;
    }
    if (contexto == 'open') {

        document.getElementById('novaParcela').innerHTML = "";

        let op_data = document.getElementById('op_data').value;

        if (op_data == "" || op_data == null) {
            alert("Informe a data de operação para gerar uma parcela");
        } else {
            let parcela = "" +
                "<div class=\"row\" id=\"parcela_" + numero_controle + "\">" +
                "<div class=\"col-10\" style=\"padding-right: 0px;\">" +
                "<input id=\"diasParcela_" + numero_controle + "\" onchange=\"update_nova_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 10%;padding:0px;text-align:center\" value=\"" + "" + "\" />" +
                "<input id=\"vencParcela_" + numero_controle + "\" onchange=\"update_nova_parcela(this.id, this.value)\" type =\"text\" class=\"include_item datepicker\" style=\"width: 25%\" value=\"" + "" + "\" />" +
                "<input id=\"vlrParcela_" + numero_controle + "\" onchange=\"update_nova_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 25%\" value=\"" + "0,00" + "\" />" +
                "<select id=\"fpParcela_" + numero_controle + "\" onchange=\"update_nova_parcela(this.id, this.value)\" class=\"include_item\" style=\"width: 25%\">" + optionsTxt + "</select>" +
                "<input id=\"obsParcela_" + numero_controle + "\" onchange=\"update_nova_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 15%\" />" +
                "" +
                "</div>" +
                "<div class=\"col-2\" style=\"text-align: right;padding-top: 6px;padding-left: 0px;\">" +
                "</div>" +
                "</div>";

            document.getElementById('parcela_numeroControle').value = numero_controle;
            let op_data = document.getElementById('op_data').value;
            document.getElementById('data_operacao').value = op_data;
            $('#novaParcela').append(parcela);
            $('#modal_parcela_insert').modal('show');
        }
    }

    if (contexto == 'gravar') {
        let controle = document.getElementById('parcela_numeroControle').value;

        let s = document.querySelector('#fpParcela_' + controle); //Forma de pagamento selecionada;        
        let optionsTxt_f = "";

        for (let x = 0; x < s.children.length; x++) {
            let item = s.children[x].outerHTML;
            let textoItem = s.children[x].innerText;
            let selecionado = s.selectedOptions[0].innerText;

            let txt = "";
            if (textoItem == selecionado) {
                txt = item.substring(0, 16) + "\" selected='selected'" + item.substring(17, 200);
            } else {
                txt = item;
            }

            optionsTxt_f += txt;
        }



        let Op_parcelas = {
            op_parcela_dias: document.getElementById('diasParcela_' + controle).value,
            op_parcela_vencimento: document.getElementById('vencParcela_' + controle).value,
            op_parcela_fp_id: document.getElementById('fpParcela_' + controle).value,
            op_parcela_valor: document.getElementById('vlrParcela_' + controle).value,
            op_parcela_obs: document.getElementById('obsParcela_' + controle).value,
            op_parcela_saldo: '',
            op_parcela_op_id: '',
            op_parcela_id: '',
            controleEdit: 'insert',
            op_parcela_numero_controle: numero_controle,
        };

        operacao.parcelas.push(Op_parcelas);

        let parcela = "" +
            "<div class=\"row\" id=\"parcela_" + numero_controle + "\">" +
            "<div class=\"col-10\" style=\"padding-right: 0px;\">" +
            "<input id=\"diasParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 10%;padding:0px;text-align:center\" value=\"" + document.getElementById('diasParcela_' + controle).value + "\" />" +
            "<input id=\"vencParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" type =\"text\" class=\"include_item datepicker\" style=\"width: 25%\" value=\"" + document.getElementById('vencParcela_' + controle).value + "\" />" +
            "<input id=\"vlrParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 25%\" value=\"" + document.getElementById('vlrParcela_' + controle).value + "\" />" +
            "<select id=\"fpParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" class=\"include_item\" style=\"width: 25%\">" + optionsTxt_f + "</select>" +
            "<input id=\"obsParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 15%\" value=\"" + document.getElementById('obsParcela_' + controle).value + "\" />" +
            "" +
            "</div>" +
            "<div class=\"col-2\" style=\"text-align: right;padding-top: 6px;padding-left: 0px;\">" +
            "<svg id=\"D" + numero_controle + "\" onclick=\"delete_parcela(this.id, 'confirmação')\" width =\"1em\" height=\"1em\" viewBox=\"0 0 16 16\" class=\"bi bi-trash\" fill=\"currentColor\" xmlns=\"http://www.w3.org/2000/svg\" style=\"cursor:pointer\">" +
            "<path d =\"M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z\" />" +
            "<path fill - rule=\"evenodd\" d=\"M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4L4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z\" />" +
            "</svg>" +
            "</div>" +
            "</div>";

        $('#parcelas').append(parcela);
        $('#modal_parcela_insert').modal('hide');
    }

    execDatapicker(); 
}

function execDatapicker() {
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
}

function delete_parcela(id, confirma) {
    let codigo = id.substring(1, 15);

    if (confirma == 'confirmação') {
        document.getElementById('parcela_delete').value = codigo;

        $('#modal_parcela_delete').modal('show');
    }

    if (confirma == 'confirmado') {
        let codigo = document.getElementById('parcela_delete').value;
        $('#modal_parcela_delete').modal('hide');

        let item = operacao.parcelas.find(item => item.op_parcela_numero_controle == codigo);

        for (let i = 0; i < operacao.parcelas.length; i++) {
            if (operacao.parcelas[i].op_parcela_numero_controle == codigo) {
                if (operacao.parcelas[i].controleEdit == 'update') {
                    operacao.parcelas[i].controleEdit = 'delete';
                    //remove linha da view
                    let item_linha = 'parcela_' + item.op_parcela_numero_controle;                    
                    document.getElementById(item_linha).remove();                    
                } else {
                    //remove item do objeto operacao da matriz itens
                    operacao.parcelas.splice(i, 1);
                    //remove linha da view
                    let item_linha = 'parcela_' + item.op_parcela_numero_controle;                    
                    document.getElementById(item_linha).remove();                                     
                }
            }
        }
    }
}

function update_parcela(id, vlr) {    
    let numero_controle = id.split('_');
    let item = operacao.parcelas.find(item => item.op_parcela_numero_controle == numero_controle[1]);

    for (let i = 0; i < operacao.parcelas.length; i++) {
        if (operacao.parcelas[i].op_parcela_numero_controle == numero_controle[1]) {
            if (numero_controle[0] == 'diasParcela') {
                operacao.parcelas[i].op_parcela_dias = vlr;

                let op_data = document.getElementById('op_data').value;                
                let data = new Date(Date.parse(op_data.substring(6, 10) + "/" + (op_data.substring(3, 5)) + "/" + op_data.substring(0, 2)));                
                let vencimento = new Date();
                vencimento = data;                
                vencimento.setDate(vencimento.getDate() + parseInt(vlr));                
                operacao.parcelas[i].op_parcela_vencimento = vencimento.toLocaleDateString();
                document.getElementById('vencParcela_' + numero_controle[1]).value = operacao.parcelas[i].op_parcela_vencimento;
            }
            if (numero_controle[0] == 'vencParcela') {
                operacao.parcelas[i].op_parcela_vencimento = vlr;

                let dataOperacao = document.getElementById('op_data').value.split('/');
                let novaData = vlr.split('/')
                let data1 = new Date(dataOperacao[2], dataOperacao[1], dataOperacao[0]);
                let data2 = new Date(novaData[2], novaData[1], novaData[0]);
                let umDia = 1000 * 60 * 60 * 24;
                let dias = (data2 - data1) / umDia;
                document.getElementById('diasParcela_' + numero_controle[1]).value = dias;
                operacao.parcelas[i].op_parcela_dias = dias.toString();
            }
            if (numero_controle[0] == 'vlrParcela') {
                decimal(id, vlr, '6');
                operacao.parcelas[i].op_parcela_valor = document.getElementById(id).value;
                
            }            
            if (numero_controle[0] == 'fpParcela') {
                operacao.parcelas[i].op_parcela_fp_id = vlr;
            }
            if (numero_controle[0] == 'obsParcela') {
                operacao.parcelas[i].op_parcela_obs = vlr;
            }
        }
    }    
}

function update_nova_parcela(id, vlr) {    
    let numero_controle = id.split('_');    
    if (numero_controle[0] == 'diasParcela') {       

        let op_data = document.getElementById('op_data').value;        
        let data = new Date(Date.parse(op_data.substring(6, 10) + "/" + (op_data.substring(3, 5)) + "/" + op_data.substring(0, 2)));        
        let vencimento = new Date();
        vencimento = data;        
        vencimento.setDate(vencimento.getDate() + parseInt(vlr));
        document.getElementById('vencParcela_' + numero_controle[1]).value = vencimento.toLocaleDateString();
    }
    if (numero_controle[0] == 'vencParcela') {       

        let dataOperacao = document.getElementById('op_data').value.split('/');
        let novaData = vlr.split('/')
        let data1 = new Date(dataOperacao[2], dataOperacao[1], dataOperacao[0]);
        let data2 = new Date(novaData[2], novaData[1], novaData[0]);
        let umDia = 1000 * 60 * 60 * 24;
        let dias = (data2 - data1) / umDia;        
        document.getElementById('diasParcela_' + numero_controle[1]).value = dias;        
    }
    if (numero_controle[0] == 'vlrParcela') {
        decimal(id, vlr, '6');
    }        
}

function modFrete(vlr) {    

    if (vlr == '9') {
        document.getElementById('op_transportador_nome').value = "";
        document.getElementById('op_transportador_cnpj_cpf').value = "";
        document.getElementById('op_transportador_volume_qtd').value = "";
        document.getElementById('op_transportador_volume_peso_bruto').value = "";

        document.getElementById('op_transportador_nome').disabled = true;
        document.getElementById('op_transportador_cnpj_cpf').disabled = true;
        document.getElementById('op_transportador_volume_qtd').disabled = true;
        document.getElementById('op_transportador_volume_peso_bruto').disabled = true;
    } else {
        document.getElementById('op_transportador_nome').disabled = false;
        document.getElementById('op_transportador_cnpj_cpf').disabled = false;
        document.getElementById('op_transportador_volume_qtd').disabled = false;
        document.getElementById('op_transportador_volume_peso_bruto').disabled = false;
    }
}

function getTransportador(id) {
    let id_campo = "#" + id;    
    $(id_campo).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Participante/consultaParticipante",
                data: { __RequestVerificationToken: gettoken(), termo: request.term },
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
                            value: results[i].participante_nome,
                            label: results[i].participante_nome + " - " + results[i].participante_cnpj_cpf,

                            id: results[i].participante_id,
                            nome: results[i].participante_nome,
                            tipo: results[i].participante_tipoPessoa,
                            cnpj_cpf: results[i].participante_cnpj_cpf,
                            cep: results[i].participante_cep,
                            rua: results[i].participante_logradouro,
                            numero: results[i].participante_numero,
                            complemento: results[i].participante_complemento,
                            bairro: results[i].participante_bairro,
                            cidade: results[i].participante_cidade,
                            uf: results[i].participante_uf,
                            pais: results[i].participante_pais,
                            categoria_id: results[i].participante_categoria,
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
            if (document.getElementById('op_transportador_nome')) {
                document.getElementById('op_transportador_nome').value = ui.item.nome;
            }            
            if (document.getElementById('op_transportador_cnpj_cpf')) {
                document.getElementById('op_transportador_cnpj_cpf').value = ui.item.cnpj_cpf;
            }
            if (document.getElementById('op_transportador_participante_id')) {
                document.getElementById('op_transportador_participante_id').value = ui.item.id;
            }            
        }
    });
}

function dadorParticipante(op_part_participante_id) {
    if (document.getElementById('nome')) {
        operacao.participante.op_part_nome = document.getElementById('nome').value;
    }
    if (document.getElementById('participante_tipoPessoa')) {
        operacao.participante.op_part_tipo = document.getElementById('participante_tipoPessoa').value;
    }
    if (document.getElementById('op_part_cnpj_cpf')) {
        operacao.participante.op_part_cnpj_cpf = document.getElementById('op_part_cnpj_cpf').value;
    }
    if (document.getElementById('cep')) {
        operacao.participante.op_part_cep = document.getElementById('cep').value;
    }
    if (document.getElementById('rua')) {
        operacao.participante.op_part_logradouro = document.getElementById('rua').value;
    }
    if (document.getElementById('numero')) {
        operacao.participante.op_part_numero = document.getElementById('numero').value;
    }
    if (document.getElementById('complemento')) {
        operacao.participante.op_part_complemento = document.getElementById('complemento').value;
    }
    if (document.getElementById('bairro')) {
        operacao.participante.op_part_bairro = document.getElementById('bairro').value;
    }
    if (document.getElementById('cidade')) {
        operacao.participante.op_part_cidade = document.getElementById('cidade').value;
    }
    if (document.getElementById('uf')) {
        operacao.participante.op_uf_ibge_codigo = document.getElementById('uf').value;
    }
    if (document.getElementById('op_paisesIBGE_codigo')) {
        operacao.participante.op_paisesIBGE_codigo = document.getElementById('op_paisesIBGE_codigo').value;
    }

    operacao.participante.op_part_participante_id = op_part_participante_id;

    $('#modal_fornecedor').modal('hide');

}