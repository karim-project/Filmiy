namespace Filmiy.ViewModels
{
    public class MovieVM
    {
        public IEnumerable<Category> categories { get; set; } 
        public IEnumerable<Cinema> cinemas { get; set; } 
        public IEnumerable<Actor> Actors { get; set; } 
    }
}
