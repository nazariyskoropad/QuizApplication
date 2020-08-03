using QuizApplication.Contracts.Entities;
using QuizApplication.Infrastructure.AppContext.Persistence;
using QuizApplication.Infrastructure.AppContext.Persistence.Repositories;

namespace QuizApplication.Contracts.Interfaces
{
    public class QuestionAnswerRepository : Repository<QuestionAnswer>
    {
        public QuestionAnswerRepository(AppDbContext dbContext)
        : base(dbContext)
        {
        }
    }
}
