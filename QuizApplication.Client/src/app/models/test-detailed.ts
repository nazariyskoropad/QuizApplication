import { Question } from './question';

export class TestDetailed {
    public id: number;
    public runsNumber: number;
    public timeLimit: number;
    public points: number;
    public name: string;
    public description: string;
    public userName: string;
    public startsAt: Date;
    public createdAt: Date;
    public endsAt: Date;
    public updatedAt: Date;
    public questions: Question[];

    constructor() {
        this.questions = new Array();
    }
}