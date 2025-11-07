using Filmiy.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Filmiy.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<ApplicationUserOTP> _applicationUserOTPRepository;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender , IRepository<ApplicationUserOTP> applicationUserOTPRepository)
        {
            _userManager = userManager;
            _signinManager = signInManager;
            _emailSender = emailSender;
           _applicationUserOTPRepository = applicationUserOTPRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            var user = new ApplicationUser()
            {
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Email = registerVM.Email,
                UserName = registerVM.UserName,
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Code);
                }
                return View(registerVM);
            }

            //send EmailConfirm

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", token, userId = user.Id }, Request.Scheme);

            await _emailSender.SendEmailAsync(registerVM.Email, "Flimiy - Confirm Your Email!"
                 , $"<h1>Confirm Your Email By Clicking <a href='{link}'>Here</a></h1>");


            return RedirectToAction("login");
        }

        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByNameAsync(loginVM.UserNameOREmail) ?? await _userManager.FindByEmailAsync(loginVM.UserNameOREmail);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid User Name / Email OR Password");
                return View(loginVM);
            }



            var result = await _signinManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    ModelState.AddModelError(string.Empty, "Too many attemps, try again after 5 min");
                else if (!user.EmailConfirmed)
                    ModelState.AddModelError(string.Empty, "Please Confirm Your Email First!!");
                else
                    ModelState.AddModelError(string.Empty, "Invalid User Name / Email OR Password");

                return View(loginVM);
            }

            return RedirectToAction("Index", "Home", new { area = "User" });
        }

        public IActionResult ResendConfirmEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendConfirmEmail(ResendConfirmEmailVM resendConfirmEmailVM)
        {
            if (!ModelState.IsValid)
                return View(resendConfirmEmailVM);

            var user = await _userManager.FindByNameAsync(resendConfirmEmailVM.UserNameOREmail) ?? await _userManager.FindByEmailAsync(resendConfirmEmailVM.UserNameOREmail);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid User Name / Email");
                return View(resendConfirmEmailVM);
            }
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Already Confirmed!");
                return View(resendConfirmEmailVM);
            }
            //send EmailConfirm

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", token, userId = user.Id }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email!, "Flimiy - Resend Confirm Your Email!"
                 , $"<h1>Confirm Your Email By Clicking <a href='{link}'>Here</a></h1>");


            return RedirectToAction("login");
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            if (!ModelState.IsValid)
                return View(forgetPasswordVM);

            var user = await _userManager.FindByNameAsync(forgetPasswordVM.UserNameOREmail) ?? await _userManager.FindByEmailAsync(forgetPasswordVM.UserNameOREmail);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid User Name / Email");
                return View(forgetPasswordVM);
            }

            var userOTPs =await _applicationUserOTPRepository.GetAsync(e =>e.ApplicationUserId ==user.Id);

            var totalOTP = userOTPs.Count(e => (DateTime.UtcNow - e.CreateAt).TotalHours<24);

            if(totalOTP > 3)
            {
                ModelState.AddModelError(string.Empty, "Too Many Attemps");
                return View(forgetPasswordVM);
            }

            var otp =new Random().Next(1000 , 9999).ToString();

            await _applicationUserOTPRepository.AddAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                CreateAt = DateTime.UtcNow,
                IsValid = true,
                OTP = otp,
                ValidTo =  DateTime.UtcNow.AddDays(1),
            });
           await _applicationUserOTPRepository.CommitAsync();

            await _emailSender.SendEmailAsync(user.Email!, "FILIMY - Reset Your Password"
                , $"<h1>Use This OTP: {otp} To Reset Your Account. Don't share it.</h1>");

            return RedirectToAction("ValidateOTP", new { userId = user.Id });
        }

        public IActionResult ValidateOTP(string userId)
        {
            return View(new ValidateOTPVM
            {
                ApplicationUserId = userId,
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateOTP(ValidateOTPVM validateOTPVM)
        {
            var result =await _applicationUserOTPRepository.GetOneAsync(e => e.ApplicationUserId == validateOTPVM.ApplicationUserId && e.OTP == validateOTPVM.OTP && e.IsValid);
            if(result is null)
            {
                return RedirectToAction(nameof(ValidateOTP), new {userId = validateOTPVM.ApplicationUserId});
            }

            return RedirectToAction("NewPassword" , new { userId = validateOTPVM.ApplicationUserId });
        }

        public IActionResult NewPassword(string userId)
        {
            return View(new NewPasswordVM
            {
                ApplicationUserId = userId,
            });

        }
        [HttpPost]
        public async Task<IActionResult> NewPassword(NewPasswordVM newPasswordVM)
        {
            var user =await _userManager.FindByIdAsync(newPasswordVM.ApplicationUserId);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid User Name / Email OR Password");
                return View(newPasswordVM);
            }

            var token =await  _userManager.GeneratePasswordResetTokenAsync(user);

            var result =await _userManager.ResetPasswordAsync(user, token ,newPasswordVM.NewPassword );

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Code);
                }

                return View(newPasswordVM);
            }

            return RedirectToAction("login");

        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                TempData["error-notification"] = "Invalid OR Expired Token";
            }
            else
            {
                TempData["success-notification"] = "Confirm Email Successfully";
            }
            return RedirectToAction("Login");
        }
    }
}
