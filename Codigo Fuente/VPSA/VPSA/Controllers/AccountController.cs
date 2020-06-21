using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VPSA.Data;
using VPSA.Models;

namespace VPSA.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly VPSAContext _context;
        private readonly SignInManager<User> _signInManager;

        public AccountController(IMapper mapper, UserManager<User> userManager, VPSAContext context, SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var vPSAContext = await _context.Users.ToListAsync();
            return View(vPSAContext);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var nroLEgajo = 0;
            if (_context.Users.Count() > 0)
            {
                nroLEgajo = _context.Users.Count();
            }
            nroLEgajo = nroLEgajo == 0 ? 1 : nroLEgajo + 1;
            userModel.Legajo = Convert.ToInt32($"2020{nroLEgajo.ToString().PadLeft(2, '0')}");

            var user = _mapper.Map<User>(userModel);

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return View(userModel);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel userModel, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var user = await _context.Users.Where(x => x.Legajo == userModel.Legajo).FirstOrDefaultAsync();

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, userModel.Password, userModel.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Denuncias");
                }
                else
                {
                    ModelState.AddModelError("", "Legajo o Contraseña Inválidos");
                    return View();
                }
            }
            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");

        }
    }
}