using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Filmiy.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        public ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            var movies = _context.Movies.Select(m => new Movie
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                MainImg = m.MainImg,
                CategoryId = m.CategoryId,
                category = m.category,
                cinema = m.cinema,
                Status = m.Status,
            }).ToList() ?? new List<Movie>();

            var categories = _context.Categories.ToList();

            ViewBag.Categories = categories;

            return View(movies);
        }

        public async Task<IActionResult> Item(int id, CancellationToken cancellationToken)
        {
            var movie = await _context.Movies
    .Include(m => m.category)
    .Include(m => m.cinema)
    .Include(m => m.SubImages)
    .Include(m => m.movieActors)
        .ThenInclude(ma => ma.Actor)
    .FirstOrDefaultAsync(m => m.Id == id);


            if (movie is null)
                return NotFound();

            return View(movie);

        }
    }
}
