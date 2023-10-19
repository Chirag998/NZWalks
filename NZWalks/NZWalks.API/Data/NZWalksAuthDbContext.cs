using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerId = "ab2eccc2-8bc5-4bc5-b224-d10f64cbafd1";
            var writerId = "42505522-162f-4556-9c0e-9dade4297b6b";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id=readerId,
                    ConcurrencyStamp=readerId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper()
                },
                new IdentityRole { Id=writerId, ConcurrencyStamp=writerId,Name="Writer",NormalizedName="Writer".ToUpper()}
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
