using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net.Sockets;
using static System.Net.WebRequestMethods;

namespace KinoTeatrBackground.Data
{
    public class AppDataContext:DbContext
    {
        //public DbSet<class> name { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<User>().ToTable("User");

        }
    }
}
