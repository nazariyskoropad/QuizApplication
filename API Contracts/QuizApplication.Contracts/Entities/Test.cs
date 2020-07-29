using System;
using System.Collections.Generic;

namespace QuizApplication.Contracts.Entities
{
    public class Test
    {
        public int Id { get; set; }
        public int RunsNumber { get; set; }
        public int TimeLimit { get; set; }
        public double Points { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Question> Questions { get; set; }
        public List<TestResult> TestResults { get; set; }


    }
}
