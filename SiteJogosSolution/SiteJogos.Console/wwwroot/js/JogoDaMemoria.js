var cards = [];

let hasFlippedCard = false;
let lockBoard = false;
let firstCard, secondCard;
let movements = 0;
let winContador = 0;

//Contadores de Tempo
let minute = 0;
let second = 0;
let millisecond = 0;
let cron;

function flipCard(id) {

    let carta = document.getElementById(id);

    if (carta.dataset.descoberto == 'true') return;

    //carta.classList.toggle('flip');

    if (lockBoard) return;
    if (carta === firstCard) return;

    carta.classList.add('flip');

    if (!hasFlippedCard) {
        hasFlippedCard = true;
        firstCard = carta;
        return;
    }

    secondCard = carta;

    checkForMatch();
}

//Conferindo se é igual

function mostraIcone(certo) {
    let icone = ".icone" + (certo ? 'Certo' : 'Errado');
    $(icone).removeClass("d-none").addClass("animate__animated animate__bounceIn").addClass("animate__bounceOut");
    setTimeout(function () {
        $(icone).addClass("d-none").removeClass("animate__animated animate__bounceIn animate__bounceOut");
    }, 1000);
}

function checkForMatch() {
    movements++;
    document.getElementById("erros").innerHTML = `${movements}`;

    if (firstCard.dataset.nome === secondCard.dataset.nome) {
        winContador++;
        mostraIcone(true);
        disableCards();

        if (winContador == (cards.length / 2)) {
            clearInterval(cron);
            insereScore();
        }

        return;
    }

    unflipCards();
}

//Desabilitando o clique nas cartas viradas

function disableCards() {
    firstCard.dataset.descoberto = 'true';
    secondCard.dataset.descoberto = 'true';
    firstCard.removeEventListener('click', flipCard);
    secondCard.removeEventListener('click', flipCard);

    resetBoard();
}

//Virando as cartas erradas de volta

function unflipCards() {
    lockBoard = true;
    mostraIcone(false);
    setTimeout(() => {
        firstCard.classList.remove('flip');
        secondCard.classList.remove('flip');
        resetBoard();
    }, 1500);
}

function resetBoard() {
    [hasFlippedCard, lockBoard] = [false, false];
    [firstCard, secondCard] = [null, null];
}

//Embaralhando cartas (IIFE) Vai ser executada assim que for lida

function getWinContador() {
    return winContador;
}

function getTempo() {
    return (minute * 60) + second + (millisecond / 1000);
}

function iniciaJogo() {
    cards = document.querySelectorAll('.card');
    document.querySelectorAll('.rz-col-6').forEach(card => {
        let ramdomPos = Math.floor(Math.random() * cards.length);
        card.style.order = ramdomPos;
    });
    cron = setInterval(() => { timer(); }, 10);
}

function timer() {
    if ((millisecond += 10) == 1000) {
        millisecond = 0;
        second++;
    }
    if (second == 60) {
        second = 0;
        minute++;
    }
    document.getElementById('minute').innerText = returnData(minute);
    document.getElementById('second').innerText = returnData(second);
}

function returnData(input) {
    return input >= 10 ? input : `0${input}`
}