using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WMS_Nikkosoft.Data;
using WMS_Nikkosoft.Data.Static;
using WMS_Nikkosoft.Data.ViewModels;
using WMS_Nikkosoft.Models;

namespace WMS_Nikkosoft.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<IActionResult> Users()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var empresa = currentUser?.idCliente;
            if (string.IsNullOrEmpty(empresa))
            {
                empresa = "envios";
            }
            var users = await _context.Users.Where(x => x.idCliente == empresa).ToListAsync();
            return View(users);
        }

        public IActionResult Login() => View(new LoginVM());

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByNameAsync(loginVM.UserName);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                TempData["Error"] = "Algo esta mal, intenta de nuevo!";
                return View(loginVM);
            }

            TempData["Error"] = "algo esta mal, intenta de nuevo!";
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult Register() => View(new RegisterVM());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {


            var currentUser = await _userManager.GetUserAsync(User);

            var empresa = currentUser.idCliente;

            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "esta direccion de correo esta en uso";
                return View(registerVM);
            }
            var nombre = await _userManager.FindByNameAsync(registerVM.UserName);
            if (nombre != null)
            {
                TempData["Error"] = "este nombre de usuario ya esta en uso";
                return View(registerVM);
            }
            var newUser = new ApplicationUser()
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.UserName,
                cedula = registerVM.cedula,
                rif = registerVM.rif,
                direccion = registerVM.CodigoVendedor,
                PhoneNumber = registerVM.phoneNumber,
                idCliente = empresa,
                Tipo = "Vendedor",

            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Vendedor);

            }


            return View("RegisterCompleted");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }

        public async Task<IActionResult> Editar(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }


            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(string Id, ApplicationUser applicationUser)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }




            user.FullName = applicationUser.FullName; ;
            user.UserName = applicationUser.UserName;
            user.Email = applicationUser.Email;
            user.PhoneNumber = applicationUser.PhoneNumber;
            user.cedula = applicationUser.cedula;
            user.direccion = applicationUser.direccion;
            user.rif = applicationUser.rif;
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Users", "Account");
        }
    }
}
