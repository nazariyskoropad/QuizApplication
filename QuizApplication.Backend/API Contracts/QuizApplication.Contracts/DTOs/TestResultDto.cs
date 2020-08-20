using System;

namespace QuizApplication.Contracts.DTOs
{
    public class TestResultDto
    {
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
