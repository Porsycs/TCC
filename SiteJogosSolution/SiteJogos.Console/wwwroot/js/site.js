function notifica(titulo, mensagem, tema, posicao = 'nfc-bottom-right')
{
    var myNotification = window.createNotification({
        title: titulo,
        message: mensagem,
        theme: tema, //success, info, warning, error, and none
        positionClass: posicao
    });

    myNotification({
        title: titulo,
        message: mensagem,
        theme: tema, //success, info, warning, error, and none
        positionClass: posicao
    });
}

function notificaIziToast(titulo, mensagem, tipo = null, tempo = 4000, posicao = 'bottomCenter') {
    if (tipo == true) {
        iziToast.success({
            title: titulo,
            message: mensagem,
            position: posicao,
            timeout: tempo,
        });
    }
    else if (tipo == false) {
        iziToast.error({
            title: titulo,
            message: mensagem,
            position: posicao,
            timeout: tempo,
            icon: 'fa-solid fa-circle-exclamation',
            iconColor: 'white',
            backgroundColor: '#f9777f',
            titleColor: "white",
            messageColor: "white"
        });
    }
    else {
        iziToast.show({
            title: titulo,
            message: mensagem,
            position: posicao,
            timeout: tempo,
        });
    }
}

function redirecionaTela(url) {
    window.location.href = url;
}

function getDados(url, data = {}, metodo = 'GET', dataType = 'json') {
    let dadosRetorno = null;
    $.ajax({
        method: metodo,
        url: url,
        data: data,
        dataType: dataType,
        async: false,
        success: function (data) {
            dadosRetorno = data;
        },
        error: function (e) {
        }
    });
    return dadosRetorno;
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

var imagem = document.createElement('img');
imagem.src = '/images/load.gif';
function exibeLoadPanel() {
    var divCentro = document.createElement('div');
    divCentro.style.position = 'absolute';
    divCentro.style.top = '50%';
    divCentro.style.left = '50%';
    divCentro.style.transform = 'translate(-50%, -50%)';

    divCentro.appendChild(imagem);
    document.body.appendChild(divCentro);
}

function escondeLoadPanel() {
    imagem.style.display = 'none'
}