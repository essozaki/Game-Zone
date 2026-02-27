using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GameZone.Data
{
    public class ApplicationDbContext :IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Game> Game { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<DeleetedGame> DeleetedGame { get; set; }
        public DbSet<GameDevice> GameDevice { get; set; }
        //Adding Composit Key to GameDevie Table
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasData(new Category[] 
                {
                    new Category{Id=1 ,Name="Sports" },
                    new Category{Id=2 ,Name="Action" },
                    new Category{Id=3 ,Name="Adventure" },
                    new Category{Id=4 ,Name="Raciing" },
                    new Category{Id=5 ,Name="Fighting" },
                    new Category{Id=6 ,Name="Film" },
                });

             modelBuilder.Entity<Device>()
                .HasData(new Device[] 
                {
                    new Device{Id=1 ,Name="Playstaion",Icon="bi bi-playstaion" },
                    new Device{Id=2 ,Name="Xbox",Icon="bi bi-xbox" },
                    new Device{Id=3 ,Name="Nintendo Switch",Icon="bi bi-nintendo-switch" },
                    new Device{Id=4 ,Name="Pc",Icon="bi bi-pc-display" },
                });


            modelBuilder.Entity<GameDevice>()
                .HasKey(e => new { e.GameId, e.DeviceId });
            base.OnModelCreating(modelBuilder);
        }
    }

}
