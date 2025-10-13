class testReply {
    constructor(id, questions) {
        this.id = id;
        this.questions = questions.map(q => 
            new questionReply(q.id, q.getSelectedAnswers())
        );
    }
}