class question {
    constructor(id, title, answers) {
        this.id = id;
        this.title = title;
        this.answers = answers.map(a => new answer(a.id, a.title));
        this.selectedAnswerId = null;
    }
}