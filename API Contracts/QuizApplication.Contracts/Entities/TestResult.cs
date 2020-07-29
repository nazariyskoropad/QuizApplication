using System;

namespace QuizApplication.Contracts.Entities
{
    public class TestResult
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public int CorrectCount { get; set; }
        public int IncorrectCount { get; set; }
        public int SkippedCount { get; set; }
        public double Points { get; set; }
        public string UserName { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
    }
}
