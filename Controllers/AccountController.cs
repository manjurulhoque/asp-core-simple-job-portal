using JobPortal.Models;
using JobPortal.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace JobPortal.Controllers
{
    [Route("accounts/")]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("employer/register")]
        public IActionResult EmployerRegister()
        {
            return View();
        }

        [HttpPost]
        [Route("employer/register")]
        public async Task<IActionResult> EmployerRegister(
            [Bind("FirstName", "LastName", "Email", "Password", "ConfirmPassword")]
            EmployerRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.FirstName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                //IdentityResult roleResult = await _roleManager.CreateAsync(new IdentityRole("Employee"));

                if (result.Succeeded)
                {
                    bool checkRole = await _roleManager.RoleExistsAsync("Employer");
                    if (!checkRole)
                    {
                        var role = new IdentityRole();
                        role.Name = "Employer";
                        await _roleManager.CreateAsync(role);

                        await _userManager.AddToRoleAsync(user, "Employer");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Employer");
                    }

                    //await _signManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View();
        }

        [HttpGet]
        [Route("employee/register")]
        public IActionResult EmployeeRegister()
        {
            return View();
        }

        [HttpPost]
        [Route("employee/register")]
        public async Task<IActionResult> EmployeeRegister(
            [Bind("FirstName", "LastName", "Email", "Password", "ConfirmPassword")]
            EmployeeRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.FirstName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                //IdentityResult roleResult = await _roleManager.CreateAsync(new IdentityRole("Employee"));

                if (result.Succeeded)
                {
                    bool checkRole = await _roleManager.RoleExistsAsync("Employee");
                    if (!checkRole)
                    {
                        var role = new IdentityRole();
                        role.Name = "Employee";
                        await _roleManager.CreateAsync(role);

                        await _userManager.AddToRoleAsync(user, "Employee");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Employee");
                    }

                    //await _signManager.SignInAsync(user, false);
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = "")
        {
            var model = new LoginViewModel {ReturnUrl = returnUrl};
            return View(model);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

                var userName = user.UserName;

                var result = await _signManager.PasswordSignInAsync(userName,
                    model.Password, model.RememberMe, lockoutOnFailure: false);


                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        [Route("employee/edit-profile")]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return View(user);
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        [Route("employee/update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] User model)
        {
//            _logger.LogError(model.Gender.ToString());
            var user = await _userManager.GetUserAsync(HttpContext.User);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Gender = model.Gender;
            
            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return RedirectToActionPermanent("EditProfile", "Account");
        }
    }
}