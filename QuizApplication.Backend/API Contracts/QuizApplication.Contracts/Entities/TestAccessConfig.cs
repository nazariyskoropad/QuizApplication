namespace QuizApplication.Contracts.Entities
{
    public class TestAccessConfig : BaseEntity
    {
        public int TestId { get; set; }
        public string UniqueLink { get; set; }
        public int RunsNumber { get; set; }
        public string UserName { get; set; }
        public Test Test { get; set; }
    }
}