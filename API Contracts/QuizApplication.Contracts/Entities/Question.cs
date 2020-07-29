using System.Collections.Generic;

namespace QuizApplication.Contracts.Entities
{
    public class Question
    {
        public int TimeLimit { get; set; }
        public double Points { get; set; }
        public string Description { get; set; }
        public Dictionary<string, bool> Answers { get; set; }
    }
}
