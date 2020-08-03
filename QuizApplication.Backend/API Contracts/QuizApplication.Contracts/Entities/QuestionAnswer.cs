namespace QuizApplication.Contracts.Entities
{
    public class QuestionAnswer : BaseEntity
    {
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }
        public string Description { get; set; }
        public Question Question { get; set; }
    }
}
