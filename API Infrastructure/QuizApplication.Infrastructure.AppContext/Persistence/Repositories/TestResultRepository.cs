using QuizApplication.Contracts.Entities;
using QuizApplication.Infrastructure.AppContext.Persistence;
using QuizApplication.Infrastructure.AppContext.Persistence.Repositories;

namespace QuizApplication.Contracts.Interfaces
{
    public class TestResultRepository : Repository<TestResult>, ITestResultRepository
    {
        public TestResultRepository(AppDbContext dbContext)
        : base(dbContext)
        {
        }
    }
}
