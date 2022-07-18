using Microsoft.EntityFrameworkCore;
using WeatherAcquisition.DAL.Entities;

namespace WeatherAcquisition.DAL.Context
{
    public class DataDb : DbContext
    {
        public DbSet<DataSource> Sources { get; set; }

        public DbSet<DataValue> Values { get; set; }

        public DataDb(DbContextOptions<DataDb> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DataSource>()
                .HasMany<DataValue>()
                .WithOne(v => v.Source)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<DataSource>()
            //    .Property(source => source.Name)
            //    .IsRequired();

            //modelBuilder.Entity<DataSource>()
            //    .HasIndex(source => source.Name)
            //    .IsUnique();
        }
    }
}
