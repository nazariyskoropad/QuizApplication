import { QuestionAnswer } from './questionAnswer';

export class Question {
    public id: number;
    public testId: number;
    public timeLimit: number;
    public points: number;
    public description: string;
    public questionAnswers: QuestionAnswer[]

    constructor() {
        this.questionAnswers = new Array();
    }
}