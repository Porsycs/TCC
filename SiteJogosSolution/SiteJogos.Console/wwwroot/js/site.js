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