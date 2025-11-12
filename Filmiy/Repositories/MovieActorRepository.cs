namespace Filmiy.Repositories
{
    public class MovieActorRepository : Repository<MovieActor>
    {
        private ApplicationDbContext _context;// = new();

        public MovieActorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void RemoveRange(IEnumerable<MovieActor> movieActors )
        {
            _context.RemoveRange(movieActors);
        }
    }
}
