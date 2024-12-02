var urlApi = 'https://localhost:7197' //Trocar para Pegar do COnfig !!! // IIS 7197/5199
var tabelaPosts = null;
var postSelecionado = {};
var modalAction = null;
var listaSalas = null;

$(document).ready(function () {
    $(":input[data-inputmask-mask]").inputmask();
    $(":input[data-inputmask-alias]").inputmask();
    $(":input[data-inputmask-regex]").inputmask("Regex");

    var listaPosts = ControllerRequest('GET', urlApi + '/api/posts', {}, false);

    var listColumns = [
        { "className": 'details-view-control', "orderable": false, "data": null, "defaultContent": '' },
        { "className": 'details-control-edit', "orderable": false, "data": null, "defaultContent": '' },
        { "className": 'details-control-delete', "orderable": false, "data": null, "defaultContent": '' },

        { "data": "id" }, { "data": "title" }, { "data": "summary" },
        { "data": "publishedOn" }, { "data": "userBlog" }];


    listaPosts.forEach(function (elem, i) {
        elem.publishedOn = FormatDate(elem.publishedOn);

    });

    tabelaPosts = ConstruirTabela('#tabelaPosts', listaPosts, listColumns, true, [6, 'asc']);


    $('#tabelaPosts tbody').on('click', 'tr td.details-view-control', function () {
        modalAction = 'view';

        $('#postsModal').modal('show');
        postSelecionado = tabelaPosts.row(this).data();
    });


    $('#listaPostsTbody').on('click', 'td.details-control-edit', function () {
        modalAction = 'edit';
        $('#postsModal').modal('show');
        postSelecionado = tabelaPosts.row(this).data();
    });

    $('#listaPostsTbody').on('click', 'td.details-control-delete', function () {
        modalAction = 'delete';
        var rowData = tabelaPosts.row(this).data();

        var result = confirm("Deseja excluir esse post?");
        if (result) {
            ControllerRequestCallBack('DELETE', urlApi + '/api/posts/' + rowData.id, {}, true, ObterLista);
        }
    });

    $('#postsModal').on('shown.bs.modal', function () {

        if (modalAction == 'view') {
            $('#hInfoModalPostModal').html('Visualização do Post');
        }
        else if (modalAction == 'edit') {
            $('#hInfoModalPostModal').html('Edição do Post');
        }
        else if (modalAction == 'create') {
            $('#hInfoModalPostModal').html('Criação do Post');
        }

        $('#txtTitle').val(postSelecionado.title);
        $('#txtSummary').val(postSelecionado.summary);
        $('#txtContent').val(postSelecionado.content);
        $('#txtPublishedOn').val(postSelecionado.publishedOn);
        $('#txtUserBlog').val(postSelecionado.userBlog);

        ReadOnly();
    });

    $('#postsModal').on('hidden.bs.modal', function () {
        //Limpar dados do modal
        postSelecionado = null;
    });

});

function ReadOnly() {
    if (modalAction == 'view') {
        $('#txtTitle').prop('disabled', true);
        $('#txtSummary').prop('disabled', true);
        $('#txtContent').prop('disabled', true);
        $('#txtUserBlog').prop('disabled', true);
        $('#txtPublishedOn').prop('disabled', true);

        $('#btnSalvar').css('display', 'none');
    }
    else if (modalAction == 'edit') {
        $('#txtTitle').prop('disabled', false);
        $('#txtSummary').prop('disabled', false);
        $('#txtContent').prop('disabled', false);
        $('#txtUserBlog').prop('disabled', true);
        $('#txtPublishedOn').prop('disabled', true);

        $('#btnSalvar').css('display', 'block');
    }
    else if (modalAction == 'create') {
        $('#txtTitle').prop('disabled', false);
        $('#txtSummary').prop('disabled', false);
        $('#txtContent').prop('disabled', false);
        $('#txtUserBlog').prop('disabled', true);
        $('#divPublishedOn').prop('display', 'none');

        $('#btnSalvar').css('display', 'block');
    }
}

function ConstruirTabela(id, lista, columnsList, showFilter, orderList, drawCallback = undefined, _lengthMenuValue = undefined, _lengthMenuText = undefined, _pageLength = 5) {
    var lengthMenuValue = (_lengthMenuValue == undefined ? [5, 10, 20, 30, 40, -1] : _lengthMenuValue);
    var lengthMenuText = (_lengthMenuText == undefined ? ['5', '10', '20', '30', '40', 'Exibir todos'] : _lengthMenuText);

    var table = $(id).DataTable({
        language: {
            "url": "../../json-language/Portuguese-Brasil.json"
        },
        bFilter: showFilter, //Searchbox
        lengthChange: true,
        pagingType: "first_last_numbers",
        data: lista,
        columns: columnsList,
        pageLength: _pageLength,
        order: [orderList],
        dom: 'Bfrtip',
        lengthMenu: [lengthMenuValue, lengthMenuText],
        buttons: ['pageLength', 'copy', 'csv', 'excel', 'pdf', 'print', 'colvis'],

        "drawCallback": function (settings) {
            if (drawCallback != undefined && drawCallback != null) {
                drawCallback();
            }
        }
    });

    return table;
}

function ObterLista(data) {
    var listaPosts = ControllerRequest('GET', urlApi + '/api/posts', {}, false);

    listaPosts.forEach(function (elem, i) {
        elem.publishedOn = FormatDate(elem.publishedOn);
    });

    if (data.sucesso) {
        tabelaPosts.clear().draw();
        tabelaPosts.rows.add(listaPosts); // Add new data
        tabelaPosts.columns.adjust().draw(); // Redraw the DataTable

        switch (modalAction) {
            case 'edit':
                alert('Atualização efetuada com sucesso!');
                $('#postsModal').modal('toggle');
                break;
            case 'create':
                alert('Criação efetuada com sucesso!');
                $('#postsModal').modal('toggle');
                break;
            case 'delete':
                alert('Exclusão efetuada com sucesso!');
                break;

            default:
                console.log('Comando inválido ${modalAction}.');
        }
    }

    modalAction = null;
}


function SalvarDados() {
    if (validaCampos()) {
        var isPost = (modalAction === 'create' ? true : false);

        var dados = JSON.stringify(
            {
                Id: (isPost ? null : postSelecionado.id),
                Title: $('#txtTitle').val(),
                Summary: $('#txtSummary').val(),
                Content: $('#txContent').val(),
            });

        ControllerRequestCallBack((isPost ? 'POST' : 'PUT'), urlApi + '/api/posts', dados, true, ObterLista);
    }
}

function CriarPost() {
    LimparCampos();

    modalAction = 'create';
    $('#postsModal').modal('show');
}

function LimparCampos() {
    $('#txtTitle').val('');
    $('#txtSummary').val('');
    $('#txtContent').val('');
    $('#txtUserBlog').val('');
}

function validaCampos() {
    var campos = ""


    if ($('#txtTitle').val() == "")
        campos += "\n - Titulo";

    if ($('#txtSummary').val() == "")
        campos += "\n - Sumário";

    if ($('#txtContent').val() == "")
        campos += "\n - Conteúdo";

    if (campos != "") {
        alert('O(s) campo(s) abaixo devem ser preenchidos:' + campos);
        return false;
    }
    else {
        return true;
    }
}
