let perguntas_respostas = [
    pergunta_resposta = {
        pergunta: "Preciso passar as minhas senhas bancárias?",
        resposta: "Não, precisamos de uma acesso limitado, apenas para consultar o extrato bancário."
    },
    pergunta_resposta = {
        pergunta: "Quais são as responsabilidades do cliente?",
        resposta: "Enviar as informações e validar os agendamentos de pagamentos no banco."
    },
    pergunta_resposta = {
        pergunta: "Preciso enviar documentos físicos?",
        resposta: "Não, para facilitar seu dia a dia pode nos enviar pelo whatsapp ou e-mail. Notas fiscais eletrônicas são fornecidas pela receita federal com acesso por certificado digital."
    },
    pergunta_resposta = {
        pergunta: "Eu não tenho muito tempo, como será meu atendimento?",
        resposta: "Sabemos como é corrido o dia a dia nas empresas para o empresário e por isso utilizamos da tecnologia para facilitar a sua vida. Nossa comunicação será pelo whatsapp ou e-mail."
    },
    pergunta_resposta = {
        pergunta: "A Contadorcomvocê pode enviar meus documentos para a minha contabildade?",
        resposta: "Sim, com a sua autorização enviaremos regularmente e organizada toda a documentação para a contabilidade."
    },
    pergunta_resposta = {
        pergunta: "O contrato com a Contadorcomvocê possui fidelidade?",
        resposta: "Não, fique tranquilo. Caso deseja encerrar o serviço basta nos comunicar por e-mail."
    },
];

function perguntas() {
    let p = '';   

    for (let i = 0; i < perguntas_respostas.length; i++) {
        p += '<div class="card">';
        p += '<div class="card-header pegunta_accordion_header" id="heading' + i + '" data-toggle="collapse" data-target="#collapse' + i + '" aria-expanded="true" aria-controls="collapse' + i + '>;'
        p += '<span class="pegunta_accordion" type="button">' + perguntas_respostas[i].pergunta + '</span></div>';
        if (i == 0) {
            p += '<div id="collapse' + i + '" class="collapse show" aria-labelledby="headingOne" data-parent="#myaccordion">';
        } else {
            p += '<div id="collapse' + i + '" class="collapse" aria-labelledby="headingOne" data-parent="#myaccordion">';
        }
        p += '<div class="card-body pegunta_accordion_body">' + perguntas_respostas[i].resposta + '</div></div></div>';
    }

    document.getElementById('myaccordion').innerHTML = p;
}

$(document).ready(function () {
    if (document.getElementById('myaccordion')) {
        perguntas();
    }
});


let rfm = {};

let produto_op_selecionado = {};

let fcc = {
    fcc_id: 0,
    fcc_forma_pagamento_id: 0,
    fcc_nome_cartao: '',
    fcc_situacao: '',
    fcc_data_corte: '',
    fcc_data_vencimento: '',
    fcc_movimentos: [],
}

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
    if (document.getElementById('saldo_total')) {
        document.getElementById('saldo_total').innerHTML = convertDoubleString(convertStringDouble(document.getElementById('input_saldo_total').value));
        document.getElementById('total_original').innerHTML = convertDoubleString(convertStringDouble(document.getElementById('input_original_total').value));
    }
    
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
    let local = window.location.origin + window.location.pathname + "/Delete?id=";
    var id = $(this).attr("data-id");
    $("#modal").load(local + id, function () {
        $("#modal").modal('show');
    })
});

$(".EditPassword").click(function () {
    let local = window.location.origin + window.location.pathname + "/EditPassword?id=";
    var id = $(this).attr("data-id");
    $("#modal").load(local + id, function () {
        $("#modal").modal('show');
    })
});

$(".createBco").click(function () {
    //var id = $(this).attr("data-id");    
    let local = window.location.origin + window.location.pathname + "/CreateCxBanco";
    $("#modal").load(local , function () {
        $("#modal").modal('show');
    })
});

$(".deleteBco").click(function () {
    let local = window.location.origin + window.location.pathname + "/DeleteCxBanco?id=";
    var id = $(this).attr("data-id");
    var descricao = $(this).attr("data-descricao");
    $("#modal").load(local + id + "&descricao=" + encodeURIComponent(descricao), function () {
        $("#modal").modal('show');
    })
});

$(".create").click(function () {
    let local = window.location.origin + window.location.pathname + "/Create?id=";
    var id = $(this).attr("data-id");
    $("#modal").load(local + id, function () {
        $("#modal").modal('show');
    })
});

$(".edit").click(function () {
    let local = window.location.origin + window.location.pathname + "/Edit?id=";
    var id = $(this).attr("data-id");
    $("#modal").load(local + id, function () {
        $("#modal").modal('show');
    })
});

$(".createConta").click(function () {
    let local = window.location.origin + window.location.pathname + "/Create?id=";
    var id = $(this).attr("data-id");
    $("#modal").load(local + id, function () {
        $("#modal").modal('show');
    })
});


$(document).ready(function () {
    $('#Esconder').delay(4000).fadeOut();
});

//Categoria
$(".createGrupoCategoria").click(function () {
    let local = window.location.origin + window.location.pathname + "/CreateGrupoCategoria?escopo=";
    var escopo = $(this).attr("data-escopo");
    $("#modal").load(local + escopo, function () {
        $("#modal").modal('show');
    })
});

$(".createCategoria").click(function () {
    let local = window.location.origin + window.location.pathname + "/Create?grupo=";
    var grupo = $(this).attr("data-grupo");
    var escopo = $(this).attr("data-escopo");
    $("#modal").load(local + grupo + "&escopo=" + escopo, function () {
        $("#modal").modal('show');
    })
});

$(".editCategoria").click(function () {
    let local = window.location.origin + window.location.pathname + "/Edit?id=";
    var id = $(this).attr("data-id");
    var tipo = $(this).attr("data-tipo");
    $("#modal").load(local + id + "&tipo=" + tipo, function () {
        $("#modal").modal('show');
    })
});

$(".deleteCategoria").click(function () {
    let local = window.location.origin + window.location.pathname + "/Delete?id=";
    var id = $(this).attr("data-id");
    var tipo = $(this).attr("data-tipo");
    $("#modal").load(local + id + "&tipo=" + tipo, function () {
        $("#modal").modal('show');
    })
});

$(".copiarPlanoCategorias").click(function () { 
    copiarPlanoCategorias
    $("#modal").load(local, function () {
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
    let local = window.location.origin + window.location.pathname + "/DeleteContabilidade";
    $("#modal").load(local, function () {        
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
    let local = window.location.origin + window.location.pathname + "/Edit?ccontabil_id=";
    var ccontabil_id = $(this).attr("data-ccontabil_id");
    var plano_id = $(this).attr("data-plano_id");
    $("#modal").load(local + ccontabil_id + "&plano_id=" + plano_id, function () {
        $("#modal").modal('show');
    })
});

$(".deleteContaContabil").click(function () {
    let local = window.location.origin + window.location.pathname + "/Delete?ccontabil_id=";
    var ccontabil_id = $(this).attr("data-ccontabil_id");
    var plano_id = $(this).attr("data-plano_id");
    $("#modal").load(local + ccontabil_id + "&plano_id=" + plano_id, function () {
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
    $(".js_example_basic_single").select2({
        "language": "pt-BR"
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
    let local_ = window.location.origin + "/Contabilidade/CCO/Create?categoria_id=";
    var categoria_id = $(this).attr("data-categoria_id");
    var plano_id = $(this).attr("data-plano_id");
    var local = $(this).attr("data-local");
    console.log(plano_id);
    $("#modal").load(local_ + categoria_id + "&plano_id=" + plano_id + "&local=" + local, function () {
        $("#modal").modal('show');
    })
});

$(".DetailsCCO").click(function () {
    let local = window.location.origin + "/Contabilidade/CCO/Details?id=";
    var id = $(this).attr("data-id");
    $("#modal").load(local + id, function () {
        $("#modal").modal('show');
    })
});

$(".SelectPlano").click(function () {
    let local = window.location.origin + "/Contabilidade/CategoriasPlano/SelectPlano?pc_id=";
    var pc_id = $(this).attr("data-pc_id");
    $("#modal").load(local + pc_id, function () {
        $("#modal").modal('show');
    })
});


//Plano de Categorias
$(".createGrupoCategoriaPlano").click(function () {
    let local = window.location.origin + window.location.pathname + "/CreateGrupoCategoria?escopo=";
    var escopo = $(this).attr("data-escopo");
    var planoCategorias_id = $(this).attr("data-planoCategorias_id");
    var planoContas_id = $(this).attr("data-planoContas_id");
    $("#modal").load(local + escopo + "&planoCategorias_id=" + planoCategorias_id + "&planoContas_id=" + planoContas_id, function () {
        $("#modal").modal('show');
    })
});

$(".createCategoriaPlano").click(function () {
    let local = window.location.origin + "/Contabilidade/CategoriasPlano/Create?grupo=";
    var grupo = $(this).attr("data-grupo");
    var escopo = $(this).attr("data-escopo");
    var planoCategorias_id = $(this).attr("data-planoCategorias_id");
    var planoContas_id = $(this).attr("data-planoContas_id");
    $("#modal").load(local + grupo + "&escopo=" + escopo + "&planoCategorias_id=" + planoCategorias_id + "&planoContas_id=" + planoContas_id, function () {
        $("#modal").modal('show');
    })
});

$(".editCategoriaPlano").click(function () {
    let local = window.location.origin + "/Contabilidade/CategoriasPlano/Edit?id=";
    var id = $(this).attr("data-id");
    var tipo = $(this).attr("data-tipo");
    var planoCategorias_id = $(this).attr("data-planoCategorias_id");
    var planoContas_id = $(this).attr("data-planoContas_id");
    $("#modal").load(local + id + "&tipo=" + tipo + "&planoCategorias_id=" + planoCategorias_id + "&planoContas_id=" + planoContas_id, function () {
        $("#modal").modal('show');
    })
});

$(".deleteCategoriaPlano").click(function () {
    let local = window.location.origin + "/Contabilidade/CategoriasPlano/Delete?id=";
    var id = $(this).attr("data-id");
    var tipo = $(this).attr("data-tipo");
    var planoCategorias_id = $(this).attr("data-planoCategorias_id");
    var planoContas_id = $(this).attr("data-planoContas_id");
    $("#modal").load(local + id + "&tipo=" + tipo + "&planoCategorias_id=" + planoCategorias_id + "&planoContas_id=" + planoContas_id, function () {
        $("#modal").modal('show');
    })
});

//Vinculo conta on line categorias cliente visão contador
$(".createCategoria_contaonlinePlano").click(function () {
    let local = window.location.origin + "/Contabilidade/CCOPlanoCategorias/Create?categoria_id=";
    var categoria_id = $(this).attr("data-categoria_id");    
    var planoCategorias_id = $(this).attr("data-planoCategorias_id");
    var planoContas_id = $(this).attr("data-planoContas_id");    
    $("#modal").load(local + categoria_id + "&planoContas_id=" + planoContas_id + "&planoCategorias_id=" + planoCategorias_id, function () {
        $("#modal").modal('show');
    })
});

$(".DetailsCCOPlano").click(function () {
    let local = window.location.origin + "/Contabilidade/CCOPlanoCategorias/Details?id=";
    var id = $(this).attr("data-id");
    $("#modal").load(local + id, function () {
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
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],        
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
    if (vlr == 0 || vlr == '0' || vlr == '0.00' || vlr == '0,00' || isNaN(vlr.toString().replaceAll('.', '').replaceAll(',', '.') * 1)) {        
        document.getElementById(id).value = (0).toFixed(2);

        if (isNaN(vlr.toString().replaceAll('.', '').replaceAll(',', '.') * 1)) {
            alert('O valor: ' + vlr + ' não é número. Digite um número separado por vírgula na decimal.');
            document.getElementById(id).focus();
        }

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
function consultaParticipante(id, contexto) {
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
            dadorParticipante('insert', ui.item.id);

            if (contexto == 'operacao') {
                if (operacao.op_part_participante_id != 0) {
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

                    document.getElementById('participante').disabled = true;

                    //gravando informação que operação é com participante
                    operacao.operacao.op_comParticipante = true;
                }
            }
            
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
                            origem: results[i].produtos_origem,
                            ncm: results[i].produtos_ncm,
                            cest: results[i].produtos_cest,
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
            produto_op_selecionado = ui.item;

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
    if (isNaN(vlr.toString().replaceAll('.', '').replaceAll(',', '.') * 1)) {
        alert('O valor: ' + vlr + ' não é número. Digite um número separado por vírgula no decimal.');
        document.getElementById(id).value = (0).toFixed(2);
        document.getElementById(id).focus();
        return;
    }

    let qtd = document.getElementById('prod_quantidade').value;
    let vlrProd = document.getElementById('prod_valor').value;
    let total = qtd.toString().replace(".", "").replace(",", ".") * vlrProd.toString().replace(".", "").replace(",", ".");

    decimal('prod_valorTotal', total.toString().replace(".", ","), '6', true);
    decimal(id, vlr, '6', true);    
}

function changeItens(id, vlr, inputTotalizador) {
    if (isNaN(vlr.toString().replaceAll('.', '').replaceAll(',', '.') * 1)) {
        alert('O valor: ' + vlr + ' não é número. Digite um número separado por vírgula no decimal.');
        document.getElementById(id).value = (0).toFixed(2);
        document.getElementById(id).focus();
        return;
    }

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
    let local = window.location.origin + "/Baixa/Create?parcela_id=";
    var parcela_id = $(this).attr("data-parcela_id");        
    var contexto = $(this).attr("data-contexto");
    $("#modal").load(local + parcela_id + "&contexto=" + contexto, function () {
        $("#modal").modal('show');
    })
});

$(".editBaixa").click(function () {
    let local_ = window.location.origin + "/Baixa/Edit?baixa_id=";
    var baixa_id = $(this).attr("data-baixa_id");
    var local = $(this).attr("data-local");
    var contacorrente_id = $(this).attr("data-contacorrente_id");
    var dataInicio = $(this).attr("data-dataInicio");
    var dataFim = $(this).attr("data-dataFim");
    //var ndataInicio = new Date(dataInicio.substr(6, 4) + ',' + dataInicio.substr(3, 2) + ',' + dataInicio.substr(0, 2));
    //var ndataFim = new Date(dataFim.substr(6, 4) + ',' + dataFim.substr(3, 2) + ',' + dataFim.substr(0, 2));
    var ndataInicio = dataInicio.substr(6, 4) + '-' + dataInicio.substr(3, 2) + '-' + dataInicio.substr(0, 2);
    var ndataFim = dataFim.substr(6, 4) + '-' + dataFim.substr(3, 2) + '-' + dataFim.substr(0, 2);

    $("#modal").load(local_ + baixa_id + "&local=" + local + "&contacorrente_id=" + contacorrente_id + "&dataInicio=" + ndataInicio + "&dataFim=" + ndataFim, function () {
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
    let local = window.location.origin + "/Transferencia/Create?contacorrente_id=";
    var contacorrente_id = $(this).attr("data-contacorrente_id");    
    var dataInicio = $(this).attr("data-dataInicio");
    var dataFim = $(this).attr("data-dataFim");    
    var ndataInicio = dataInicio.substr(6, 4) + '-' + dataInicio.substr(3, 2) + '-' + dataInicio.substr(0, 2);
    var ndataFim = dataFim.substr(6, 4) + '-' + dataFim.substr(3, 2) + '-' + dataFim.substr(0, 2);

    $("#modal").load(local + contacorrente_id + "&dataInicio=" + ndataInicio + "&dataFim=" + ndataFim, function () {
        $("#modal").modal('show');
    })
});

$(".createCCM").click(function () {
    let local = window.location.origin + "/ContaCorrenteMov/Create?contacorrente_id=";
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
    let local = window.location.origin + "/ContaCorrenteMov/Edit?contacorrente_id=";
    var ccm_id = $(this).attr("data-ccm_id");
    var contacorrente_id = $(this).attr("data-contacorrente_id");
    if (contacorrente_id == 0) {
        contacorrente_id = document.getElementById('contacorrente_id').value;
    }
    var dataInicio = $(this).attr("data-dataInicio");
    var dataFim = $(this).attr("data-dataFim");
    var ndataInicio = dataInicio.substr(6, 4) + '-' + dataInicio.substr(3, 2) + '-' + dataInicio.substr(0, 2);
    var ndataFim = dataFim.substr(6, 4) + '-' + dataFim.substr(3, 2) + '-' + dataFim.substr(0, 2);

    $("#modal").load(local + contacorrente_id + "&dataInicio=" + ndataInicio + "&dataFim=" + ndataFim + "&ccm_id=" + ccm_id, function () {
        $("#modal").modal('show');
    });
});

$(".editTransferencia").click(function () {
    let local = window.location.origin + "/Transferencia/Edit?contacorrente_id=";
    var ccm_id = $(this).attr("data-ccm_id");
    var contacorrente_id = $(this).attr("data-contacorrente_id");
    var dataInicio = $(this).attr("data-dataInicio");
    var dataFim = $(this).attr("data-dataFim");
    var ndataInicio = dataInicio.substr(6, 4) + '-' + dataInicio.substr(3, 2) + '-' + dataInicio.substr(0, 2);
    var ndataFim = dataFim.substr(6, 4) + '-' + dataFim.substr(3, 2) + '-' + dataFim.substr(0, 2);

    $("#modal").load(local + contacorrente_id + "&dataInicio=" + ndataInicio + "&dataFim=" + ndataFim + "&ccm_id=" + ccm_id, function () {
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
                if (results.retorno.includes('Erro')) {
                    document.getElementById("retorno_fc_existe").innerHTML = results.retorno;
                    document.getElementById("retorno_fc_existe").style.display = 'block';                    

                    return;
                }

                if (results.retorno.includes('Referência não existe!')) {
                    document.getElementById("retorno_fc_existe").innerHTML = "";
                    document.getElementById("retorno_fc_existe").style.display = 'none';
                    document.getElementById("obs_fatura").style.display = 'block';
                }

                if (results.retorno.includes('Referência existe!')) {
                    document.getElementById("retorno_fc_existe").innerHTML = "Já existe uma fatura para este cartão nesta competência. As parcelas selecionadas serão acrescentadas a esta fatura!";
                    document.getElementById("retorno_fc_existe").style.display = 'block';
                    document.getElementById("op_obs").innerHTML = '';
                    document.getElementById("obs_fatura").style.display = 'none';
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
    let local = window.location.origin + "/Parcela/Index?parcela_id=";
    var parcela_id = $(this).attr("data-parcela_id");
    $("#modal_parcela").load(local + parcela_id, function () {
        $("#modal_parcela").modal('show');
    })
});

function gravarLancamentoCCM(contexto) {
    document.getElementById('btn_gravar_ccm').disabled = true; //bloqueia o botão para evitar duplo clique;
    document.getElementById('validaForm').innerHTML = '';
    retorno = '';
    validacao = true;

    if (document.getElementById('ccm_data')) {
        dl = moment(document.getElementById('ccm_data').value, 'DD/MM/YYYY', 'pt', true);
        if (!dl.isValid()) {
            retorno += 'Data inválida.;';
            validacao = false;
        }

        /*let data = (document.getElementById('ccm_data').value).split('/');
        let d = new Date(data[2], data[1], data[0]);
        if (data.length < 3 || d == 'Invalid Date' || data[2].length < 4 || data[1] > 12 || data[1] < 1 || data[0] > 31 || data[0] < 1 || data == '' || data == null) {
            retorno += 'Data inválida.;';
            validacao = false;
        }*/
    }

    //Data da competência
    if (document.getElementById('ccm_data_competencia')) {
        dc = moment(document.getElementById('ccm_data_competencia').value, 'DD/MM/YYYY', 'pt', true);
        if (!dc.isValid()) {
            retorno += 'Data da competência inválida.;';
            validacao = false;
        } 

        /*let data = (document.getElementById('ccm_data_competencia').value).split('/');
        let d = new Date(data[2], data[1], data[0]);
        if (data.length < 3 || d == 'Invalid Date' || data[2].length < 4 || data[1] > 12 || data[1] < 1 || data[0] > 31 || data[0] < 1 || data == '' || data == null) {
            retorno += 'Data da competência inválida.;';
            validacao = false;
        }*/
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
                document.getElementById('btn_gravar_ccm').disabled = true;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("erro");
                document.getElementById('btn_gravar_ccm').disabled = false;
            },
            success: function (data, textStatus, XMLHttpRequest) {
                var results = JSON.parse(data);
                //console.log(results);

                if (XMLHttpRequest.responseJSON.includes('Erro')) {                    
                    document.getElementById('msg_retorno').innerHTML = XMLHttpRequest.responseJSON;
                    $("#Esconder").fadeTo(4000, 500).slideUp(500, function () {
                        $("#Esconder").slideUp(500);
                    });                    
                    return;
                }                
                if (XMLHttpRequest.responseJSON.includes('cadastrado com sucesso!')) {                                        
                    $("#ccm_gravado_sucesso").modal('show');
                    document.getElementById('btn_gravar_ccm').disabled = false;
                    return;
                }

                if (XMLHttpRequest.responseJSON.includes('alterado com sucesso!')) { 
                    $("#ccm_alterado_sucesso").modal('show');
                    document.getElementById('btn_gravar_ccm').disabled = false;
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
    let local = window.location.origin + "/ContasFinanceiras/CFR_realizacao?parcela_id=";
    modal_modal();
    var parcela_id = $(this).attr("data-parcela_id");    
    var contexto = $(this).attr("data-contexto");
    $("#modal").load(local + parcela_id + '&contexto=' + contexto, function () {
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
    $("#" + id).modal({ backdrop: 'static' });
    $("#" + id).modal({ keyboard: false });
    $("#" + id).modal('show');
}

function modal_itemEspecifico() {
    modal_modal();

    $("#modal_item").modal('show');
}

function calculaTotalCCM(id, vlr) {
    if (isNaN(vlr.toString().replaceAll('.', '').replaceAll(',', '.') * 1)) {
        alert('O valor: ' + vlr + ' não é número. Digite um número separado por vírgula no decimal.');
        document.getElementById(id).value = (0).toFixed(2);
        document.getElementById(id).focus();
        return;
    }

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

    if (document.getElementById('parcela_obs')) {
        tamanhoDigitado('parcela_obs', vlr, tamanho, id_input_msg);
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

                    //gravando informação que operação é com participante
                    operacao.operacao.op_comParticipante = true;
                    
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
                    //modal_sobre_modal_open('modal_participante'); ==> Alterado em 04/03/2021. Após fechar não abrirá a tela de participante.
                    document.getElementById('btn_participante').focus();
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

    if (document.getElementById('parcela_obs')) {
        document.getElementById('parcela_obs').value = nome;
        $("#search_modal").modal('hide');
    }
}

function gravarCCM_transferencia(escopo, contexto) {
    if (contexto == 'close') {
        document.getElementById('sub_form').disabled = false;
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
        document.getElementById('sub_form').disabled = true;
        document.getElementById('validacao_transfer').innerHTML = '';
        let de = document.getElementById('ccorrente_de').value;
        let para = document.getElementById('ccorrente_para').value;
        let valor = document.getElementById('valor').value;
        let memorando = document.getElementById('memorando').value;
        let data_digitada = document.getElementById('data').value;
        let ccm_id = 0;
        if (document.getElementById('ccm_id')) {
            ccm_id = document.getElementById('ccm_id').value;
        } 

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
                    memorando: memorando,
                    ccm_id: ccm_id,
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
            rfm = JSON.parse(data);            
            if (textStatus == 'error') {
                document.getElementById('corpo_rfm').innerHTML = '<span class="text-primary">error</span>';
            }

            if (textStatus == 'success') {
                let saldo_inicial_total = 0;
                let saldo_final_total = 0;
                let entradas = 0;
                let saidas = 0;


                let c = '<div class="table-responsive">';
                c += '<table class="table table-sm" id="rfm_table">';
                c += '<caption>Relatório financeiro por período</caption>';
                c += '<thead>';
                for (let i = 0; i < rfm.vm_rfm_saldo_inicial_contas.length; i++) {
                    c += '<tr class="rfm_si"><td colspan="2" style="text-align:left">' + rfm.vm_rfm_saldo_inicial_contas[i].conta_corrente_nome + '</td><td style="text-align:right">' + fomat_numeroToStringLocal(rfm.vm_rfm_saldo_inicial_contas[i].conta_corrente_saldo) + '</td></tr>';
                    saldo_inicial_total += rfm.vm_rfm_saldo_inicial_contas[i].conta_corrente_saldo;
                }
                c += '<tr class="rfm_si" style="font-weight:bold"><td colspan="2" style="text-align:left">' + 'Saldo Inicial Total' + '</td><td style="text-align:right">' + fomat_numeroToStringLocal(saldo_inicial_total) + '</td></tr>';
                c += '<tr class="rfm_si"><td colspan="2" style="text-align:left"></td><td style="text-align:right"></td></tr>';
                //c += '<tr style="height: 30px;"><td colspan="3"></td></tr>';                
                c += '<tr class="thead_title"><th style="text-align:left">Classificação</th><th style="text-align:left">Descrição</th><th style="text-align:right">Valor</th></tr>'
                c += '</thead>';
                c += '<tbody>';
                for (let i = 0; i < rfm.vm_rfm_categorias.length; i++) {
                    if (rfm.vm_rfm_categorias[i].rfmc_categoria_classificacao.length < 5) {
                        c += '<tr class="rfm_bloco" style="font-weight:bold"><td style="text-align:left">' + rfm.vm_rfm_categorias[i].rfmc_categoria_classificacao + '</td><td style="text-align:left">' + rfm.vm_rfm_categorias[i].rfmc_categoria_descricao + '</td><td style="text-align:right">' + fomat_numeroToStringLocal(rfm.vm_rfm_categorias[i].rfmc_categoria_valor) + '</td></tr>';
                    } else {
                        c += '<tr class="rfm_bloco" style="cursor:pointer" onclick="rfm_details_gerar(\'' + rfm.vm_rfm_categorias[i].rfmc_categoria_classificacao + '\',\'' + rfm.vm_rfm_categorias[i].rfmc_categoria_descricao + '\')"><td style="text-align:left">' + rfm.vm_rfm_categorias[i].rfmc_categoria_classificacao + '</td><td style="text-align:left">' + rfm.vm_rfm_categorias[i].rfmc_categoria_descricao + '</td><td style="text-align:right">' + fomat_numeroToStringLocal(rfm.vm_rfm_categorias[i].rfmc_categoria_valor) + '</td></tr>';
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
                //console.log(rfm);
                document.getElementById('corpo_rfm').innerHTML = c;
                //Se chegou até aqui gera o botão gerar pdf
                if (document.getElementById('btn_imprimir') == null) {
                    document.getElementById('gr_btn').innerHTML += '<button type="button" class="btn btn-info" id=\'btn_imprimir\' onclick="gerarPDF(\'rfm_table\')">Imprimir</button>';
                }
            }
        }
    });
}

function rfm_details_gerar(classificacao, nome) {
    document.getElementById('categoria_selecionada').innerHTML = '';
    document.getElementById('lista_lancamentos').innerHTML = '';

    $.ajax({
        url: "/RfmDetails/Create",
        data: { __RequestVerificationToken: gettoken(), classificacao: classificacao, data_inicio: rfm.filtro.data_inicio, data_fim: rfm.filtro.data_fim },
        type: 'POST',
        dataType: 'json',
        beforeSend: function (XMLHttpRequest) {
            document.getElementById('categoria_selecionada').innerHTML = '<span class="text-info">Gerando detalhamento, aguarde...</span>';
            modal_sobre_modal_open('m_details');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            document.getElementById('categoria_selecionada').innerHTML = '<span class="text-danger">Erro no processamento das informações</span>';
        },
        success: function (data, textStatus, XMLHttpRequest) {
            rfmd = JSON.parse(data);
            console.log(rfmd);
            if (textStatus == 'error') {
                document.getElementById('categoria_selecionada').innerHTML = '<span class="text-primary">Erro na comunicação com o servidor</span>';
            }

            if (textStatus == 'success') {                
                let vlr = 0;
                document.getElementById('categoria_selecionada').innerHTML = '<span class="text-primary">' + classificacao + ' - ' + nome + '</span><hr class="hrLine" />';
                let c = '<div class="table-responsive">';
                c += '<table class="table table-sm">';
                c += '<caption>Detalhamento</caption>';
                c += '<thead>';
                c += '<tr class="thead_title"><th style="text-align:center">Data</th><th style="text-align:center">Origem</th><th style="text-align:left">Participante</th><th style="text-align:left">Memorando</th><th style="text-align:right">Valor</th></tr>'
                c += '</thead>';
                c += '<tbody>';
                for (let i = 0; i < rfmd.length; i++) {
                    c += '<tr>';
                    c += '<td style="text-align:center">' + convertData_DataSimples(rfmd[i].data) + '</td>';
                    c += '<td style="text-align:center">' + rfmd[i].origem + '</td>';
                    c += '<td style="text-align:left">' + rfmd[i].participante + '</td>';
                    c += '<td style="text-align:left">' + rfmd[i].memorando + '</td>';
                    c += '<td style="text-align:right">' + convertDoubleString(rfmd[i].valor) + '</td>';
                    c += '</tr>';
                    vlr += rfmd[i].valor;
                }
                c += '<tr><td colspan="5" style="text-align:right;font-weight:bold">' + convertDoubleString(vlr) + '</td></tr>';
                c += '</tbody>';
                c += '</table>';
                c += '</div>';                
                document.getElementById('lista_lancamentos').innerHTML = c;                
            }
        }
    });
}

function rp_gerar() {
    let d = $("#tipos_participante").select2('data');
    let tipos_participantes = [];

    for (let i = 0; i < d.length; i++) {
        tipos_participantes.push(parseInt(d[i].id));
    }

    let c = $("#categorias").select2('data');
    let categorias = [];

    for (let i = 0; i < c.length; i++) {
        categorias.push(parseInt(c[i].id));
    }

    let filtro = {
        ano: document.getElementById('ano').value,
        ignorar_zerados: document.getElementById('ignorar_zerados').value,        
        ocultar_nomes: document.getElementById('ocultar_nomes').value,
        tipos_participante: tipos_participantes,
        categorias: categorias
    };

    $.ajax({
        url: "/Rp/Create",
        data: { __RequestVerificationToken: gettoken(), filtro: filtro },
        type: 'POST',
        dataType: 'json',
        beforeSend: function (XMLHttpRequest) {
            document.getElementById('corpo_rp').innerHTML = '<span class="text-info">Gerando relatório, aguarde...</span>';
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            document.getElementById('corpo_rp').innerHTML = '<span class="text-danger">Erro no processamento do relatório</span>';
        },
        success: function (data, textStatus, XMLHttpRequest) {
            rp = JSON.parse(data);
            if (textStatus == 'error') {
                document.getElementById('corpo_rp').innerHTML = '<span class="text-primary">error</span>';
            }

            if (textStatus == 'success') {
                if (rp.retorno.includes("Erro")) {
                    document.getElementById('corpo_rp').innerHTML = '<span class="text-danger">Erro no processamento do relatório</span>';

                    return;
                }

                let c = '<div class="table-responsive">';
                c += '<table class="table table-sm" id="table_part">';
                //c += '<caption>Relatório Participante Anual</caption>';
                c += '<thead>';      
                c += '<tr>'
                c += '<th style="text-align:center">Código</th>'
                c += '<th style="text-align:left;white-space:nowrap;">Participante</th>'
                c += '<th style="text-align:right">Janeiro</th>'
                c += '<th style="text-align:right">Fevereiro</th>'
                c += '<th style="text-align:right">Março</th>'
                c += '<th style="text-align:right">Abril</th>'
                c += '<th style="text-align:right">Maio</th>'
                c += '<th style="text-align:right">Junho</th>'
                c += '<th style="text-align:right">Julho</th>'
                c += '<th style="text-align:right">Agosto</th>'
                c += '<th style="text-align:right">Setembro</th>'
                c += '<th style="text-align:right">Outubro</th>'
                c += '<th style="text-align:right">Novembro</th>'
                c += '<th style="text-align:right">Dezembro</th>'
                c += '<th style="text-align:right">Total</th>'
                c += '</tr>'
                c += '</thead>';
                c += '<tbody>';
                for (let i = 0; i < rp.rps.length; i++) {
                    c += '<tr>';
                    c += '<td style="text-align:center">' + convertDoubleString(rp.rps[i].participante_codigo) + '</td>'
                    c += '<td style="text-align:left;white-space:nowrap;" class="rp_col_nome">' + convertDoubleString(rp.rps[i].participante_nome) + '</td>'
                    c += '<td style="text-align:right">' + convertDoubleString(rp.rps[i].jan) + '</td>'
                    c += '<td style="text-align:right">' + convertDoubleString(rp.rps[i].fev) + '</td>'
                    c += '<td style="text-align:right">' + convertDoubleString(rp.rps[i].marc) + '</td>'
                    c += '<td style="text-align:right">' + convertDoubleString(rp.rps[i].abr) + '</td>'
                    c += '<td style="text-align:right">' + convertDoubleString(rp.rps[i].mai) + '</td>'
                    c += '<td style="text-align:right">' + convertDoubleString(rp.rps[i].jun) + '</td>'
                    c += '<td style="text-align:right">' + convertDoubleString(rp.rps[i].jul) + '</td>'
                    c += '<td style="text-align:right">' + convertDoubleString(rp.rps[i].ago) + '</td>'
                    c += '<td style="text-align:right">' + convertDoubleString(rp.rps[i].sete) + '</td>'
                    c += '<td style="text-align:right">' + convertDoubleString(rp.rps[i].outu) + '</td>'
                    c += '<td style="text-align:right">' + convertDoubleString(rp.rps[i].nov) + '</td>'
                    c += '<td style="text-align:right">' + convertDoubleString(rp.rps[i].dez) + '</td>'
                    c += '<td style="text-align:right">' + convertDoubleString(rp.rps[i].total) + '</td>'
                    c += '</tr>';
                }
                c += '<tr style="background-color: #ffe699;">';
                c += '<th style="text-align:left" colspan="2">Total</th>'
                c += '<th style="text-align:right">' + convertDoubleString(rp.total.jan) + '</th>'
                c += '<th style="text-align:right">' + convertDoubleString(rp.total.fev) + '</th>'
                c += '<th style="text-align:right">' + convertDoubleString(rp.total.marc) + '</th>'
                c += '<th style="text-align:right">' + convertDoubleString(rp.total.abr) + '</th>'
                c += '<th style="text-align:right">' + convertDoubleString(rp.total.mai) + '</th>'
                c += '<th style="text-align:right">' + convertDoubleString(rp.total.jun) + '</th>'
                c += '<th style="text-align:right">' + convertDoubleString(rp.total.jul) + '</th>'
                c += '<th style="text-align:right">' + convertDoubleString(rp.total.ago) + '</th>'
                c += '<th style="text-align:right">' + convertDoubleString(rp.total.sete) + '</th>'
                c += '<th style="text-align:right">' + convertDoubleString(rp.total.outu) + '</th>'
                c += '<th style="text-align:right">' + convertDoubleString(rp.total.nov) + '</th>'
                c += '<th style="text-align:right">' + convertDoubleString(rp.total.dez) + '</th>'
                c += '<th style="text-align:right">' + convertDoubleString(rp.total.total) + '</th>'
                c += '</tr>';
                c += '</tbody>';
                c += '</table>';
                c += '</div>';
                //console.log(rfm);
                document.getElementById('corpo_rp').innerHTML = c;
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

function novoObjFCC(fcc_id, fcc_forma_pagamento_id, fcc_situacao, fcc_data_corte, fcc_data_vencimento, fcc_movimentos) {
    let fcc = {
        fcc_id: fcc_id,
        fcc_forma_pagamento_id: fcc_forma_pagamento_id,
        fcc_situacao: fcc_situacao,
        fcc_data_corte: fcc_data_corte,
        fcc_data_vencimento: fcc_data_vencimento,
        fcc_movimentos: fcc_movimentos,
    }

    return fcc;
}

function fatura_cartao_credito(contexto, cartao_id) {
    if (contexto == 'open') {
        let d = new Date();
        let f = novoObjFCC(0, cartao_id, '', '', '', []);       

        pesquisaFatura('open', ajustesFCC(f));
    }

    if (contexto == 'close') {
        document.getElementById('fcc_id').innerHTML = '';
        document.getElementById('fcc_referencia').innerHTML = '';
        document.getElementById('fcc_situacao').innerHTML = '';
        document.getElementById('fcc_data_corte').value = '';
        document.getElementById('fcc_data_vencimento').value = '';
        document.getElementById('table_movimentos_fatura_cartao').innerHTML = '';
        $('#fcc_modal').modal('hide');
    }

    if (contexto == 'next') {
        let f = novoObjFCC(fcc.fcc_id, fcc.fcc_forma_pagamento_id, fcc.fcc_situacao, fcc.fcc_data_corte, fcc.fcc_data_vencimento, []);
        pesquisaFatura('next', ajustesFCC(f));
    }

    if (contexto == 'previous') {
        let f = novoObjFCC(fcc.fcc_id, fcc.fcc_forma_pagamento_id, fcc.fcc_situacao, fcc.fcc_data_corte, fcc.fcc_data_vencimento, []);
        pesquisaFatura('previous', ajustesFCC(f));
    }

    if (contexto == 'fechar' || contexto == 'abrir') {
        $.ajax({
            url: "/CartaoCredito/fechar_abrir_cartao", //Parada
            data: { __RequestVerificationToken: gettoken(), fcc: ajustesFCC(fcc), contexto: contexto },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                if (contexto == 'fechar') {
                    document.getElementById('fcc_mensagem').innerHTML = '<span class="text-info">Fechando a fatura do cartão, aguarde...</span>';
                }
                if (contexto == 'abrir') {
                    document.getElementById('fcc_mensagem').innerHTML = '<span class="text-info">Abrindo a fatura do cartão, aguarde...</span>';
                }                                                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.getElementById('fcc_mensagem').innerHTML = '<span class="text-danger">Erro ao tentar ' + contexto + ' a fatura do cartão!</span>';                
            },
            success: function (data, textStatus, XMLHttpRequest) {
                r = JSON.parse(data);
                if (textStatus == 'error') {
                    document.getElementById('fcc_mensagem').innerHTML = '<span class="text-danger">Erro ao tentar ' + contexto + ' a fatura do cartão!</span>';
                }

                if (textStatus == 'success') {
                    if (r.includes('Erro')) {
                        document.getElementById('fcc_mensagem').innerHTML = '<span class="text-danger">' + r + '</span>';
                    }
                    if (r.includes('Sucesso') || r.includes('sucesso')) {
                        alert(r);
                        pesquisaFatura('reboot', fcc);
                    }
                }
            }
        });
    }


}

function fatura_cartao_credito_edit_datas(contexto, id) {
    if (contexto == 'open') {
        document.getElementById(id).disabled = false;
    }
    if (contexto == 'gravar') {
        let dc = moment(document.getElementById('fcc_data_corte').value, 'DD/MM/YYYY', 'pt', true);
        let dv = moment(document.getElementById('fcc_data_vencimento').value, 'DD/MM/YYYY', 'pt', true);

        if (!dc.isValid() || !dv.isValid()) {
            alert('Data inválida!');
            document.getElementById(id).disabled = true;
        } else {
            document.getElementById(id).disabled = true;
            pesquisaFatura('reboot', novoObjFCC(fcc.fcc_id, fcc.fcc_forma_pagamento_id, fcc.fcc_situacao, dc._i, dv._i, []));
        }

        if (fcc.fcc_movimentos.length == 0) {
            alert("Fatura não possui movimentos. Para alterar precisa ter pelo menos um movimento.");
            return;
        } else {
            //Ajax
            $.ajax({
                url: "/CartaoCredito/edit_datas_cartao",
                data: { __RequestVerificationToken: gettoken(), data_fechamento: dc._i, data_vencimento: dv._i, fcc_id: fcc.fcc_id },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    document.getElementById('fcc_mensagem').innerHTML = '<span class="text-info">Gravando dados, aguarde...</span>';
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    document.getElementById('fcc_mensagem').innerHTML = '<span class="text-danger">Erro ao se comunicar com o servidor</span>';
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    ret = JSON.parse(data);
                    if (textStatus == 'error') {
                        document.getElementById('fcc_mensagem').innerHTML = '<span class="text-danger">Erro ao se comunicar com o servidor</span>';
                    }

                    if (textStatus == 'success') {
                        if (XMLHttpRequest.responseJSON.includes('sucesso')) {
                            document.getElementById('fcc_mensagem').innerHTML = '<span class="text-success">' + ret + '</span>';
                        }

                        if (XMLHttpRequest.responseJSON.includes('Erro')) {
                            document.getElementById('fcc_mensagem').innerHTML = '<span class="text-danger">' + ret + '</span>';
                        }
                    }
                }
            });
        }
    }    
}

function parcelamentoFatura(contexto) {
    if (contexto == 'open') {
        modal_sobre_modal_open('parcelamento_fatura');
    }

    if (contexto == 'close') {
        $('#parcelamento_fatura').modal('hide');
    }
}

function pesquisaFatura(contexto, f) {
    f.fcc_id = 0; //Zerando o fcc_id, pois o servidor buscar a fcc pela competência.
    $.ajax({
        url: "/CartaoCredito/Details",
        data: { __RequestVerificationToken: gettoken(), contexto: contexto, fcc: f },
        type: 'POST',
        dataType: 'json',
        beforeSend: function (XMLHttpRequest) {
            //Limpando tabela
            document.getElementById('table_movimentos_fatura_cartao').innerHTML = '';
            document.getElementById('fcc_valor_total').innerHTML = '';
            document.getElementById('btn_fatura').value = '';
            document.getElementById('btn_fatura').innerHTML = '&nbsp';
            document.getElementById('fcc_mensagem').innerHTML = '<span class="text-info">Carregando dados da fatura do cartão de crédito, aguarde...</span>';
            //Estilo cabeçalho
            document.getElementById('fcc_header').style.backgroundColor = '';
            document.getElementById('fcc_header').style.color = 'white';            
            modal_sobre_modal_open('fcc_modal');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            document.getElementById('fcc_mensagem').innerHTML = '<span class="text-danger">Erro ao se comunicar com o servidor!!</span>';
        },
        success: function (data, textStatus, XMLHttpRequest) {
            fcc = JSON.parse(data);
            if (textStatus == 'error') {
                document.getElementById('fcc_mensagem').innerHTML = '<span class="text-danger">Erro ao se comunicar com o servidor!!</span>';
            }

            if (textStatus == 'success') {                
                document.getElementById('fcc_id').innerHTML = fcc.fcc_id;                
                document.getElementById('fcc_situacao').innerHTML = fcc.fcc_situacao;
                document.getElementById('fcc_data_corte').value = convertData_DataSimples(fcc.fcc_data_corte);
                document.getElementById('fcc_data_vencimento').value = convertData_DataSimples(fcc.fcc_data_vencimento);
                if (fcc.fcc_situacao == 'Fechada') {
                    document.getElementById('btn_fatura').value = 'abrir';
                    document.getElementById('btn_fatura').innerHTML = 'Reabrir Fatura';
                    //Estilos
                    document.getElementById('fcc_header').style.backgroundColor = '#90de59';
                    document.getElementById('fcc_header').style.color = 'white';
                } else {
                    document.getElementById('btn_fatura').value = 'fechar';
                    document.getElementById('btn_fatura').innerHTML = 'Fechar Fatura';
                    //Estilos
                    document.getElementById('fcc_header').style.backgroundColor = '#f29b18';
                    document.getElementById('fcc_header').style.color = 'white';
                }                                

                let valor_total = 0;
                let debito = 0;
                let credito = 0;

                //Limpando tabela
                document.getElementById('table_movimentos_fatura_cartao').innerHTML = '';
                //Atribuindo novos dados a tabela
                for (let i = 0; i < fcc.fcc_movimentos.length; i++) {

                    valor_total += fcc.fcc_movimentos[i].mcc_valor;
                    if (fcc.fcc_movimentos[i].mcc_movimento == 'D') {
                        debito += fcc.fcc_movimentos[i].mcc_valor;
                    } else {
                        credito += fcc.fcc_movimentos[i].mcc_valor;
                    }
                    
                    let t = '';
                    if (fcc.fcc_movimentos[i].mcc_movimento == 'C') {
                        t += '<tr style="color: green">';
                    } else {
                        t += '<tr>';
                    }                    
                    t += '<td style="text-align:center">';
                    if (fcc.fcc_movimentos[i].mcc_movimento == 'D' && (fcc.user.Role == 'adm' || fcc.user._permissoes.cartaoCreditoEdit)) {
                        t += '<span style="cursor:pointer;margin-right:7px;" onclick="fatura_cartao_credito_edit_competencia(\'edit_competencia\',\'' + fcc.fcc_movimentos[i].mcc_tipo + '\',\'' + fcc.fcc_movimentos[i].mcc_tipo_id + '\')"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-left-right" viewBox="0 0 16 16"><path fill - rule="evenodd" d = "M1 11.5a.5.5 0 0 0 .5.5h11.793l-3.147 3.146a.5.5 0 0 0 .708.708l4-4a.5.5 0 0 0 0-.708l-4-4a.5.5 0 0 0-.708.708L13.293 11H1.5a.5.5 0 0 0-.5.5zm14-7a.5.5 0 0 1-.5.5H2.707l3.147 3.146a.5.5 0 1 1-.708.708l-4-4a.5.5 0 0 1 0-.708l4-4a.5.5 0 1 1 .708.708L2.707 4H14.5a.5.5 0 0 1 .5.5z"/></svg></span>';                        
                    }
                    if (fcc.fcc_movimentos[i].mcc_movimento == 'D' && (fcc.user.Role == 'adm' || fcc.user._permissoes.operacaoEdit)) {                        
                        t += '<span style="cursor:pointer" onclick="Ajuste_parcelas_op(\'open\',\'' + fcc.fcc_movimentos[i].mcc_tipo + '\',\'' + fcc.fcc_movimentos[i].mcc_tipo_id + '\')"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-clipboard-data" viewBox="0 0 16 16"><path d = "M4 11a1 1 0 1 1 2 0v1a1 1 0 1 1-2 0v-1zm6-4a1 1 0 1 1 2 0v5a1 1 0 1 1-2 0V7zM7 9a1 1 0 0 1 2 0v3a1 1 0 1 1-2 0V9z" /><path d="M4 1.5H3a2 2 0 0 0-2 2V14a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V3.5a2 2 0 0 0-2-2h-1v1h1a1 1 0 0 1 1 1V14a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V3.5a1 1 0 0 1 1-1h1v-1z" /><path d="M9.5 1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5h3zm-3-1A1.5 1.5 0 0 0 5 1.5v1A1.5 1.5 0 0 0 6.5 4h3A1.5 1.5 0 0 0 11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3z" /></svg></span>';
                    }
                    if (fcc.fcc_movimentos[i].mcc_movimento == 'C' && (fcc.user.Role == 'adm' || fcc.user._permissoes.cartaoCreditoDelete)) {
                        t += '<svg style="color:red;cursor:pointer;" onclick="cartaoCreditoPagamento(\'delete\',\'' + fcc.fcc_movimentos[i].mcc_id +'\')" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16"><path d = "M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z" /><path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4L4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z" /></svg >';
                    }
                    t += '</td>';
                    t += '<td style="text-align:center">' + convertData_DataSimples(fcc.fcc_movimentos[i].mcc_data) + '</td>';
                    t += '<td style="text-align:center">' + fcc.fcc_movimentos[i].mcc_movimento + '</td>';
                    t += '<td style="text-align:left">' + fcc.fcc_movimentos[i].mcc_descricao + '</td>';
                    t += '<td style="text-align:right">' + convertDoubleString(fcc.fcc_movimentos[i].mcc_valor) + '</td>';
                    t += '</tr>';

                    $('#table_movimentos_fatura_cartao').append(t);
                }

                let saldo = debito - credito;

                document.getElementById('fcc_mensagem').innerHTML = '';
                //document.getElementById('fcc_valor_total').innerHTML = convertDoubleString(valor_total);
                document.getElementById('fcc_debito').innerHTML = convertDoubleString(debito);
                document.getElementById('fcc_credito').innerHTML = convertDoubleString(credito);
                document.getElementById('fcc_saldo').innerHTML = convertDoubleString(saldo);
                let mes_ano = fcc.fcc_data_corte.substr(5, 2) + '/' + fcc.fcc_data_corte.substr(0, 4);
                document.getElementById('fcc_competencia').innerHTML = ' ' + mes_ano;
                document.getElementById('fcc_referencia').innerHTML = mes_ano;

                //Permissões
                if (fcc.user.Role != 'adm' && fcc.user._permissoes.cartaoCreditoEdit == false) {
                    document.getElementById('btn_fatura').disabled = true;
                    document.getElementById('btn_fatura_pgto').disabled = true;
                } else {
                    document.getElementById('btn_fatura').disabled = false;
                    document.getElementById('btn_fatura_pgto').disabled = false;
                }
            }
        }
    });
}

function fatura_cartao_credito_edit_competencia(contexto, mcc_tipo, mcc_tipo_id) {
    if (contexto == 'edit_competencia') {
        if (fcc.fcc_situacao == 'Fechada') {
            alert('Fatura está fechada. Não permitido alocação de competência da parcela.');

            return;
        }
        document.getElementById('valida_comp').innerHTML = '';
        document.getElementById('mcc_tipo_selecionado').value = mcc_tipo;
        document.getElementById('mcc_tipo_id_selecionado').value = mcc_tipo_id;
        document.getElementById('competencia').value = document.getElementById('fcc_data_corte').value;
        modal_sobre_modal_open('alocacaoFcc');
    }

    if (contexto == 'next') {
        let c = moment(document.getElementById('competencia').value, 'DD/MM/YYYY', 'pt', true);
        let c_next = c.add(1, 'M').format('DD/MM/YYYY');
        document.getElementById('competencia').value = c_next;

        let v = moment(convertData_DataSimples(fcc.fcc_data_vencimento), 'DD/MM/YYYY', 'pt', true);
        let v_next = v.add(1, 'M').format('DD/MM/YYYY');
        document.getElementById('vencimento').value = v_next;
    }
    if (contexto == 'previous') {
        let c = moment(document.getElementById('competencia').value, 'DD/MM/YYYY', 'pt', true);
        let c_previous = c.add(-1, 'M').format('DD/MM/YYYY');
        document.getElementById('competencia').value = c_previous;

        let v = moment(convertData_DataSimples(fcc.fcc_data_vencimento), 'DD/MM/YYYY', 'pt', true);
        let v_next = v.add(-1, 'M').format('DD/MM/YYYY');
        document.getElementById('vencimento').value = v_next;
    }
    if (contexto == 'close') {
        $('#alocacaoFcc').modal('hide');
    }
    if (contexto == 'gravar') {
        let m_tipo = document.getElementById('mcc_tipo_selecionado').value;
        let m_tipo_id = document.getElementById('mcc_tipo_id_selecionado').value;
        let competencia = document.getElementById('competencia').value;

        let f = $.extend(true, {}, fcc);
        f.fcc_movimentos.splice(0, f.fcc_movimentos.length);
        for (let i = 0; i < fcc.fcc_movimentos.length; i++) {
            if (fcc.fcc_movimentos[i].mcc_tipo_id == m_tipo_id && fcc.fcc_movimentos[i].mcc_tipo == m_tipo) {
                let m = $.extend(true, {}, fcc.fcc_movimentos[i]);                
                f.fcc_movimentos.push(m);
                break;
            }
        }

        //Pegando datas de vencimento e competência atual
        f.fcc_data_corte = document.getElementById('competencia').value;
        f.fcc_data_vencimento = document.getElementById('vencimento').value;

        $.ajax({
            url: "/CartaoCredito/alocacaoCompetencia",
            data: { __RequestVerificationToken: gettoken(), competencia: competencia, fcc: ajustesFCC(f) },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                document.getElementById('valida_comp').innerHTML = '<span class="text-info">Gravando dados, aguarde...</span>';
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.getElementById('valida_comp').innerHTML = '<span class="text-danger">Erro ao se comunicar com o servidor</span>';
            },
            success: function (data, textStatus, XMLHttpRequest) {
                ret = JSON.parse(data);
                if (textStatus == 'error') {
                    document.getElementById('valida_comp').innerHTML = '<span class="text-danger">Erro ao se comunicar com o servidor</span>';
                }

                if (textStatus == 'success') {
                    if (XMLHttpRequest.responseJSON.includes('sucesso')) {
                        pesquisaFatura('reboot', novoObjFCC(f.fcc_id, f.fcc_forma_pagamento_id, f.fcc_situacao, document.getElementById('fcc_data_corte').value, document.getElementById('fcc_data_vencimento').value,[]));
                        document.getElementById('valida_comp').innerHTML = '';
                        $('#alocacaoFcc').modal('hide');
                    }

                    if (XMLHttpRequest.responseJSON.includes('Erro')) {
                        document.getElementById('valida_comp').innerHTML = '<span class="text-danger">' + XMLHttpRequest.responseJSON +'</span>';
                    }
                }
            }
        });
    }
}

function criarMovimentoFCC(mcc_id, mcc_fcc_id, mcc_tipo, mcc_tipo_id, mcc_movimento, mcc_data, mcc_descricao, mcc_valor) {
    let mcc = {
        mcc_id: mcc_id,
        mcc_fcc_id: mcc_fcc_id,
        mcc_tipo: mcc_tipo,
        mcc_tipo_id: mcc_tipo_id,
        mcc_movimento: mcc_movimento,
        mcc_data: mcc_data,
        mcc_descricao: mcc_descricao,
        mcc_valor: mcc_valor,
    };

    return mcc;
}

function ajustesFCC(fcc) {
    for (let i = 0; i < fcc.fcc_movimentos.length; i++) {
        fcc.fcc_movimentos[i].mcc_valor = convertDoubleString(fcc.fcc_movimentos[i].mcc_valor);
    }

    return fcc;
}

function Ajuste_parcelas_op(contexto, mcc_tipo, mcc_tipo_id) {
    if (contexto == 'open') {
        if (fcc.fcc_situacao == 'Fechada') {
            alert('Fatura está fechada. Não permitido ajuste no valor desta parcela.');

            return;
        }
        document.getElementById('valida_apc').innerHTML = '';
        $.ajax({
            url: "/CartaoCredito/AjusteParcelasOperacao",
            data: { __RequestVerificationToken: gettoken(), mcc_tipo: mcc_tipo, mcc_tipo_id: mcc_tipo_id },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                document.getElementById('apo_vlr_operacao').innerHTML = '';
                document.getElementById('apo_parcela_id').value = 0;
                document.getElementById('apo_parcela_id_edit').value = 0;
                document.getElementById('apo_tbody').innerHTML = '<span class="text-info">Buscando valores parcelas, aguarde...</span>';
                document.getElementById('canc_apo').disabled = false;
                document.getElementById('sub_apo').disabled = false;
                modal_sobre_modal_open('ajuste_p_op');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.getElementById('apo_tbody').innerHTML = '<span class="text-danger">Erro na busca dos dados da parcela!</span>';
            },
            success: function (data, textStatus, XMLHttpRequest) {
                apc = JSON.parse(data);
                console.log(apc);
                if (textStatus == 'error') {
                    document.getElementById('apo_tbody').innerHTML = '<span class="text-danger">Erro na busca dos dados da parcela!</span>';
                }

                if (textStatus == 'success') {
                    document.getElementById('apo_tbody').innerHTML = '';
                    document.getElementById('apo_vlr_operacao').innerHTML = fomat_numeroToStringLocal(apc.operacao_valor_total);
                    document.getElementById('apo_parcela_id').value = mcc_tipo_id;
                    let retencoes = 0;
                    for (let l = 0; l < apc.parcelas.length; l++) {
                        retencoes += apc.parcela_retencoes;
                    }

                    if (retencoes > 0) {
                        alert('As parcelas da operação possui retenções tributárias e não podem sofrer ajustes nos valores.');
                    } else {
                        for (let i = 0; i < apc.parcelas.length; i++) {
                            apc.parcelas[i].parcela_valor = fomat_numeroToStringLocal(apc.parcelas[i].parcela_valor);
                            apc.parcelas[i].parcela_data = convertData_DataSimples(apc.parcelas[i].parcela_data);
                            apc.parcelas[i].parcela_data_operacao = convertData_DataSimples(apc.parcelas[i].parcela_data_operacao);

                            let td = '<tr>';
                            td += '<td>';
                            if (apc.parcelas[i].parcela_baixas > 0) {
                                td += '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16" style="cursor:pointer" onclick="Ajuste_parcelas_op(\'edit_negado\',\'op_parcelas\',\'' + apc.parcelas[i].parcela_id +'\')"><path d = "M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" /><path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" /></svg >';
                            } else {
                                td += '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16" style="cursor:pointer" onclick="Ajuste_parcelas_op(\'edit\',\'op_parcelas\',\'' + apc.parcelas[i].parcela_id +'\')"><path d = "M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" /><path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" /></svg >';
                            }
                            td += '</td>';
                            td += '<td>' + apc.parcelas[i].op_parcela_numero + '/' + apc.parcelas[i].op_parcela_numero_total + '</td>';
                            td += '<td>' + apc.parcelas[i].parcela_data + '</td>';
                            td += '<td>' + apc.parcelas[i].parcela_valor + '</td>';
                            td += '</tr>';
                            $('#apo_tbody').append($(td));
                        }
                    }
                }
            }
        });
    }

    if (contexto == 'edit') {
        for (let i = 0; i < apc.parcelas.length; i++) {
            if (apc.parcelas[i].parcela_id == mcc_tipo_id) {
                if (apc.parcelas[i].parcela_fatura_status == 'Fechada') {
                    alert('A fatura selecionada está incluída em uma fatura de cartão com situação fechada e não pode ser alterada.');
                } else {
                    document.getElementById('apo_parcela_data').value = apc.parcelas[i].parcela_data;
                    document.getElementById('apo_parcela_valor').value = apc.parcelas[i].parcela_valor;
                    document.getElementById('apo_parcela_id_edit').value = apc.parcelas[i].parcela_id;
                }                
                break;
            }
        }
    }

    if (contexto == 'edit_salvar') {
        document.getElementById('apo_tbody').innerHTML = '';
        let p = document.getElementById('apo_parcela_id_edit').value;
        for (let i = 0; i < apc.parcelas.length; i++) {
            if (apc.parcelas[i].parcela_id == p) {
                //apc.parcelas[i].op_parcela_vencimento_alterado = document.getElementById('apo_parcela_data').value;
                apc.parcelas[i].parcela_valor = document.getElementById('apo_parcela_valor').value;

                document.getElementById('apo_parcela_data').value = '';
                document.getElementById('apo_parcela_valor').value = '';
            }

            let td = '<tr>';
            td += '<td>';
            if (apc.parcelas[i].parcela_baixas > 0) {
                td += '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16" style="cursor:pointer" onclick="Ajuste_parcelas_op(\'edit_negado\',\'op_parcelas\',\'' + apc.parcelas[i].parcela_id + '\')"><path d = "M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" /><path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" /></svg >';
            } else {
                td += '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16" style="cursor:pointer" onclick="Ajuste_parcelas_op(\'edit\',\'op_parcelas\',\'' + apc.parcelas[i].parcela_id + '\')"><path d = "M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" /><path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" /></svg >';
            }
            td += '</td>';
            td += '<td>' + apc.parcelas[i].op_parcela_numero + '/' + apc.parcelas[i].op_parcela_numero_total + '</td>';
            td += '<td>' + apc.parcelas[i].parcela_data + '</td>';
            td += '<td>' + apc.parcelas[i].parcela_valor + '</td>';
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
        for (let i = 0; i < apc.parcelas.length; i++) {
            vlr_total += convertStringDouble(apc.parcelas[i].parcela_valor);
        }

        vlr_total = vlr_total.toFixed(2);
        let op = apc.operacao_valor_total.toFixed(2);

        if (convertStringDouble(vlr_total) != convertStringDouble(op)) {
            alert('Valor das parcelas é diferente do valor total da operação. \nValor Total Operação: ' + fomat_numeroToStringLocal(apc.operacao_valor_total) + '\nValor Total das Parcelas: ' + fomat_numeroToStringLocal(vlr_total));
        } else {
            $.ajax({
                url: "/CartaoCredito/AjusteParcelasOperacaoGravar", //Parada
                data: { __RequestVerificationToken: gettoken(), apc: apc },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    document.getElementById('valida_apc').innerHTML = '';
                    document.getElementById('canc_apo').disabled = true;
                    document.getElementById('sub_apo').disabled = true;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('Erro no envio dos dados');
                    document.getElementById('canc_apo').disabled = false;
                    document.getElementById('sub_apo').disabled = false;
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    r = JSON.parse(data);
                    if (textStatus == 'error') {
                        alert('Erro no envio dos dados');
                        document.getElementById('canc_apo').disabled = false;
                        document.getElementById('sub_apo').disabled = false;
                    }

                    if (textStatus == 'success') {
                        if (r.length > 0) {
                            for (let i = 0; i < r.length; i++) {
                                $('#valida_apc').append(r[i]);
                            }
                        } else {
                            alert('Ajustes nas parcelas gravado com sucesso');
                            pesquisaFatura('reboot', fcc);
                            $('#ajuste_p_op').modal('hide');
                        }
                    }
                }
            });
        }
    }
}

function cartaoCreditoPagamento(contexto, id) {
    if (contexto == 'open') {
        document.getElementById('gravarPgto').disabled = false;
        let saldo = convertStringDouble(document.getElementById('fcc_saldo').innerHTML);
        if (saldo = 0) {
            alert('Fatura com saldo zero!');
        } else {
            document.getElementById('valorPgto').value = '';
            document.getElementById('valida_pagamento').innerHTML = '';
            modal_sobre_modal_open('PagamentoFcc');
        }
    }
    if (contexto == 'close') {
        document.getElementById('valorPgto').value = '';
        document.getElementById('valida_pagamento').innerHTML = '';
        $('#PagamentoFcc').modal('hide');
    }
    if (contexto == 'gravar') {
        let debito = convertStringDouble(document.getElementById('fcc_debito').innerHTML);
        let saldo = convertStringDouble(document.getElementById('fcc_saldo').innerHTML);
        let valorPgto = convertStringDouble(document.getElementById('valorPgto').value);        
        let dataPgto = moment(document.getElementById('dataPgto').value, 'DD/MM/YYYY', 'pt', true);
        let conta_corrente_Pgto = document.getElementById('conta_corrente_Pgto').value;

        if (valorPgto > debito || valorPgto > saldo || valorPgto < 0 || dataPgto.isValid() == false) {
            alert('Valor do pagamento não pode ser superior ao débito ou saldo da fatura!\n e/ou \n Valor do pagamento não pode ser inferior a zero!\n e/ou \n Data do pagamento inválida!');
        } else {
            $.ajax({
                url: "/CartaoCredito/PagamentoFatura",
                data: { __RequestVerificationToken: gettoken(), valorPgto: convertDoubleString(valorPgto), fcc: ajustesFCC(fcc), dataPgto: dataPgto._i, conta_corrente_Pgto: conta_corrente_Pgto },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    document.getElementById('valida_pagamento').innerHTML = '<span class="text-info">Gravando pagamento, aguarde...</span>';
                    document.getElementById('gravarPgto').disabled = true;                    
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    document.getElementById('valida_pagamento').innerHTML = '<span class="text-danger">Erro ao se comunicar com o servidor!</span>';
                    document.getElementById('gravarPgto').disabled = false;
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    r = JSON.parse(data);
                    if (textStatus == 'error') {
                        document.getElementById('valida_pagamento').innerHTML = '<span class="text-danger">Erro ao se comunicar com o servidor!</span>';
                        document.getElementById('gravarPgto').disabled = false;
                    }

                    if (textStatus == 'success') {
                        if (r.includes('Erro')) {
                            document.getElementById('gravarPgto').disabled = false;
                            document.getElementById('valida_pagamento').innerHTML = '<span class="text-danger">' + r + '</span>';
                            document.getElementById('gravarPgto').disabled = false;
                        }

                        if (r.includes('sucesso')) {
                            alert('Pagamento realizado com sucesso!');
                            document.getElementById('gravarPgto').disabled = false;
                            pesquisaFatura('reboot', novoObjFCC(fcc.fcc_id, fcc.fcc_forma_pagamento_id, fcc.fcc_situacao, document.getElementById('fcc_data_corte').value, document.getElementById('fcc_data_vencimento').value, []));
                            $('#PagamentoFcc').modal('hide');
                        }
                    }
                }
            });
        }
    }   

    if (contexto == 'valida') {
        document.getElementById('valida_pagamento').innerHTML = '';
        let debito = convertStringDouble(document.getElementById('fcc_debito').innerHTML);
        let saldo = convertStringDouble(document.getElementById('fcc_saldo').innerHTML);
        let valorPgto = convertStringDouble(document.getElementById('valorPgto').value);
        let dataPgto = moment(document.getElementById('dataPgto').value, 'DD/MM/YYYY', 'pt', true);

        if (!dataPgto.isValid()) {
            document.getElementById('valida_pagamento').innerHTML = '<span class="text-danger">Data do pagamento não é valida!</span>';
        }

        if (valorPgto > debito || valorPgto > saldo) {
            document.getElementById('valida_pagamento').innerHTML = '<span class="text-danger">Valor do pagamento não pode ser superior ao débito ou saldo da fatura!</span>';
        }

        if (valorPgto < 0) {
            document.getElementById('valida_pagamento').innerHTML = '<span class="text-danger">Valor do pagamento não pode ser inferior a zero!</span>';
        }
    }

    if (contexto == 'delete') {
        $.ajax({
            url: "/CartaoCredito/DeletePagamento",
            data: { __RequestVerificationToken: gettoken(), mcc_id: id },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                document.getElementById('fcc_mensagem').innerHTML = '<span class="text-info">Deletando pagamento, aguarde...</span>';                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.getElementById('fcc_mensagem').innerHTML = '<span class="text-danger">Erro ao se comunicar com o servidor!</span>';                
            },
            success: function (data, textStatus, XMLHttpRequest) {
                r = JSON.parse(data);
                if (textStatus == 'error') {
                    document.getElementById('fcc_mensagem').innerHTML = '<span class="text-danger">Erro ao se comunicar com o servidor!</span>';                    
                }

                if (textStatus == 'success') {
                    if (r.includes('Erro')) {                        
                        document.getElementById('fcc_mensagem').innerHTML = '<span class="text-danger">' + r + '</span>';                        
                    }

                    if (r.includes('sucesso')) {
                        alert('Pagamento excluído com sucesso!');
                        document.getElementById('gravarPgto').disabled = false;
                        pesquisaFatura('reboot', novoObjFCC(fcc.fcc_id, fcc.fcc_forma_pagamento_id, fcc.fcc_situacao, document.getElementById('fcc_data_corte').value, document.getElementById('fcc_data_vencimento').value, []));                        
                    }
                }
            }
        });
    }
}


function gerarContasCorrentes() {
    let tipo = ['Caixa','Banco'];
    $.ajax({
        url: "/ContaCorrente/gerarContasCorrentes_ajax",
        data: { __RequestVerificationToken: gettoken(), tipo: tipo },
        type: 'POST',
        dataType: 'json',
        beforeSend: function (XMLHttpRequest) {
            console.log('Gerando...');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(errorThrown);
            console.log(XMLHttpRequest);
            console.log(gettoken());
            console.log(tipo);
            alert("erro");
        },
        success: function (data, textStatus, XMLHttpRequest) {
            var results = JSON.parse(data);
            console.log(results);

            /*
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
            */
        }
    });
}

function rfm_btns(contexto) {
    if (contexto == 'next') {
        let c = moment(document.getElementById('data_inicio').value, 'DD/MM/YYYY', 'pt', true);
        let c_next = c.add(1, 'M').format('DD/MM/YYYY');
        document.getElementById('data_inicio').value = c_next;

        let v = moment(document.getElementById('data_fim').value, 'DD/MM/YYYY', 'pt', true);        
        document.getElementById('data_fim').value = v.add(1, 'M').endOf('month').format('DD/MM/YYYY');
    }
    if (contexto == 'previous') {
        let c = moment(document.getElementById('data_inicio').value, 'DD/MM/YYYY', 'pt', true);
        let c_next = c.add(-1, 'M').format('DD/MM/YYYY');
        document.getElementById('data_inicio').value = c_next;

        let v = moment(document.getElementById('data_fim').value, 'DD/MM/YYYY', 'pt', true);        
        document.getElementById('data_fim').value = v.add(-1, 'M').endOf('month').format('DD/MM/YYYY');
    }
}

function gestao_tipo_participante(id, vlr, contexto) {
    document.getElementById('pt_index').innerHTML = '';
    //let btn_pt_gravar = document.getElementById('btn_pt_gravar').value;

    if (contexto == 'open' || contexto == 'reboot') {
        $.ajax({
            url: "/Participante_tipo/Index",
            data: { __RequestVerificationToken: gettoken() },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {                                
                if (contexto == 'open') {
                    document.getElementById('pt_index').innerHTML = '<span class="text-info">Buscando tipos de participante, aguarde...</span>';
                    modal_sobre_modal_open('p_tipo_modal');
                }

                if (contexto == 'reboot') {
                    document.getElementById('pt_index').innerHTML = '<span class="text-info">Atualizando lista de tipos de participante, aguarde...</span>';
                }
                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.getElementById('pt_index').innerHTML = '<span class="text-danger">Erro na busca dos dados!</span>';
            },
            success: function (data, textStatus, XMLHttpRequest) {
                let pt = JSON.parse(data);                
                if (textStatus == 'error') {
                    document.getElementById('pt_index').innerHTML = '<span class="text-danger">Erro na busca dos dados!</span>';
                }

                if (textStatus == 'success') {
                    document.getElementById('pt_index').innerHTML = '';

                    let c = '<div class="table-responsive">';
                    c += '<table class="table table-sm" id="table_pt">';
                    c += '<caption>Clique no nome para selecionar</caption>';
                    c += '<thead>';
                    c += '<tr><th style="text-align:left">Nome</th><th style="text-align:right">Ações</th></tr>';
                    c += '</thead>';
                    c += '<tbody>';
                    for (let i = 0; i < pt.length; i++) {
                        c += '<tr>';
                        c += '<td style="text-align:left;cursor:pointer;" id="' + pt[i].pt_id + '" onclick="gestao_tipo_participante(\'' + pt[i].pt_id + '\', \'' + pt[i].pt_nome + '\',\'select\')">' + pt[i].pt_nome + '</td>';
                        c += '<td style="text-align:right">';
                        c += '<span style="cursor:pointer;"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16" onclick="gestao_tipo_participante(\'' + pt[i].pt_id + '\', \'' + pt[i].pt_nome + '\',\'edit\')"><path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" /><path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" /></svg></span>';
                        c += '<span style="cursor:pointer;"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16" onclick="gestao_tipo_participante(\'' + pt[i].pt_id + '\', \'' + pt[i].pt_nome + '\',\'delete\')"><path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z" /><path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z" /></svg></span>';
                        c += '</td>';
                        c += '</tr>';
                    }
                    c += '</tbody>';
                    c += '</table>';
                    c += '</div>';
                    document.getElementById('pt_index').innerHTML = c;
                }
            }
        });

        return
    }

    if (contexto == 'close') {
        $("#p_tipo_modal").modal('hide');
        return
    }

    if (contexto == 'close_cofirme') {
        $("#p_tipo_confirm_confirm_modal").modal('hide');
        return
    }

    if (contexto == 'gravar') {
        let pt_nome = document.getElementById('pt_nome').value;

        if (pt_nome.length == 0 || pt_nome)
            if (pt_nome.length == 0 || pt_nome == null || pt_nome == "") {
            alert('Preenche o nome para o tipo de participante!');
        }else {
            $.ajax({
                url: "/Participante_tipo/Create",
                data: { __RequestVerificationToken: gettoken(), pt_nome: pt_nome },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    document.getElementById('pt_index').innerHTML = '<span class="text-info">Gravando tipo de participante, aguarde...</span>';                    
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    document.getElementById('pt_index').innerHTML = '<span class="text-danger">Erro ao gravar o tipo de participante!</span>';
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    let pt = JSON.parse(data);                    
                    if (textStatus == 'error') {
                        document.getElementById('pt_index').innerHTML = '<span class="text-danger">Erro ao gravar o tipo de participante!</span>';
                    }

                    if (textStatus == 'success') {

                        if (pt.includes('Erro')) {
                            document.getElementById('pt_index').innerHTML = '<span class="text-danger">' + pt + '</span>';
                        }

                        if (pt.includes('sucesso')) {
                            document.getElementById('pt_nome').value = '';
                            document.getElementById('pt_nome').focus();
                            alert('Tipo de participante gravado com sucesso!');                            
                            gestao_tipo_participante('','','reboot');
                        }
                    }
                }
            });
            }
        return;
    }

    if (contexto == 'edit') {

        let d = '<div class="row">';
        d += '<div class="col-12">';
        d += '<label class="control-label">Nome</label>';
        d += '<div class="input-group mb-3">';
        d += '<input type="text" class="form-control" aria-label="Gravar" value="' + vlr + '" aria-describedby="basic-addon15" id="pt_nome_edit">';
        d += '<input type="hidden" id="pt_id_edit" value="' + id + '" />';
        d += '<div class="input-group-append">';
        d += '<button class="btn btn-outline-secondary" type="button" onclick="gestao_tipo_participante(this.id, this.value,\'edit_gravar\')">Gravar</button>';
        d += '</div>';
        d += '</div>';
        d += '</div>';
        d += '</div>';

        document.getElementById('p_tipo_confirm_label').innerHTML = 'Alterar tipo de participante';
        document.getElementById('p_tipo_confirm_body_conteudo').innerHTML = d;
        modal_sobre_modal_open('p_tipo_confirm_confirm_modal');

        return
    }

    if (contexto == 'edit_gravar') {
        let pt_nome = document.getElementById('pt_nome_edit').value;
        let pt_id = document.getElementById('pt_id_edit').value;

        if (pt_nome.length == 0 || pt_nome)
            if (pt_nome.length == 0 || pt_nome == null || pt_nome == "") {
                alert('Preenche o nome para o tipo de participante!');
            } else {
                $.ajax({
                    url: "/Participante_tipo/Edit",
                    data: { __RequestVerificationToken: gettoken(), id: pt_id, pt_nome: pt_nome },
                    type: 'POST',
                    dataType: 'json',
                    beforeSend: function (XMLHttpRequest) {
                        document.getElementById('p_tipo_confirm_body_msg').innerHTML = '<span class="text-info">Gravando tipo de participante, aguarde...</span>';
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        document.getElementById('p_tipo_confirm_body_msg').innerHTML = '<span class="text-danger">Erro ao gravar o tipo de participante!</span>';
                    },
                    success: function (data, textStatus, XMLHttpRequest) {
                        let pt_2 = JSON.parse(data);
                        if (textStatus == 'error') {
                            document.getElementById('p_tipo_confirm_body_msg').innerHTML = '<span class="text-danger">Erro ao gravar o tipo de participante!</span>';
                        }

                        if (textStatus == 'success') {

                            if (pt_2.includes('Erro')) {
                                document.getElementById('p_tipo_confirm_body_msg').innerHTML = '<span class="text-danger">' + pt_2 + '</span>';
                            }

                            if (pt_2.includes('sucesso')) {
                                document.getElementById('p_tipo_confirm_body_conteudo').innerHTML = '';
                                document.getElementById('p_tipo_confirm_body_msg').innerHTML = '';                                
                                alert(pt_2);
                                $("#p_tipo_confirm_confirm_modal").modal('hide');
                                gestao_tipo_participante('', '', 'reboot');
                            }
                        }
                    }
                });
            }
        return;
    }

    if (contexto == 'delete') {

        let d = '<p>Confirma a exclusão do tipo de participante: ' + vlr  + '</p>';
        d += '<button type="button" class="btn btn-danger" onclick="gestao_tipo_participante(\'' + id + '\',\'' + vlr + '\',\'delete_confirma\')">Confirmo</button>';
        
        document.getElementById('p_tipo_confirm_label').innerHTML = 'Exclusão de tipo de participante';
        document.getElementById('p_tipo_confirm_body_conteudo').innerHTML = d;
        modal_sobre_modal_open('p_tipo_confirm_confirm_modal');

        return
    }

    if (contexto == 'delete_confirma') {
        $.ajax({
            url: "/Participante_tipo/Delete",
            data: { __RequestVerificationToken: gettoken(), id: id },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                document.getElementById('p_tipo_confirm_body_msg').innerHTML = '<span class="text-info">Excluindo tipo de participante, aguarde...</span>';
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.getElementById('p_tipo_confirm_body_msg').innerHTML = '<span class="text-danger">Erro ao excluir o tipo de participante!</span>';
            },
            success: function (data, textStatus, XMLHttpRequest) {
                let pt_2 = JSON.parse(data);
                if (textStatus == 'error') {
                    document.getElementById('p_tipo_confirm_body_msg').innerHTML = '<span class="text-danger">Erro ao excluir o tipo de participante!</span>';
                }

                if (textStatus == 'success') {

                    if (pt_2.includes('Erro')) {
                        document.getElementById('p_tipo_confirm_body_msg').innerHTML = '<span class="text-danger">' + pt_2 + '</span>';
                    }

                    if (pt_2.includes('sucesso')) {
                        document.getElementById('p_tipo_confirm_body_conteudo').innerHTML = '';
                        document.getElementById('p_tipo_confirm_body_msg').innerHTML = '';
                        alert(pt_2);
                        $("#p_tipo_confirm_confirm_modal").modal('hide');
                        gestao_tipo_participante('', '', 'reboot');
                    }
                }
            }
        });

        return
    }

    if (contexto == 'select') {
        document.getElementById('participante_tipo').value = id;
        document.getElementById('participante_tipo_nome').value = vlr;
        $("#p_tipo_modal").modal('hide');
        return
    }
}

function gerarPDF(id) {
    if (document.getElementById(id) == null) {
        alert('Favor gerar primeiramente o relatório para ser impresso!');

        return;
    }

    var minhaTabela = document.getElementById(id).outerHTML;
    var style = "<style>";
    style = style + "table {width: 100%;font: 20px Calibri;}";
    style = style + "table, th, td {border: solid 1px #DDD; border-collapse: collapse;";
    style = style + "padding: 2px 3px;text-align: center;}";
    style = style + "</style>";
    // CRIA UM OBJETO WINDOW
    var win = window.open('', '', 'height=700,width=700');
    win.document.write('<html><head>');
    win.document.write('<title></title>');   // <title> CABEÇALHO DO PDF.
    win.document.write(style);                                     // INCLUI UM ESTILO NA TAB HEAD
    win.document.write('</head>');
    win.document.write('<body>');
    win.document.write(minhaTabela);                          // O CONTEUDO DA TABELA DENTRO DA TAG BODY
    win.document.write('</body></html>');
    win.document.close(); 	                                         // FECHA A JANELA
    win.print(); 
}

function gerarExcel() {
    
}

function lead_atendente(contexto, id) {
    if (contexto == 'index') {
        $.ajax({
            url: "/Lead_atendentes/Index",
            data: { __RequestVerificationToken: gettoken() },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                document.getElementById('lead_atendente_msg_2').innerHTML = '<span class="text-info">Buscando cadastro de atendentes, aguarde...</span>';
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.getElementById('lead_atendente_msg_2').innerHTML = '<span class="text-danger">Erro na busca!!!</span>';
                console.log(textStatus);
                console.log(errorThrown);
            },
            success: function (data, textStatus, XMLHttpRequest) {                
                let retorno = JSON.parse(data);                
                if (textStatus == 'error') {
                    document.getElementById('lead_atendente_msg_2').innerHTML = '<span class="text-danger">Erro na busca!!!</span>';
                    console.log(textStatus);
                    console.log(errorThrown);
                }

                if (textStatus == 'success') {

                    if (retorno.status.includes('Erro')) {
                        document.getElementById('lead_atendente_msg_2').innerHTML = '<span class="text-danger">' + retorno.status + '</span>';
                    }

                    if (retorno.status.includes('sucesso')) {
                        let c = '<div class="table-responsive">';
                        c += '<table class="table table-sm" id="table_lead_atendentes">';                        
                        c += '<thead>';
                        c += '<tr>'
                        c += '<th style = "text-align:left" >Nome</th>';
                        c += '<th style = "text-align:center" >Celular</th>';
                        c += '<th style = "text-align:center" >E-mail</th>'; 
                        c += '<th style = "text-align:center" >Fila Ozaki</th>';
                        c += '<th style = "text-align:center" >Fila Contadorcomvocê</th>';
                        c += '<th>Ações</th>';
                        c += '</tr>'
                        c += '</thead>';
                        c += '<tbody>';
                        for (let i = 0; i < retorno.atendentes.length; i++) {
                            c += '<tr>';
                            c += '<td style = "text-align:left">' + retorno.atendentes[i].lead_atendentes_nome + '</td>';
                            c += '<td>' + retorno.atendentes[i].lead_atendentes_celular + '</td>';
                            c += '<td>' + retorno.atendentes[i].lead_atendentes_email + '</td>';
                            if (retorno.atendentes[i].lead_atendentes_atende_fila_um) {
                                c += '<td>' + retorno.atendentes[i].lead_atendentes_filaUm + '</td>';
                            } else {
                                c += '<td>-</td>';
                            }
                            if (retorno.atendentes[i].lead_atendentes_atende_fila_dois) {
                                c += '<td>' + retorno.atendentes[i].lead_atendentes_filaDois + '</td>';
                            } else {
                                c += '<td>-</td>';
                            }                           
                            c += '<td style="text-align:right">';
                            c += '<span style="cursor:pointer;"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16" onclick="lead_atendente(\'edit_open\',\'' + retorno.atendentes[i].lead_atendentes_id +'\')"><path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" /><path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" /></svg></span>';
                            c += '<span style="cursor:pointer;"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16" onclick="lead_atendente(\'delete_open\',\'' + retorno.atendentes[i].lead_atendentes_id +'\')"><path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z" /><path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z" /></svg></span>';
                            c += '</td>';
                            c += '</tr>';
                        }
                        c += '</tbody>';
                        c += '</table>';
                        c += '</div>';
                        document.getElementById('lead_atendente_msg_2').innerHTML = '';
                        document.getElementById('conteudo_lead').innerHTML = c;
                    }
                }
            }
        });
    }

    if (contexto == 'create_open') {
        document.getElementById('lead_atendentes_nome').value = '';
        document.getElementById('lead_atendentes_celular').value = '';
        document.getElementById('lead_atendentes_email').value = '';
        document.getElementById('lead_atendentes_atende_fila_um').value = false;
        document.getElementById('lead_atendentes_atende_fila_dois').value = false;
        document.getElementById('modal_lead_atendente_titulo').innerHTML = 'Inserir Novo Atendente'; 
        document.getElementById('modal_lead_atendente_rodape').innerHTML = '<button type="button" class="btn btn-secondary" onclick="lead_atendente(\'cancel\')">Cancelar</button><button type="button" class="btn btn-info" id="lead_atendente_gravar" onclick="lead_atendente(\'Create\')">Gravar</button>'; 
        modal_sobre_modal_open('modal_lead_atendente');
        document.getElementById('lead_atendentes_nome').focus();
    }

    if (contexto == 'cancel') {
        if (document.getElementById('lead_atendentes_nome')) {
            document.getElementById('lead_atendentes_nome').value = '';
        }
        if (document.getElementById('lead_atendentes_celular')) {
            document.getElementById('lead_atendentes_celular').value = '';
        }
        if (document.getElementById('lead_atendentes_email')) {
            document.getElementById('lead_atendentes_email').value = '';
        }
        if (document.getElementById('lead_atendentes_atende_fila_um')) {
            document.getElementById('lead_atendentes_atende_fila_um').checked = false;
        }
        if (document.getElementById('lead_atendentes_atende_fila_dois')) {
            document.getElementById('lead_atendentes_atende_fila_dois').checked = false;
        }
        if (document.getElementById('modal_lead_atendente_titulo')) {
            document.getElementById('modal_lead_atendente_titulo').innerHTML = '';
        }

        if ('lead_atendente_msg_2') {
            document.getElementById('lead_atendente_msg_2').innerHTML = '';
        }

        if (document.getElementById('modal_lead_atendente')) {
            $("#modal_lead_atendente").modal('hide');
        }

        if (document.getElementById('modal_delete_atendente')) {
            $("#modal_delete_atendente").modal('hide');
        }        
        
    }

    if (contexto == 'Create' || contexto == 'Edit') {
        let lead_atendentes_id = document.getElementById('lead_atendentes_id').value;
        let nome = document.getElementById('lead_atendentes_nome').value;
        let celular = document.getElementById('lead_atendentes_celular').value.replaceAll('(','').replaceAll(')','').replaceAll('-','').replaceAll(' ','');
        let e_mail = document.getElementById('lead_atendentes_email').value;
        let fila_um = document.getElementById('lead_atendentes_atende_fila_um').value;
        let fila_dois = document.getElementById('lead_atendentes_atende_fila_dois').value;

        let erros = [];

        if (nome.length < 5 || nome == null || nome == '') {
            erros.push('Nome não pode ser vazio ou menor que cinco caracteres.');
        }

        if (celular.length != 11) {
            erros.push('Celular não pode ser vazio ou estar diferente da máscara (##) #.####-####.');
        }

        var regex = new RegExp('([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\.[a-zA-Z0-9._-]+)');
        if (regex.test(e_mail) == false) {
            erros.push('E-mail inváliso.');
        }

        if (erros.length > 0) {
            let mensagem = "";
            for (let i = 0; i < erros.length; i++) {
                mensagem +=  erros[i] + '\n';
            }
            alert(mensagem);
        } else {
            $.ajax({
                url: "/Lead_atendentes/" + contexto,
                data: {
                    __RequestVerificationToken: gettoken(),
                    lead_atendentes_nome: nome,
                    lead_atendentes_celular: celular,
                    lead_atendentes_email: e_mail,
                    lead_atendentes_atende_fila_um: fila_um,
                    lead_atendentes_atende_fila_dois: fila_dois,
                    lead_atendentes_id: lead_atendentes_id
                },
                type: 'POST',
                dataType: 'json',
                beforeSend: function (XMLHttpRequest) {
                    document.getElementById('lead_atendente_msg').innerHTML = '<span class="text-info">Gravando dados do atendente, aguarde...</span>';
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (contexto == 'Create') {
                        document.getElementById('lead_atendente_msg').innerHTML = '<span class="text-danger">Erro ao cadatrar o atendente!!</span>';
                    }
                    if (contexto == 'Edit') {
                        document.getElementById('lead_atendente_msg').innerHTML = '<span class="text-danger">Erro ao alterar o atendente!!</span>';
                    }                                        
                },
                success: function (data, textStatus, XMLHttpRequest) {
                    let retorno = JSON.parse(data);                    
                    if (textStatus == 'error') {
                        if (contexto == 'Create') {
                            document.getElementById('lead_atendente_msg').innerHTML = '<span class="text-danger">Erro ao cadatrar o atendente!!</span>';
                        }
                        if (contexto == 'Edit') {
                            document.getElementById('lead_atendente_msg').innerHTML = '<span class="text-danger">Erro ao alterar o atendente!!</span>';
                        }
                    }

                    if (textStatus == 'success') {

                        if (retorno.includes('Erro')) {
                            document.getElementById('lead_atendente_msg').innerHTML = '<span class="text-danger">' + retorno + '</span>';
                        }

                        if (retorno.includes('sucesso')) {
                            alert(retorno);
                            document.getElementById('lead_atendente_msg').innerHTML = '';
                            lead_atendente('index');
                            lead_atendente('cancel');
                        }
                    }
                }
            });
        }
    }

    if (contexto == 'busca' || contexto == 'edit_open' || contexto == 'delete_open') {
        $.ajax({
            url: "/Lead_atendentes/Busca",
            data: {
                __RequestVerificationToken: gettoken(),
                id: id
            },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                document.getElementById('lead_atendente_msg_2').innerHTML = '<span class="text-info">Buscando dados do atendente, aguarde...</span>';
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.getElementById('lead_atendente_msg_2').innerHTML = '<span class="text-danger">Erro ao buscar os dados do atendente!!</span>';
            },
            success: function (data, textStatus, XMLHttpRequest) {
                let retorno = JSON.parse(data);                
                if (textStatus == 'error') {
                    document.getElementById('lead_atendente_msg_2').innerHTML = '<span class="text-danger">Erro ao buscar os dados do atendente!!</span>';
                }

                if (textStatus == 'success') {

                    if (contexto == 'edit_open') {
                        if (retorno.lead_atendentes_id == 0) {
                            document.getElementById('lead_atendente_msg_2').innerHTML = '<span class="text-danger">Erro ao buscar os dados do atendente!!</span>';
                        } else {
                            document.getElementById('lead_atendentes_id').value = retorno.lead_atendentes_id;
                            document.getElementById('lead_atendentes_nome').value = retorno.lead_atendentes_nome;
                            document.getElementById('lead_atendentes_celular').value = retorno.lead_atendentes_celular;
                            document.getElementById('lead_atendentes_email').value = retorno.lead_atendentes_email;
                            document.getElementById('lead_atendentes_atende_fila_um').value = retorno.lead_atendentes_atende_fila_um;
                            document.getElementById('lead_atendentes_atende_fila_dois').value = retorno.lead_atendentes_atende_fila_dois;
                            document.getElementById('lead_atendentes_atende_fila_um').checked = retorno.lead_atendentes_atende_fila_um;
                            document.getElementById('lead_atendentes_atende_fila_dois').checked = retorno.lead_atendentes_atende_fila_dois;
                            document.getElementById('modal_lead_atendente_titulo').innerHTML = 'Alterar Atendente: ' + retorno.lead_atendentes_nome;
                            document.getElementById('modal_lead_atendente_rodape').innerHTML = '<button type="button" class="btn btn-secondary" onclick="lead_atendente(\'cancel\')">Cancelar</button><button type="button" class="btn btn-info" id="lead_atendente_gravar" onclick="lead_atendente(\'Edit\')">Gravar</button>';
                            document.getElementById('lead_atendentes_nome').focus();
                            document.getElementById('lead_atendente_msg_2').innerHTML = '';
                            modal_sobre_modal_open('modal_lead_atendente');
                        }
                    }

                    if (contexto == 'delete_open') {
                        if (retorno.lead_atendentes_id == 0) {
                            document.getElementById('lead_atendente_msg_2').innerHTML = '<span class="text-danger">Erro ao buscar os dados do atendente!!</span>';
                        } else {
                            document.getElementById('modal_delete_atendente_body').innerHTML = '<p>Deseja realmente excluir o atendente: ' + retorno.lead_atendentes_nome + '?</p>';
                            document.getElementById('lead_atendentes_id_delete').value = retorno.lead_atendentes_id;
                            modal_sobre_modal_open('modal_delete_atendente');
                        }
                    }
                }
            }
        });
    }   

    if (contexto == 'delete') {
        let lead_atendentes_id = document.getElementById('lead_atendentes_id_delete').value;

        $.ajax({
            url: "/Lead_atendentes/Delete",
            data: {
                __RequestVerificationToken: gettoken(),                
                lead_atendentes_id: lead_atendentes_id
            },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('Erro ao tentar excluir o atendente!!');
            },
            success: function (data, textStatus, XMLHttpRequest) {
                let retorno = JSON.parse(data);
                if (textStatus == 'error') {
                    alert('Erro ao tentar excluir o atendente!!');
                }

                if (textStatus == 'success') {

                    if (retorno.includes('Erro') || retorno.includes('não pode ser excluído')) {
                        alert(retorno);
                        lead_atendente('cancel');
                    }

                    if (retorno.includes('sucesso')) {
                        alert(retorno);                        
                        lead_atendente('index');
                        lead_atendente('cancel');
                    }
                }
            }
        });
    }
}

function lead(contexto, id, tipo) {
    if (contexto == 'index') {
        $.ajax({
            url: "/Lead/Index",
            data: { __RequestVerificationToken: gettoken() },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                document.getElementById('lead_msg').innerHTML = '<span class="text-info">Carregando lead´s, aguarde...</span>';
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.getElementById('lead_msg').innerHTML = '<span class="text-danger">Erro na busca!!!</span>';
                console.log(textStatus);
                console.log(errorThrown);
            },
            success: function (data, textStatus, XMLHttpRequest) {
                let retorno = JSON.parse(data);
                if (textStatus == 'error') {
                    document.getElementById('lead_msg').innerHTML = '<span class="text-danger">Erro na busca!!!</span>';
                    console.log(textStatus);
                    console.log(errorThrown);
                }

                if (textStatus == 'success') {

                    if (retorno.status.includes('Erro')) {
                        document.getElementById('lead_msg').innerHTML = '<span class="text-danger">' + retorno.status + '</span>';
                    }

                    if (retorno.status.includes('Sucesso')) {
                        let c = '<input type="text" id="myInput_lead" value="" placeholder="Pesquisar na tabela" class="form-control form-control-sm pesquisa" style="max-width:100%;margin-bottom: 20px;" />';
                        c += '<div class="table-responsive">';
                        c += '<table class="table table-sm" id="table_lead">';
                        c += '<thead>';
                        c += '<tr>'
                        c += '<th style="text-align:center"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="#ff4500" class="bi bi-envelope" viewBox="0 0 16 16"><path d = "M0 4a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2V4zm2-1a1 1 0 0 0-1 1v.217l7 4.2 7-4.2V4a1 1 0 0 0-1-1H2zm13 2.383-4.758 2.855L15 11.114v-5.73zm-.034 6.878L9.271 8.82 8 9.583 6.728 8.82l-5.694 3.44A1 1 0 0 0 2 13h12a1 1 0 0 0 .966-.739zM1 11.114l4.758-2.876L1 5.383v5.73z"/></svg></th>';
                        c += '<th style="text-align:left" >Nome</th>';
                        c += '<th style="text-align:center" >Celular</th>';
                        c += '<th style="text-align:center" >E-mail</th>';
                        c += '<th style="text-align:center" >Origem</th>';
                        c += '<th style="text-align:center" >Tipo</th>';
                        c += '<th style="text-align:center" >Situação</th>';
                        c += '<th style="text-align:center" >Atendente</th>';
                        c += '<th style="text-align:right">Ações</th>';
                        c += '</tr>'
                        c += '</thead>';
                        c += '<tbody id="myTable_lead">';
                        if (retorno.leads.length == 0) {
                            c += '<tr><td colspan="8">Nenhum lead cadastrado</td></tr>';
                        } else {
                            for (let i = 0; i < retorno.leads.length; i++) {
                                c += '<tr>';
                                c += '<td style="color: #ff4500; font-weight:bold"><div style="background-color: bisque;border-radius: 50%;cursor: pointer;white-space: nowrap;">' + retorno.leads[i].lead_contato_nao_lida + '</div></td>';
                                c += '<td style = "text-align:left">' + retorno.leads[i].lead_nome + '</td>';
                                c += '<td>' + retorno.leads[i].lead_celular + '</td>';
                                c += '<td>' + retorno.leads[i].lead_email + '</td>';
                                c += '<td>' + retorno.leads[i].lead_site_origem + '</td>';
                                c += '<td>' + retorno.leads[i].lead_tipo + '</td>';
                                c += '<td>' + retorno.leads[i].lead_situacao + '</td>';
                                c += '<td>' + retorno.leads[i].lead_atendentes_nome + '</td>';                                
                                c += '<td style="text-align:right;white-space:nowrap;">';
                                c += '<span style="cursor:pointer;margin-left:5px;"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="#ff4500" class="bi bi-cash-coin" viewBox="0 0 16 16">< path fill - rule="evenodd" d = "M11 15a4 4 0 1 0 0-8 4 4 0 0 0 0 8zm5-4a5 5 0 1 1-10 0 5 5 0 0 1 10 0z" /><path d="M9.438 11.944c.047.596.518 1.06 1.363 1.116v.44h.375v-.443c.875-.061 1.386-.529 1.386-1.207 0-.618-.39-.936-1.09-1.1l-.296-.07v-1.2c.376.043.614.248.671.532h.658c-.047-.575-.54-1.024-1.329-1.073V8.5h-.375v.45c-.747.073-1.255.522-1.255 1.158 0 .562.378.92 1.007 1.066l.248.061v1.272c-.384-.058-.639-.27-.696-.563h-.668zm1.36-1.354c-.369-.085-.569-.26-.569-.522 0-.294.216-.514.572-.578v1.1h-.003zm.432.746c.449.104.655.272.655.569 0 .339-.257.571-.709.614v-1.195l.054.012z" /><path d="M1 0a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h4.083c.058-.344.145-.678.258-1H3a2 2 0 0 0-2-2V3a2 2 0 0 0 2-2h10a2 2 0 0 0 2 2v3.528c.38.34.717.728 1 1.154V1a1 1 0 0 0-1-1H1z" /><path d="M9.998 5.083 10 5a2 2 0 1 0-3.132 1.65 5.982 5.982 0 0 1 3.13-1.567z"/></svg></span>';
                                c += '<span style="cursor:pointer;margin-left:5px;"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="#ff4500" class="bi bi-pencil-square" viewBox="0 0 16 16" onclick="lead_atendente(\'edit_open\',\'' + retorno.leads[i].lead_id + '\')"><path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" /><path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" /></svg></span>';
                                c += '<span style="cursor:pointer;margin-left:5px;"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="#ff4500" class="bi bi-trash" viewBox="0 0 16 16" onclick="lead_atendente(\'delete_open\',\'' + retorno.leads[i].lead_id + '\')"><path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z" /><path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z" /></svg></span>';
                                c += '</td>';
                                c += '</tr>';
                            }
                        }                        
                        c += '</tbody>';
                        c += '</table>';
                        c += '</div>';
                        document.getElementById('lead_msg').innerHTML = '';
                        document.getElementById('lead_body').innerHTML = c;

                        $("#myInput_lead").on("keyup", function () {
                            var value = $(this).val().toLowerCase();
                            $("#myTable_lead tr").filter(function () {
                                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                            });
                        });
                    }
                }
            }
        });
    }

    if (contexto == 'create') {                
        let atendente_id = 0;
        let lead_nome = '';
        let lead_celular = '';
        let lead_email = '';
        let lead_tipo = '';
        let lead_situacao = '';
        let lead_contato_tipo = '';
        let lead_contato_msg = '';
        let lead_plano_selecionado = '';

        if (tipo == 'site') {
            document.getElementById('form_contato').preventDefault;
            atendente_id = document.getElementById('atendente_id').value;
            lead_nome = document.getElementById('contato_nome').value;
            lead_celular = document.getElementById('contato_telefone').value;
            lead_email = document.getElementById('contato_e_mail').value;
            lead_contato_msg = document.getElementById('contato_mensagem').value;
            lead_tipo = 'Site';
            lead_plano_selecionado = document.getElementById('plano_selecionado').value;
        }

        if (tipo == 'Whatsapp') {
            document.getElementById('w_form').preventDefault;
            atendente_id = document.getElementById('w_atendente').value;
            lead_nome = document.getElementById('w_nome').value;
            lead_celular = document.getElementById('w_celular').value;
            lead_email = document.getElementById('w_email').value;
            lead_contato_msg = '';
            lead_tipo = 'Whatsapp';  
            lead_plano_selecionado = 'Não se aplica - Whatsapp';
        }

        $.ajax({
            url: "/Site/Create",
            data: {
                __RequestVerificationToken: gettoken(),
                atendente_id: atendente_id,
                lead_nome: lead_nome,
                lead_celular: lead_celular,
                lead_email: lead_email,
                lead_contato_msg: lead_contato_msg,
                lead_tipo: lead_tipo,
                lead_plano_selecionado: lead_plano_selecionado
            },
            type: 'POST',
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.getElementById('modal_confirm_form_lead_label').innerHTML = '<span class="text-danger">Erro</span>';
                document.getElementById('modal_confirm_form_lead_body').innerHTML = '<span class="text-danger">Desculpe, infelizmente tivemos um problema com o envio do contato. Favor tentar novamente.</span>';
                modal_sobre_modal_open('modal_confirm_form_lead');
            },
            success: function (data, textStatus, XMLHttpRequest) {
                let retorno = JSON.parse(data);
                if (textStatus == 'error') {
                    document.getElementById('modal_confirm_form_lead_label').innerHTML = '<span class="text-danger">Erro</span>';
                    document.getElementById('modal_confirm_form_lead_body').innerHTML = '<span class="text-danger">Desculpe, infelizmente tivemos um problema com o envio do contato. Favor tentar novamente.</span>';
                    modal_sobre_modal_open('modal_confirm_form_lead');
                }

                if (textStatus == 'success') {
                    if (retorno.includes('Desculpe')) {
                        if (document.getElementById('w_box')) {
                            document.getElementById('w_box').style.display = 'none';
                        }                        
                        document.getElementById('modal_confirm_form_lead_label').innerHTML = '<span class="text-danger">Erro</span>';
                        document.getElementById('modal_confirm_form_lead_body').innerHTML = '<span class="text-danger">Desculpe, infelizmente tivemos um problema com o envio do contato. Favor tentar novamente.</span>';
                        modal_sobre_modal_open('modal_confirm_form_lead');
                    }

                    if (retorno.includes('sucesso')) {                        
                        if (tipo == 'site') {         
                            document.getElementById('modal_confirm_form_lead_label').innerHTML = '<span class="text-success"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-hand-thumbs-up" viewBox="0 0 16 16"><path d = "M8.864.046C7.908-.193 7.02.53 6.956 1.466c-.072 1.051-.23 2.016-.428 2.59-.125.36-.479 1.013-1.04 1.639-.557.623-1.282 1.178-2.131 1.41C2.685 7.288 2 7.87 2 8.72v4.001c0 .845.682 1.464 1.448 1.545 1.07.114 1.564.415 2.068.723l.048.03c.272.165.578.348.97.484.397.136.861.217 1.466.217h3.5c.937 0 1.599-.477 1.934-1.064a1.86 1.86 0 0 0 .254-.912c0-.152-.023-.312-.077-.464.201-.263.38-.578.488-.901.11-.33.172-.762.004-1.149.069-.13.12-.269.159-.403.077-.27.113-.568.113-.857 0-.288-.036-.585-.113-.856a2.144 2.144 0 0 0-.138-.362 1.9 1.9 0 0 0 .234-1.734c-.206-.592-.682-1.1-1.2-1.272-.847-.282-1.803-.276-2.516-.211a9.84 9.84 0 0 0-.443.05 9.365 9.365 0 0 0-.062-4.509A1.38 1.38 0 0 0 9.125.111L8.864.046zM11.5 14.721H8c-.51 0-.863-.069-1.14-.164-.281-.097-.506-.228-.776-.393l-.04-.024c-.555-.339-1.198-.731-2.49-.868-.333-.036-.554-.29-.554-.55V8.72c0-.254.226-.543.62-.65 1.095-.3 1.977-.996 2.614-1.708.635-.71 1.064-1.475 1.238-1.978.243-.7.407-1.768.482-2.85.025-.362.36-.594.667-.518l.262.066c.16.04.258.143.288.255a8.34 8.34 0 0 1-.145 4.725.5.5 0 0 0 .595.644l.003-.001.014-.003.058-.014a8.908 8.908 0 0 1 1.036-.157c.663-.06 1.457-.054 2.11.164.175.058.45.3.57.65.107.308.087.67-.266 1.022l-.353.353.353.354c.043.043.105.141.154.315.048.167.075.37.075.581 0 .212-.027.414-.075.582-.05.174-.111.272-.154.315l-.353.353.353.354c.047.047.109.177.005.488a2.224 2.224 0 0 1-.505.805l-.353.353.353.354c.006.005.041.05.041.17a.866.866 0 0 1-.121.416c-.165.288-.503.56-1.066.56z" /></svg></span>';
                            document.getElementById('modal_confirm_form_lead_body').innerHTML = '<span class="text-dark">Legal, é um prazer poder atende-lo.</br>Em breve entraremos em contato.</br> Obrigado pelo Contato.</span>';
                            document.getElementById('contato_nome').value = '';
                            document.getElementById('contato_telefone').value = '';
                            document.getElementById('contato_e_mail').value = '';
                            document.getElementById('contato_mensagem').value = '';
                            modal_sobre_modal_open('modal_confirm_form_lead');
                        }
                        if (tipo == 'Whatsapp') {
                            let l = document.getElementById('w_link').value;
                            document.getElementById('w_nome').value = '';
                            document.getElementById('w_celular').value = '';
                            document.getElementById('w_email').value = '';                            
                            window.open(l, '_blank');
                        }
                    }
                }
            }
        });
    }

    if (contexto == 'create_site') {
        console.log('create_site');
        lead('create', '0','site');
    }
}

function whats(contexto) {
    if (contexto == 'close') {
        document.getElementById('w_box').style.display = 'none';       
    }
    if (contexto == 'open') {
        let estado = document.getElementById('w_box').style.display;
        if (estado == 'none') {
            document.getElementById('w_box').style.display = 'block';
        } else {
            document.getElementById('w_box').style.display = 'none';
        }
        
    }
    if (contexto == 'iniciar') {
        lead('create', '0', 'Whatsapp');
    }
}

$('.scroll_inter').on('click', function (e) {
    e.preventDefault();
    var id = $(this).attr('href'),
        targetOffset = $(id).offset().top;

    $('html, body').animate({
        scrollTop: targetOffset - 100
    }, 500);
});

//$("#contato_telefone").mask("(00) 0000-00009");

//jQuery("input.telefone")
//    .mask("(99) 9999-9999?9")
//    .focusout(function (event) {
//        var target, phone, element;
//        target = (event.currentTarget) ? event.currentTarget : event.srcElement;
//        phone = target.value.replace(/\D/g, '');
//        element = $(target);
//        element.unmask();
//        if (phone.length > 10) {
//            element.mask("(99) 99999-9999");
//        } else {
//            element.mask("(99) 9999-99999");
//        }
//    });

function plano_contador(context) {
    let vlr = convertStringDouble(document.getElementById('qtd_socio_func').value);
    if (context == 'verifica') {
        if (vlr < 0 || vlr == 0) {
            document.getElementById('qtd_socio_func').value = 1;
        }

        return;
    } 

    if (context == 'subtrai') {
        if (vlr != 0) {
            vlr = (vlr - 1);
        }        
    }

    if (context == 'soma') {
        if (vlr != 0) {
            vlr = (vlr + 1);
        } 
    }

    if (vlr == 0 || vlr < 1) {
        document.getElementById('qtd_socio_func').value = 1;
    } else {
        document.getElementById('qtd_socio_func').value = vlr;
    }

}

function planoSelect(plano) {
    document.getElementById('plano_selecionado').value = plano;
}

function mask(o, f) {
    setTimeout(function () {
        var v = mphone(o.value);
        if (v != o.value) {
            o.value = v;
        }
    }, 1);
}

function mphone(v) {
    var r = v.replace(/\D/g, "");
    r = r.replace(/^0/, "");
    if (r.length > 10) {
        r = r.replace(/^(\d\d)(\d{5})(\d{4}).*/, "($1) $2-$3");
    } else if (r.length > 5) {
        r = r.replace(/^(\d\d)(\d{4})(\d{0,4}).*/, "($1) $2-$3");
    } else if (r.length > 2) {
        r = r.replace(/^(\d\d)(\d{0,5})/, "($1) $2");
    } else {
        r = r.replace(/^(\d*)/, "($1");
    }
    return r;
}

function validacaoEmail(field) {

    if (field == '' || field == null) {
        return false;
    }

    let usuario = field.substring(0, field.indexOf("@"));
    let dominio = field.substring(field.indexOf("@") + 1, field.length);
    if ((usuario.length >= 1) &&
        (dominio.length >= 3) &&
        (usuario.search("@") == -1) &&
        (dominio.search("@") == -1) &&
        (usuario.search(" ") == -1) &&
        (dominio.search(" ") == -1) &&
        (dominio.search(".") != -1) &&
        (dominio.indexOf(".") >= 1) &&
        (dominio.lastIndexOf(".") < dominio.length - 1)) {
        return true;
    }
}









