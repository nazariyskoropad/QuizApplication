import { Question } from './question';
import { TestResult } from './test-result';

export class TestDetailed {
    public id: number;
    public timeLimit: number;
    public points: number;
    public name: string;
    public description: string;
    public startsAt: Date;
    public createdAt: Date;
    public endsAt: Date;
    public updatedAt: Date;
    public questions: Question[];
    public testResults: TestResult[];

    constructor() {
        this.questions = new Array();
    }
}