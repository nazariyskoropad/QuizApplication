using System.Collections.Generic;

namespace QuizApplication.Contracts.Entities
{
    public class Question : BaseEntity
    {
        public int TestId { get; set; }
        public int TimeLimit { get; set; }
        public double Points { get; set; }
        public string Description { get; set; }
        public List<QuestionAnswer> QuestionAnswers { get; set; }
        public Test Test { get; set; }
    }
}
