using Filmiy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Filmiy.Data.EntityConfigurations
{
    public class MovieActorConfiguration : IEntityTypeConfiguration<MovieActor>
    {
        public void Configure(EntityTypeBuilder<MovieActor> builder)
        {
            
            builder.HasKey(ma => new { ma.MovieId, ma.ActorId });

           
            builder.HasOne(ma => ma.Movie)
                   .WithMany(m => m.movieActors)
                   .HasForeignKey(ma => ma.MovieId);

           
            builder.HasOne(ma => ma.Actor)
                   .WithMany(a => a.MovieActors)
                   .HasForeignKey(ma => ma.ActorId);
        }
    }
}
