namespace Filmiy.ViewModels
{
   
        public class MovieVM
        {
            public Movie Movie { get; set; }

            public IEnumerable<Category> categories { get; set; }
            public IEnumerable<Cinema> cinemas { get; set; }
            public IEnumerable<Actor> Actors { get; set; }

            public IEnumerable<int> SelectedActors { get; set; }
        }

}
