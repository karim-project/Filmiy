using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Filmiy.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class CartController : Controller
    {  
        public IRepository<Cart> _cartRepository { get; }
        public UserManager<ApplicationUser> _userManager { get; }
        public IRepository<Movie> _movieRepository { get; }

        public CartController(IRepository<Cart> cartRepository,UserManager<ApplicationUser> userManager ,IRepository<Movie> movieRepository) 
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _movieRepository = movieRepository;
        }

      

        public IActionResult Index()
        {
            return View();
        }
    }
}
