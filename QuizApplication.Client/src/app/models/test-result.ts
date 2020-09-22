export class TestResult {
    public testId: number; 
    public correctCount: number;
    public incorrectCount: number;
    public skippedCount: number;
    public points: number
    public userName: string;
    public startedAt: Date;
    public endedAt: Date;
}