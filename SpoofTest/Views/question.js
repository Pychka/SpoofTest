class question {
    constructor(id, content, answers, isOnce) {
        this.id = id;
        this.content = content;
        this.isOnce = isOnce;
        this.answers = answers.map(a => new answer(a.id, a.content));
        this.selectedAnswerId = null;
        this.selectedAnswerIds = [];
    }

    toggleAnswer(answerId) {
        if (this.isOnce) {
            if (this.selectedAnswerId === answerId) {
                this.selectedAnswerId = null;
            } else {
                this.selectedAnswerId = answerId;
            }
        } else {
            if (!this.selectedAnswerIds) {
                this.selectedAnswerIds = [];
            }
            
            const index = this.selectedAnswerIds.indexOf(answerId);
            if (index === -1) {
                this.selectedAnswerIds.push(answerId);
            } else {
                this.selectedAnswerIds.splice(index, 1);
            }
        }
    }
    
    getSelectedAnswers() {
        if (this.isOnce) {
            return this.selectedAnswerId ? [this.selectedAnswerId] : [];
        } else {
            return this.selectedAnswerIds || [];
        }
    }
}