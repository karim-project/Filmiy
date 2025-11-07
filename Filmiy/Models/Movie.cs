using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Filmiy.Models
{
    public enum MovieStatus
    {
        ComingSoon,
        NowShowing,
        Finished
    }
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public MovieStatus Status { get; set; }

        public DateTime DateTime { get; set; }

        public string? MainImg { get; set; }
        [ValidateNever]
        public ICollection<MovieImage> SubImages { get; set; }
        [ValidateNever]
        public ICollection <MovieActor> movieActors { get; set; }
       
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category category { get; set; }

        public int CinemaId { get; set; }
        [ValidateNever]
        public Cinema cinema { get; set; }
    }
}
