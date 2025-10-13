let container = document.getElementById("main-container");
let testContainer = document.getElementById("test-container");
let resultContainer = document.getElementById("result-container");
let questionTitle = document.getElementById("question-title");
let answers = document.getElementById("answers");
let min = document.getElementById("min");
let max = document.getElementById("max");
let progress = document.getElementById("progress");
let sendButton = document.getElementById("send-button");
let student = JSON.parse(localStorage.getItem("student-me"));
let currentIndex;
let currentTest;

document.addEventListener('DOMContentLoaded', function() {
    showTasks();
});

async function showTasks(){
    try {
        const response = await fetch(`https://localhost:7221/api/Test/Info/Many?personeId=${student.id}&persone=1`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
        });
        if (response.ok) {
            let result = await response.json();
            if (result.success && Array.isArray(result.data)) {
                showTests(result.data, container);
            } else {
                showMessage(result.message || 'Неверный формат данных от сервера', true);
            }
        } else {
            let result = await response.json();
            showMessage(result.error || 'Неопознаная ошибка', true);
        }
            
    } catch (error) {
        console.log(error);
        showMessage('Сервер сдох, попробуй позже');
    }
}

async function selectTest(testId){
    try {
        const response = await fetch(`https://localhost:7221/api/Test/Info?testId=${testId}&full=true`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
        });
        if (response.ok) {
            let result = await response.json();
            if (result.success) {
                showTest(result.data, container);
                startTimer(result.data.limitMinutes, send);
            } else {
                showMessage(result.message || 'Неверный формат данных от сервера', true);
            }
        } else {
            let result = await response.json();
            showMessage(result.error || 'Неопознаная ошибка', true);
        }
            
    } catch (error) {
        showMessage('Сервер сдох, попробуй позже', true);
    }
}

function showTests(tests, container){
    container.style.display = "flex";
    testContainer.style.display = "none";
    resultContainer.style.display = "none";
    currentTest = null;
    container.innerHTML  = `
        ${tests.map(test =>
        {
            const questionCount = test.questions ? test.questions.length : 0;
            const timeLimit = test.limitMinutes || 0;
            const title = test.title || 'Без названия';
            const description = test.description || 'Описание отсутствует';
            const testId = test.id || 0;
            const score = test.score;
        return `
            <div class="clickable-div" onclick="selectTest(${testId})" ${score == null ? "disabled" : ""}>
                <span>
                    <h1>${title}</h1>
                    ${questionCount} вопрос(а/ов) / Время на прохождение ${timeLimit}мин<br>
                    Ваш результат:${score || "не пройдено"}
                </span>
                <p>${description}</p>
            </div>
        `;
        }
        ).join('')}        
    `;
}

function select(answerId){
    const question = currentTest.questions[currentIndex];
    question.toggleAnswer(answerId);
}

function showTest(test){
    container.style.display = "none";
    testContainer.style.display = "flex";
    min.textContent = 1;
    currentIndex = 0;
    max.textContent = test.questions.length;
    progress.value = 1;
    progress.max = test.questions.length;
    if(test.questions.length > 0)
    {
        currentTest = parseTestFromJSON(test);
        showQuestion(test.questions[0]);
    }
}

function navigation(value){
    currentIndex += value;
    if(currentIndex < 0)
        currentIndex = 0;
    else if(currentIndex > currentTest.questions.length - 1)
        currentIndex = currentTest.questions.length - 1;
    else
    {
        progress.value = currentIndex+1;
        showQuestion(currentTest.questions[currentIndex]);
    }
    sendButton.disabled = currentIndex != currentTest.questions.length - 1;
}

async function send() {
    try {
        console.log(JSON.stringify(new testReply(currentTest.id, currentTest.questions)));
        const response = await fetch(`https://localhost:7221/api/Test/Reply?studentId=${student.id}`, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(new testReply(currentTest.id, currentTest.questions))
        });
        if (response.ok) {
            let result = await response.json();
            console.log(JSON.stringify(result));
            if (result.success) {
                testContainer.style.display = "none";
                resultContainer.style.display = "flex";
                let testTitle = document.getElementById("test-title");
                let testResult = document.getElementById("result-text");
                testTitle.textContent = currentTest.title;
                testResult.textContent = `Ваша оценка: ${result.data.score}`;
                currentTest = null;
            } else {
                showMessage(result.message || 'Неверный формат данных от сервера', true);
            }
        } else {
            let result = await response.json();
            showMessage(result.error || 'Неопознаная ошибка', true);
        }
            
    } catch (error) {
        showMessage('Сервер сдох, попробуй позже', true);
    }
}

function attachAnswerHandlers(question) {
    const inputs = answers.querySelectorAll('input[type="radio"], input[type="checkbox"]');
    
    inputs.forEach(input => {
        input.addEventListener('change', function() {
            const answerId = parseInt(this.value);
            select(answerId);
        });
    });
}

function showQuestion(question){
    questionTitle.textContent = question.content;
    answers.innerHTML = `${
        question.answers.map(answer =>{
            return `
            <label><input type="${question.isOnce ? 'radio' : 'checkbox'}" value="${answer.id}" name="${question.id}">${answer.content}</label>
            `;
        }).join('')
    }`;
    attachAnswerHandlers(question);
}

function showMessage(message, error = false, time = 5000){
    const errorDiv = document.getElementById('error-message');
    const mess = document.getElementById('message');
    const image = document.getElementById('image');
    mess.textContent = message;
    errorDiv.style.display = 'flex';
    image.style.display = error ? 'block' : 'none';
    setTimeout(() => {
        errorDiv.style.display = 'none';
    }, time);
}


function startTimer(minutes, onFinish) {
    let totalSeconds = minutes * 60;
    const timerElement = document.getElementById('timer');
        
    function updateTimer() {
        const minutes = Math.floor((totalSeconds % 3600) / 60);
        const seconds = totalSeconds % 60;
        
        const formattedTime = 
            String(minutes).padStart(2, '0') + ':' +
            String(seconds).padStart(2, '0');
        
        timerElement.textContent = formattedTime;
        
        if (totalSeconds <= 0) {
            timerElement.style.color = 'red';
            onFinish();
            return;
        }
        
        totalSeconds--;
        setTimeout(updateTimer, 1000);
    }

    updateTimer(totalSeconds, timerElement, onFinish);
}