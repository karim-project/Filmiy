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
    public class CategoriesController : Controller
    {
        //ApplicationDbContext _context = new();
        private readonly IRepository<Category> _categoryRepository;// = new Repository<Category>();

        public CategoriesController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAsync(tracked: false, cancellationToken: cancellationToken);
           
            return View(categories.AsEnumerable());
        }

        //---------Create----------//
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category , CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) 
            {
                return View(category);

            }
            await _categoryRepository.AddAsync(category , cancellationToken : cancellationToken);
            await _categoryRepository.CommitAsync(cancellationToken);

            return RedirectToAction("Index");
        }

        //---------Edit----------//
        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(int id ,CancellationToken cancellationToken)
        {
            var category =await _categoryRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
            if (category == null)
                return NotFound();
            return View(category);
        }
        [HttpPost]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(Category category , CancellationToken cancellationToken)
        {
            if (ModelState.IsValid) 
            {
                _categoryRepository.Update(category);
               await _categoryRepository.CommitAsync(cancellationToken);
                return RedirectToAction("Index");
            }
            
            return View(category);
        }

        //---------Delete----------//
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE} , {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Delete(int id , CancellationToken cancellationToken) 
        {
            var category =await _categoryRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
            if (category == null)
                return NotFound();


            _categoryRepository.Delete(category);
            await _categoryRepository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

    }
}
