using Filmiy.Data;
using Filmiy.Models;
using Filmiy.Utitlies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Filmiy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE} , {SD.EMPLOYEE_ROLE}")]
    public class ActorController : Controller
    {
        //ApplicationDbContext _context = new ApplicationDbContext();
        private readonly IRepository<Actor> _actorRepository;//= new();

        public ActorController(IRepository<Actor> actorRepository)
        {
            _actorRepository = actorRepository;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken , int page = 1)
        {
            var actors =await _actorRepository.GetAsync(tracked:false , cancellationToken: cancellationToken);


            ViewBag.TotalPages = Math.Ceiling(actors.Count() / 8.0);
            ViewBag.CurrentPage = page;
            actors = actors.Skip((page - 1) * 8).Take(8); // 0 .. 8 

            return View(actors);
        }
        //---------Create----------//
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Actor actor , IFormFile? image , CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
                return View(actor);

            // image upload 

            if(image != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\actors", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    image.CopyTo(stream);
                }
                // save img in db
                actor.Image = fileName;
            }
           await _actorRepository.AddAsync(actor , cancellationToken);
            await _actorRepository.CommitAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        //---------Edit----------//
        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(int id , CancellationToken cancellationToken) 
        {
            var actor = await _actorRepository.GetOneAsync(a => a.Id == id, cancellationToken: cancellationToken);
            if(actor == null)
                return NotFound();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(Actor actor , IFormFile? image, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(actor);

            var actorInDb =await _actorRepository.GetOneAsync(e => e.Id == actor.Id , tracked: false , cancellationToken : cancellationToken);
            if(actorInDb != null)
                return NotFound();
            // image upload 

            if (image != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\actors", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    image.CopyTo(stream);
                }
                // save img in db
                actor.Image = fileName;
            }
            else
            {
                actor.Image = actorInDb.Image;
            }
            _actorRepository.Update(actor);
            await _actorRepository.CommitAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        //---------Delete----------//
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Delete(int id , CancellationToken cancellationToken)
        {
            var actor =await _actorRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
            if (actor == null)
                return NotFound();

            _actorRepository.Delete(actor);
            await _actorRepository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}
