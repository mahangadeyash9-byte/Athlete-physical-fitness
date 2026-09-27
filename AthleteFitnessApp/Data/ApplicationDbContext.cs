using Microsoft.EntityFrameworkCore;
using AthleteFitnessApp.Models;

namespace AthleteFitnessApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}