using System;
using System.Collections.Generic;

namespace QuizApplication.Contracts.Entities
{
    public class Test : BaseEntity
    {
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
        public ICollection<Question> Questions { get; set; }
        public ICollection<TestResult> TestResults { get; set; }


    }
}
