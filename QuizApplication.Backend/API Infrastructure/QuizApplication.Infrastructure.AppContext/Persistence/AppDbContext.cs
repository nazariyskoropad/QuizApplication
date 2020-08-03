using Microsoft.EntityFrameworkCore;
using QuizApplication.Contracts.Entities;
using System.Reflection;

namespace QuizApplication.Infrastructure.AppContext.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
