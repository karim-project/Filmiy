using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Threading.Tasks;

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

      

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var user =await _userManager.GetUserAsync(User);

            if (user is null)
                return NotFound();

            var cart =await _cartRepository.GetAsync(e => e.ApplicationUserId == user.Id, includes: [e => e.Movie, e => e.ApplicationUser] , cancellationToken : cancellationToken);
            
            return View(cart);
        }

        public IActionResult Seats() 
        {
            return View();
        }

        public async Task<IActionResult> AddToCart(int count , int movieId , CancellationToken cancellationToken) 
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return NotFound();

            var movieInDB =await _cartRepository.GetOneAsync(e=>e.ApplicationUserId == user.Id && e.MovieId== movieId);

            if (movieInDB is not null) 
            {
                movieInDB.Count += count;
                await _cartRepository.CommitAsync();

                TempData["success-notification"] = "Update Movie Count to cart successfully";

                return RedirectToAction("index", "Home");
            }
            await _cartRepository.AddAsync(new()
            {
                MovieId = movieId,
                Count = count,
                ApplicationUserId = user.Id,
                Price = (await _movieRepository.GetOneAsync(e => e.Id == movieId)!).Price
            }, cancellationToken : cancellationToken);
           await _cartRepository.CommitAsync(cancellationToken);

            TempData["success-notification"] = "Update Movie Count to cart successfully";

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> DeleteMovie(int movieId , CancellationToken cancellationToken ) 
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return NotFound();

            var movie =await _cartRepository.GetOneAsync(e=>e.ApplicationUserId == user.Id&&e.MovieId== movieId, cancellationToken:cancellationToken);

            if (movie is null)
                return NotFound();

            _cartRepository.Delete(movie);
          await  _cartRepository.CommitAsync(cancellationToken);

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Pay()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return NotFound();

            var cart = await _cartRepository.GetAsync(e=>e.ApplicationUserId== user.Id , includes: [e=> e.Movie]);

            if (cart is null)
                return NotFound();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/customer/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/customer/checkout/cancel",
            };

            foreach (var item in cart)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Movie.Name,
                            Description = item.Movie.Description,
                        },
                        UnitAmount = (long)item.Price * 100,
                    },
                    Quantity = item.Count,
                });
            }

            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }
    }
}
