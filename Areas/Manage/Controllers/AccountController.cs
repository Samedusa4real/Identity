using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokTemplate.Areas.Manage.ViewModels;
using PustokTemplate.Models;

namespace PustokTemplate.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser user = new AppUser
        //    {
        //        UserName = "admin",
        //        IsAdmin = true,
        //    };

        //    var result = await _userManager.CreateAsync(user,"Admin123");

        //    return Json(result);
        //}

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginViewModel adminVM, string returnUrl = null)
        {
            AppUser appUser = await _userManager.FindByNameAsync(adminVM.UserName);

            if (appUser == null || !appUser.IsAdmin)
            {
                ModelState.AddModelError("", "Username or Password incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(appUser, adminVM.Password,false,false);

            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password incorrect!");
                return View();
            }

            if (returnUrl!=null)
                return  Redirect(returnUrl);

            return RedirectToAction("Index","Dashboard");
        }
        
        public IActionResult ShowUser()
        {
            return Json(new
            {
                isAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
            });
        }
    }
}
