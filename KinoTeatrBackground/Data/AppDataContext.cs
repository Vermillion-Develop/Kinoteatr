using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net.Sockets;
using KinoTeatrShared.Models;
using KinoTeatrShared.ModelsDTO;
using static System.Net.WebRequestMethods;

namespace KinoTeatrBackground.Data
{
    public class AppDataContext:DbContext
    {

        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
        }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Staff>().ToTable("Staff");
            modelBuilder.Entity<Specialization>().ToTable("Specialization");

        }
    }
}
