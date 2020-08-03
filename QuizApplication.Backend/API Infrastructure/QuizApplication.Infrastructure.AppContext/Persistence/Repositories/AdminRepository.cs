using QuizApplication.Contracts.Entities;
using QuizApplication.Infrastructure.AppContext.Persistence;
using QuizApplication.Infrastructure.AppContext.Persistence.Repositories;

namespace QuizApplication.Contracts.Interfaces
{
    public class AdminRepository : Repository<Admin>
    {
        public AdminRepository(AppDbContext dbContext)
        : base(dbContext)
        {
        }
    }
}
