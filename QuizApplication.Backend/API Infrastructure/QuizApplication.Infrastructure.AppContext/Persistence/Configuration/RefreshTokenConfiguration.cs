using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApplication.Contracts.Entities;

namespace QuizApplication.Infrastructure.AppContext.Persistence.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);
            builder.HasOne(rt => rt.Admin)
                .WithMany(a => a.RefreshTokens)
                .HasForeignKey(rt => rt.AdminId);
        }
    }
}
