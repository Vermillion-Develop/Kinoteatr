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
        public DbSet<DataLog> DataLogs { get; set; }
        public DbSet<VideoProduct> VideoProducts { get; set; }
        public DbSet<VidVideoProduct> VidVideoProducts { get; set; }
        public DbSet<StatusVideoProduct> StatusesVideoProduct{ get; set; }
        public DbSet<AdultVideoProduct> AdultVideoProducts { get; set; }
        public DbSet<Zatrat> Zatrats{ get; set; }
        public DbSet<TypeZatrat> TypesZatrats{ get; set; }
        public DbSet<StaffGroup> StaffGroups{ get; set; }
        public DbSet<CinemaGroup> Groups{ get; set; }
        public DbSet<Area> Areas{ get; set; }
        public DbSet<TypeArea> TypeAreas { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<VideoProductInCinemas> VideoInCinemas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Staff>().ToTable("Staff");
            modelBuilder.Entity<Specialization>().ToTable("Specialization");
            modelBuilder.Entity<DataLog>().ToTable("DataLog");

            modelBuilder.Entity<VideoProduct>().ToTable("VideoProduct");
            modelBuilder.Entity<VidVideoProduct>().ToTable("VidVideoProduct");
            modelBuilder.Entity<StatusVideoProduct>().ToTable("StatusVideoProduct");
            modelBuilder.Entity<AdultVideoProduct>().ToTable("AdultVideoProduct");
            modelBuilder.Entity<Zatrat>().ToTable("Zatrat");
            modelBuilder.Entity<TypeZatrat>().ToTable("TypeZatrat");
            modelBuilder.Entity<StaffGroup>().ToTable("StaffGroup");
            modelBuilder.Entity<CinemaGroup>().ToTable("Group");

            modelBuilder.Entity<Area>().ToTable("Area");
            modelBuilder.Entity<TypeArea>().ToTable("TypeArea");
            modelBuilder.Entity<Town>().ToTable("Town");
            modelBuilder.Entity<Street>().ToTable("Street");

            modelBuilder.Entity<Cinema>().ToTable("Cinema");
            modelBuilder.Entity<VideoProductInCinemas>().ToTable("VideoProductInCinemas");

        }
    }
}
