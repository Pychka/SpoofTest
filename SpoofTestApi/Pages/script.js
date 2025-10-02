let title = document.getElementById("title");
let group = document.getElementById("group");
let lastName = document.getElementById("last-name");
let name = document.getElementById("name");
let patronymic = document.getElementById("patronymic");
let progressbar = document.getElementById("progressbar");
let progresstext = document.getElementById("progresstext");
let blob = document.getElementById("blob-container");
let question = document.getElementById("question-container");
let sendButton = document.getElementById("send-button");
let currentTest
let testId = document.getElementById("test-id");

async function GoClick(){
    if(!lastName || !name || !patronymic || !group)
    {
        alert("Укажите все поля!");
        return;
    }
    var answer = await fetch(`https://localhost:7007/api/Test?id=${testId.value}`);
    if(!answer.ok)
    {

        alert("Теста не существует");
        return;
    }
    currentTest = parseTestFromJSON(await answer.json());
    progressbar.max = currentTest.questions.length;
    blob.style.display = 'none';
    question.style.display = 'block';
    renderTest(currentTest);
}

function goNext() {
    if (currentTest && currentTest.nextQuestion()){
        renderTest(currentTest);
        if(currentTest.currentQuestionIndex+1 == currentTest.questions.length)
            sendButton.style.display = 'block';
    }
}

function goPrevious() {
    if (currentTest && currentTest.previousQuestion())
    {
        renderTest(currentTest);
        sendButton.style.display = 'none';
    }
}

function OnChange(value){
    currentTest.selectAnswer(value);
}

function renderTest(test) {
    const container = document.getElementById('questions');
    const currentQuestion = test.getCurrentQuestion();

    container.innerHTML = `
        ${currentQuestion.answers.map(answer =>
            `
            <p>
                <input type="radio" onchange="OnChange(${answer.id})" name="${currentQuestion.id}">${answer.title}
            </p>
            `
        ).join('')}        
    `;
    title.textContent = currentQuestion.title;
    progressbar.value = test.currentQuestionIndex + 1;
    progresstext.textContent = `${test.currentQuestionIndex + 1} / ${test.questions.length}`
}

async function submitTest() {
    if (!currentTest) return;

    try {
        document.getElementById("left").style.display = 'none';
        document.getElementById("right").style.display = 'none';
        const answers = currentTest.getAnswersForSubmission();
        
        const response = await fetch('https://localhost:7007/api/Test', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                testId: currentTest.id,
                answers: answers,
                lastName: lastName,
                patronymic: patronymic,
                name: name,
                group: group
            })
        });

    } catch (error) {
        console.error('Ошибка:', error);
        alert("Ошибка отправки");
    }
}

class Answer {
    constructor(id, title) {
        this.id = id;
        this.title = title;
        this.selected = false;
    }
}

class Question {
    constructor(id, title, answers) {
        this.id = id;
        this.title = title;
        this.answers = answers.map(a => new Answer(a.id, a.title));
        this.selectedAnswerId = null;
    }
}

class Test {
    constructor(id, title, limitMinutes, questions) {
        this.id = id;
        this.title = title;
        this.limitMinutes = limitMinutes;
        this.questions = questions.map(q => 
            new Question(q.id, q.title, q.answers)
        );
        this.currentQuestionIndex = 0;
    }

    getCurrentQuestion() {
        return this.questions[this.currentQuestionIndex];
    }

    selectAnswer(answerId) {
        this.questions[this.currentQuestionIndex].selectedAnswerId = answerId;
    }

    nextQuestion() {
        if (this.currentQuestionIndex < this.questions.length - 1) {
            this.currentQuestionIndex++;
            return true;
        }
        return false;
    }

    previousQuestion() {
        if (this.currentQuestionIndex > 0) {
            this.currentQuestionIndex--;
            return true;
        }
        return false;
    }

    isLastQuestion() {
        return this.currentQuestionIndex === this.questions.length - 1;
    }

    getAnswersForSubmission() {
        return this.questions.map(q => ({
            questionId: q.id,
            answerId: q.selectedAnswerId
        }));
    }
}

function parseTestFromJSON(jsonData) {
    return new Test(
        jsonData.id,
        jsonData.title,
        jsonData.limitMinutes,
        jsonData.questions
    );
}