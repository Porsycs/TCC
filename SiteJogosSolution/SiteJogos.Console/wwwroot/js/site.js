function notifica(titulo, mensagem, tema, posicao = 'nfc-bottom-right')
{
    var myNotification = window.createNotification({

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