using Filmiy.Data.EntityConfigurations;
using Filmiy.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Filmiy.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Actor> Actors { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Cinema> Cinema { get; set; } = null!;
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<MovieActor> MovieActors { get; set; } = null!;
        public DbSet<MovieImage> MovieImages { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        
       
        public DbSet<ApplicationUserOTP> ApplicationUserOTP { get; set; } = null!;




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MovieActorConfiguration());
        }

        internal void DeleteRange(IEnumerable<MovieActor> movieActors)
        {
            throw new NotImplementedException();
        }



        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //    optionsBuilder.UseSqlServer("Data Source=.;Database=CinemaBook;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        //}


    }
}
