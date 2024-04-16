using Microsoft.EntityFrameworkCore;
using C_Sharp_lab_4.Models;
namespace C_Sharp_lab_4.DbContexts
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
        {
        }
        public virtual DbSet<User> Users { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //   => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=qwerty1234;Database=C_Sharp_lab_4_db;Pooling=true;SSL Mode=Prefer;Trust Server Certificate=true;");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("userdb");
                entity.Property(e => e.Id)
                .HasColumnName("id");
                entity.Property(e => e.Login)
                .HasColumnName("login")
                .HasMaxLength(40);
                entity.Property(e => e.Password)
                .HasColumnName("password")
                .HasMaxLength(40);
                entity.Property(e => e.FIO)
                .HasColumnName("fio")
                .HasMaxLength(120);
            });
        }


    }
}