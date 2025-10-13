class testResult{
    constructor(rightCount, allCount, percentage, score) {
        this.rightCount = rightCount;
        this.allCount = allCount;
        this.percentage = percentage;
        this.score = score;
    }

    getMessage() {
        return `Ваша оценка ${this.score}`;
    }
}