using Microsoft.EntityFrameworkCore;
using MyWebsite.Models;

namespace MyWebsite.Tools
{
    public class DataDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }    
        public DbSet<Location> Locations { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<CV> CVs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Data Source=(LocalDb)\MyDB;Initial Catalog=master;Integrated Security=True");
            optionsBuilder.UseSqlServer("Server=tcp:psycox.database.windows.net,1433;Initial Catalog=psycox;Persist Security Info=False;User ID=azureuser;Password=b84d5d767f02!BDD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}
