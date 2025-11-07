using Filmiy.Data;
using Filmiy.Models;
using Filmiy.Utitlies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Filmiy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE} , {SD.EMPLOYEE_ROLE}")]
    public class CinemaController : Controller
    {
        //ApplicationDbContext _context = new();
        private readonly IRepository<Cinema> _cinemaRepository; //= new();

        public CinemaController(IRepository<Cinema> cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var cinemas = await _cinemaRepository.GetAsync(tracked: false, cancellationToken: cancellationToken);
            return View(cinemas);
        }

        //---------Create----------//

        [HttpGet]
        public IActionResult Create()
        { 
        return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Cinema cinema , IFormFile? image ,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) 
                return View(cinema);

            // image Upload
            if(image != null && image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath =Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\cinemas", fileName);

                using(var stream = System.IO.File.Create(filePath))
                {
                    image.CopyTo(stream);
                }
                // save img in db
                cinema.Image = fileName;
            }
           await _cinemaRepository.AddAsync(cinema);
            await _cinemaRepository.CommitAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }


        //---------Edit----------//
        
        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(int id , CancellationToken cancellationToken)
        {
            var cinema = await _cinemaRepository.GetOneAsync(c => c.Id == id , cancellationToken : cancellationToken);
            if(cinema== null)
                return NotFound();

            return View(cinema);
        }
        [HttpPost]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(Cinema cinema , IFormFile? image , CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(cinema);

            var cinemaInDb =await _cinemaRepository.GetOneAsync(c => c.Id == cinema.Id , tracked:false  , cancellationToken: cancellationToken);

            if(cinemaInDb== null)
                return NotFound();

            // image Upload
            if (image != null && image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\cinemas", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    image.CopyTo(stream);
                }
                // save img in db
                cinema.Image = fileName;
            }
            else
            {
                cinema.Image = cinemaInDb.Image;
            }

            _cinemaRepository.Update(cinema);
           await _cinemaRepository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }



        //---------Delete----------//
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Delete(int id , CancellationToken cancellationToken) 
        {
            var cinema = await _cinemaRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);

            if (cinema == null)
                return NotFound();

           

                var OldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\cinemas", cinema.Image);

                if (System.IO.File.Exists(OldPath))
                {
                    System.IO.File.Delete(OldPath);
                }
            

          
            _cinemaRepository.Delete(cinema);
            await _cinemaRepository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}
