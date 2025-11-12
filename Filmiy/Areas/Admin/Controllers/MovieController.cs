using Filmiy.Data;
using Filmiy.Models;
using Filmiy.Utitlies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Filmiy.Areas.Admin.Controllers
{
     [Area("Admin")]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE} , {SD.EMPLOYEE_ROLE}")]
    public class MovieController : Controller
    {
        private readonly  ApplicationDbContext _context;// = new();
        private readonly  IRepository<Movie> _movieRepositry;// = new();
        private readonly  IRepository<Category> _categoryRepository;// = new();
        private readonly  IRepository<Actor> _actorRepository;//= new();
        private readonly  IRepository<Cinema> _cinemaRepository;//=  new();
        private readonly  IRepository<MovieImage> _movieImgRepository;// = new();
        private readonly  IRepository<MovieActor> _movieActorRepository;// = new();

        public MovieController(ApplicationDbContext context,IRepository<Movie> movieRepositry, IRepository<Category> categoryRepository, IRepository<Actor> actorRepository, IRepository<Cinema> cinemaRepository, IRepository<MovieImage> movieImgRepository, IRepository<MovieActor> movieActorRepository)
        {
            _context = context;
            _movieRepositry = movieRepositry;
            _categoryRepository = categoryRepository;
            _actorRepository = actorRepository;
            _cinemaRepository = cinemaRepository;
            _movieImgRepository = movieImgRepository;
            _movieActorRepository = movieActorRepository;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            
            var movies =await _movieRepositry.GetAsync(includes: [ m => m.cinema, m => m.category], tracked: false, cancellationToken: cancellationToken);
            return View(movies);
        }
        //---------Create----------//
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken )
        {
           
            return View(new MovieVM()
            {
                categories = await _categoryRepository.GetAsync(cancellationToken: cancellationToken),
                cinemas = await _cinemaRepository.GetAsync(cancellationToken: cancellationToken),
                Actors = await _actorRepository.GetAsync(cancellationToken: cancellationToken)
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(Movie movie, IFormFile? MainImg, List<IFormFile>? SubImages, int[] actorIds, CancellationToken cancellationToken)
        {
            // upload main image
            if (MainImg is not null && MainImg.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/movies/main", fileName);

                using var stream = System.IO.File.Create(filePath);
                MainImg.CopyTo(stream);

                movie.MainImg = fileName;
            }

            // save movie first
            var MovieCreate = await _movieRepositry.AddAsync(movie);
            await _movieRepositry.CommitAsync(cancellationToken);

            // upload sub images
            if (SubImages is not null && SubImages.Count > 0)
            {
                foreach (var img in SubImages)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/movies/sub", fileName);

                    using var stream = System.IO.File.Create(filePath);
                    img.CopyTo(stream);

                    await _movieImgRepository.AddAsync(new MovieImage
                    {
                        MovieId = MovieCreate.Id,
                        Img = fileName
                    });
                }

                await _movieImgRepository.CommitAsync(cancellationToken);
            }

            // Save actors
            if (actorIds != null && actorIds.Length > 0)
            {
                foreach (var actorId in actorIds)
                {
                    await _movieActorRepository.AddAsync(new MovieActor
                    {
                        MovieId = MovieCreate.Id,
                        ActorId = actorId
                    });
                }

                await _movieActorRepository.CommitAsync(cancellationToken);
            }

            return RedirectToAction(nameof(Index));
        }

        //---------Edit----------//
        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var movie = await _movieRepositry.GetOneAsync(
                m => m.Id == id,
                includes: [m => m.movieActors, m => m.SubImages],
                cancellationToken: cancellationToken
            );

            if (movie == null)
                return NotFound();

            // فقط لإرسال البيانات الإضافية للـ View
            ViewBag.Categories = await _categoryRepository.GetAsync(cancellationToken: cancellationToken);
            ViewBag.Cinemas = await _cinemaRepository.GetAsync(cancellationToken: cancellationToken);
            ViewBag.Actors = await _actorRepository.GetAsync(cancellationToken: cancellationToken);

            return View(movie); // هنا Movie وليس MovieVM
        }


        [HttpPost]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(Movie movie, IFormFile? MainImg, List<IFormFile>? subImgs, int[] actorIds, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Content("Errors: " + string.Join(", ", errors));
            }

            var movieInDb = await _movieRepositry.GetOneAsync(
                m => m.Id == movie.Id,
                tracked: true,
                includes: [m => m.movieActors, m => m.SubImages],
                cancellationToken: cancellationToken
            );

            if (movieInDb == null)
                return NotFound();

            // --- تحديث بيانات الفيلم ---
            movieInDb.Name = movie.Name;
            movieInDb.Price = movie.Price;
            movieInDb.Description = movie.Description;
            movieInDb.CategoryId = movie.CategoryId;
            movieInDb.CinemaId = movie.CinemaId;
            movieInDb.DateTime = movie.DateTime;
            movieInDb.Status = movie.Status;

            // --- تحديث الصورة الرئيسية ---
            if (MainImg != null && MainImg.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\movies\\main", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    MainImg.CopyTo(stream);
                }

                // حذف الصورة القديمة
                if (!string.IsNullOrEmpty(movieInDb.MainImg))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\movies\\main", movieInDb.MainImg);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                movieInDb.MainImg = fileName;
            }

            _movieRepositry.Update(movieInDb);
            await _movieRepositry.CommitAsync(cancellationToken);

            // --- تحديث الممثلين بدون DeleteRange ---
            if (movieInDb.movieActors != null && movieInDb.movieActors.Any())
            {
                foreach (var ma in movieInDb.movieActors.ToList())
                {
                    _movieActorRepository.Delete(ma);
                }
                await _movieActorRepository.CommitAsync(cancellationToken);
            }

            if (actorIds != null && actorIds.Length > 0)
            {
                foreach (var actorId in actorIds)
                {
                    await _movieActorRepository.AddAsync(new MovieActor
                    {
                        MovieId = movieInDb.Id,
                        ActorId = actorId
                    }, cancellationToken);
                }
                await _movieActorRepository.CommitAsync(cancellationToken);
            }

            // --- تحديث الصور الفرعية ---
            if (subImgs != null && subImgs.Count > 0)
            {
                movieInDb.SubImages ??= new List<MovieImage>();

                foreach (var img in subImgs)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\movies\\sub", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        img.CopyTo(stream);
                    }

                    await _movieImgRepository.AddAsync(new MovieImage
                    {
                        MovieId = movieInDb.Id,
                        Img = fileName
                    }, cancellationToken);
                }
                await _movieImgRepository.CommitAsync(cancellationToken);
            }

            return RedirectToAction(nameof(Index));
        }



        //---------Delete----------//
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Delete(int id , CancellationToken cancellationToken)
        {
            var movie =await _movieRepositry.GetOneAsync(m => m.Id == id , cancellationToken : cancellationToken);
            if (movie == null)
                return NotFound();

            _movieRepositry.Delete(movie);
            await _movieRepositry.CommitAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        } 
    }
}
