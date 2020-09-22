namespace QuizApplication.Contracts.DTOs
{
    public class TestAccessConfigDto : BaseDto
    {
        public int TestId { get; set; }
        public string UniqueLink { get; set; }
        public int RunsNumber { get; set; }
        public string UserName { get; set; }
        public TestDto Test { get; set; }
    }
}
