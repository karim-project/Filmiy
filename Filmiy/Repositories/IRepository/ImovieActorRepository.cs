namespace Filmiy.Repositories.IRepository
{
    public interface ImovieActorRepository : IRepository<MovieActor>
    {
        void RemoveRange(IEnumerable<MovieActor> movieActors);
    }
}
