using QuizApplication.Contracts.Entities;
using QuizApplication.Infrastructure.AppContext.Persistence;
using QuizApplication.Infrastructure.AppContext.Persistence.Repositories;

namespace QuizApplication.Contracts.Interfaces
{
    public class TestRepository : Repository<Test>
    {
        public TestRepository(AppDbContext dbContext)
        : base(dbContext)
        {
        }
    }
}
