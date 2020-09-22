export class UserAnswers {
    public userName: string;
    public startedAt: Date;
    public endedAt: Date;
    public QApairs: QApair[]

    constructor() {
        this.QApairs = new Array();
    }
}

export class QApair {
    public questionId: number;
    public answerId: number;
}