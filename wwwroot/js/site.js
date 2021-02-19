//import * as saveAs from "./FileSaver";

//Variaveis globais
let fechamentoCartao = {
    
    totalFatura: 0,
    meioPgto: 03,
    parcelas_agrupar: [],
};
let fechamento_cartao = {
    fc_forma_pagamento: 0,
    fc_meio_pagamento: 0,
    fc_qtd_parcelas: 0,
    fc_valor_total: 0,
    fc_tarifas_bancarias: 0,
    fc_seguro_cartao: 0,
    fc_estornos: 0,
    fc_abatimentos_cartao: 0,
    fc_acrescimos_cartao: 0,
    fc_referencia: '',
    fc_vencimento: '',
    fc_nome_cartão: '',
    fc_matriz_parcelas: [],
    fc_matriz_parcelas_text: '',
    fc_forma_pgto_boleto_fatura: 0,
    fc_op_obs: '',
};
var operacao = {
    operacao: {
        op_id: 0,
        op_tipo: 'OutrasDespesas',
        op_data: '',
        op_previsao_entrega: '',
        op_data_saida: '',
        op_obs: '',
        op_numero_ordem: '',
        op_categoria_id: 0,
        op_comParticipante: false,
        op_comRetencoes: false,
        op_comTransportador: false,
        possui_parcelas: false,
        op_comNF: 0,
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
        existe: false,
        controleEdit: '',
    },
    itens: [],
    retencoes: {
        op_ret_id: '',
        op_ret_pis: '',
        op_ret_cofins: '',
        op_ret_csll: '',
        op_ret_irrf: '',
        op_ret_inss: '',
        op_ret_issqn: '',
        existe: false,
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
        op_totais_saldoLiquidacao: 0,
        op_totais_preco_servicos: 0,
        op_totais_valor_outras_operacoes: 0,
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
        existe: false,
    },
    nf: {
        op_nf_id: 0,
        op_nf_op_id: 0,
        op_nf_tipo: 0,
        op_nf_chave: '',
        op_nf_data_emissao: '',
        op_nf_data_entrada_saida: '',
        op_nf_serie: '',
        op_nf_numero: '',
        existe: false,
    },
    servico: {
        op_servico_id: 0,
        op_servico_op_id: 0,
        op_servico_equipamento: '',
        op_servico_nSerie: '',
        op_servico_problema: '',
        op_servico_obsReceb: '',
        op_servico_servico_executado: '',
        op_servico_valor: 0,
        op_servico_status: '',
        op_servico_lc116: '',
    },
};

let ilcs = {
    status: '',
    qunatidade_erros: 0,
    list_ilc: [],
    filtro: {
        cliente_id: 0,
        data_inicial: '',
        data_final: '',
        gera_provisao_categoria_fiscal: false,
    }
};

let ilc = {
    ilc_sequencia: 0,
    ilc_data_lancamento: '',
    ilc_conta_debito: '',
    ilc_conta_credito: '',
    ilc_valor_lancamento: 0,
    ilc_codigo_historico: '',
    ilc_complemento_historico: '',
    ilc_numero_documento: '',
    ilc_lote_lancamento: '',
    ilc_cnpj_cpf_debito: '',
    ilc_cnpj_cpf_credito: '',
    ilc_contabilizacao_ifrs: '',
    ilc_transacao_sped: '',
    ilc_indicador_conciliacao: '',
    ilc_indicador_pendencia_concialiacao: '',
    ilc_obs_conciliacao_debito: '',
    ilc_obs_conciliacao_credito: '',
    status: '',
    mensagem: '',
    origem: '',
};

let apo = {};

//Onload página layout
function Page() {

    //executa o datapicker existentes
    execDatapicker();

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

    /*//Tela balancete contábil
    let Elemet_data_inicial = document.getElementById("data_inicial");
    if (Elemet_data_inicial) {
        let data = new Date();
        document.getElementById("data_inicial").value = '01/' + (data.getMonth() + 1) + "/" + data.getFullYear();

        var ultimoDia = new Date(data.getFullYear(), data.getMonth() + 1, 0);
        document.getElementById('data_final').value = ultimoDia.toLocaleDateString();
    }
    */

    //Tela create Participante
    let tipo_Pessoa = document.getElementById("participante_tipoPessoa");
    if (tipo_Pessoa) {
        tipoPessoa(tipo_Pessoa.value);
    }
    //Carregar página do edit compra
    if (document.getElementById('carregamentoCompra')) {
        carregarEdit(document.getElementById('carregamentoCompra').value);
    }
    //Carregar data nos input de id = op_data
    if (document.getElementById('op_data')) {
        let x = window.location.href.split('/');
        if (x.includes('Create')) {
            let d = new Date();
            document.getElementById('op_data').value = d.toLocaleDateString();
        }
    }

    //Carregar formas pagamento no contas financeiras
    if (document.getElementById('text_formaPgto')) {
        GerarSelectFormaPagamento();
    } 

    if (document.getElementById('op_categoria_id')) {
        $("#op_categoria_id").select2({
            placeholder: "Selecione uma categoria",
            allowClear: true
        });
    }

    //Função para desabilitar scroll
    $.fn.disableScroll = function () {
        window.oldScrollPos = $(window).scrollTop();

        $(window).on('scroll.scrolldisabler', function (event) {
            $(window).scrollTop(window.oldScrollPos);
            event.preventDefault();
        });
    };
    //Função para habilitar scroll
    $.fn.enableScroll = function () {
        $(window).off('scroll.scrolldisabler');
    };

    //let dp = document.querySelectorAll('.datepicker');
    //if (dp.length > 0) {
    //    execDatapicker();
    //}
}

function carregarEdit(id) {   
    $.ajax({
        url: "/Compra/consultaCompra",
        data: { __RequestVerificationToken: gettoken(), id: id },
        type: 'POST',
        dataType: 'json',
        beforeSend: function (XMLHttpRequest) {            
            if (document.getElementById('sub')) {
                document.getElementById('sub').disabled = true;
            }
            document.getElementById('carregamento').style.display = "block";
            document.getElementById('carregamento').innerHTML = "carregando...";
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("erro no carregamento da página!");
        },
        success: function (data, textStatus, XMLHttpRequest) {            
            var results = JSON.parse(data);            
            operacao = results;
            for (let i = 0; i < operacao.itens.length; i++) {
                operacao.itens[i].op_item_desconto = operacao.itens[i].op_item_desconto.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
                operacao.itens[i].op_item_desp_aces = operacao.itens[i].op_item_desp_aces.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
                operacao.itens[i].op_item_frete = operacao.itens[i].op_item_frete.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
                operacao.itens[i].op_item_preco = operacao.itens[i].op_item_preco.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
                operacao.itens[i].op_item_qtd = operacao.itens[i].op_item_qtd.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
                operacao.itens[i].op_item_seguros = operacao.itens[i].op_item_seguros.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
                operacao.itens[i].op_item_valor_total = operacao.itens[i].op_item_valor_total.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
                operacao.itens[i].op_item_vlr_icms_st = operacao.itens[i].op_item_vlr_icms_st.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
                operacao.itens[i].op_item_vlr_ipi = operacao.itens[i].op_item_vlr_ipi.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
                operacao.itens[i].op_item_numero_controle = operacao.itens[i].op_item_id;
            }

            for (let i = 0; i < operacao.parcelas.length; i++) {
                operacao.parcelas[i].op_parcela_saldo = operacao.parcelas[i].op_parcela_saldo.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
                operacao.parcelas[i].op_parcela_valor = operacao.parcelas[i].op_parcela_valor.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
                operacao.parcelas[i].op_parcela_valor_bruto = operacao.parcelas[i].op_parcela_valor_bruto.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
                operacao.parcelas[i].op_parcela_ret_inss = operacao.parcelas[i].op_parcela_ret_inss.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
                operacao.parcelas[i].op_parcela_ret_issqn = operacao.parcelas[i].op_parcela_ret_issqn.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
                operacao.parcelas[i].op_parcela_ret_irrf = operacao.parcelas[i].op_parcela_ret_irrf.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
                operacao.parcelas[i].op_parcela_ret_pis = operacao.parcelas[i].op_parcela_ret_pis.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
                operacao.parcelas[i].op_parcela_ret_cofins = operacao.parcelas[i].op_parcela_ret_cofins.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
                operacao.parcelas[i].op_parcela_ret_csll = operacao.parcelas[i].op_parcela_ret_csll.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
                operacao.parcelas[i].op_parcela_numero_controle = operacao.parcelas[i].op_parcela_id;
                let data = operacao.parcelas[i].op_parcela_vencimento.substring(0, 10).split('-');                
                operacao.parcelas[i].op_parcela_vencimento = data[2] + '/' + data[1] + '/' + data[0];

                let id = '#fpParcela_' + operacao.parcelas[i].op_parcela_id;
                $(id).val(operacao.parcelas[i].op_parcela_fp_id.toString());
            }
            //retenções
            operacao.retencoes.op_ret_pis = operacao.retencoes.op_ret_pis.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.retencoes.op_ret_cofins = operacao.retencoes.op_ret_cofins.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.retencoes.op_ret_csll = operacao.retencoes.op_ret_csll.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.retencoes.op_ret_inss = operacao.retencoes.op_ret_inss.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.retencoes.op_ret_irrf = operacao.retencoes.op_ret_irrf.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.retencoes.op_ret_issqn = operacao.retencoes.op_ret_issqn.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            //totais
            operacao.totais.op_totais_desconto = operacao.totais.op_totais_desconto.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.totais.op_totais_desp_aces = operacao.totais.op_totais_desp_aces.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.totais.op_totais_frete = operacao.totais.op_totais_frete.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.totais.op_totais_icms_st = operacao.totais.op_totais_icms_st.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.totais.op_totais_ipi = operacao.totais.op_totais_ipi.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.totais.op_totais_preco_itens = operacao.totais.op_totais_preco_itens.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.totais.op_totais_qtd_itens = operacao.totais.op_totais_qtd_itens.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.totais.op_totais_retencoes = operacao.totais.op_totais_retencoes.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.totais.op_totais_saldoLiquidacao = operacao.totais.op_totais_saldoLiquidacao.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.totais.op_totais_seguro = operacao.totais.op_totais_seguro.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.totais.op_totais_total_op = operacao.totais.op_totais_total_op.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            //transportador
            operacao.transportador.op_transportador_volume_peso_bruto = operacao.transportador.op_transportador_volume_peso_bruto.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            operacao.transportador.op_transportador_volume_qtd = operacao.transportador.op_transportador_volume_qtd.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });

            //Verificando se existe retenções e exibir na tela de acordo com a informações
            if (document.getElementById('op_comRetencoes')) {
                if (operacao.operacao.op_comRetencoes) {
                    document.getElementById('box_retencoes').style.display = 'block';
                } else {
                    document.getElementById('box_retencoes').style.display = 'none';
                }
            }

            //verificando se há nota e liberar disabled dos campos
            if (operacao.operacao.op_comNF != 0) {
                let x = document.URL;
                if (!x.includes('Details')) {
                    if (operacao.operacao.op_comNF == 1) {
                        document.getElementById('op_nf_chave').disabled = false;
                        document.getElementById('op_nf_data_emissao').disabled = false;
                        document.getElementById('op_nf_data_entrada_saida').disabled = false;
                        document.getElementById('op_nf_serie').disabled = true;
                        document.getElementById('op_nf_numero').disabled = true;
                    }
                    if (operacao.operacao.op_comNF == 2) {
                        document.getElementById('op_nf_chave').disabled = true;
                        document.getElementById('op_nf_data_emissao').disabled = false;
                        document.getElementById('op_nf_data_entrada_saida').disabled = false;
                        document.getElementById('op_nf_serie').disabled = false;
                        document.getElementById('op_nf_numero').disabled = false;
                    }                    
                }
            }



            //Reabilitando pós carregamento
            document.getElementsByTagName('svg').disabled = false;
            if (document.getElementById('sub')) {
                document.getElementById('sub').disabled = false;
            }            
            document.getElementById('carregamento').innerHTML = "";
            document.getElementById('carregamento').style.display = "nome";

            if (operacao.operacao.possui_parcelas) { 
                document.getElementById('carregamento').innerHTML = "";
                document.getElementById('carregamento').style.display = "block";
                document.getElementById('carregamento').innerHTML = "Esta operação possui baixas e não pode ser alterada!";
            }




        }
    });
}

//habilita tooltip
$(function () {
    $('[data-toggle="tooltip"]').tooltip()
});



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
            //document.getElementById("corpo").style.marginLeft = "250px";
        } else {
            document.getElementById("mySidenav").style.width = "0";
            //document.getElementById("corpo").style.marginLeft = "0";
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
    if (tamanho > 4) {
        document.getElementById(id).value = valor.substring(0, 4);
    }
}

function verificaClassificacaoGrupo(id, vlr) {
    let tamanho = vlr.length;
    if (tamanho != 4) {
        alert('A Classificação deve ter 4 caracteres, exemplo: 2.01');
        document.getElementById(id).focus();
    }
}

function montaClassificacao(valor, grupo) {
    document.getElementById("categoria_classificacao").value = grupo + "." + valor.padStart(3, '0');
}

//Bread alteração de empresa selecionada pelo contador;
$(".SelectClient").click(function () {
    //var id = $(this).attr("data-id");
    var url = window.location.href;
    url =  url.replace('&', '|');
    console.log(url);    
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
        width: 'resolve',
        containerCssClass: ':all:',        
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
        changeMonth: true,
        changeYear: true,
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

function decimal(id, vlr, limit, alerta) {

    if (vlr == 0 || vlr == '0' || vlr == '0.00' || vlr == '0,00') {
        document.getElementById(id).value = (0).toFixed(2);;
        return;
    }

    let matriz = vlr.toString().replace('.','').split(","); 
    //console.log(vlr);
    //console.log(matriz);

    if (matriz.length > 1) {
        let tamanho = matriz[1].length;

        if (tamanho > limit) {
            if (alerta) {
                alert('Permitido até ' + limit + ' dígitos nas casas decimais!');
            }            
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
function consultaParticipante(id) {
    let id_campo = "#" + id;
    $(id_campo).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Participante/consultaParticipante",
                data: { __RequestVerificationToken: gettoken(), termo: request.term },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    if (document.getElementById('msg_consuta_participante')) {
                        document.getElementById('msg_consuta_participante').innerHTML = 'pesquisando...';
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    document.getElementById('fornecedor').value = 'Nenhum participante informado';
                    alert("erro");
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    if (document.getElementById('msg_consuta_participante')) {
                        document.getElementById('msg_consuta_participante').innerHTML = '';
                    }
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
            if (document.getElementById('op_part_participante_id')) {
                document.getElementById('op_part_participante_id').value = ui.item.id;
            }

            //Atribuindo a categoria no select2
            if (document.getElementById('cf_categoria_id')) {
                $('#cf_categoria_id').val(ui.item.categoria_id.toString()); 
                $('#cf_categoria_id').trigger('change');
            }
            //Atribuindo a categoria no select2 quando id = op_categoria_id
            if (document.getElementById('op_categoria_id')) {
                $('#op_categoria_id').val(ui.item.categoria_id.toString());
                $('#op_categoria_id').trigger('change');
            }

            //Carregando dados do parcitipante no opjeto operação.
            dadorParticipante('insert',ui.item.id);
            
        }
    });
}

function verificaParticipante(id, vlr) { 
    if (vlr == "" || vlr == null || operacao.participante.op_part_nome != vlr) {
        participante = {
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
            existe: false,
            controleEdit: '',
        };

        operacao.participante = participante;

        document.getElementById(id).value = '';
    }
}

//Autocomplete lista de produtos
function consultaProdutos(id, contexto) {
    let id_campo = "#" + id;
    $(id_campo).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Produtos/consultaProdutos",
                data: { __RequestVerificationToken: gettoken(), termo: request.term },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    if (document.getElementById('msg_pesquisa_item')) {
                        document.getElementById('msg_pesquisa_item').innerHTML = 'pesquisando...';
                    }
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

                    if (document.getElementById('msg_pesquisa_item')) {
                        document.getElementById('msg_pesquisa_item').innerHTML = '';
                    }
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
                    decimal('prod_valor', ui.item.valorCompra.toString().replace(".", ","), '6',false);
                    decimal('prod_valorTotal', ui.item.valorCompra.toString().replace(".", ","), '6', false);                    
                }
            }
            if (contexto == 'venda') {
                if (document.getElementById('prod_valor')) {
                    decimal('prod_valor', ui.item.valorVenda.toString().replace(".", ","), '6', false);
                    decimal('prod_valorTotal', ui.item.valorVenda.toString().replace(".", ","), '6', false);
                }
            }
            if (document.getElementById('prod_quantidade')) {
                decimal('prod_quantidade', '1', '6', false);
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

        if (isNaN((prod_valor.toString().replace('.', '').replace(',', '.') * 1)) || isNaN((prod_quantidade.toString().replace('.', '').replace(',', '.') * 1)) || isNaN((prod_valorTotal.toString().replace('.', '').replace(',', '.') * 1))) {
            alert('Valor incorreto no valor unitário, quantidade ou total do item');
            document.getElementById('prod_descricao').focus();
        } else {
            let item_produto = {
                op_item_numero_controle: numero_controle, //id é o produto_id selecionado. Adicionaod ao objeto para fins de controle (localiza-lo no momento da edição dos dados);
                op_item_produto_id: id,
                op_item_id: numero_controle,
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

            //Inserindo em totais total de itens
            operacao.totais.op_totais_itens += 1;
            //Inserindo em totais a soma das quantidades totais
            let qtd_total = (operacao.totais.op_totais_qtd_itens.toString().replace('.', '').replace(',', '.') * 1) + (prod_quantidade.toString().replace('.', '').replace(',', '.') * 1);
            operacao.totais.op_totais_qtd_itens = qtd_total.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });

            document.getElementById('prod_descricao').focus();

            totaisOperacao();
        }
        
    } else {
        document.getElementById('prod_descricao').focus();
    }    
}

function ajusta_item(id, vlr) {
    let qtd = document.getElementById('prod_quantidade').value;
    let vlrProd = document.getElementById('prod_valor').value;
    let total = qtd.toString().replace(".", "").replace(",", ".") * vlrProd.toString().replace(".", "").replace(",", ".");

    decimal('prod_valorTotal', total.toString().replace(".", ","), '6', true);
    decimal(id, vlr, '6', true);    
}

function changeItens(id, vlr, inputTotalizador) {
    let preco = document.getElementById('op_item_preco').value.toString().replace('.', '').replace(',', '.') * 1;
    let qtd = document.getElementById('op_item_qtd').value.toString().replace('.', '').replace(',', '.') * 1;
    let frete = document.getElementById('op_item_frete').value.toString().replace('.', '').replace(',', '.') * 1;
    let seguros = document.getElementById('op_item_seguros').value.toString().replace('.', '').replace(',', '.') * 1;
    let despesas = document.getElementById('op_item_desp_aces').value.toString().replace('.', '').replace(',', '.') * 1;
    let descontos = document.getElementById('op_item_desconto').value.toString().replace('.', '').replace(',', '.') * 1;
    
    let total = (preco * qtd);    

    decimal(id, vlr.toString().replace('.', ','), '6',true);
    decimal('item_valor_total', total.toString().replace('.',','),'6',true);
}

function edit_item(id) {
    let codigo = id.substring(1, 15);
    let item = operacao.itens.find(item => item.op_item_numero_controle == codigo);    

    document.getElementById('op_item_numero_controle_edicao_item').value = item.op_item_numero_controle;
    document.getElementById('op_item_nome').value = item.op_item_nome;
    document.getElementById('op_item_codigo').value = item.op_item_codigo;
    document.getElementById('op_item_cod_fornecedor').value = item.op_item_cod_fornecedor;
    document.getElementById('op_item_unidade').value = item.op_item_unidade;
    decimal('op_item_preco', item.op_item_preco.toString().replace('.', ''), '6', true);
    decimal('op_item_qtd', item.op_item_qtd.toString().replace('.', ''), '6', true);
    decimal('item_valor_total', item.op_item_valor_total.toString().replace('.', ''), '6', true);
    decimal('op_item_frete', item.op_item_frete.toString().replace('.', ''), '6', true);
    decimal('op_item_seguros', item.op_item_seguros.toString().replace('.', ''), '6', true);
    decimal('op_item_desp_aces', item.op_item_desp_aces.toString().replace('.', ''), '6', true);
    decimal('op_item_desconto', item.op_item_desconto.toString().replace('.', ''), '6', true);
    decimal('op_item_vlr_ipi', item.op_item_vlr_ipi.toString().replace('.', ''), '6', true);
    decimal('op_item_vlr_icms_st', item.op_item_vlr_icms_st.toString().replace('.', ''), '6', true);

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

        //Inserindo em totais total de itens
        operacao.totais.op_totais_itens -= 1;
        //Inserindo em totais a soma das quantidades totais
        let qtd_total = (operacao.totais.op_totais_qtd_itens.toString().replace('.', '').replace(',', '.') * 1) - (item.op_item_qtd.toString().replace('.', '').replace(',', '.') * 1);
        operacao.totais.op_totais_qtd_itens = qtd_total.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
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
    let op_totais_preco_servicos = 0;

    /*if (operacao.operacao.op_tipo == 'ServicoPrestado' || operacao.operacao.op_tipo == 'ServicoTomado') {
        
    }*/

    if (document.getElementById('op_totais_preco_servicos')) {        
        op_totais_preco_servicos = document.getElementById('op_servico_valor').value.replace('.', '').replace(',', '.') * 1;

        operacao.totais.op_totais_preco_servicos = op_totais_preco_servicos.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
    }   

    if (document.getElementById('op_totais_preco_servicos')) {        
        decimal('op_totais_preco_servicos', operacao.totais.op_totais_preco_servicos.toString().replace('.', ''), '2', false);
    }

    

    for (let i = 0; i < operacao.itens.length; i++) {
        if (operacao.itens[i].controleEdit == 'update' || operacao.itens[i].controleEdit == 'insert') {
            op_totais_frete += ((operacao.itens[i].op_item_frete).toString().replace('.', '').replace(',', '.') * 1);
            op_totais_seguro += ((operacao.itens[i].op_item_seguros).toString().replace('.', '').replace(',', '.') * 1);
            op_totais_desp_aces += ((operacao.itens[i].op_item_desp_aces).toString().replace('.', '').replace(',', '.') * 1);
            op_totais_desconto += ((operacao.itens[i].op_item_desconto).toString().replace('.', '').replace(',', '.') * 1);
            op_totais_preco_itens += ((operacao.itens[i].op_item_valor_total).toString().replace('.', '').replace(',', '.') * 1);            
            op_totais_icms_st += ((operacao.itens[i].op_item_vlr_icms_st).toString().replace('.', '').replace(',', '.') * 1);            
            op_totais_ipi += ((operacao.itens[i].op_item_vlr_ipi).toString().replace('.', '').replace(',', '.') * 1);            
        }
    }

    let op_totais_total_op = op_totais_preco_itens + op_totais_frete + op_totais_seguro + op_totais_desp_aces - op_totais_desconto + op_totais_icms_st + op_totais_ipi + op_totais_preco_servicos;

    decimal('op_totais_frete', op_totais_frete.toString().replace('.', ','), '6', false);
    decimal('op_totais_seguro', op_totais_seguro.toString().replace('.', ','), '6', false);
    decimal('op_totais_desp_aces', op_totais_desp_aces.toString().replace('.', ','), '6', false);
    decimal('op_totais_desconto', op_totais_desconto.toString().replace('.', ','), '6', false);
    if (document.getElementById('op_totais_preco_itens')) {
        decimal('op_totais_preco_itens', op_totais_preco_itens.toString().replace('.', ','), '6', false);
    }
    decimal('op_totais_total_op', op_totais_total_op.toString().replace('.', ','), '6', false);
    if (document.getElementById('op_totais_icms_st')) {
        decimal('op_totais_icms_st', op_totais_icms_st.toString().replace('.', ','), '6', false);
    }
    if (document.getElementById('op_totais_ipi')) {
        decimal('op_totais_ipi', op_totais_ipi.toString().replace('.', ','), '6', false);
    }    

    if (document.getElementById('op_totais_preco_itens')) {
        operacao.totais.op_totais_preco_itens = document.getElementById('op_totais_preco_itens').value;
    }
    if (document.getElementById('op_totais_frete')) {
        operacao.totais.op_totais_frete = document.getElementById('op_totais_frete').value;
    }
    if (document.getElementById('op_totais_seguro')) {
        operacao.totais.op_totais_seguro = document.getElementById('op_totais_seguro').value;
    }
    if (document.getElementById('op_totais_desp_aces')) {
        operacao.totais.op_totais_desp_aces = document.getElementById('op_totais_desp_aces').value;
    }
    if (document.getElementById('op_totais_desconto')) {
        operacao.totais.op_totais_desconto = document.getElementById('op_totais_desconto').value;
    }
    if (document.getElementById('op_totais_total_op')) {
        operacao.totais.op_totais_total_op = document.getElementById('op_totais_total_op').value;
    }
    if (document.getElementById('op_totais_icms_st')) {
        operacao.totais.op_totais_icms_st = document.getElementById('op_totais_icms_st').value;
    }
    if (document.getElementById('op_totais_ipi')) {
        operacao.totais.op_item_vlr_ipi = document.getElementById('op_totais_ipi').value;
    }
    if (document.getElementById('op_servico_valor')) {
        operacao.totais.op_totais_preco_servicos = document.getElementById('op_servico_valor').value;
    }

    if (document.getElementById('totais')) {
        decimal('totais', operacao.totais.op_totais_total_op.toString().replace('.', ''), '2', false);
    }

}

function gerarParcela() {    
    document.getElementById('parcelas').innerHTML = ""; //Limpar view
    //Apagando parcelas existentes
    operacao.parcelas.splice(0, operacao.parcelas.length);
    
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
            //Apuração do valor da parcela em decorrência das Retenções
            valorParcela = (totalCompra / condicaoPgto.length);
            let inss = 0;
            let issqn = 0;
            let irrf = 0;

            if (document.getElementById('op_comRetencoes') && document.getElementById('op_comRetencoes').checked == true) { //se switch 'Operação com retenções tributárias' existe e está checado

                if (document.getElementById('ret_inss_parcela') && document.getElementById('ret_inss_parcela').checked == true) {
                    if (i == 0) {
                        valorParcela -= ((document.getElementById('op_ret_inss').value).toString().replace('.', '').replace(',', '.')) * 1;
                        inss = ((document.getElementById('op_ret_inss').value).toString().replace('.', '').replace(',', '.')) * 1;
                    }
                }
                if (document.getElementById('ret_inss_parcela') && document.getElementById('ret_inss_parcela').checked == false) {
                    valorParcela -= (((document.getElementById('op_ret_inss').value).toString().replace('.', '').replace(',', '.')) * 1) / condicaoPgto.length;
                    inss = (((document.getElementById('op_ret_inss').value).toString().replace('.', '').replace(',', '.')) * 1) / condicaoPgto.length;
                }               

                if (document.getElementById('ret_iss_parcela') && document.getElementById('ret_iss_parcela').checked == true) {
                    if (i == 0) {
                        valorParcela -= ((document.getElementById('op_ret_issqn').value).toString().replace('.', '').replace(',', '.')) * 1;
                        issqn = ((document.getElementById('op_ret_issqn').value).toString().replace('.', '').replace(',', '.')) * 1;
                    }
                }
                if (document.getElementById('ret_iss_parcela') && document.getElementById('ret_iss_parcela').checked == false) {
                    valorParcela -= (((document.getElementById('op_ret_issqn').value).toString().replace('.', '').replace(',', '.')) * 1) / condicaoPgto.length;
                    issqn = (((document.getElementById('op_ret_issqn').value).toString().replace('.', '').replace(',', '.')) * 1) / condicaoPgto.length;
                }
                if (document.getElementById('ret_irrf_parcela') && document.getElementById('ret_irrf_parcela').checked == true) {
                    if (i == 0) {
                        valorParcela -= ((document.getElementById('op_ret_irrf').value).toString().replace('.', '').replace(',', '.')) * 1;
                        irrf = ((document.getElementById('op_ret_irrf').value).toString().replace('.', '').replace(',', '.')) * 1;
                    }
                }
                if (document.getElementById('ret_irrf_parcela') && document.getElementById('ret_irrf_parcela').checked == false) {
                    valorParcela -= (((document.getElementById('op_ret_irrf').value).toString().replace('.', '').replace(',', '.')) * 1) / condicaoPgto.length;
                    irrf = (((document.getElementById('op_ret_irrf').value).toString().replace('.', '').replace(',', '.')) * 1) / condicaoPgto.length;
                }
            }            
            //Fim

            let vencimento = new Date();                    
            vencimento.setDate(data.getDate() + parseInt(condicaoPgto[i]));

            let numero_controle = i + Math.floor(Math.random() * 100000) + 1;

            let Op_parcelas = {
                op_parcela_dias: condicaoPgto[i],
                op_parcela_vencimento: vencimento.toLocaleDateString(),
                op_parcela_fp_id: formaPgto.value,
                op_parcela_valor: (valorParcela).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" }),
                op_parcela_valor_bruto: (totalCompra / condicaoPgto.length).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" }),
                op_parcela_obs: '',
                op_parcela_saldo: '',
                op_parcela_op_id: '',
                op_parcela_id: '',
                controleEdit: 'insert',
                op_parcela_numero_controle: numero_controle,
                op_parcela_ret_inss: (inss).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" }),
                op_parcela_ret_issqn: (issqn).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" }),
                op_parcela_ret_irrf: (irrf).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" }),
                op_parcela_ret_pis: '0.00',
                op_parcela_ret_cofins: '0.00',
                op_parcela_ret_csll: '0.00',
            };

            operacao.parcelas.push(Op_parcelas);

            let parcela = "" +
                "<div class=\"row\" id=\"parcela_" + numero_controle + "\">" +               
                "<div class=\"col-12\" style=\"padding-right: 0px;\">" +
                "<input id=\"diasParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 10%;padding:0px;text-align:center\" value=\"" + Op_parcelas.op_parcela_dias + "\" />" +
                "<input id=\"vencParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" type =\"text\" class=\"include_item datepicker\" style=\"width: 25%\" value=\"" + Op_parcelas.op_parcela_vencimento + "\" />" +
                "<input readonly=\"readonly\" id=\"vlrParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 25%\" value=\"" + Op_parcelas.op_parcela_valor + "\" />" +
                "<select id=\"fpParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" class=\"include_item\" style=\"width: calc(40% - 95px)\">" + optionsTxt + "</select>" +                
                "<div class=\"input-group-append\" style=\"float: left\" id=\"E" + numero_controle + "\" onclick=\"edit_parcela(this.id,'open')\"><button class=\"btn btn-outline-secondary\" type=\"button\" data-toggle=\"modal\" data-target=\"#modal_retencoes\"><svg width =\"1em\" height=\"1em\" viewBox=\"0 0 16 16\" class=\"bi bi-clipboard-data\" fill=\"currentColor\" xmlns=\"http://www.w3.org/2000/svg\"><path fill - rule=\"evenodd\" d=\"M4 1.5H3a2 2 0 0 0-2 2V14a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V3.5a2 2 0 0 0-2-2h-1v1h1a1 1 0 0 1 1 1V14a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V3.5a1 1 0 0 1 1-1h1v-1z\"/><path fill - rule=\"evenodd\" d=\"M9.5 1h-3a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h3a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm-3-1A1.5 1.5 0 0 0 5 1.5v1A1.5 1.5 0 0 0 6.5 4h3A1.5 1.5 0 0 0 11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3z\"/><path d =\"M4 11a1 1 0 1 1 2 0v1a1 1 0 1 1-2 0v-1zm6-4a1 1 0 1 1 2 0v5a1 1 0 1 1-2 0V7zM7 9a1 1 0 0 1 2 0v3a1 1 0 1 1-2 0V9z\"/></svg ></button ></div >" +
                "<div class='input-group-append' style=\"float: left\" id=\"D" + numero_controle + "\" onclick=\"delete_parcela(this.id, 'confirmação')\" ><button class='btn btn-outline-secondary' type='button'><svg  width =\"1em\" height=\"1em\" viewBox=\"0 0 16 16\" class=\"bi bi-trash\" fill=\"red\" xmlns=\"http://www.w3.org/2000/svg\" style=\"cursor:pointer\"><path d =\"M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z\" /><path fill - rule=\"evenodd\" d=\"M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4L4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z\" /></svg></button ></div> " +
                "</div>" +                
                "</div>";

            $('#parcelas').append(parcela);

            execDatapicker();            
        }
    }
}

function edit_parcela(id, contexto) {
    if (contexto == 'open') {
        let codigo = id.substring(1, 15);
        let item = operacao.parcelas.find(item => item.op_parcela_numero_controle == codigo);

        if (document.getElementById('op_parcela_ret_inss')) {
            decimal('op_parcela_ret_inss', item.op_parcela_ret_inss.replace('.', ''), '2', false);
        }
        if (document.getElementById('op_parcela_ret_issqn')) {
            decimal('op_parcela_ret_issqn', item.op_parcela_ret_issqn.replace('.', ''), '2', false);
        }
        if (document.getElementById('op_parcela_ret_irrf')) {
            decimal('op_parcela_ret_irrf', item.op_parcela_ret_irrf.replace('.', ''), '2', false);
        }
        if (document.getElementById('op_parcela_ret_pis')) {
            decimal('op_parcela_ret_pis', item.op_parcela_ret_pis.replace('.', ''), '2', false);
        }
        if (document.getElementById('op_parcela_ret_cofins')) {
            decimal('op_parcela_ret_cofins', item.op_parcela_ret_cofins.replace('.', ''), '2', false);
        }
        if (document.getElementById('op_parcela_ret_csll')) {
            decimal('op_parcela_ret_csll', item.op_parcela_ret_csll.replace('.', ''), '2', false);
        }

        if (document.getElementById('op_parcela_valor_bruto')) {
            decimal('op_parcela_valor_bruto', item.op_parcela_valor_bruto.replace('.', ''), '2', false);
        }
        if (document.getElementById('op_parcela_valor')) {
            decimal('op_parcela_valor', item.op_parcela_valor.replace('.', ''), '2', false);
        }
        if (document.getElementById('controleEditParcela')) {
            document.getElementById('controleEditParcela').value = codigo;
        }
    }

    if (contexto == 'gravar') {
        let controle = document.getElementById('controleEditParcela').value;
        for (let i = 0; i < operacao.parcelas.length; i++) {
            if (operacao.parcelas[i].op_parcela_numero_controle == controle) {
                if (document.getElementById('op_parcela_ret_inss')) { operacao.parcelas[i].op_parcela_ret_inss = document.getElementById('op_parcela_ret_inss').value }
                if (document.getElementById('op_parcela_ret_issqn')) {operacao.parcelas[i].op_parcela_ret_issqn = document.getElementById('op_parcela_ret_issqn').value}
                if (document.getElementById('op_parcela_ret_irrf')) { operacao.parcelas[i].op_parcela_ret_irrf = document.getElementById('op_parcela_ret_irrf').value}
                if (document.getElementById('op_parcela_ret_pis')) { operacao.parcelas[i].op_parcela_ret_pis = document.getElementById('op_parcela_ret_pis').value }
                if (document.getElementById('op_parcela_ret_cofins')) { operacao.parcelas[i].op_parcela_ret_cofins = document.getElementById('op_parcela_ret_cofins').value }
                if (document.getElementById('op_parcela_ret_csll')) { operacao.parcelas[i].op_parcela_ret_csll = document.getElementById('op_parcela_ret_csll').value }
                if (document.getElementById('op_parcela_valor')) { operacao.parcelas[i].op_parcela_valor = document.getElementById('op_parcela_valor').value }
                if (document.getElementById('op_parcela_valor_bruto')) { operacao.parcelas[i].op_parcela_valor_bruto = document.getElementById('op_parcela_valor_bruto').value }
                document.getElementById('vlrParcela_' + operacao.parcelas[i].op_parcela_numero_controle).value = operacao.parcelas[i].op_parcela_valor;
                break;
            }
        }
        $('#modal_retencoes').modal('hide');
    }

    let inss = 0;
    let iss = 0;
    let ir = 0;
    let pis = 0;
    let cofins = 0;
    let cs = 0;
    let valorTotalParcelas = 0;
    for (let i = 0; i < operacao.parcelas.length; i++) {
        valorTotalParcelas += ((operacao.parcelas[i].op_parcela_valor_bruto).toString().replace('.', '').replace(',', '.')) * 1;
        inss += ((operacao.parcelas[i].op_parcela_ret_inss).toString().replace('.', '').replace(',', '.')) * 1;
        iss += ((operacao.parcelas[i].op_parcela_ret_issqn).toString().replace('.', '').replace(',', '.')) * 1;
        ir += ((operacao.parcelas[i].op_parcela_ret_irrf).toString().replace('.', '').replace(',', '.')) * 1;
        pis += ((operacao.parcelas[i].op_parcela_ret_pis).toString().replace('.', '').replace(',', '.')) * 1;
        cofins += ((operacao.parcelas[i].op_parcela_ret_cofins).toString().replace('.', '').replace(',', '.')) * 1;
        cs += ((operacao.parcelas[i].op_parcela_ret_csll).toString().replace('.', '').replace(',', '.')) * 1;
    }
    let totalCompra = ((document.getElementById('op_totais_total_op').value).toString().replace('.', '').replace(',', '.')) * 1;
    //Atualiza valores da retenções no objeto de operação
    operacao.retencoes.op_ret_inss = inss.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
    operacao.retencoes.op_ret_issqn = iss.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
    operacao.retencoes.op_ret_irrf = ir.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
    operacao.retencoes.op_ret_pis = pis.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
    operacao.retencoes.op_ret_cofins = cofins.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
    operacao.retencoes.op_ret_csll = cs.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
    //Atualizando os campos da view com as retenções
    if (document.getElementById('op_ret_inss')) { decimal('op_ret_inss', operacao.retencoes.op_ret_inss, '2', false) }
    if (document.getElementById('op_ret_issqn')) { decimal('op_ret_issqn', operacao.retencoes.op_ret_issqn, '2', false) }
    if (document.getElementById('op_ret_irrf')) { decimal('op_ret_irrf', operacao.retencoes.op_ret_irrf, '2', false) }
    if (document.getElementById('op_ret_pis')) { decimal('op_ret_pis', operacao.retencoes.op_ret_pis, '2', false) }
    if (document.getElementById('op_ret_cofins')) { decimal('op_ret_cofins', operacao.retencoes.op_ret_cofins, '2', false) }
    if (document.getElementById('op_ret_csll')) { decimal('op_ret_csll', operacao.retencoes.op_ret_csll, '2', false) }    

    if (valorTotalParcelas > totalCompra) {
        document.getElementById('alert_vlr_parcela').innerHTML = '<p style="color:red">O valor total bruto das parcelas é maior que o total da compra. Isso impedirá a gravação da operação! Total bruto igual a <strong>' + valorTotalParcelas.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" }) + '</strong></p>';
    } else {
        document.getElementById('alert_vlr_parcela').innerHTML = "";
    }
}

function edit_vlr_parcela(id, vlr) {
    vlr = vlr.replace('.', '').replace(',', '.') * 1;

    let inss = 0;
    let iss = 0;
    let ir = 0;
    let pis = 0;
    let cofins = 0;
    let cs = 0;
    //atribui valores as variaveis se existir o input
    if (document.getElementById('op_parcela_ret_inss')) { inss = ((document.getElementById('op_parcela_ret_inss').value).toString().replace('.', '').replace(',', '.')) * 1 }
    if (document.getElementById('op_parcela_ret_issqn')) { iss = ((document.getElementById('op_parcela_ret_issqn').value).toString().replace('.', '').replace(',', '.')) * 1 }
    if (document.getElementById('op_parcela_ret_irrf')) { ir = ((document.getElementById('op_parcela_ret_irrf').value).toString().replace('.', '').replace(',', '.')) * 1 }
    if (document.getElementById('op_parcela_ret_pis')) { pis = ((document.getElementById('op_parcela_ret_pis').value).toString().replace('.', '').replace(',', '.')) * 1 }
    if (document.getElementById('op_parcela_ret_cofins')) { cofins = ((document.getElementById('op_parcela_ret_cofins').value).toString().replace('.', '').replace(',', '.')) * 1 }
    if (document.getElementById('op_parcela_ret_csll')) { cs = ((document.getElementById('op_parcela_ret_csll').value).toString().replace('.', '').replace(',', '.')) * 1 }


    if (id == 'op_parcela_valor_bruto') {
        let liquido = ((document.getElementById('op_parcela_valor').value).toString().replace('.', '').replace(',', '.')) * 1;
        decimal('op_parcela_valor', (vlr - inss - iss - ir - pis - cofins - cs).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" }).replace('.', ''), '2', true);
        decimal(id, vlr.toString(), '2', true);
    }
    if (id == 'op_parcela_valor') {
        let bruto = ((document.getElementById('op_parcela_valor_bruto').value).toString().replace('.', '').replace(',', '.')) * 1;        
        decimal('op_parcela_valor_bruto', (vlr + inss + iss + ir + pis + cofins + cs).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" }), '2', true);        
        decimal(id, vlr.toString(), '2', true);
    }

    if (id != 'op_parcela_valor' && id != 'op_parcela_valor_bruto') {
        decimal(id, (vlr).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" }).replace('.', ''), '2', true);
        let valorBruto = ((document.getElementById('op_parcela_valor_bruto').value).toString().replace('.', '').replace(',', '.')) * 1;
        decimal('op_parcela_valor', (valorBruto - inss - iss - ir - pis - cofins - cs).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" }).replace('.', ''), '2', true);
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
                "<div class=\"col-12\" style=\"padding-right: 0px;\">" +
                "<input id=\"diasParcela_" + numero_controle + "\" onchange=\"update_nova_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 10%;padding:0px;text-align:center\" value=\"" + '' + "\" />" +
                "<input id=\"vencParcela_" + numero_controle + "\" onchange=\"update_nova_parcela(this.id, this.value)\" type =\"text\" class=\"include_item datepicker\" style=\"width: 25%\" value=\"" + '' + "\" />" +
                "<input id=\"vlrParcela_" + numero_controle + "\" onchange=\"update_nova_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 25%\" value=\"" + '' + "\" />" +
                "<select id=\"fpParcela_" + numero_controle + "\" onchange=\"update_nova_parcela(this.id, this.value)\" class=\"include_item\" style=\"width: 38%\">" + optionsTxt + "</select>" +                
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
            op_parcela_valor_bruto: document.getElementById('vlrParcela_' + controle).value,
            op_parcela_obs: '',
            op_parcela_saldo: '',
            op_parcela_op_id: '',
            op_parcela_id: '',
            controleEdit: 'insert',
            op_parcela_numero_controle: numero_controle,
            op_parcela_ret_inss: '0,00',
            op_parcela_ret_issqn: '0,00',
            op_parcela_ret_irrf: '0,00',
            op_parcela_ret_pis: '0,00',
            op_parcela_ret_cofins: '0,00',
            op_parcela_ret_csll: '0,00',
        };

        operacao.parcelas.push(Op_parcelas);

        let parcela = "" +
            "<div class=\"row\" id=\"parcela_" + numero_controle + "\">" +
            "<div class=\"col-12\" style=\"padding-right: 0px;\">" +
            "<input id=\"diasParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 10%;padding:0px;text-align:center\" value=\"" + document.getElementById('diasParcela_' + controle).value + "\" />" +
            "<input id=\"vencParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" type =\"text\" class=\"include_item datepicker\" style=\"width: 25%\" value=\"" + document.getElementById('vencParcela_' + controle).value + "\" />" +
            "<input readonly=\"readonly\" id=\"vlrParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" type =\"text\" class=\"include_item\" style=\"width: 25%\" value=\"" + document.getElementById('vlrParcela_' + controle).value + "\" />" +
            "<select id=\"fpParcela_" + numero_controle + "\" onchange=\"update_parcela(this.id, this.value)\" class=\"include_item\" style=\"width: calc(40% - 95px)\">" + optionsTxt_f + "</select>" +
            "<div class=\"input-group-append\" style=\"float: left\" id=\"E" + numero_controle + "\" onclick=\"edit_parcela(this.id,'open')\"><button class=\"btn btn-outline-secondary\" type=\"button\" data-toggle=\"modal\" data-target=\"#modal_retencoes\"><svg width =\"1em\" height=\"1em\" viewBox=\"0 0 16 16\" class=\"bi bi-clipboard-data\" fill=\"currentColor\" xmlns=\"http://www.w3.org/2000/svg\"><path fill - rule=\"evenodd\" d=\"M4 1.5H3a2 2 0 0 0-2 2V14a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V3.5a2 2 0 0 0-2-2h-1v1h1a1 1 0 0 1 1 1V14a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V3.5a1 1 0 0 1 1-1h1v-1z\"/><path fill - rule=\"evenodd\" d=\"M9.5 1h-3a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h3a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm-3-1A1.5 1.5 0 0 0 5 1.5v1A1.5 1.5 0 0 0 6.5 4h3A1.5 1.5 0 0 0 11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3z\"/><path d =\"M4 11a1 1 0 1 1 2 0v1a1 1 0 1 1-2 0v-1zm6-4a1 1 0 1 1 2 0v5a1 1 0 1 1-2 0V7zM7 9a1 1 0 0 1 2 0v3a1 1 0 1 1-2 0V9z\"/></svg ></button ></div >" +
            "<div class='input-group-append' style=\"float: left\" id=\"D" + numero_controle + "\" onclick=\"delete_parcela(this.id, 'confirmação')\" ><button class='btn btn-outline-secondary' type='button'><svg  width =\"1em\" height=\"1em\" viewBox=\"0 0 16 16\" class=\"bi bi-trash\" fill=\"red\" xmlns=\"http://www.w3.org/2000/svg\" style=\"cursor:pointer\"><path d =\"M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z\" /><path fill - rule=\"evenodd\" d=\"M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4L4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z\" /></svg></button ></div> " +
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
        changeMonth: true,
        changeYear: true,
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
                decimal(id, vlr, '6', true);
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
        decimal(id, vlr, '6', true);
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

function dadorParticipante(contexto, op_part_participante_id) {
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

    if (contexto == 'insert') {
        operacao.participante.op_part_participante_id = op_part_participante_id;
    }

    if (document.getElementById('modal_fornecedor')) {
        $('#modal_fornecedor').modal('hide');
    }
}

function dadosOperacao() {
    if (document.getElementById('op_numero_ordem')) {
        operacao.operacao.op_numero_ordem = document.getElementById('op_numero_ordem').value;
    }
    if (document.getElementById('op_data')) {
        operacao.operacao.op_data = document.getElementById('op_data').value;
    }
    if (document.getElementById('op_previsao_entrega')) {
        operacao.operacao.op_previsao_entrega = document.getElementById('op_previsao_entrega').value;
    }
    if (document.getElementById('op_obs')) {
        operacao.operacao.op_obs = document.getElementById('op_obs').value;
    }
    if (document.getElementById('op_categoria_id')) {
        operacao.operacao.op_categoria_id = document.getElementById('op_categoria_id').value;
    }

    let data = document.getElementById('op_data').value.split('/');
    let d = new Date(data[2], data[1], data[0]);
    if (d == 'Invalid Date' || data[2].length < 4 || data[1] > 12 || data[1] < 1 || data[0] > 31 || data[0] < 1) {
        return 'Data da compra é inválida;';
    }

    return true;
}

function dadosTransportador() {
    let mod = document.getElementById('op_transportador_modalidade_frete').value;
    let nome = document.getElementById('op_transportador_nome').value;
    let cnpj_cpf = document.getElementById('op_transportador_cnpj_cpf').value;
    let participante_id = document.getElementById('op_transportador_participante_id').value;

    operacao.transportador.op_transportador_modalidade_frete = mod;
    operacao.transportador.op_transportador_participante_id = participante_id;

    if (mod != '9') {
        if (nome == "" || nome == null || cnpj_cpf == "" || cnpj_cpf == null) {
            return 'Campo nome ou CNPJ/CPF do transportador estão vazios!';
        }
        if (participante_id == "" || participante_id == null) {
            return 'O transportador não corresponde a um participante válido';
        }

        if (document.getElementById('op_transportador_nome')) {
            operacao.transportador.op_transportador_nome = document.getElementById('op_transportador_nome').value;
        }
        if (document.getElementById('op_transportador_cnpj_cpf')) {
            operacao.transportador.op_transportador_cnpj_cpf = document.getElementById('op_transportador_cnpj_cpf').value;
        }
        if (document.getElementById('op_transportador_volume_qtd')) {
            operacao.transportador.op_transportador_volume_qtd = document.getElementById('op_transportador_volume_qtd').value;
        }
        if (document.getElementById('op_transportador_volume_peso_bruto')) {
            operacao.transportador.op_transportador_volume_peso_bruto = document.getElementById('op_transportador_volume_peso_bruto').value;
        }
    }

    return true;    
}

function validaParticipante() {
    
    if (operacao.participante.op_part_participante_id == "" || operacao.participante.op_part_participante_id == null) {
        return 'O participante não corresponde a um cadastro válido.';
    }

    if (operacao.participante.op_part_nome == "" || operacao.participante.op_part_nome == null) {
        return 'Nome do participante está vazio.';
    }

    if (operacao.participante.op_part_cnpj_cpf == "" || operacao.participante.op_part_cnpj_cpf == null) {
        return 'CNPJ ou CPF do participante está vazio.';
    }

    return true;
}

function validaItens() {
    let retorno = '';

    if (operacao.itens.length == 0 && (operacao.operacao.op_tipo != 'ServicoPrestado' && operacao.operacao.op_tipo != 'ServicoTomado')) {
        return 'Obrigatório informar pelo menos um item na compra';
    }

    if ((operacao.itens.length > 0)) {
        for (let i = 0; i < operacao.itens.length; i++) {
            if (operacao.itens[i].op_item_nome == "" || operacao.itens[i].op_item_nome == null) {
                retorno += 'Item número ' + (i + 1) + ': campo Nome está vázio.;';
            }
            if (operacao.itens[i].op_item_codigo == "" || operacao.itens[i].op_item_codigo == null) {
                retorno += 'Item número ' + (i + 1) + ': campo Código Empresa está vázio.;';
            }            
            if (operacao.itens[i].op_item_unidade == "" || operacao.itens[i].op_item_unidade == null) {
                retorno += 'Item número ' + (i + 1) + ': campo Unidade está vázio.;';
            }

            if ((operacao.itens[i].op_item_preco.toString().replace('.', '').replace(',','.') * 1) == 0 || operacao.itens[i].op_item_preco == null) {
                retorno += 'Item número ' + (i + 1) + ': campo Preço está vázio.;';
            }
            if ((operacao.itens[i].op_item_qtd.toString().replace('.', '').replace(',', '.') * 1) == 0 || operacao.itens[i].op_item_qtd == null) {
                retorno += 'Item número ' + (i + 1) + ': campo Quantidade está vázio.;';
            }
        }        
    }

    if (retorno == "") {
        return true;
    } else {
        return retorno;
    }
}

function validaParcelas() {

    let retorno = "";
    let contador = 1;

    if (operacao.parcelas.length == 0) {
        retorno = 'Ínforme ao menos uma parcela!;';
    } else{
        for (let i = 0; i < operacao.parcelas.length; i++) {
            if (operacao.parcelas[i].controleEdit == 'insert' || operacao.parcelas[i].controleEdit == 'update') {

                if ((operacao.parcelas[i].op_parcela_valor.toString().replace('.', '').replace(',', '.') * 1) == 0 || operacao.parcelas[i].op_parcela_valor == "" || operacao.parcelas[i].op_parcela_valor == null) {
                    retorno = 'Parcela número ' + contador + ': Valor não pode ser vázio ou zero.;';
                }

                let data = (operacao.parcelas[i].op_parcela_vencimento).split('/');
                let d = new Date(data[2], data[1], data[0]);
                if (data.length < 3 || d == 'Invalid Date' || data[2].length < 4 || data[1] > 12 || data[1] < 1 || data[0] > 31 || data[0] < 1) {
                    retorno = 'Parcela número ' + contador + ': Data inválida.;';
                }

                contador += 1;
            }
        }
    }

    if (retorno == "") {
        return true;
    } else {
        return retorno;
    }
}

function dadosNF() {

    let retorno = '';

    chave = document.getElementById('op_nf_chave').value;
    dt_emissao = document.getElementById('op_nf_data_emissao').value;
    dt_e_s = document.getElementById('op_nf_data_entrada_saida').value;
    serie = document.getElementById('op_nf_serie').value;
    numero = document.getElementById('op_nf_numero').value;

    if (document.getElementById('r_sem_nf').checked) {
        operacao.operacao.op_comNF = 0;
        operacao.nf.op_nf_chave = '';
        operacao.nf.op_nf_data_emissao = '';
        operacao.nf.op_nf_data_entrada_saida = '';
        operacao.nf.op_nf_serie = '';
        operacao.nf.op_nf_numero = '';
    }
    if (document.getElementById('r_nf_eletronica').checked) {
        operacao.operacao.op_comNF = 1;
        operacao.nf.op_nf_chave = document.getElementById('op_nf_chave').value;
        operacao.nf.op_nf_data_emissao = document.getElementById('op_nf_data_emissao').value;
        operacao.nf.op_nf_data_entrada_saida = document.getElementById('op_nf_data_entrada_saida').value;
        operacao.nf.op_nf_serie = document.getElementById('op_nf_serie').value;
        operacao.nf.op_nf_numero = document.getElementById('op_nf_numero').value;

        if (operacao.operacao.op_tipo != 'ServicoPrestado' && operacao.operacao.op_tipo != 'ServicoTomado') {
            if (chave.length != 44) {
                retorno += 'Chave de acesso da nota fiscal inválida.;';
            }
        }        
        if (chave.length == 0) {
            retorno += 'Chave de acesso da nota fiscal é obrigatória.;';
        }
        if (numero.length == 0) {
            retorno += 'Número da nota fiscal é obrigatório.;';
        }
        if (serie.length == 0) {
            retorno += 'Série da nota fiscal é obrigatório.;';
        }
        if (dt_emissao.length == 0) {
            retorno += 'Série da nota fiscal é obrigatório.;';
        }
        let data = dt_emissao.split('/');
        let d = new Date(data[2], data[1], data[0]);
        if (data.length < 3 || d == 'Invalid Date' || data[2].length < 4 || data[1] > 12 || data[1] < 1 || data[0] > 31 || data[0] < 1) {
            retorno += 'Data de emissão da nota fiscal é inválida;';
        }

    }
    if (document.getElementById('r_nf_manual').checked) {
        operacao.operacao.op_comNF = 2;
        operacao.nf.op_nf_chave = '';
        operacao.nf.op_nf_data_emissao = document.getElementById('op_nf_data_emissao').value;
        operacao.nf.op_nf_data_entrada_saida = document.getElementById('op_nf_data_entrada_saida').value;
        operacao.nf.op_nf_serie = document.getElementById('op_nf_serie').value;
        operacao.nf.op_nf_numero = document.getElementById('op_nf_numero').value;

        if (numero.length == 0) {
            retorno += 'Número da nota fiscal é obrigatório.;';
        }
        if (serie.length == 0) {
            retorno += 'Série da nota fiscal é obrigatório.;';
        }
        if (dt_emissao.length == 0) {
            retorno += 'Série da nota fiscal é obrigatório.;';
        }
        let data = dt_emissao.split('/');
        let d = new Date(data[2], data[1], data[0]);
        if (data.length < 3 || d == 'Invalid Date' || data[2].length < 4 || data[1] > 12 || data[1] < 1 || data[0] > 31 || data[0] < 1) {
            retorno += 'Data de emissão da nota fiscal é inválida;';
        }
    }

    if (retorno == '') {
        return true;
    } else {
        return retorno;
    }
}

function dadosServico() {    
    let retorno = '';

    if (document.getElementById('op_servico_servico_executado')) {
        let servico = document.getElementById('op_servico_servico_executado').value;
        if (servico.length < 5) {
            retorno = 'A descrição do serviços é obrigatória e deve ter no mínimo 5 caracteres.;';
        }
    }
    if (document.getElementById('op_servico_valor')) {
        let vlr_serv = document.getElementById('op_servico_valor').value.toString().replace('.', '').replace(',', '.') * 1;
        if (vlr_servico == 0) {
            retorno = 'Valor do serviço é obrigatório';
        }
    }    
    //gerando dados do serviço para o objeto
    if (document.getElementById('op_servico_equipamento')) {
        operacao.servico.op_servico_equipamento = document.getElementById('op_servico_equipamento').value;
    }
    if (document.getElementById('op_servico_nSerie')) {
        operacao.servico.op_servico_nSerie = document.getElementById('op_servico_nSerie').value;
    }
    if (document.getElementById('op_servico_problema')) {
        operacao.servico.op_servico_problema = document.getElementById('op_servico_problema').value;
    }
    if (document.getElementById('op_servico_obsReceb')) {
        operacao.servico.op_servico_obsReceb = document.getElementById('op_servico_obsReceb').value;
    }
    if (document.getElementById('op_servico_servico_executado')) {
        operacao.servico.op_servico_servico_executado = document.getElementById('op_servico_servico_executado').value;
    }
    if (document.getElementById('op_servico_valor')) {
        operacao.servico.op_servico_valor = document.getElementById('op_servico_valor').value;
    }
    if (document.getElementById('op_servico_status')) {
        operacao.servico.op_servico_status = document.getElementById('op_servico_status').value;
    }    

    if (retorno == '') {
        return true;
    } else {
        return retorno;
    }
}



function gravarOperacao(contexto, tipo_operacao) {
    //Inserção de atributos de controle
    if (tipo_operacao == 'Compra') {        
        operacao.operacao.op_comRetencoes = false;
        operacao.operacao.op_comParticipante = true;
        operacao.operacao.op_comTransportador = true;

        if (contexto == 'Create') {
            operacao.participante.existe = false;            
            operacao.retencoes.existe = false;
            operacao.transportador.existe = false;
        }
    }

    if (tipo_operacao == 'Venda') {
        if (operacao.retencoes.op_ret_inss.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_issqn.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_irrf.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_pis.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_cofins.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_csll.replace('.', '').replace(',', '.') * 1 > 0) {
            operacao.operacao.op_comRetencoes = true;
        } else {
            operacao.operacao.op_comRetencoes = false;
        }
        operacao.operacao.op_comParticipante = true;
        operacao.operacao.op_comTransportador = true;

        if (contexto == 'Create') {
            operacao.participante.existe = false;
            operacao.retencoes.existe = false;
            operacao.transportador.existe = false;
        }
    }

    if (tipo_operacao == 'ServicoPrestado') {
        if (operacao.retencoes.op_ret_inss.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_issqn.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_irrf.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_pis.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_cofins.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_csll.replace('.', '').replace(',', '.') * 1 > 0) {
            operacao.operacao.op_comRetencoes = true;
        } else {
            operacao.operacao.op_comRetencoes = false;
        }
        operacao.operacao.op_comParticipante = true;
        operacao.operacao.op_comTransportador = false;

        if (contexto == 'Create') {
            operacao.participante.existe = false;
            operacao.retencoes.existe = false;
            operacao.transportador.existe = false;
        }
    }   

    //29/12
    if (tipo_operacao == 'ServicoTomado') {
        if (operacao.retencoes.op_ret_inss.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_issqn.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_irrf.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_pis.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_cofins.replace('.', '').replace(',', '.') * 1 > 0 || operacao.retencoes.op_ret_csll.replace('.', '').replace(',', '.') * 1 > 0) {
            operacao.operacao.op_comRetencoes = true;
        } else {
            operacao.operacao.op_comRetencoes = false;
        }
        operacao.operacao.op_comParticipante = true;
        operacao.operacao.op_comTransportador = false;

        if (contexto == 'Create') {
            operacao.participante.existe = false;
            operacao.retencoes.existe = false;
            operacao.transportador.existe = false;
        }
    }

    totalRetencoes(); //Verificando as retenções

    let validacao = [];
    let erros = [];
    
    validacao.splice(0, validacao.length); //removendo itens da validação
    document.getElementById('msg_valid').innerHTML = "";

    if (operacao.operacao.op_tipo != "ServicoPrestado" && operacao.operacao.op_tipo != "ServicoTomado") {
        validacao.push(dadosTransportador());
    } 
    if (operacao.operacao.op_tipo == "ServicoPrestado" || operacao.operacao.op_tipo == "ServicoTomado") {
        operacao.operacao.op_comTransportador = false; //Se operação for de serviço prestado ou tomado operação é sem transportador
    }    
    
    validacao.push(dadosOperacao());
    validacao.push(validaParticipante());
    if (validaItens() != true) {
        let itens = validaItens().split(';');
        validacao = validacao.concat(itens);    
    }    
    if (validaParcelas() != true) {        
        let parcelas = validaParcelas().split(';');
        validacao = validacao.concat(parcelas);
    }
    if (dadosNF() != true) {
        let validNF = dadosNF().split(';');
        validacao = validacao.concat(validNF);
    }    
    if (dadosServico() != true) {
        let validaServico = dadosServico().split(';');
        validacao = validacao.concat(validaServico);
    }

    for (let i = 0; i < validacao.length; i++) {
        if (validacao[i] != true) {
            erros.push(validacao[i]);
            $('#msg_valid').append('<span class="text-danger">' + validacao[i] + '</span></br>');
        }
    }    

    if (erros.length > 0) {
        alert('Há informações incorretas nos dados preenchidos. Verifique a lista erros no final da página!');
        console.log(erros);
    } else {
        //Postar o formulário de compra

        //alert('Tudo ok. Para os próximos episódios teremos a persistência no banco de dados!');        

        
        $.ajax({
            url: "/" + tipo_operacao + "/" + contexto,
            data: { __RequestVerificationToken: gettoken(), op: operacao},
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                document.getElementById('staticLabel').innerHTML = "";
                document.getElementById('staticLabel').innerHTML = tipo_operacao;
                document.getElementById('conteudo').innerHTML = "";
                document.getElementById('conteudo').innerHTML = "<p>Gravando " + tipo_operacao.toLowerCase() + ", aguarde...</p>";
                document.getElementById('rodape').innerHTML = "";                
                $('#modal_mensagem').modal('show');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("erro");
            },
            success: function (data, textStatus, XMLHttpRequest) {                
                var results = JSON.parse(data);

                if (XMLHttpRequest.responseJSON.includes('alterada com sucesso')) {
                    $('#modal_mensagem').modal('hide');
                    document.getElementById('mensagem_retorno_label').innerHTML = "";
                    document.getElementById('mensagem_retorno_label').innerHTML = "Operação";
                    document.getElementById('mensagem_retorno_conteudo').innerHTML = "";
                    document.getElementById('mensagem_retorno_conteudo').innerHTML = "<p>" + XMLHttpRequest.responseJSON + "</p>";
                    document.getElementById('mensagem_retorno_rodape').innerHTML = "";
                    if (tipo_operacao == 'Compra') {
                        document.getElementById('mensagem_retorno_rodape').innerHTML = '<a class="btn btn-secondary" href="https://contadorcomvc.com.br/Compra/Index">Fechar</a>';
                    }
                    if (tipo_operacao == 'Venda') {
                        document.getElementById('mensagem_retorno_rodape').innerHTML = '<a class="btn btn-secondary" href="https://contadorcomvc.com.br/Venda/Index">Fechar</a>';
                    }
                    if (tipo_operacao == 'ServicoPrestado') {
                        document.getElementById('mensagem_retorno_rodape').innerHTML = '<a class="btn btn-secondary" href="https://contadorcomvc.com.br/ServicoPrestado/Index">Fechar</a>';
                    }
                    if (tipo_operacao == 'ServicoTomado') {
                        document.getElementById('mensagem_retorno_rodape').innerHTML = '<a class="btn btn-secondary" href="https://contadorcomvc.com.br/ServicoTomado/Index">Fechar</a>';
                    }
                    $('#modal_mensagem_retorno').modal('show');

                    return;
                }


                if (XMLHttpRequest.responseJSON.includes('cadastrada com sucesso')) {
                    $('#modal_mensagem').modal('hide');                   
                    $('#modal_mensagem_sucesso').modal('show');

                    return;
                }

                if (XMLHttpRequest.responseJSON.includes('Erro')) {
                    $('#modal_mensagem').modal('hide');
                    document.getElementById('mensagem_retorno_label').innerHTML = "";
                    document.getElementById('mensagem_retorno_label').innerHTML = "ERRO";
                    document.getElementById('mensagem_retorno_conteudo').innerHTML = "";
                    document.getElementById('mensagem_retorno_conteudo').innerHTML = "<p>" + XMLHttpRequest.responseJSON + "</p>";
                    document.getElementById('mensagem_retorno_rodape').innerHTML = "";
                    document.getElementById('mensagem_retorno_rodape').innerHTML = '<button type="button" class="btn btn-secondary" data-dismiss="modal">Voltar</button>';
                    $('#modal_mensagem_retorno').modal('show');

                    return;
                }
            }
        });
        
        
    }
    
}

//Baixa contas a pgar
$(".createBaixa").click(function () {
    var parcela_id = $(this).attr("data-parcela_id");        
    var contexto = $(this).attr("data-contexto");        
    $("#modal").load("/Baixa/Create?parcela_id=" + parcela_id + "&contexto=" + contexto, function () {
        $("#modal").modal('show');
    })
});

$(".editBaixa").click(function () {
    var baixa_id = $(this).attr("data-baixa_id");
    var local = $(this).attr("data-local");
    var contacorrente_id = $(this).attr("data-contacorrente_id");
    var dataInicio = $(this).attr("data-dataInicio");
    var dataFim = $(this).attr("data-dataFim");
    //var ndataInicio = new Date(dataInicio.substr(6, 4) + ',' + dataInicio.substr(3, 2) + ',' + dataInicio.substr(0, 2));
    //var ndataFim = new Date(dataFim.substr(6, 4) + ',' + dataFim.substr(3, 2) + ',' + dataFim.substr(0, 2));
    var ndataInicio = dataInicio.substr(6, 4) + '-' + dataInicio.substr(3, 2) + '-' + dataInicio.substr(0, 2);
    var ndataFim = dataFim.substr(6, 4) + '-' + dataFim.substr(3, 2) + '-' + dataFim.substr(0, 2);

    $("#modal").load("/Baixa/Edit?baixa_id=" + baixa_id + "&local=" + local + "&contacorrente_id=" + contacorrente_id + "&dataInicio=" + ndataInicio + "&dataFim=" + ndataFim, function () {
        $("#modal").modal('show');
    })
});

$.validator.methods.number = function (value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
}

function validaBaixa(contexto_requisicao) {    
    let retorno = "";

    let saldo = (document.getElementById('saldo_parcela').value.toString().replace('.', '').replace(',', '.') * 1);
    let valor = (document.getElementById('valor').value.toString().replace('.', '').replace(',', '.') * 1);

    let juros = (document.getElementById('juros').value.toString().replace('.', '').replace(',', '.') * 1);
    let multa = (document.getElementById('multa').value.toString().replace('.', '').replace(',', '.') * 1);
    let desconto = (document.getElementById('desconto').value.toString().replace('.', '').replace(',', '.') * 1);
    let obs = document.getElementById('parcela_obs').value;    

    if (valor > saldo) {

        if (contexto_requisicao == 'Create') {
            retorno += 'O Valor do Pagamento não pode ser superior ao Saldo a Pagar da parcela!;';           
        }

        if (contexto_requisicao == 'Edit') {
            retorno += 'O Valor do Pagamento não pode ser superior ao Limite de Pagamento!';            
        }
    }

    if (valor <= 0) {
        retorno += 'O campo valor não pode ser inferior ou igual a zero!;'; 
    }

    if (juros < 0 || multa < 0 || desconto < 0) {
        retorno += 'Os juros, multa e descoto não pode ser inferiror a zero!;'; 
    }

    if (obs.length < 4) {
        retorno += 'Obrigatório informar o memorando com no mínimo 4 caracteres!;'; 
    }

    let data = document.getElementById('data').value.split('/');
    let d = new Date(data[2], data[1], data[0]);
    if (d == 'Invalid Date' || data[2].length < 4 || data[1] > 12 || data[1] < 1 || data[0] > 31 || data[0] < 1) {
        retorno += 'Data Pagamento é inválida!;';
    }


    if (retorno == "") {
        return true;
    } else {
        return retorno;
    }
}

function gravarBaixa(contexto_requisicao, local, contaCorrete_id, dataInicio, dataFim) {    
    //Limpeza da div de erros
    document.getElementById('msg_valid').innerHTML = "";

    let validacao = [];
    let erros = [];
    //Limpara a matrix validação;
    validacao.splice(0, validacao.length);   

    //Popula matrix validação com o retorno da validação validaBaixa
    let retValida = validaBaixa(contexto_requisicao);
    if (retValida != true) {
        validacao = retValida.split(';');
    }    

    //limpa os retorno true e alimenta a matriz de erros com os retornos diferentes de true
    for (let i = 0; i < validacao.length; i++) {
        if (validacao[i] != true) {
            erros.push(validacao[i]);
            $('#msg_valid').append('<span class="text-danger">' + validacao[i] + '</span></br>');
        }
    }   

    if (erros.length == 0) //Gera a requisição somente se os erros não existirem
    {        
        valor = document.getElementById('valor').value;
        juros = document.getElementById('juros').value;
        multa = document.getElementById('multa').value;
        desconto = document.getElementById('desconto').value;
        obs = document.getElementById('parcela_obs').value;
        contacorrente_id = document.getElementById('contacorrente_id').value;
        data = document.getElementById('data').value;
        parcela_id = document.getElementById('parcela_id').value;
        baixa_id = document.getElementById('baixa_id').value;
        contexto = 'ContasPagar';       

        $.ajax({
            url: "/Baixa/" + contexto_requisicao,
            data: { __RequestVerificationToken: gettoken(), valor: valor, juros: juros, multa: multa, desconto: desconto, obs: obs, contacorrente_id: contacorrente_id, data: data, parcela_id: parcela_id, contexto: contexto, baixa_id: baixa_id  },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {                
                alert("erro");
            },
            success: function (data, textStatus, XMLHttpRequest) {                
                var results = JSON.parse(data);
                console.log(XMLHttpRequest.responseJSON);
                if (XMLHttpRequest.responseJSON.includes('Baixa realizada com sucesso!')) {     
                    $('#modal_mensagem_sucesso').modal('show');
                    return;
                }
                if (XMLHttpRequest.responseJSON.includes('Erro')) {                    
                    $('#modal').modal('hide');
                    document.getElementById('mensagem_retorno_label').innerHTML = "";
                    document.getElementById('mensagem_retorno_label').innerHTML = "ERRO";
                    document.getElementById('mensagem_retorno_conteudo').innerHTML = "";
                    document.getElementById('mensagem_retorno_conteudo').innerHTML = "<p>" + XMLHttpRequest.responseJSON + "</p>";
                    document.getElementById('mensagem_retorno_rodape').innerHTML = "";
                    document.getElementById('mensagem_retorno_rodape').innerHTML = '<button type="button" class="btn btn-secondary" data-dismiss="modal">Voltar</button>';
                    $('#modal_mensagem_retorno').modal('show');
                    return;
                }
                if (XMLHttpRequest.responseJSON.includes('Baixa alterada com sucesso!')) {                    
                    var ndataInicio = dataInicio.substr(6, 4) + '-' + dataInicio.substr(3, 2) + '-' + dataInicio.substr(0, 2);
                    var ndataFim = dataFim.substr(6, 4) + '-' + dataFim.substr(3, 2) + '-' + dataFim.substr(0, 2);

                    if (location.hostname === "localhost" || location.hostname === "127.0.0.1") {
                        window.location.href = "https://localhost:44339/ContaCorrenteMov/Index?dataInicio=" + ndataInicio + "&dataFim=" + ndataFim + "&contacorrente_id=" + contaCorrete_id;
                    } else {
                        window.location.href = "https://contadorcomvc.com.br/ContaCorrenteMov/Index?dataInicio=" + ndataInicio + "&dataFim=" + ndataFim + "&contacorrente_id=" + contaCorrete_id;
                    }
                }

            }
        });
    }
}

function deleteBaixa(contexto, local, contaCorrete_id, dataInicio, dataFim) {    
    baixa_id = document.getElementById('baixa_id').value;

    if (contexto == 'confirmar') {
        $('#modal_mensagem_delete').modal('show');
        return;
    }

    if (contexto == 'confirmado') {
        console.log(contexto);
        $.ajax({
            url: "/Baixa/Delete",
            data: { __RequestVerificationToken: gettoken(), baixa_id: baixa_id },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("erro");
            },
            success: function (data, textStatus, XMLHttpRequest) {
                if (XMLHttpRequest.responseJSON.includes('Erro')) {
                    $('#modal').modal('show');
                    document.getElementById('mensagem_retorno_label').innerHTML = "";
                    document.getElementById('mensagem_retorno_label').innerHTML = "ERRO";
                    document.getElementById('mensagem_retorno_conteudo').innerHTML = "";
                    document.getElementById('mensagem_retorno_conteudo').innerHTML = "<p>" + XMLHttpRequest.responseJSON + "</p>";
                    document.getElementById('mensagem_retorno_rodape').innerHTML = "";
                    document.getElementById('mensagem_retorno_rodape').innerHTML = '<button type="button" class="btn btn-secondary" data-dismiss="modal">Voltar</button>';
                    $('#modal_mensagem_retorno').modal('show');
                    return;
                }
                if (XMLHttpRequest.responseJSON.includes('Baixa excluída com sucesso!')) {
                    var ndataInicio = dataInicio.substr(6, 4) + '-' + dataInicio.substr(3, 2) + '-' + dataInicio.substr(0, 2);
                    var ndataFim = dataFim.substr(6, 4) + '-' + dataFim.substr(3, 2) + '-' + dataFim.substr(0, 2);

                    if (location.hostname === "localhost" || location.hostname === "127.0.0.1") {
                        window.location.href = "https://localhost:44339/ContaCorrenteMov/Index?dataInicio=" + ndataInicio + "&dataFim=" + ndataFim + "&contacorrente_id=" + contaCorrete_id;
                    } else {
                        window.location.href = "https://contadorcomvc.com.br/ContaCorrenteMov/Index?dataInicio=" + ndataInicio + "&dataFim=" + ndataFim + "&contacorrente_id=" + contaCorrete_id;
                    }
                }

            }
        });
    }
}

function filter_ccm() {
    let b_filter = document.getElementById('b_filter_ccm').style.display;

    if (b_filter == 'none' || b_filter == '' || b_filter == null) {
        document.getElementById('b_filter_ccm').style.display = 'block';
        document.getElementById('icon_ccm').innerHTML = '- Filtros';
    } else {
        document.getElementById('b_filter_ccm').style.display = 'none';
        document.getElementById('icon_ccm').innerHTML = '+ Filtros';
    }
}

$(".createTransferencia").click(function () {    
    var contacorrente_id = $(this).attr("data-contacorrente_id");    
    var dataInicio = $(this).attr("data-dataInicio");
    var dataFim = $(this).attr("data-dataFim");    
    var ndataInicio = dataInicio.substr(6, 4) + '-' + dataInicio.substr(3, 2) + '-' + dataInicio.substr(0, 2);
    var ndataFim = dataFim.substr(6, 4) + '-' + dataFim.substr(3, 2) + '-' + dataFim.substr(0, 2);

    $("#modal").load("/Transferencia/Create?contacorrente_id=" + contacorrente_id + "&dataInicio=" + ndataInicio + "&dataFim=" + ndataFim, function () {
        $("#modal").modal('show');
    })
});

$(".createCCM").click(function () {    
    var contacorrente_id = $(this).attr("data-contacorrente_id");
    if (contacorrente_id == 0) {
        contacorrente_id = document.getElementById('contacorrente_id').value;
    }
    var dataInicio = $(this).attr("data-dataInicio");
    var dataFim = $(this).attr("data-dataFim");
    var ndataInicio = dataInicio.substr(6, 4) + '-' + dataInicio.substr(3, 2) + '-' + dataInicio.substr(0, 2);
    var ndataFim = dataFim.substr(6, 4) + '-' + dataFim.substr(3, 2) + '-' + dataFim.substr(0, 2);

    $("#modal").load("/ContaCorrenteMov/Create?contacorrente_id=" + contacorrente_id + "&dataInicio=" + ndataInicio + "&dataFim=" + ndataFim, function () {
        $("#modal").modal('show');
    })
});

$(".editCCM").click(function () {
    var ccm_id = $(this).attr("data-ccm_id");
    var contacorrente_id = $(this).attr("data-contacorrente_id");
    if (contacorrente_id == 0) {
        contacorrente_id = document.getElementById('contacorrente_id').value;
    }
    var dataInicio = $(this).attr("data-dataInicio");
    var dataFim = $(this).attr("data-dataFim");
    var ndataInicio = dataInicio.substr(6, 4) + '-' + dataInicio.substr(3, 2) + '-' + dataInicio.substr(0, 2);
    var ndataFim = dataFim.substr(6, 4) + '-' + dataFim.substr(3, 2) + '-' + dataFim.substr(0, 2);

    $("#modal").load("/ContaCorrenteMov/Edit?contacorrente_id=" + contacorrente_id + "&dataInicio=" + ndataInicio + "&dataFim=" + ndataFim + "&ccm_id=" + ccm_id, function () {
        $("#modal").modal('show');
    })
});

$(".editTransferencia").click(function () {
    var ccm_id = $(this).attr("data-ccm_id");
    var contacorrente_id = $(this).attr("data-contacorrente_id");
    var dataInicio = $(this).attr("data-dataInicio");
    var dataFim = $(this).attr("data-dataFim");
    var ndataInicio = dataInicio.substr(6, 4) + '-' + dataInicio.substr(3, 2) + '-' + dataInicio.substr(0, 2);
    var ndataFim = dataFim.substr(6, 4) + '-' + dataFim.substr(3, 2) + '-' + dataFim.substr(0, 2);

    $("#modal").load("/Transferencia/Edit?contacorrente_id=" + contacorrente_id + "&dataInicio=" + ndataInicio + "&dataFim=" + ndataFim + "&ccm_id=" + ccm_id, function () {
        $("#modal").modal('show');
    })
});

function imput_retencoes(id, vlr, limit) {
    //Autualiza view
    decimal(id, vlr, limit, true);
    //Atualiza objeto retenções
    if (id == 'op_ret_inss') { operacao.retencoes.op_ret_inss = vlr; }
    if (id == 'op_ret_issqn') { operacao.retencoes.op_ret_issqn = vlr; }
    if (id == 'op_ret_irrf') { operacao.retencoes.op_ret_irrf = vlr; }
    if (id == 'op_ret_pis') { operacao.retencoes.op_ret_pis = vlr; }
    if (id == 'op_ret_cofins') { operacao.retencoes.op_ret_cofins = vlr; }
    if (id == 'op_ret_csll') { operacao.retencoes.op_ret_csll = vlr; }    
}

function op_retencao(id, box_id) {
    let cheque = document.getElementById(id);
    if (cheque.checked == true) {
        document.getElementById(box_id).style.display = 'block';
        if (document.getElementById('op_ret_inss')) {
            document.getElementById('op_ret_inss').focus();
        }        
    } else {
        document.getElementById(box_id).style.display = 'none';
        if (document.getElementById('op_ret_inss')) {
            imput_retencoes('op_ret_inss', '0,00', '2');
        }
        if (document.getElementById('op_ret_issqn')) {
            imput_retencoes('op_ret_issqn', '0,00', '2');
        }
        if (document.getElementById('op_ret_irrf')) {
            imput_retencoes('op_ret_irrf', '0,00', '2');     
        }
    }
}

function operacao_nf(id) {
    
    if (id == 'r_sem_nf') {
        document.getElementById('op_nf_chave').disabled = true;
        document.getElementById('op_nf_data_emissao').disabled = true;
        if (document.getElementById('op_nf_data_entrada_saida')) {
            document.getElementById('op_nf_data_entrada_saida').disabled = true;
            document.getElementById('op_nf_data_entrada_saida').value = "";
        }        
        document.getElementById('op_nf_serie').disabled = true;
        document.getElementById('op_nf_numero').disabled = true;
        document.getElementById('op_nf_data_emissao').value = "";        
        document.getElementById('op_nf_serie').value = "";
        document.getElementById('op_nf_numero').value = "";
        document.getElementById('op_nf_chave').value = "";
        document.getElementById('chave_acesso').innerHTML  = "";
    }

    if (id == 'r_nf_eletronica') {
        document.getElementById('op_nf_chave').disabled = false;
        document.getElementById('op_nf_data_emissao').disabled = false;
        if (document.getElementById('op_nf_data_entrada_saida')) {
            document.getElementById('op_nf_data_entrada_saida').disabled = false;
        }        
        document.getElementById('op_nf_serie').disabled = true;
        document.getElementById('op_nf_numero').disabled = true;
        if (document.getElementById('op_data')) {
            document.getElementById('op_nf_data_entrada_saida').value = document.getElementById('op_data').value;
            document.getElementById('op_nf_data_emissao').value = document.getElementById('op_data').value;
        }
        
        
    }

    if (id == 'r_nf_manual') {
        document.getElementById('op_nf_chave').disabled = true;
        document.getElementById('op_nf_data_emissao').disabled = false;
        if (document.getElementById('op_nf_data_entrada_saida')) {
            document.getElementById('op_nf_data_entrada_saida').disabled = false;
        }
        document.getElementById('op_nf_serie').disabled = false;
        document.getElementById('op_nf_numero').disabled = false;
        if (document.getElementById('op_data')) {
            document.getElementById('op_nf_data_entrada_saida').value = document.getElementById('op_data').value;
            document.getElementById('op_nf_data_emissao').value = document.getElementById('op_data').value;
        }        
        document.getElementById('op_nf_serie').value = "";
        document.getElementById('op_nf_numero').value = "";
        document.getElementById('op_nf_chave').value = "";
        document.getElementById('chave_acesso').innerHTML = "";
    }
}

function chave_acesso_nf_op(id, vlr) {
    if (operacao.operacao.op_tipo != 'ServicoPrestado' && operacao.operacao.op_tipo != 'ServicoTomado') {
        if (vlr.length < 44) {
            alert('Chave de acesso precisa ter 44 digitos');
        }
    }

    if (vlr.length == 44) {
        let serie = vlr.substr(22, 3);
        let numero = vlr.substr(25, 9);

        document.getElementById('op_nf_serie').value = serie * 1;
        document.getElementById('op_nf_numero').value = numero * 1;
    }    
}

function vlr_servico(id, vlr) {    
    decimal(id, vlr, '2', true);    
    decimal('op_totais_preco_servicos', vlr, '2', true);
    totaisOperacao();
}

function updateRetençõesTotais(id, vlr, contexto) {    
    $('#modal_mensagem_retorno').modal('hide'); //Fechar modal se estiver aberto
   
    if (operacao.parcelas.length > 0) {
        if (contexto == 'confirmar') {
            document.getElementById('mensagem_retorno_label').innerHTML = "";
            document.getElementById('mensagem_retorno_label').innerHTML = "Alteração de Retenção";
            document.getElementById('mensagem_retorno_conteudo').innerHTML = "";
            document.getElementById('mensagem_retorno_conteudo').innerHTML = "<p>" + 'A operação possui parcelas informadas. Como deseja proceder com o valor informado?' + "</p>";
            document.getElementById('mensagem_retorno_conteudo').innerHTML += '<div class="row"><div class="col-12"><button type="button" class="btn btn-info" style="width: 100%;margin-bottom: 10px;" onclick="updateRetençõesTotais(\'' + id + '\',\'' + vlr + '\',\'confirmadoDiluir\')">Distribuir o valor em todas as parcelas</button></div><div class="col-12"><button type="button" class="btn btn-info" style="width: 100%;margin-bottom: 10px;" onclick="updateRetençõesTotais(\'' + id + '\',\'' + vlr + '\',\'confirmadoPrimeira\')">Descontar na primeira parcela</button></div></div>';
            document.getElementById('mensagem_retorno_rodape').innerHTML = "";
            $('#modal_mensagem_retorno').modal('show');
        }

        if (contexto == 'confirmadoDiluir') {

            for (let i = 0; i < operacao.parcelas.length; i++) {

                if (id == 'op_ret_inss') {
                    operacao.parcelas[i].op_parcela_ret_inss = (vlr / operacao.parcelas.length).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
                }
                if (id == 'op_ret_issqn') {
                    operacao.parcelas[i].op_ret_issqn = (vlr / operacao.parcelas.length).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
                }
                if (id == 'op_ret_irrf') {
                    operacao.parcelas[i].op_ret_irrf = (vlr / operacao.parcelas.length).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
                }
                operacao.parcelas[i].op_parcela_valor = ((((operacao.parcelas[i].op_parcela_valor_bruto).toString().replace('.', '').replace(',', '.')) * 1) - ((((operacao.parcelas[i].op_parcela_ret_inss).toString().replace('.', '').replace(',', '.')) * 1) + (((operacao.parcelas[i].op_parcela_ret_issqn).toString().replace('.', '').replace(',', '.')) * 1) + (((operacao.parcelas[i].op_parcela_ret_irrf).toString().replace('.', '').replace(',', '.')) * 1) + (((operacao.parcelas[i].op_parcela_ret_pis).toString().replace('.', '').replace(',', '.')) * 1) + (((operacao.parcelas[i].op_parcela_ret_cofins).toString().replace('.', '').replace(',', '.')) * 1) + (((operacao.parcelas[i].op_parcela_ret_csll).toString().replace('.', '').replace(',', '.')) * 1))).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });

                let id_par = 'vlrParcela_' + operacao.parcelas[i].op_parcela_numero_controle;
                decimal(id_par, (operacao.parcelas[i].op_parcela_valor), '2', false);
            }
            totalRetencoes();

        }
        if (contexto == 'confirmadoPrimeira') {
            if (id == 'op_ret_inss') {
                operacao.parcelas[0].op_parcela_ret_inss = (vlr).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
                totalRetencoes();
            }
            if (id == 'op_ret_issqn') {
                peracao.totais.op_totais_preco_servicos = document.getElementById('op_servico_valor').value;
                operacao.parcelas[0].op_ret_issqn = (vlr).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
                totalRetencoes();
            }
            if (id == 'op_ret_irrf') {
                operacao.parcelas[0].op_ret_irrf = (vlr).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
                totalRetencoes();
            }

            operacao.parcelas[0].op_parcela_valor = ((((operacao.parcelas[0].op_parcela_valor_bruto).toString().replace('.', '').replace(',', '.')) * 1) - ((((operacao.parcelas[0].op_parcela_ret_inss).toString().replace('.', '').replace(',', '.')) * 1) + (((operacao.parcelas[0].op_parcela_ret_issqn).toString().replace('.', '').replace(',', '.')) * 1) + (((operacao.parcelas[0].op_parcela_ret_irrf).toString().replace('.', '').replace(',', '.')) * 1) + (((operacao.parcelas[0].op_parcela_ret_pis).toString().replace('.', '').replace(',', '.')) * 1) + (((operacao.parcelas[0].op_parcela_ret_cofins).toString().replace('.', '').replace(',', '.')) * 1) + (((operacao.parcelas[0].op_parcela_ret_csll).toString().replace('.', '').replace(',', '.')) * 1))).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
            let id_par = 'vlrParcela_' + operacao.parcelas[0].op_parcela_numero_controle;
            decimal(id_par, (operacao.parcelas[0].op_parcela_valor), '2', false);
        }

        decimal(id, (vlr.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" })), '2', true);

    } else {
        decimal(id, (vlr.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" })), '2', true);
    }
}

function totalRetencoes() {
    let inss = 0;
    let iss = 0;
    let ir = 0;
    let pis = 0;
    let cofins = 0;
    let cs = 0;

    for (let i = 0; i < operacao.parcelas.length; i++) {
        inss += ((operacao.parcelas[0].op_parcela_ret_inss).toString().replace('.', '').replace(',', '.')) * 1;
        iss += ((operacao.parcelas[0].op_parcela_ret_issqn).toString().replace('.', '').replace(',', '.')) * 1;
        ir += ((operacao.parcelas[0].op_parcela_ret_irrf).toString().replace('.', '').replace(',', '.')) * 1;
        pis += ((operacao.parcelas[0].op_parcela_ret_pis).toString().replace('.', '').replace(',', '.')) * 1;
        cofins += ((operacao.parcelas[0].op_parcela_ret_cofins).toString().replace('.', '').replace(',', '.')) * 1;
        cs += ((operacao.parcelas[0].op_parcela_ret_csll).toString().replace('.', '').replace(',', '.')) * 1;
    }

    operacao.retencoes.op_ret_inss = (inss).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
    operacao.retencoes.op_ret_issqn = (iss).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
    operacao.retencoes.op_ret_irrf = (ir).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
    operacao.retencoes.op_ret_pis = (pis).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
    operacao.retencoes.op_ret_cofins = (cofins).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
    operacao.retencoes.op_ret_csll = (cs).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
}


function agrupaParcela() {
    let vlrSoma = document.getElementById('somaParcela').innerHTML.replace('.', '').replace(',', '.') * 1;
    fechamentoCartao.totalFatura = vlrSoma.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
    console.log(fechamentoCartao);
}

function fechamento_de_cartao(id) {
    let parcela_id = id.substr(3, id.length);
    let meio_Pgto_parcela = document.getElementById('meioPgto_' + parcela_id).value.replace('.', '').replace(',', '.') * 1;
    let forma_Pgto_parcela = document.getElementById('formaPgto_' + parcela_id).value.replace('.', '').replace(',', '.') * 1;
    let vlrOriginal = document.getElementById('vlr_' + parcela_id).value.replace('.', '').replace(',', '.') * 1;
    let nomeCartao = document.getElementById('nomeCartao_' + parcela_id).value;

    if (meio_Pgto_parcela != 3) {
        alert('Apenas selecione parcelas cuja forma de pagamento é do tipo Cartão de Crédito!');
        document.getElementById(id).checked = false;
        return;
    }

    if (document.getElementById(id) && document.getElementById(id).checked) {
        if (fechamento_cartao.fc_qtd_parcelas == 0) {
            fechamento_cartao.fc_forma_pagamento = forma_Pgto_parcela;
            fechamento_cartao.fc_meio_pagamento = meio_Pgto_parcela;
            fechamento_cartao.fc_matriz_parcelas.push(parcela_id * 1);
            fechamento_cartao.fc_valor_total = vlrOriginal;
            fechamento_cartao.fc_nome_cartão = nomeCartao;
            fechamento_cartao.fc_qtd_parcelas = 1;            
        } else {
            if (forma_Pgto_parcela != fechamento_cartao.fc_forma_pagamento) {
                alert('Parcela com forma de pagamento diferente da selecionada anteriormente. Não permitido!');
                document.getElementById(id).checked = false;
                return;
            } else {
                fechamento_cartao.fc_matriz_parcelas.push(parcela_id * 1);
                fechamento_cartao.fc_valor_total += vlrOriginal;
                fechamento_cartao.fc_qtd_parcelas += 1;
            }
        }
    } else {
        //remove a parcela_id da matriz
        for (let i = 0; i < fechamento_cartao.fc_matriz_parcelas.length; i++) {
            if (fechamento_cartao.fc_matriz_parcelas[i] == parcela_id) {
                fechamento_cartao.fc_matriz_parcelas.splice(i, 1);
            }
        }
        fechamento_cartao.fc_valor_total -= vlrOriginal;
        fechamento_cartao.fc_qtd_parcelas -= 1;

        if (fechamento_cartao.fc_valor_total == 0) {
            fechamento_cartao.fc_forma_pagamento = 0;
            fechamento_cartao.fc_meio_pagamento = 0;
        }
    }

    document.getElementById('somaParcela').innerHTML = (fechamento_cartao.fc_valor_total).toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
    console.log(fechamento_cartao);
}

function gravar_fatura_cartao(contexto) {
    if (fechamento_cartao.fc_qtd_parcelas > 0) {
        if (contexto == 'confirmar') {
            document.getElementById('modal_quantidade_parcelas').innerHTML = fechamento_cartao.fc_qtd_parcelas;
            document.getElementById('modal_vlr_total').innerHTML = fechamento_cartao.fc_valor_total.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
            document.getElementById('modal_fc_tarifas_bancarias').value = fechamento_cartao.fc_tarifas_bancarias.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
            document.getElementById('modal_fc_seguro_cartao').value = fechamento_cartao.fc_seguro_cartao.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
            document.getElementById('modal_fc_acrescimos_cartao').value = fechamento_cartao.fc_acrescimos_cartao.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
            document.getElementById('modal_fc_abatimentos_cartao').value = fechamento_cartao.fc_abatimentos_cartao.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
            $("#modal_fechamento_cartao").modal('show');
        }

        if (contexto == 'enviar') {
            fechamento_cartao.fc_tarifas_bancarias = document.getElementById('modal_fc_tarifas_bancarias').value.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
            fechamento_cartao.fc_seguro_cartao = document.getElementById('modal_fc_seguro_cartao').value.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
            fechamento_cartao.fc_acrescimos_cartao = document.getElementById('modal_fc_acrescimos_cartao').value.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
            fechamento_cartao.fc_abatimentos_cartao = document.getElementById('modal_fc_abatimentos_cartao').value.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
            fechamento_cartao.fc_referencia = document.getElementById('modal_fc_referencia').value;
            fechamento_cartao.fc_vencimento = document.getElementById('modal_fc_vencimento').value;
            fechamento_cartao.fc_forma_pgto_boleto_fatura = document.getElementById('modal_fc_forma_pgto_boleto_fatura').value;
            fechamento_cartao.fc_valor_total = fechamento_cartao.fc_valor_total.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });
            fechamento_cartao.fc_op_obs = document.getElementById('op_obs').value;

            let matriz = "";

            for (let x = 0; x < fechamento_cartao.fc_matriz_parcelas.length; x++)
            {
                if (x + 1 == fechamento_cartao.fc_matriz_parcelas.length) {
                    matriz += fechamento_cartao.fc_matriz_parcelas[x];
                }
                else {
                    matriz += fechamento_cartao.fc_matriz_parcelas[x] + ", ";
                }
            }

            fechamento_cartao.fc_matriz_parcelas_text = matriz;


            console.log(fechamento_cartao);

            let valida = true;

            if (fechamento_cartao.fc_referencia.length < 7 || fechamento_cartao.fc_referencia == "" || fechamento_cartao.fc_referencia == null) {
                valida = false;
                alert('Referência inválida');
                return; 
            }

            if (fechamento_cartao.fc_op_obs.length < 4) {
                valida = false;
                alert('Obrigatório informar uma memorando com no mínimo 4 caracteres');
                return;
            }

            if (fechamento_cartao.fc_vencimento.length < 10 || fechamento_cartao.fc_vencimento == "" || fechamento_cartao.fc_vencimento == null) {
                valida = false;
                alert('Data de vencimento inválida');
                return;
            }

            if (fechamento_cartao.fc_forma_pgto_boleto_fatura.length = 0 || fechamento_cartao.fc_forma_pgto_boleto_fatura == "" || fechamento_cartao.fc_forma_pgto_boleto_fatura == null) {
                valida = false;
                alert('Forma de pagamento inválida. Necessário informar uma forma de pagamento do tipo boleto bancário!');
                return;
            }

            if (valida) {
                $.ajax({
                    url: "/FechamentoCartao/Create",
                    data: { __RequestVerificationToken: gettoken(), fc: fechamento_cartao },
                    type: 'POST',
                    dataType: 'json',
                    beforeSend: function (XMLHttpRequest) {

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("erro");
                    },
                    success: function (data, textStatus, XMLHttpRequest) {
                        if (XMLHttpRequest.responseJSON.includes('Erro')) {
                            $("#modal_fechamento_cartao").modal('hide');
                            document.getElementById('mensagem_retorno_labelF').innerHTML = "";
                            document.getElementById('mensagem_retorno_labelF').innerHTML = "ERRO";
                            document.getElementById('mensagem_retorno_conteudoF').innerHTML = "";
                            document.getElementById('mensagem_retorno_conteudoF').innerHTML = "<p>" + XMLHttpRequest.responseJSON + "</p>";
                            document.getElementById('mensagem_retorno_rodapeF').innerHTML = "";
                            document.getElementById('mensagem_retorno_rodapeF').innerHTML = '<button type="button" class="btn btn-secondary" data-dismiss="modal">Voltar</button>';
                            $('#modal_mensagem_retornoF').modal('show');
                            return;
                        }
                        if (XMLHttpRequest.responseJSON.includes('sucesso!')) {                           
                            $("#modal_fechamento_cartao").modal('hide');
                            document.getElementById('cartao_gravado_sucesso').innerHTML = XMLHttpRequest.responseJSON;
                            $('#modal_sucesso_faturaCartao').modal('show');
                            /*
                            if (location.hostname === "localhost" || location.hostname === "127.0.0.1") {
                                window.location.href = "https://localhost:44339/ContasPagar/Index";
                            } else {
                                window.location.href = "https://contadorcomvc.com.br/ContasPagar/Index";
                            }
                            */
                        }

                    }
                });
            }
        }
    } else {
        alert("Nenhuma parcela selecionada!!");
    }    
}

function monthPicker(id, contexto, vlr) {   
    if (contexto == 'open') {
        let d = new Date();
        let m = d.getMonth() + 1;
        let y = d.getFullYear()

        //Primeiro remove o selected padrão
        $("#box_monthPicker_mes").each(function () {
            $(this).removeAttr('selected');
        });
        //Adiciona o selectd na cst da regra
        $('#box_monthPicker_mes option').each(function () {
            if (this.text == m) {
                this.setAttribute("selected", "selected");
                return false;
            }
        });

        //Primeiro remove o selected padrão
        $("#box_monthPicker_ano").each(function () {
            $(this).removeAttr('selected');
        });
        //Adiciona o selectd na cst da regra
        $('#box_monthPicker_ano option').each(function () {
            if (this.text == y) {
                this.setAttribute("selected", "selected");
                return false;
            }
        });

        document.getElementById('box_monthPicker').style.display = 'block';
        document.getElementById('id_cliente').value = id;
    }
    if (contexto == 'close') {
        let id_cliente = document.getElementById('id_cliente').value;
        let mes = document.getElementById('box_monthPicker_mes').value;
        let ano = document.getElementById('box_monthPicker_ano').value;

        document.getElementById(id_cliente).value = mes + "/" + ano;

        document.getElementById('box_monthPicker').style.display = 'none';

        $.ajax({
            url: "/FechamentoCartao/fc_existe",
            data: { __RequestVerificationToken: gettoken(), fc_forma_pagamento: fechamento_cartao.fc_forma_pagamento, fc_referencia: mes + "/" + ano},
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                document.getElementById('gravar_fatura').setAttribute("disabled", "disabled");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("erro");
            },
            success: function (data, textStatus, XMLHttpRequest) {
                var results = JSON.parse(data);
                if (results != null) {
                    document.getElementById("retorno_fc_existe").innerHTML = "Já existe uma fatura para este cartão nesta competência. As parcelas selecionadas serão acrescentadas a esta fatura!";
                    document.getElementById("retorno_fc_existe").style.display = 'block';
                    document.getElementById("op_obs").innerHTML = '';
                    document.getElementById("obs_fatura").style.display = 'none';
                } else {
                    document.getElementById("retorno_fc_existe").innerHTML = "";
                    document.getElementById("retorno_fc_existe").style.display = 'none';
                    document.getElementById("obs_fatura").style.display = 'block';
                }

                document.getElementById('gravar_fatura').removeAttribute("disabled");
            }
        });
    }
}

$(".bolinhas").mousedown(function (ev) {
    if (ev.which == 3) {
        alert("Right mouse button clicked on element with id myId");
    }
});



//Rotina para clicar com botão direito da tela.
/*
oncontextmenu = "cliqueRigth(event,@item.parcela_id, '@item.referencia', '@item.saldo.ToString("N2")', '@item.valorOriginal.ToString("N2")')"
function cliqueRigth(e, parcela_id, referencia, saldo, valorOriginal) {
    console.log(saldo);
    console.log(valorOriginal);    
    if (referencia.includes('Fechamento Cartão Crédito')) {
        e.preventDefault();
        if (typeof e === 'object') {
            if (saldo == valorOriginal) {
                document.getElementById('confirmeDeleteCartao').innerHTML = 'Confirma a exclusão da ' + referencia;
                document.getElementById('footerCartao').innerHTML = '<a class="btn btn-danger"  id="btn_excluir_cartao" onclick="excluirFaturaCartao(' + parcela_id + ')"> Excluir</a><button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>';
            } else {
                document.getElementById('confirmeDeleteCartao').innerHTML = 'A fatura do cartão possui baixa e não pode ser excluída. Primeiramente exclua as baixas!';
                document.getElementById('footerCartao').innerHTML = '<button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>';                
            }
            $('#modal_opcoes_rigth').modal('show');
        }
    }
}

function excluirFaturaCartao(parcela_id) {
    console.log(parcela_id);
}
*/

$(".detalhesParcela").click(function () {
    var parcela_id = $(this).attr("data-parcela_id");    
    $("#modal_parcela").load("/Parcela/Index?parcela_id=" + parcela_id, function () {
        $("#modal_parcela").modal('show');
    })
});

function gravarLancamentoCCM(contexto) {
    document.getElementById('validaForm').innerHTML = '';
    retorno = '';
    validacao = true;

    if (document.getElementById('ccm_data')) {
        let data = (document.getElementById('ccm_data').value).split('/');
        let d = new Date(data[2], data[1], data[0]);
        if (data.length < 3 || d == 'Invalid Date' || data[2].length < 4 || data[1] > 12 || data[1] < 1 || data[0] > 31 || data[0] < 1 || data == '' || data == null) {
            retorno += 'Data inválida.;';
            validacao = false;
        }
    }

    if (document.getElementById('ccm_valor')) {
        let vlr = document.getElementById('ccm_valor').value;
        if (vlr == '' || vlr == null || vlr <= 0) {
            retorno += "Valor inválido.;";
            validacao = false;
        }
    }

    if (document.getElementById('ccm_memorando') && document.getElementById('ccm_memorando').value.length < 3) {
        retorno += 'Obrigatório memorando com no mínimo três caracteres.';
        validacao = false;    
    }

    let nota = document.getElementById('nf_ccm').checked;
    if (nota) {
        if (document.getElementById('ccm_nf_data_emissao') && (document.getElementById('ccm_nf_data_emissao').value == 0 || document.getElementById('ccm_nf_data_emissao').value == '' || document.getElementById('ccm_nf_data_emissao').value == null)) {
            retorno += "Data de emissão da nota fiscal é inválida.;";
            validacao = false;
        }
        if (document.getElementById('ccm_nf_valor') && (document.getElementById('ccm_nf_valor').value == 0 || document.getElementById('ccm_nf_valor').value == '' || document.getElementById('ccm_nf_valor').value == null)) {
            retorno += "Valor da nota fiscal é inválida.;";
            validacao = false;
        }
        if (document.getElementById('ccm_nf_serie') && (document.getElementById('ccm_nf_serie').value == '' || document.getElementById('ccm_nf_serie').value == null)) {
            retorno += "Série da nota fiscal é inválida.;";
            validacao = false;
        }
        if (document.getElementById('ccm_nf_numero') && (document.getElementById('ccm_nf_numero').value == 0 || document.getElementById('ccm_nf_numero').value == '' || document.getElementById('ccm_nf_numero').value == null)) {
            retorno += "Número da nota fiscal é inválida.;";
            validacao = false;
        }
    }

    if (validacao == false) {
        let erros = retorno.split(';');
        document.getElementById('validaForm').style.display = 'block';
        for (let i = 0; i < erros.length; i++) {
            document.getElementById('validaForm').innerHTML += '<p class="text-danger">' + erros[i] +'</p>'; 
        }
        return;
    }

    if (validacao) {
        let date = document.getElementById('ccm_data').value;
        let ccm_data_competencia = document.getElementById('ccm_data_competencia').value;
        let valor = document.getElementById('ccm_valor').value;
        let memorando = document.getElementById('ccm_memorando').value;
        let categoria_id = document.getElementById('categoria_id_ccm').value;
        let ccm_participante_id = document.getElementById('ccm_participante_id').value;
        let ccorrente_id = document.getElementById('ccorrente_id').value;
        let ccm_nf = document.getElementById('nf_ccm').checked;
        let ccm_nf_data_emissao = document.getElementById('ccm_nf_data_emissao').value;
        let ccm_nf_valor = document.getElementById('ccm_nf_valor').value;
        let ccm_nf_serie = document.getElementById('ccm_nf_serie').value;
        let ccm_nf_numero = document.getElementById('ccm_nf_numero').value;
        let ccm_nf_chave = document.getElementById('ccm_nf_chave').value;
        let ccm_valor_principal = document.getElementById('ccm_valor_principal').value;
        let ccm_multa = document.getElementById('ccm_multa').value;
        let ccm_juros = document.getElementById('ccm_juros').value;
        let ccm_desconto = document.getElementById('ccm_desconto').value;
        let ccm_id = 0;
        if (document.getElementById('ccm_id')) {
            ccm_id = document.getElementById('ccm_id').value;
        }


        $.ajax({
            url: "/ContaCorrenteMov/" + contexto,
            data: {
                __RequestVerificationToken: gettoken(),
                data: date,
                ccm_data_competencia: ccm_data_competencia,
                valor: valor,
                memorando: memorando,
                categoria_id: categoria_id,
                participante_id: ccm_participante_id,
                ccorrente_id: ccorrente_id,
                ccm_nf: ccm_nf,
                ccm_nf_data_emissao: ccm_nf_data_emissao,
                ccm_nf_valor: ccm_nf_valor,
                ccm_nf_serie: ccm_nf_serie,
                ccm_nf_numero: ccm_nf_numero,
                ccm_nf_chave: ccm_nf_chave,
                ccm_id: ccm_id,
                ccm_valor_principal: ccm_valor_principal,
                ccm_multa: ccm_multa,
                ccm_juros: ccm_juros,
                ccm_desconto: ccm_desconto,
            },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("erro");
            },
            success: function (data, textStatus, XMLHttpRequest) {
                var results = JSON.parse(data);
                console.log(results);

                if (XMLHttpRequest.responseJSON.includes('Erro')) {
                    document.getElementById('msg_retorno').innerHTML = XMLHttpRequest.responseJSON;
                    $("#Esconder").fadeTo(4000, 500).slideUp(500, function () {
                        $("#Esconder").slideUp(500);
                    });                    
                    return;
                }                
                if (XMLHttpRequest.responseJSON.includes('cadastrado com sucesso!')) {                    
                    $("#ccm_gravado_sucesso").modal('show');
                    return;
                }

                if (XMLHttpRequest.responseJSON.includes('alterado com sucesso!')) {                    
                    $("#ccm_alterado_sucesso").modal('show');
                    return;
                }
            }
        });
    }
}

function consultaParticipanteCCM(id) {
    let id_campo = "#" + id;
    $(id_campo).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Participante/consultaParticipante",
                data: { __RequestVerificationToken: gettoken(), termo: request.term },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    if (document.getElementById('pesquisa_participante')) {
                        document.getElementById('pesquisa_participante').innerHTML = 'pesquisando...';
                    }
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
                            categoria_id: results[i].participante_categoria,
                            categoria_nome: results[i].categoria_nome,
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
            if (document.getElementById('pesquisa_participante')) {
                document.getElementById('pesquisa_participante').innerHTML = '';
            }

            if (document.getElementById('ccm_participante_id')) {
                document.getElementById('ccm_participante_id').value = ui.item.id;
            }
            if (document.getElementById('participante_id')) {
                document.getElementById('participante_id').value = ui.item.id;
            }

            //Atribuindo a categoria no select2
            /*
            if (document.getElementById('categoria_id')) {
                $('#categoria_id').val(ui.item.categoria_id.toString());
                $('#categoria_id').trigger('change');
            }*/

            //Desabilitando o campo para nova inclusão de participante;
            if (document.getElementById('participante')) {
                document.getElementById('participante').setAttribute("disabled", "disabled");
            }

            //Incluindo a categoria atribuida ao participante
            if (document.getElementById('categoria')) {
                if (ui.item.categoria_id > 0) {
                    document.getElementById('categoria').value = ui.item.categoria_nome;
                    if (document.getElementById('categoria_id_ccm')) {
                        document.getElementById('categoria_id_ccm').value = ui.item.categoria_id;
                    }
                    document.getElementById('categoria').setAttribute("disabled", "disabled");
                }
            }
        }
    });
}

function alteraParticipanteCCM() {
    if (document.getElementById('ccm_participante_id')) {
        document.getElementById('ccm_participante_id').value = '';
    }
    if (document.getElementById('participante')) {
        document.getElementById('participante').removeAttribute("disabled");
        document.getElementById('participante').value = '';
        document.getElementById('participante').focus();
    }    
}

function ccm_nf(id, box) {
    let check = document.getElementById(id);
    if (check.checked) {
        document.getElementById(box).style.display = 'block';
    } else {
        document.getElementById(box).style.display = 'none';
        document.getElementById('ccm_nf_data_emissao').value = "";
        document.getElementById('ccm_nf_valor').value = "";
        document.getElementById('ccm_nf_serie').value = "";
        document.getElementById('ccm_nf_numero').value = "";
        document.getElementById('ccm_nf_chave').value = "";
    }
}

function modal_modal() {
    //Instrução para correções na abertura de modal sobre modal
    $(document).on('show.bs.modal', '.modal', function () {
        var zIndex = 1040 + (10 * $('.modal:visible').length);
        $(this).css('z-index', zIndex);
        setTimeout(function () {
            $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
        }, 0);
    });
    //Instrução para correção da barra de rolagem no fechamento de modal sobre modal
    $(document).on('hidden.bs.modal', '.modal', function () {
        $('.modal:visible').length && $(document.body).addClass('modal-open');
    });
}

function ajustaEditFormaPgto() {
    let c = document.getElementById('fp_vinc_conta_corrente').value;
    let m = document.getElementById('fp_meio_pgto_nfe').value;
    meioPgto(m);
    document.getElementById('fp_vinc_conta_corrente').value = c;
}

function contasFinanceiras(id, vlr) {
    if (vlr == 'Realizada') {                        
        document.getElementById('grupo_formaPagamento').style.display = 'block';        
        document.getElementById('grupo_vlrParcelas').style.display = 'block';        
        document.getElementById('text_dataFinal_recorencia').innerHTML = '';
        document.getElementById('grupo_dadosNF').style.display = 'block';        
        document.getElementById('lab_cf_data_inicial').innerHTML = 'Data Primeira Parcela';
        document.getElementById('lab_cf_data_final').innerHTML = 'Data Última Parcela';
        if (document.getElementById('label_parcelamento')) {
            document.getElementById('label_parcelamento').innerHTML = 'Parcelamento';
        }
        if (document.getElementById('label_cf_recorrencia')) {
            document.getElementById('label_cf_recorrencia').innerHTML = 'Recorrência Parcelas';
        }
    }

    if (vlr == 'Realizar') {        
        document.getElementById('grupo_formaPagamento').style.display = 'none';        
        document.getElementById('grupo_vlrParcelas').style.display = 'none';        
        document.getElementById('grupo_dadosNF').style.display = 'none';
        operacao_nf('r_sem_nf'); //zerar campos relacionadas a nota fiscal        
        document.getElementById('text_dataFinal_recorencia').innerHTML = 'Na conta do tipo "À Realizar" se não for informada uma Data Limite o sistema gerará a quantidade de ocorrências até o último mês do ano seguinte. Na baixa da parcela será incluída uma nova recorrência ao final.';
        document.getElementById('lab_cf_data_inicial').innerHTML = 'Data Primeira Ocorrência';
        document.getElementById('lab_cf_data_final').innerHTML = 'Data Limite';
        if (document.getElementById('label_parcelamento')) {
            document.getElementById('label_parcelamento').innerHTML = 'Recorrência';
        }
        if (document.getElementById('label_cf_recorrencia')) {
            document.getElementById('label_cf_recorrencia').innerHTML = 'Recorrência';
        }
    }

}

function changeRecorrencia(id, vlr) {
    if (vlr == "Unica") {
        document.getElementById('cf_data_final').value = '';
        document.getElementById('cf_data_final').setAttribute("disabled", "disabled");
    } else {
        document.getElementById('cf_data_final').removeAttribute("disabled");
    }
}

function editCategoriaContasFinanceiras() {
    GerarSelectFormaPagamento();
}

function tipoNF(id, vlr) {
    if (vlr == 0) {
        document.getElementById('op_nf_data_emissao').setAttribute("disabled", "disabled");
        document.getElementById('op_nf_chave').setAttribute("disabled", "disabled");
        document.getElementById('op_nf_serie').setAttribute("disabled", "disabled");
        document.getElementById('op_nf_numero').setAttribute("disabled", "disabled");

        document.getElementById('op_nf_data_emissao').value = "";
        document.getElementById('op_nf_chave').value = "";
        document.getElementById('op_nf_serie').value = "";
        document.getElementById('op_nf_numero').value = "";
    } else {
        document.getElementById('op_nf_data_emissao').removeAttribute("disabled");        
        document.getElementById('op_nf_serie').removeAttribute("disabled");
        document.getElementById('op_nf_numero').removeAttribute("disabled");

        if (vlr == 1) {
            document.getElementById('op_nf_chave').removeAttribute("disabled");
        } else {
            document.getElementById('op_nf_chave').setAttribute("disabled", "disabled");
        } 
    }

    
}

function GerarSelectFormaPagamento() {
    let categoria_id = 0;
    if (document.getElementById('cf_categoria_id')) {
        categoria_id = document.getElementById('cf_categoria_id').value;
    }

    if (categoria_id > 0) {
        $.ajax({
            url: "/ContasFinanceiras/GerarSelectFormaPagamento",
            data: { __RequestVerificationToken: gettoken(), categoria_id: categoria_id },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                if (document.getElementById('text_formaPgto')) {
                    document.getElementById('text_formaPgto').innerHTML = "Gerando lista de formas de pagamento";
                    document.getElementById('sub').setAttribute("disabled", "disabled");
                }                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("erro");
            },
            success: function (data, textStatus, XMLHttpRequest) {
                var results = JSON.parse(data);
                $('#op_parcela_fp_id').children('option').remove(); //Remove todos os itens do select

                for (i = 0; i < results.length; i++) { //Adiciona os item recebidos no results no select
                    $('#op_parcela_fp_id').append($("<option></option>").attr("value", results[i].value).text(results[i].text));
                }                
                if (document.getElementById('text_formaPgto')) {
                    document.getElementById('text_formaPgto').innerHTML = "";
                    document.getElementById('sub').removeAttribute("disabled");
                }
            }
        });
    }
}

function gravarContasFinanceiras(context) {
    let valida = [];
    let nome = document.getElementById('cf_nome');
    let categoria = document.getElementById('cf_categoria_id');
    let op_parcela_fp_id = document.getElementById('op_parcela_fp_id');
    let vlrOperacao = document.getElementById('cf_valor_operacao');
    let cf_valor_parcela_bruta = document.getElementById('cf_valor_parcela_bruta');
    let cf_valor_parcela_liquida = document.getElementById('cf_valor_parcela_liquida');
    let cf_data_inicial = document.getElementById('cf_data_inicial');
    let cf_data_final = document.getElementById('cf_data_final');
    let participante_id = document.getElementById('op_part_participante_id');

    let op_nf_data_emissao = document.getElementById('op_nf_data_emissao');
    let op_nf_chave = document.getElementById('op_nf_chave');
    let op_nf_numero = document.getElementById('op_nf_numero');
    let op_nf_tipo = document.getElementById('op_nf_tipo');

    //Limpara a matrix validação;
    valida.splice(0, valida.length);
    //Limpa div validação
    document.getElementById('msg_valid').innerHTML = '';

    let cf_tipo = document.getElementById('cf_tipo');

    //nome    
    if (nome.value.length < 5 && (nome.value == null || nome.value == '')) {
        valida.push('Nome não pode ser vazio e deve possuir mais de três caracteres!');
    }

    //categoria    
    if (categoria.value == 0 || categoria.value == null) {
        valida.push('Obrigatório informar uma categoria!');
    }

    //Participante
    if (participante_id.value == null || participante_id.value == "" || participante_id.value == 0) {
        valida.push('É obrigatório informar um participante!');
    }

    //Forma de pagamento        
    if (cf_tipo.value == 'Realizada' && (op_parcela_fp_id.value == 0 || op_parcela_fp_id.value == null)) {
        valida.push('É obrigatório informar a forma de pagamento para contas do tipo Realizada!');
    }

    //Valor da operação    
    if (vlrOperacao.value == 0 || vlrOperacao.value == null || vlrOperacao.value < 0) {
        valida.push('O valor da operação deve ser maior que zero!');
    }

    //Data inicial
    let data_in = (cf_data_inicial.value).split('/');
    let d_in = new Date(data_in[2], data_in[1], data_in[0]);
    if (data_in.length < 3 || d_in == 'Invalid Date' || data_in[2].length < 4 || data_in[1] > 12 || data_in[1] < 1 || data_in[0] > 31 || data_in[0] < 1 || data_in == '' || data_in == null) {
        valida.push('Data da primeira parcela inválida!');
    }

    //Valor das parcelas    
    if (cf_tipo.value == 'Realizada') {

        //Valores das parcelas
        if (cf_valor_parcela_bruta.value == 0 || cf_valor_parcela_bruta.value == null || cf_valor_parcela_bruta.value < 0) {
            valida.push('O valor da parcela integral deve ser maior que zero!');
        }
        if (cf_valor_parcela_liquida.value == 0 || cf_valor_parcela_liquida.value == null || cf_valor_parcela_liquida.value < 0) {
            valida.push('O valor da parcela sem juros deve ser maior que zero!');
        }

        //Dados do documento        
        if (op_nf_tipo.value != 0) {

            if (op_nf_tipo.value == 1 && (op_nf_chave.value.length != 44)) {
                valida.push('A chave de acesso do documento é obrigatório e deve ter 44 digitos!');
            }

            let data = (op_nf_data_emissao.value).split('/');
            let d = new Date(data[2], data[1], data[0]);
            if (data.length < 3 || d == 'Invalid Date' || data[2].length < 4 || data[1] > 12 || data[1] < 1 || data[0] > 31 || data[0] < 1 || data == '' || data == null) {
                valida.push('Data de emissão do documento inválido!');
            }

            if (op_nf_numero.value == 0 || op_nf_numero.value == null || op_nf_numero.value.length < 1) {
                valida.push('Número do documento inválido!');
            }
        }
    }

    if (valida.length > 0) {
        for (let i = 0; i < valida.length; i++) {
            $('#msg_valid').append('<span class="text-danger">' + valida[i] + '</span></br>');
        }
    } else {
        cf = {
            op: {
                op_id: 0,
                op_tipo: '',
                op_data: op_nf_data_emissao.value,
                op_previsao_entrega: '',
                op_data_saida: '',
                op_obs: document.getElementById('op_obs').value,
                op_numero_ordem: '',
                op_categoria_id: categoria.value,
                op_comParticipante: false,
                op_comRetencoes: false,
                op_comTransportador: false,
                possui_parcelas: false,
                op_comNF: 0,
            },
            cf: {
                cf_id: 0,
                cf_nome: nome.value,
                cf_categoria_id: categoria.value,
                cf_valor_operacao: vlrOperacao.value.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" }),
                cf_valor_parcela_bruta: '0,00',
                cf_valor_parcela_liquida: '0,00',
                cf_recorrencia: document.getElementById('cf_recorrencia').value,
                cf_data_inicial: cf_data_inicial.value,
                cf_data_final: null,
                cf_tipo: cf_tipo.value,
                cf_status: document.getElementById('cf_status').value,
            },
            parcelas: {
                op_parcela_fp_id: 0,
            },
            nf: {
                op_nf_id: 0,
                op_nf_op_id: 0,
                op_nf_chave: '',
                op_nf_data_emissao: '',
                op_nf_data_entrada_saida: '',
                op_nf_serie: '',
                op_nf_numero: '',
                existe: false,
                op_nf_tipo: 0,
            },
            participante: {
                op_part_participante_id: 0,
            }
        };

        if (document.getElementById('op_part_participante_id').value > 0) {
            cf.op.op_comParticipante = true;
            cf.participante.op_part_participante_id = document.getElementById('op_part_participante_id').value;
        }

        if (cf_tipo.value == 'Realizada') {
            cf.op.op_comNF = op_nf_tipo.value;
            cf.nf.op_nf_tipo = op_nf_tipo.value;
            cf.cf.cf_data_final = cf_data_final.value;

            if (op_nf_tipo.value != 0) {
                cf.nf.op_nf_chave = op_nf_chave.value;
                cf.nf.op_nf_data_emissao = op_nf_data_emissao.value;
                cf.nf.op_nf_numero = op_nf_numero.value;
                cf.nf.op_nf_serie = document.getElementById('op_nf_serie').value;
            }

            cf.cf.cf_valor_parcela_bruta = cf_valor_parcela_bruta.value.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            cf.cf.cf_valor_parcela_liquida = cf_valor_parcela_liquida.value.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            cf.parcelas.op_parcela_fp_id = op_parcela_fp_id.value;
        } else {
            cf.op.op_comNF = 0;
            cf.nf.op_nf_tipo = 0;
        }

        if (cf_tipo.value == 'Realizar') {
            cf.cf.cf_valor_parcela_bruta = vlrOperacao.value.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });
            cf.cf.cf_valor_parcela_liquida = vlrOperacao.value.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "6" });

            if (cf_data_final.value == "" || cf_data_final.value == null) {
                cf.cf.cf_data_final = null;
            } else {
                cf.cf.cf_data_final = cf_data_final.value;
            }
        }

        //número de parcelas
        let parcelas = ContasFinanceiras_gerarParcelas();
        cf.cf.cf_numero_parcelas = parcelas.length;

        let parcela_id = 0;

        if (context == 'CFR_realizacao') {
            parcela_id = document.getElementById('parcela_id').value;
        }

        

        $.ajax({
            url: "/ContasFinanceiras/" + context,
            data: { __RequestVerificationToken: gettoken(), vmcf: cf, parcela_id: parcela_id },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                document.getElementById('text_gravando').innerHTML = "Gravando conta financeira, aguarde...";
                document.getElementById('sub').setAttribute("disabled", "disabled");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("erro");
            },
            success: function (data, textStatus, XMLHttpRequest) {
                var results = JSON.parse(data);
                console.log(results);

                document.getElementById('text_gravando').innerHTML = "";
                document.getElementById('sub').removeAttribute("disabled");

                if (XMLHttpRequest.responseJSON.includes('Erro')) {
                    document.getElementById('msg_retorno').innerHTML = XMLHttpRequest.responseJSON;
                    document.getElementById('btn_ok').style.display = 'none';
                    document.getElementById('btn_cancel').style.display = 'block';
                    $('#modal_retorno').modal('show');
                    return;
                }
                if (XMLHttpRequest.responseJSON.includes('sucesso!')) {
                    document.getElementById('msg_retorno').innerHTML = XMLHttpRequest.responseJSON;
                    document.getElementById('btn_ok').style.display = 'block';
                    document.getElementById('btn_cancel').style.display = 'none';
                    $('#modal_retorno').modal('show');
                }
            }
        });

    }
}

function ContasFinanceiras_gerarParcelas() {    
    let di = document.getElementById('cf_data_inicial').value;
    let df = document.getElementById('cf_data_final').value
    let vlr_i = "0";
    if (document.getElementById('cf_valor_parcela_bruta')) {
        vlr_i = document.getElementById('cf_valor_parcela_bruta').value;
    }
    let vlr_l = "0";
    if (document.getElementById('cf_valor_parcela_liquida')) {
        vlr_l = document.getElementById('cf_valor_parcela_liquida').value;
    }
    let vlr_operacao = document.getElementById('cf_valor_operacao').value;
    let data_start = new Date(di.substr(6, 4) + ',' + di.substr(3, 2) + ',' + di.substr(0, 2));
    let data_end = new Date(df.substr(6, 4) + ',' + df.substr(3, 2) + ',' + df.substr(0, 2));
    let recorrencia = document.getElementById('cf_recorrencia').value;
    let tipo = document.getElementById('cf_tipo').value;
    let vencimento = new Date();
    let parcelas = [];    

    if (data_start == 'Invalid Date') {
        if (tipo == 'Realizada') {
            alert('Data Primeira Parcela Inválida!')
        }
        if (tipo == 'Realizar') {
            alert('Data Primeira Ocorrência Inválida!')
        }        
        return;
    }

    if (recorrencia == 'Unica') {
        let parcela = {
            vencimento: data_start.toLocaleDateString(),
            valor_integral: vlr_i,
            valor_liquido: vlr_l,
        };
        parcelas.push(parcela);

        return parcelas;
    }

    if (tipo == 'Realizada') {
        if (data_end == 'Invalid Date') {
            alert('Data Última Parcela Inválida!');
            return;
        }        
    }


    if (tipo == 'Realizar') {
        if (data_end == 'Invalid Date') {
            let agora = new Date();
            let depois = new Date(agora.getFullYear() + 1, 11, 31);
            data_end = depois;
        }  

        vlr_i = vlr_operacao;
        vlr_l = vlr_operacao;
    }

    vencimento = data_start;
    

    

    while (vencimento < data_end) {        
        let parcela = {
            vencimento: vencimento.toLocaleDateString(),
            valor_integral: vlr_i,
            valor_liquido: vlr_l,
        };
        parcelas.push(parcela);        

        if (recorrencia == 'Semanal') {
            vencimento.setHours(vencimento.getHours() + (24 * 7));
        }

        if (recorrencia == 'Quinzenal') {
            vencimento.setHours(vencimento.getHours() + (24 * 15));
        }

        if (recorrencia == 'Mensal') {
            vencimento.setMonth(vencimento.getMonth() + 1);
        }

        if (recorrencia == 'Bimestral') {
            vencimento.setMonth(vencimento.getMonth() + 2);
        }

        if (recorrencia == 'Trimestral') {
            vencimento.setMonth(vencimento.getMonth() + 3);
        }

        if (recorrencia == 'Semestral') {
            vencimento.setMonth(vencimento.getMonth() + 6);
        }

        if (recorrencia == 'Anual') {
            vencimento.setMonth(vencimento.getMonth() + 12);
        }
    }      

    if (tipo == 'Realizada' || (tipo == 'Realizar' && data_end != 'Invalid Date')) {
        let parcela = {
            vencimento: data_end.toLocaleDateString(),
            valor_integral: vlr_i,
            valor_liquido: vlr_l,
        };
        parcelas.push(parcela);
    }

    return parcelas;
}

function visualizarParcelasCtasF() {
    let parcelas = ContasFinanceiras_gerarParcelas();
    document.getElementById('linhas_parcelas').innerHTML = '';

    if (parcelas.length > 0) {
        for (let i = 0; i < parcelas.length; i++) {
            document.getElementById('linhas_parcelas').innerHTML += '<tr><th scope="row">' + (parseInt(i) + 1) + '</th><td>' + parcelas[i].vencimento + '</td><td>' + parcelas[i].valor_integral + '</td><td>' + parcelas[i].valor_liquido + '</td></tr>';
        }
    } 

    $("#modal_parcelas").modal('show');
}

//Realizar conta recorrente
$(".CFR_realizacao").click(function () {
    modal_modal();
    var parcela_id = $(this).attr("data-parcela_id");    
    var contexto = $(this).attr("data-contexto");
    $("#modal").load("/ContasFinanceiras/CFR_realizacao?parcela_id=" + parcela_id + '&contexto=' + contexto, function () {
        $("#modal").modal('show');
    })
});

function closeModal(id) {
    $("#" + id).modal('hide');
}

function CFR_dataFinal(id, vlr) {
    document.getElementById('cf_data_final').value = vlr;    
}

function editVencimento(context, id, vlr, parcela_id) {
    if (context == 'open') {
        $("#modal_edit_venc").modal('show');
        document.getElementById('venc').value = vlr;
        document.getElementById('nParcela_id').value = parcela_id;
    }

    execDatapicker();
}

function modal_sobre_modal_open(id){
    //Instrução para correções na abertura de modal sobre modal
    $(document).on('show.bs.modal', '.modal', function () {
        var zIndex = 1040 + (10 * $('.modal:visible').length);
        $(this).css('z-index', zIndex);
        setTimeout(function () {
            $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
        }, 0);
    });
    //Instrução para correção da barra de rolagem no fechamento de modal sobre modal
    $(document).on('hidden.bs.modal', '.modal', function () {
        $('.modal:visible').length && $(document.body).addClass('modal-open');
    });
    $("#" + id).modal({ backdrop: 'static', keyboard: false})
    $("#" + id).modal('show');
}

function modal_itemEspecifico() {
    modal_modal();

    $("#modal_item").modal('show');
}

function calculaTotalCCM(id,vlr) {    
    let vPrincipal = document.getElementById('ccm_valor_principal').value.toString().replace('.', '').replace(',', '.') * 1;
    let vjuros = document.getElementById('ccm_multa').value.toString().replace('.', '').replace(',', '.') * 1;
    let vMulta = document.getElementById('ccm_juros').value.toString().replace('.', '').replace(',', '.') * 1;
    let vDesconto = document.getElementById('ccm_desconto').value.toString().replace('.', '').replace(',', '.') * 1;

    decimal('ccm_valor', ((vPrincipal + vjuros + vMulta - vDesconto).toFixed(2).toString().replace('.', ',')), '2', false);
    decimal(id, (vlr.replace('.','')), '2', true);  
}

function ignorarZeradasRelCat(id) {
    let cheque = document.getElementById(id).checked;
    if (cheque) {
        document.getElementById(id).value = 'true';
    } else {
        document.getElementById(id).value = 'false';
    }
}

function change_op_data_CFR(id, vlr) {
    document.getElementById('cf_data_inicial').value = vlr;
    document.getElementById('cf_data_final').value = vlr;
}
function change_cf_valor_operacao_CFR(id, vlr) {
    decimal('cf_valor_parcela_bruta', vlr.replaceAll('.',''), '2', false);
    decimal('cf_valor_parcela_liquida', vlr.replaceAll('.', ''), '2', false);    
}

function blockSubmit(id, form) {        
    if ($("form").valid()) {
        let f = document.getElementById(form);
        document.getElementById(id).setAttribute("disabled", "disabled");
        document.getElementById('msg_blockSubmit').innerHTML = 'Enviando informarções, aguarde...';
        f.submit();
    }
}

//Input pesquisa tabela
$(document).ready(function () {
    $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
})

//Input pesquisa tabela
$(document).ready(function () {
    $("#myInput_datatables").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myTable_body tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
})

//Autocomplete lista de categoria
function consultaCategoria(id) {
    let id_campo = "#" + id;
    $(id_campo).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Categoria/GerarCategorias",
                data: { __RequestVerificationToken: gettoken(), termo: request.term },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    if (document.getElementById('pesquisa_categoria')) {
                        document.getElementById('pesquisa_categoria').innerHTML = 'pesquisando...';
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("erro");
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    if (document.getElementById('pesquisa_categoria')) {
                        document.getElementById('pesquisa_categoria').innerHTML = '';
                    }

                    var results = JSON.parse(data);                    

                    var autocompleteObjects = [];
                    for (var i = 0; i < results.length; i++) {
                        var object = {
                            value: results[i].categoria_nome,
                            label: results[i].categoria_classificacao + " - " + results[i].categoria_nome,
                            id: results[i].categoria_id,
                            tipo: results[i].categoria_tipo,                           
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
            if (ui.item.tipo == 'Sintetica') {
                alert('Não é permitido informar uma categoria do tipo Grupo');
                document.getElementById('categoria').value = '';
                document.getElementById('categoria').focus();
            } else {
                if (document.getElementById('categoria_id_ccm')) {
                    document.getElementById('categoria_id_ccm').value = ui.item.id;
                }
                //Desabilitando o campo para nova inclusão de participante;
                if (document.getElementById('categoria')) {
                    document.getElementById('categoria').setAttribute("disabled", "disabled");
                }
            }
        }
    });
}

function alteraCategoriaCCM() {
    if (document.getElementById('categoria_id_ccm')) {
        document.getElementById('categoria_id_ccm').value = '';
    }
    if (document.getElementById('categoria')) {
        document.getElementById('categoria').removeAttribute("disabled");
        document.getElementById('categoria').value = '';
        document.getElementById('categoria').focus();
    }
}

function registro_change_tipo(id, vlr) {
    if (vlr == 'Pessoa Física') {
        $('#conta_dcto').attr({ placeholder: "CPF" });        
    } else {
        $('#conta_dcto').attr({ placeholder: "CNPJ" });
    }
}

//Autocomplete lista memorando
function consultaMemorando(id,vlr,tamanho, id_input_msg) {
    if (document.getElementById('ccm_memorando')) {
        tamanhoDigitado('ccm_memorando', vlr, tamanho, id_input_msg);
    }
    if (document.getElementById('op_obs')) {
        tamanhoDigitado('op_obs', vlr, tamanho, id_input_msg);
    } 

    let id_campo = "#" + id;
    $(id_campo).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Memorando/consultaMemorando",
                data: { __RequestVerificationToken: gettoken(), termo: request.term },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    if (document.getElementById('pesquisa_memorando')) {
                        document.getElementById('pesquisa_memorando').innerHTML = 'pesquisando...';
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("erro");
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    if (document.getElementById('pesquisa_memorando')) {
                        document.getElementById('pesquisa_memorando').innerHTML = '';
                    }

                    var results = JSON.parse(data);

                    var autocompleteObjects = [];
                    for (var i = 0; i < results.length; i++) {
                        var object = {
                            value: results[i].memorando_descricao,
                            label: results[i].memorando_codigo + " - " + results[i].memorando_descricao,
                            id: results[i].memorando_id,                            
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
            if (document.getElementById('ccm_memorando')) {
                document.getElementById('ccm_memorando').value = ui.item.value;
            }
            if (document.getElementById('op_obs')) {
                document.getElementById('op_obs').value = ui.item.value;
            } 
        }
    });
}

function convertData_DataSimples(data) {
    let d = data.substr(8, 2) + '/' + data.substr(5, 2) + '/' + data.substr(0, 4);

    return d;
}

function convertDoubleString(vlr) {
    vlr_str = vlr.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });

    return vlr_str;
}

function convertStringDouble(vlr) {
    let valor = vlr.toString().replace('.', '').replace(',', '.') * 1;

    return valor;
}

function gerar_sci_ilc() {
    let cliente_id = document.getElementById('cliente_id').value;
    let data_inicial = document.getElementById('data_inicial').value;
    let data_final = document.getElementById('data_final').value;
    let gera_provisao_categoria_fiscal = document.getElementById('gera_provisao_categoria_fiscal').checked;


    $.ajax({
        url: "/Contabilidade/ImportacaoLancamentosContabeis/Create",
        type: 'post',
        data: {
            __RequestVerificationToken: gettoken(),
            cliente_id: cliente_id,
            data_inicial: data_inicial,
            data_final: data_final,
            gera_provisao_categoria_fiscal: gera_provisao_categoria_fiscal
        },
        beforeSend: function () {
            alert('before');
        }
    }).done(function (msg) {
            console.log(msg);            
    }).fail(function (jqXHR, textStatus, msg) {
            console.log(jqXHR);            
            console.log(textStatus);            
            alert(msg);
    });
    
}

function switch_bootstrap(id, vlr) {
    let c = document.getElementById(id).checked;

    if (c) {
        document.getElementById(id).value = true;
    } else {
        document.getElementById(id).value = false;
    }
}

function ilc_inputs_filtros() {
    let e = document.getElementById('ilc_inputs_filtros').style.display;

    if (e == 'none') {
        document.getElementById('ilc_inputs_filtros').style.display = 'block';
    } else {
        document.getElementById('ilc_inputs_filtros').style.display = 'none';
    }
}

//Autocomplete lista de cliente
function consultaClientesContador(id) {
    let id_campo = "#" + id;
    $(id_campo).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: window.location.origin + "/Contabilidade/Clientes/GerarClientes",
                data: { __RequestVerificationToken: gettoken(), termo: request.term },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    if (document.getElementById('pesquisa_cliente')) {
                        document.getElementById('pesquisa_cliente').innerHTML = 'pesquisando...';
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("erro");
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    if (document.getElementById('pesquisa_cliente')) {
                        document.getElementById('pesquisa_cliente').innerHTML = '';
                    }

                    var results = JSON.parse(data);

                    var autocompleteObjects = [];
                    for (var i = 0; i < results.length; i++) {
                        var object = {
                            value: results[i].cliente_nome,
                            label: results[i].cliente_id + " - " + results[i].cliente_nome,
                            id: results[i].cliente_id,                            
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
            document.getElementById('cliente_id').value = ui.item.id;
        }
    });
}

function alteraClienteContador() {
    if (document.getElementById('cliente_id')) {
        document.getElementById('cliente_id').value = '';
    }
    if (document.getElementById('cliente')) {
        document.getElementById('cliente').removeAttribute("disabled");
        document.getElementById('cliente').value = '';
        document.getElementById('cliente').focus();
    }
}

function box_opc_dots(id, e) {      
    let elem = document.getElementById('box_opc_dots').style.display;
    document.getElementById('id_element').value = id;    
    if (elem == 'none') {
        $("#body").disableScroll();
        document.getElementById('box_opc_dots_glass').style.display = 'block';
        document.getElementById('box_opc_dots').style.display = 'block';
        document.getElementById('line_' + id).style.backgroundColor = '#f5f4ba';

        let altura_elem = document.getElementById('box_opc_dots').offsetHeight * 1;        
        let altura_screen = window.innerHeight * 1;
        let altura_clique = e.clientY * 1;        
        if ((altura_elem + altura_clique) > altura_screen) {
            $('#box_opc_dots').removeAttr('style');
            document.getElementById('box_opc_dots').style.bottom = '5px';            
        } else {
            $('#box_opc_dots').removeAttr('style');
            document.getElementById('box_opc_dots').style.top = (e.clientY - 10) + 'px';                    
        }
        document.getElementById('box_opc_dots').style.left = (e.clientX - 182) + 'px';
    } else {
        document.getElementById('box_opc_dots_glass').style.display = 'none';
        document.getElementById('box_opc_dots').style.display = 'none';
        document.getElementById('line_' + id).style.backgroundColor = '';
    }
}
function box_opc_dots_glass() {
    $("#body").enableScroll();
    let id_element = document.getElementById('id_element').value;
    document.getElementById('line_' + id_element).style.backgroundColor = '';
    document.getElementById('box_opc_dots_glass').style.display = 'none';
    document.getElementById('box_opc_dots').style.display = 'none';    
}

function categoria_define_padrao(valor, contexto) {
    let id_element = document.getElementById('id_element').value;

    $.ajax({
        url: "/Categoria/DefinirPadraoCategoria",
        data: { __RequestVerificationToken: gettoken(), padrao: valor, categoria_id: id_element },
        type: 'POST',
        dataType: 'json',
        beforeSend: function (XMLHttpRequest) {
            document.getElementById('btn_cancel').style.display = 'none';
            document.getElementById('btn_ok_cliente').style.display = 'none';
            document.getElementById('btn_ok_contabilidade').style.display = 'none';
            document.getElementById('btn_ok_cliente_contador').style.display = 'none';
            document.getElementById('msg_retorno').innerHTML = "Gravando informação, aguarde...";            
            $('#modal_retorno').modal('show');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            document.getElementById('msg_retorno').innerHTML = "Ocorreu um erro no envio da informação. Tente novamente, se persisitir entre em contato com o suporte!";
            document.getElementById('btn_ok_cliente').style.display = 'none';
            document.getElementById('btn_ok_contabilidade').style.display = 'none';
            document.getElementById('btn_ok_cliente_contador').style.display = 'none';
            document.getElementById('btn_cancel').style.display = 'block';
            $('#modal_retorno').modal('show');

            console.log(XMLHttpRequest);
            console.log(textStatus);
            console.log(errorThrown);
            console.log(destino);

        },
        success: function (data, textStatus, XMLHttpRequest) {
            document.getElementById('btn_cancel').style.display = 'none';

            if (XMLHttpRequest.responseJSON.includes('Erro')) {
                document.getElementById('msg_retorno').innerHTML = XMLHttpRequest.responseJSON;                
                document.getElementById('btn_cancel').style.display = 'block';
                $('#modal_retorno').modal('show');
                return;
            }
            if (XMLHttpRequest.responseJSON.includes('sucesso!')) {
                if (contexto == 'Cliente') {
                    document.getElementById('btn_ok_cliente').style.display = 'block';
                }
                if (contexto == 'plano_categorias') {
                    document.getElementById('btn_ok_contabilidade').style.display = 'block';
                }
                if (contexto == 'categoria_cliente_contador') {
                    document.getElementById('btn_ok_cliente_contador').style.display = 'block';
                }


                document.getElementById('msg_retorno').innerHTML = XMLHttpRequest.responseJSON;                
                $('#modal_retorno').modal('show');
            }
        }
    });
}

function gerar_sci_id() {
    let t = document.querySelectorAll(".sci_id_line");    
    let texto = "";
    
    for (let i = 0; i < t.length; i++) {
        texto += t[i].innerHTML + '\n';
    }    

    let blob = new Blob([texto], {
        type: "text/plain;charset-utf-8"
    });
    saveAs(blob, "sci_id.txt");
    
}

function gerar_sci_ilc() {
    let t = document.querySelectorAll(".sci_id_line");
    let texto = "";

    for (let i = 0; i < t.length; i++) {
        texto += t[i].innerHTML + '\n';
    }

    let blob = new Blob([texto], {
        type: "text/plain;charset-utf-8"
    });
    saveAs(blob, "sci_ilc.txt");

}

function search_m(tipo, contexto) {
    if (contexto == 'close') {
        $("#search_modal").modal('hide');

        return
    }

    if (contexto == 'open') {
        //Categorias
        if (tipo == 'categoria') {
            //Gravando titulo
            document.getElementById('search_modal_label').innerHTML = 'Lista de Categorias';
            //Gerando cabeçalho da tabela
            document.getElementById('search_modal_table_thead').innerHTML = ''; //Limpa thead
            //Gerando cabeçalho
            let c = '';
            c += '<tr>';
            c += '<th style="white-space:nowrap;text-align:left;">Classificação</th>';
            c += '<th style="white-space:nowrap;text-align:left;">Nome</th>';            
            c += '</tr>';
            $('#search_modal_table_thead').append(c);

            //Listando catgorias. Gerando as linhas da tabela
            $.ajax({
                url: "/Categoria/ListaCategorias_ajax",
                data: { __RequestVerificationToken: gettoken()},
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {                    
                    document.getElementById('search_modal_table_tbody').innerHTML = "Gerando categorias, aguarde...";                    
                    modal_sobre_modal_open('search_modal');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    document.getElementById('search_modal_table_tbody').innerHTML = "Erro ao gerar a lista de categorias";
                    modal_sobre_modal_open('search_modal');
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    var r = JSON.parse(data);                    
                    document.getElementById('search_modal_table_tbody').innerHTML = "";

                    if (textStatus == 'error') {
                        document.getElementById('search_modal_table_tbody').innerHTML = "Erro ao gerar a lista de categorias";
                        modal_sobre_modal_open('search_modal');
                    }

                    if (textStatus == 'success') {

                        for (let i = 0; i < r.length; i++) {
                            let item = '';
                            if (r[i].categoria_tipo == 'Analítica') {
                                item += '<tr style="cursor:pointer" onclick="search_m_categoria_select(\''+ r[i].categoria_id +'\',\''+ r[i].categoria_nome +'\')">';
                            } else {
                                item += '<tr style="cursor:no-drop">';
                            }
                            item += '<td style="white-space:nowrap;text-align:left;">' + r[i].categoria_classificacao + '</td>';
                            item += '<td style="white-space:nowrap;text-align:left;">' + r[i].categoria_nome + '</td>';
                            item += '</tr>';
                            $('#search_modal_table_tbody').append(item);
                        }

                        $("#search_modal_input_search").on("keyup", function () {
                            var value = $(this).val().toLowerCase();
                            $("#search_modal_table_tbody tr").filter(function () {
                                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                            });
                        }); 
                    }
                }
            });
        }

        //Participante
        if (tipo == 'participante') {
            //Gravando titulo
            document.getElementById('search_modal_label').innerHTML = 'Lista de Participantes';
            //Gerando cabeçalho da tabela
            document.getElementById('search_modal_table_thead').innerHTML = ''; //Limpa thead
            //Gerando cabeçalho
            let c = '';
            c += '<tr>';
            c += '<th style="white-space:nowrap;text-align:left;">CNPJ/CPF</th>';
            c += '<th style="white-space:nowrap;text-align:left;">Nome</th>';
            c += '<th style="white-space:nowrap;text-align:left;">Nome Fantasia</th>';
            c += '</tr>';
            $('#search_modal_table_thead').append(c);

            //Listando catgorias. Gerando as linhas da tabela
            $.ajax({
                url: "/Participante/listaParticipante_ajax",
                data: { __RequestVerificationToken: gettoken() },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    document.getElementById('search_modal_table_tbody').innerHTML = "Gerando participantes, aguarde...";
                    modal_sobre_modal_open('search_modal');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    document.getElementById('search_modal_table_tbody').innerHTML = "Erro ao gerar a lista de participantes";
                    modal_sobre_modal_open('search_modal');
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    var r = JSON.parse(data);
                    document.getElementById('search_modal_table_tbody').innerHTML = "";

                    if (textStatus == 'error') {
                        document.getElementById('search_modal_table_tbody').innerHTML = "Erro ao gerar a lista de participantes";
                        modal_sobre_modal_open('search_modal');
                    }

                    if (textStatus == 'success') {

                        for (let i = 0; i < r.length; i++) {
                            let item = '';
                            item += '<tr style="cursor:pointer" onclick="search_m_participante_select(\'' + r[i].participante_id + '\',\'' + r[i].participante_nome + '\',\''+ r[i].participante_categoria +'\',\''+ r[i].categoria_nome +'\')">';
                            item += '<td style="white-space:nowrap;text-align:left;">' + r[i].participante_cnpj_cpf + '</td>';
                            item += '<td style="white-space:nowrap;text-align:left;">' + r[i].participante_nome + '</td>';
                            item += '<td style="white-space:nowrap;text-align:left;">' + r[i].participante_fantasia + '</td>';
                            item += '</tr>';
                            $('#search_modal_table_tbody').append(item);
                        }

                        $("#search_modal_input_search").on("keyup", function () {
                            var value = $(this).val().toLowerCase();
                            $("#search_modal_table_tbody tr").filter(function () {
                                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                            });
                        });
                    }
                }
            });
        }

        //Memorando
        if (tipo == 'memorando') {
            //Gravando titulo
            document.getElementById('search_modal_label').innerHTML = 'Lista de Memorandos';
            //Gerando cabeçalho da tabela
            document.getElementById('search_modal_table_thead').innerHTML = ''; //Limpa thead
            //Gerando cabeçalho
            let c = '';
            c += '<tr>';
            c += '<th style="white-space:nowrap;text-align:left;">Código</th>';
            c += '<th style="white-space:nowrap;text-align:left;">Descrição</th>';
            c += '</tr>';
            $('#search_modal_table_thead').append(c);

            //Listando catgorias. Gerando as linhas da tabela
            $.ajax({
                url: "/Memorando/listaMemorando_ajax",
                data: { __RequestVerificationToken: gettoken() },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    document.getElementById('search_modal_table_tbody').innerHTML = "Gerando memorandos, aguarde...";
                    modal_sobre_modal_open('search_modal');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    document.getElementById('search_modal_table_tbody').innerHTML = "Erro ao gerar a lista de memorandos";
                    modal_sobre_modal_open('search_modal');
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    var r = JSON.parse(data);
                    document.getElementById('search_modal_table_tbody').innerHTML = "";

                    if (textStatus == 'error') {
                        document.getElementById('search_modal_table_tbody').innerHTML = "Erro ao gerar a lista de memorandos";
                        modal_sobre_modal_open('search_modal');
                    }

                    if (textStatus == 'success') {

                        for (let i = 0; i < r.length; i++) {
                            let item = '';
                            item += '<tr style="cursor:pointer" onclick="search_m_memorando_select(\''+ r[i].memorando_descricao +'\')">';
                            item += '<td style="white-space:nowrap;text-align:left;">' + r[i].memorando_codigo + '</td>';
                            item += '<td style="white-space:nowrap;text-align:left;">' + r[i].memorando_descricao + '</td>';
                            item += '</tr>';
                            $('#search_modal_table_tbody').append(item);
                        }

                        $("#search_modal_input_search").on("keyup", function () {
                            var value = $(this).val().toLowerCase();
                            $("#search_modal_table_tbody tr").filter(function () {
                                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                            });
                        });
                    }
                }
            });
        }


    }

    
   
}

function search_m_categoria_select(id, nome) {
    //Incluindo a categoria
    if (document.getElementById('categoria')) {
        document.getElementById('categoria').value = nome;
        if (document.getElementById('categoria_id_ccm')) {
            document.getElementById('categoria_id_ccm').value = id;
        }
        document.getElementById('categoria').setAttribute("disabled", "disabled");
        $("#search_modal").modal('hide');
    }
}

function search_m_participante_select(id, nome, categoria_id, categoria_nome) {
    //Incluindo o participante
    if (document.getElementById('participante')) {
        document.getElementById('participante').value = nome;
        if (document.getElementById('ccm_participante_id')) {
            document.getElementById('ccm_participante_id').value = id;
        }        
        document.getElementById('participante').setAttribute("disabled", "disabled");

        //Inserindo a categoria
        if (categoria_id > 0) {
            if (document.getElementById('categoria')) {
                document.getElementById('categoria').value = categoria_nome;
                if (document.getElementById('categoria_id_ccm')) {
                    document.getElementById('categoria_id_ccm').value = categoria_id;
                }
                document.getElementById('categoria').setAttribute("disabled", "disabled");                
            }
        }
    }

    if (document.getElementById('modal_participante')) {        
        $.ajax({
            url: "/Participante/buscaParticipante_ajax",
            data: { __RequestVerificationToken: gettoken(), participante_id: id },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                document.getElementById('search_modal_msg').innerHTML = "Buscando dados do particiante...";
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.getElementById('search_modal_msg').innerHTML = "";
                alert('Erro na busca dos dados do participante selecionado!');
            },
            success: function (data, textStatus, XMLHttpRequest) {
                var p = JSON.parse(data);
                console.log(p);
                if (textStatus == 'error') {
                    document.getElementById('search_modal_msg').innerHTML = "";
                    alert('Busca dos dados do participante retornou erro. \nTente novamente, se persistir entre em contato com o suporte!');
                }

                if (textStatus == 'success') {
                    if (document.getElementById('nome')) {
                        document.getElementById('nome').value = p.participante_nome;
                    }
                    if (document.getElementById('participante_tipoPessoa')) {
                        document.getElementById('participante_tipoPessoa').value = p.participante_tipoPessoa;
                    }
                    if (document.getElementById('op_part_cnpj_cpf')) {
                        document.getElementById('op_part_cnpj_cpf').value = p.participante_cnpj_cpf;
                    }
                    if (document.getElementById('cep')) {
                        document.getElementById('cep').value = p.participante_cep;
                    }
                    if (document.getElementById('rua')) {
                        document.getElementById('rua').value = p.participante_logradouro;
                    }
                    if (document.getElementById('numero')) {
                        document.getElementById('numero').value = p.participante_numero;
                    }
                    if (document.getElementById('complemento')) {
                        document.getElementById('complemento').value = p.participante_complemento;
                    }
                    if (document.getElementById('bairro')) {
                        document.getElementById('bairro').value = p.participante_bairro;
                    }
                    if (document.getElementById('cidade')) {
                        document.getElementById('cidade').value = p.participante_cidade;
                    }
                    if (document.getElementById('uf')) {
                        document.getElementById('uf').value = p.participante_uf;
                    }
                    if (document.getElementById('op_paisesIBGE_codigo')) {
                        document.getElementById('op_paisesIBGE_codigo').value = p.participante_pais;
                    }                    
                    if (document.getElementById('op_part_participante_id')) {
                        document.getElementById('op_part_participante_id').value = p.participante_id;
                    } 

                    document.getElementById('search_modal_msg').innerHTML = "";

                    //Carregando dados do parcitipante no opjeto operação.
                    dadorParticipante('insert', p.participante_id);
                    
                    //Atribuindo a categoria no select2
                    if (document.getElementById('cf_categoria_id')) {
                        $('#cf_categoria_id').val(p.participante_categoria.toString());
                        $('#cf_categoria_id').trigger('change');
                    }
                    //Atribuindo a categoria no select2 quando id = op_categoria_id
                    if (document.getElementById('op_categoria_id')) {
                        $('#op_categoria_id').val(p.participante_categoria.toString());
                        $('#op_categoria_id').trigger('change');
                    }

                    $("#search_modal").modal('hide');
                    //Abre modal
                    modal_sobre_modal_open('modal_participante');
                    document.getElementById('nome').focus();
                }
            }
        });
    } else {
        $("#search_modal").modal('hide');
    }
}


function search_m_memorando_select(nome) {
    //Incluindo o memorando
    if (document.getElementById('ccm_memorando')) {
        document.getElementById('ccm_memorando').value = nome;
        $("#search_modal").modal('hide');
    }

    if (document.getElementById('op_obs')) {
        document.getElementById('op_obs').value = nome;
        $("#search_modal").modal('hide');
    }

    if (document.getElementById('memorando')) {
        document.getElementById('memorando').value = nome;
        $("#search_modal").modal('hide');
    }
}

function gravarCCM_transferencia(escopo, contexto) {
    if (contexto == 'close') {
        document.getElementById('valor').value = '';
        document.getElementById('memorando').value = '';
        $("#transferencia_sucesso").modal('hide');
        $('#modal').on('shown.bs.modal', function () {
            $('#valor').focus();
        });

        $('#valor').focus();
        return
    }

    if (contexto == 'gravar') {
        document.getElementById('validacao_transfer').innerHTML = '';
        let de = document.getElementById('ccorrente_de').value;
        let para = document.getElementById('ccorrente_para').value;
        let valor = document.getElementById('valor').value;
        let memorando = document.getElementById('memorando').value;
        let data_digitada = document.getElementById('data').value;

        let data = (document.getElementById('data').value).split('/');
        let valida = [];

        let d = new Date(data[2], data[1], data[0]);
        if (data.length < 3 || d == 'Invalid Date' || data[2].length < 4 || data[1] > 12 || data[1] < 1 || data[0] > 31 || data[0] < 1) {
            valida.push('Data Inválida');
        }
        if ((de * 1) <= 0) {
            valida.push('Conta corrente "de" inválida');
        }
        if ((para * 1) <= 0) {
            valida.push('Conta corrente "para" inválida');
        }
        if ((valor.replaceAll('.', '').replaceAll(',','.') * 1) <= 0) {
            valida.push('Valor inválido');
        }
        if (memorando.length <= 0) {
            valida.push('Memorando deve ter no mínimo 4 caracteres');
        }
        if (de == para) {
            valida.push('De e Para não podem ser iguais');
        }

        console.log(valida);
        console.log(de);
        console.log(para);
        console.log(valor);
        console.log(memorando);            
        if (valida.length > 0) {            
            for (let i = 0; i < valida.length; i++) {
                $('#validacao_transfer').append('<span class="text-danger">' + valida[i] + '</span></br>');
            }
        } else { 
            $.ajax({
                url: "/Transferencia/" + escopo,
                data: {
                    __RequestVerificationToken: gettoken(),
                    data: data_digitada,
                    valor: valor,
                    ccorrente_de: de,
                    ccorrente_para: para,
                    memorando: memorando
                },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    document.getElementById('sub_form').disabled = true;
                    document.getElementById('msg_blockSubmit').innerHTML = 'Gravando...';
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    document.getElementById('sub_form').disabled = false;
                    document.getElementById('validacao_transfer').innerHTML = 'Erro ao tentar gravar, tente novamente. Se persistir, entre em contato com o suporte!';
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    document.getElementById('sub_form').disabled = false;
                    document.getElementById('msg_blockSubmit').innerHTML = '';

                    if (XMLHttpRequest.responseJSON.includes('Erro')) {
                        document.getElementById('validacao_transfer').innerHTML = XMLHttpRequest.responseJSON;                        
                        return;
                    }
                    if (XMLHttpRequest.responseJSON.includes('sucesso')) {
                        document.getElementById('modal_body_retorno').innerHTML = XMLHttpRequest.responseJSON;
                        $('#transferencia_sucesso').modal('show');
                    }
                }
            });
            
        }
    }
}

function rfm_gerar() {
    let filtro = {
        vm_rfm_filtros_visao: document.getElementById('visao').value,
        data_inicio: document.getElementById('data_inicio').value,
        data_fim: document.getElementById('data_fim').value,
        vm_rfm_ignorar_categorias_zeradas: document.getElementById('ignorarZeradas').value,
    };

    $.ajax({
        url: "/Rfm/Create",
        data: { __RequestVerificationToken: gettoken(), filtro: filtro },
        type: 'POST',
        dataType: 'json',
        beforeSend: function (XMLHttpRequest) {            
            document.getElementById('corpo_rfm').innerHTML = '<span class="text-info">Gerando relatório, aguarde...</span>';
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            document.getElementById('corpo_rfm').innerHTML = '<span class="text-danger">Erro no processamento do relatório</span>';
        },
        success: function (data, textStatus, XMLHttpRequest) {
            var rfm = JSON.parse(data);            
            if (textStatus == 'error') {
                document.getElementById('corpo_rfm').innerHTML = '<span class="text-primary">error</span>';
            }

            if (textStatus == 'success') {
                let saldo_inicial_total = 0;
                let saldo_final_total = 0;
                let entradas = 0;
                let saidas = 0;


                let c = '<div class="table-responsive">';
                c += '<table class="table table-sm">';
                c += '<caption>Relatório financeiro por período</caption>';
                c += '<thead>';
                for (let i = 0; i < rfm.vm_rfm_saldo_inicial_contas.length; i++) {
                    c += '<tr class="rfm_si"><td colspan="2" style="text-align:left">' + rfm.vm_rfm_saldo_inicial_contas[i].conta_corrente_nome + '</td><td style="text-align:right">' + fomat_numeroToStringLocal(rfm.vm_rfm_saldo_inicial_contas[i].conta_corrente_saldo) + '</td></tr>';
                    saldo_inicial_total += rfm.vm_rfm_saldo_inicial_contas[i].conta_corrente_saldo;
                }
                c += '<tr class="rfm_si" style="font-weight:bold"><td colspan="2" style="text-align:left">' + 'Saldo Inicial Total' + '</td><td style="text-align:right">' + fomat_numeroToStringLocal(saldo_inicial_total) + '</td></tr>';
                c += '<tr style="height: 30px;"><td colspan="3"></td></tr>';
                c += '<tr class="thead_title"><th style="text-align:left">Classificação</th><th style="text-align:left">Descrição</th><th style="text-align:right">Valor</th></tr>'
                c += '</thead>';
                c += '<tbody>';
                for (let i = 0; i < rfm.vm_rfm_categorias.length; i++) {
                    if (rfm.vm_rfm_categorias[i].rfmc_categoria_classificacao.length < 5) {
                        c += '<tr class="rfm_bloco" style="font-weight:bold"><td style="text-align:left">' + rfm.vm_rfm_categorias[i].rfmc_categoria_classificacao + '</td><td style="text-align:left">' + rfm.vm_rfm_categorias[i].rfmc_categoria_descricao + '</td><td style="text-align:right">' + fomat_numeroToStringLocal(rfm.vm_rfm_categorias[i].rfmc_categoria_valor) + '</td></tr>';
                    } else {
                        c += '<tr class="rfm_bloco"><td style="text-align:left">' + rfm.vm_rfm_categorias[i].rfmc_categoria_classificacao + '</td><td style="text-align:left">' + rfm.vm_rfm_categorias[i].rfmc_categoria_descricao + '</td><td style="text-align:right">' + fomat_numeroToStringLocal(rfm.vm_rfm_categorias[i].rfmc_categoria_valor) + '</td></tr>';
                    }

                    if (rfm.vm_rfm_categorias[i].rfmc_categoria_classificacao.length == 1 && rfm.vm_rfm_categorias[i].rfmc_categoria_classificacao == "1") {
                        entradas += rfm.vm_rfm_categorias[i].rfmc_categoria_valor;
                    }
                    if (rfm.vm_rfm_categorias[i].rfmc_categoria_classificacao.length == 1 && rfm.vm_rfm_categorias[i].rfmc_categoria_classificacao == "2") {
                        saidas += rfm.vm_rfm_categorias[i].rfmc_categoria_valor;
                    }
                }
                c += '<tr style="height: 30px;"><td colspan="3"></td></tr>';
                if ((entradas - saidas) < 0) {
                    c += '<tr style="background-color:tomato;font-weight:bold;color:white"><td colspan="2" style="text-align:left">Resultado do Período</td><td style="text-align:right">' + fomat_numeroToStringLocal((entradas - saidas)) + '</td></tr>';
                } else {
                    c += '<tr style="background-color:forestgreen;font-weight:bold;color:white"><td colspan="2" style="text-align:left">Resultado do Período</td><td style="text-align:right">' + fomat_numeroToStringLocal((entradas - saidas)) + '</td></tr>';
                }                
                for (let i = 0; i < rfm.vm_rfm_saldo_final_contas.length; i++) {
                    c += '<tr class="rfm_sf"><td colspan="2" style="text-align:left">' + rfm.vm_rfm_saldo_final_contas[i].conta_corrente_nome + '</td><td style="text-align:right">' + fomat_numeroToStringLocal(rfm.vm_rfm_saldo_final_contas[i].conta_corrente_saldo) + '</td></tr>';
                    saldo_final_total += rfm.vm_rfm_saldo_final_contas[i].conta_corrente_saldo;
                }
                c += '<tr class="rfm_sf" style="font-weight:bold"><td colspan="2" style="text-align:left">' + 'Saldo Final Total' + '</td><td style="text-align:right">' + fomat_numeroToStringLocal(saldo_final_total) + '</td></tr>';
                c += '</tbody>';
                c += '</table>';
                c += '</div>';
                console.log(rfm);
                document.getElementById('corpo_rfm').innerHTML = c;
            }
        }
    });
}

function fomat_numeroToStringLocal(vlr, decimais) {
    vlr = vlr.toLocaleString("pt-BR", { style: "decimal", minimumFractionDigits: "2", maximumFractionDigits: "2" });

    return vlr;
}

function Ajuste_parcelas_operacao(parcela_id, contexto) {
    if (contexto == 'open') {
        $.ajax({
            url: "/Operacao/AjusteParcelasOperacao",
            data: { __RequestVerificationToken: gettoken(), parcela_id: parcela_id },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                document.getElementById('apo_vlr_operacao').innerHTML = '';
                document.getElementById('apo_parcela_id').value = 0;
                document.getElementById('apo_parcela_id_edit').value = 0;
                document.getElementById('apo_tbody').innerHTML = '<span class="text-info">Buscando valores parcelas, aguarde...</span>';                
                modal_sobre_modal_open('ajuste_p_op');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.getElementById('apo_tbody').innerHTML = '<span class="text-danger">Erro na busca dos dados da parcela!</span>';
            },
            success: function (data, textStatus, XMLHttpRequest) {
                apo = JSON.parse(data);
                console.log(apo);
                if (textStatus == 'error') {
                    document.getElementById('apo_tbody').innerHTML = '<span class="text-danger">Erro na busca dos dados da parcela!</span>';
                }

                if (textStatus == 'success') {
                    document.getElementById('apo_tbody').innerHTML = '';
                    document.getElementById('apo_vlr_operacao').innerHTML = fomat_numeroToStringLocal(apo.operacao_totais.op_totais_total_op);
                    document.getElementById('apo_parcela_id').value = parcela_id;
                    let retencoes = 0;
                    for (let l = 0; l < apo.parcelas.length; l++) {
                        retencoes += apo.parcelas[l].op_parcela_ret_inss + apo.parcelas[l].op_parcela_ret_issqn + apo.parcelas[l].op_parcela_ret_irrf + apo.parcelas[l].op_parcela_ret_pis + apo.parcelas[l].op_parcela_ret_cofins + apo.parcelas[l].op_parcela_ret_csll;
                    }

                    if (retencoes > 0) {
                        alert('As parcelas da operação possui retenções tributárias e não podem sofrer ajustes nos valores das parcelas.');
                    } else {
                        for (let i = 0; i < apo.parcelas.length; i++) {
                            apo.parcelas[i].op_parcela_valor = fomat_numeroToStringLocal(apo.parcelas[i].op_parcela_valor);
                            apo.parcelas[i].op_parcela_vencimento_alterado = convertData_DataSimples(apo.parcelas[i].op_parcela_vencimento_alterado);

                            let td = '<tr>';
                            td += '<td>';
                            if (apo.parcelas[i].baixas > 0) {
                                td += '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16" style="cursor:pointer" onclick="Ajuste_parcelas_operacao(\'' + apo.parcelas[i].op_parcela_id + '\',\'edit_negado\')"><path d = "M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" /><path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" /></svg >';
                            } else {
                                td += '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16" style="cursor:pointer" onclick="Ajuste_parcelas_operacao(\'' + apo.parcelas[i].op_parcela_id + '\',\'edit\')"><path d = "M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" /><path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" /></svg >';
                            }
                            td += '</td>';
                            td += '<td>' + apo.parcelas[i].op_parcela_vencimento_alterado + '</td>';
                            td += '<td>' + apo.parcelas[i].op_parcela_valor + '</td>';
                            td += '</tr>';
                            $('#apo_tbody').append($(td));
                        }
                    }
                }
            }
        });
    }

    if (contexto == 'edit') {
        for (let i = 0; i < apo.parcelas.length; i++) {
            if (apo.parcelas[i].op_parcela_id == parcela_id) {
                document.getElementById('apo_parcela_data').value = apo.parcelas[i].op_parcela_vencimento_alterado;
                document.getElementById('apo_parcela_valor').value = apo.parcelas[i].op_parcela_valor;
                document.getElementById('apo_parcela_id_edit').value = apo.parcelas[i].op_parcela_id;
                break;
            }
        }
    }

    if (contexto == 'edit_salvar') {
        document.getElementById('apo_tbody').innerHTML = ''; 
        let p = document.getElementById('apo_parcela_id_edit').value;
        for (let i = 0; i < apo.parcelas.length; i++) {
            if (apo.parcelas[i].op_parcela_id == p) {
                apo.parcelas[i].op_parcela_vencimento_alterado = document.getElementById('apo_parcela_data').value;
                apo.parcelas[i].op_parcela_valor = document.getElementById('apo_parcela_valor').value;  

                document.getElementById('apo_parcela_data').value = '';
                document.getElementById('apo_parcela_valor').value = '';
            }

            let td = '<tr>';
            td += '<td>';
            if (apo.parcelas[i].baixas > 0) {
                td += '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16" style="cursor:pointer" onclick="Ajuste_parcelas_operacao(\'' + apo.parcelas[i].op_parcela_id + '\',\'edit_negado\')"><path d = "M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" /><path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" /></svg >';
            } else {
                td += '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16" style="cursor:pointer" onclick="Ajuste_parcelas_operacao(\'' + apo.parcelas[i].op_parcela_id + '\',\'edit\')"><path d = "M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" /><path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" /></svg >';
            }            
            td += '</td>';
            td += '<td>' + apo.parcelas[i].op_parcela_vencimento_alterado + '</td>';
            td += '<td>' + apo.parcelas[i].op_parcela_valor + '</td>';
            td += '</tr>';
            $('#apo_tbody').append($(td));
        }       
    }

    if (contexto == 'close') {
        $('#ajuste_p_op').modal('hide');
    }

    if (contexto == 'edit_negado') {
        alert('Esta parcela possui baixas e não pode ser alterada!');
    }

    if (contexto == 'gravar') {
        let vlr_total = 0;
        for (let i = 0; i < apo.parcelas.length; i++) {
            vlr_total += convertStringDouble(apo.parcelas[i].op_parcela_valor);            
        }

        vlr_total = vlr_total.toFixed(2);
        let op = apo.operacao_totais.op_totais_total_op.toFixed(2);

        if (convertStringDouble(vlr_total) != convertStringDouble(op)) {
            alert('Valor das parcelas é diferente do valor total da operação. \nValor Total Operação: ' + fomat_numeroToStringLocal(apo.operacao_totais.op_totais_total_op) + '\nValor Total das Parcelas: ' + fomat_numeroToStringLocal(vlr_total));            
        } else {
            $.ajax({
                url: "/Operacao/AjusteParcelasOperacaoGravar",
                data: { __RequestVerificationToken: gettoken(), apo: apo },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    document.getElementById('canc_apo').disabled = true;
                    document.getElementById('sub_apo').disabled = true;                    
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('Erro no envio dos dados');
                    document.getElementById('canc_apo').disabled = false;
                    document.getElementById('sub_apo').disabled = false;
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    apo = JSON.parse(data);                    
                    if (textStatus == 'error') {
                        alert('Erro no envio dos dados');
                        document.getElementById('canc_apo').disabled = false;
                        document.getElementById('sub_apo').disabled = false;
                    }

                    if (textStatus == 'success') {
                        if (XMLHttpRequest.responseJSON.includes('sucesso')) {
                            alert('Ajustes nas parcelas gravado com sucesso');
                            window.location.href = window.location.origin + '/ContasPagar/Index';
                        }

                        if (XMLHttpRequest.responseJSON.includes('Erro')) {
                            alert(XMLHttpRequest.responseJSON);
                        }
                    }
                }
            });
        }
    }
}






