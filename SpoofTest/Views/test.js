class test {
    constructor(id, title, limitMinutes, questions) {
        this.id = id;
        this.title = title;
        this.limitMinutes = limitMinutes;
        this.questions = questions.map(q => 
            new question(q.id, q.content, q.answers, q.isOnce)
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
    return new test(
        jsonData.id,
        jsonData.title,
        jsonData.limitMinutes,
        jsonData.questions
    );
}