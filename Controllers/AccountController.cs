using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeshBrain.Models;
using LeshBrain.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeshBrain.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ContextDB _context;
        public AccountController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager,ContextDB context, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("Account/Login")]
        public IActionResult Index(string returnUrl=null)
        {
            return View(new RegisterViewModel { ReturnUrl=returnUrl});
        }

        [HttpPost]
        [Route("/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                UserEntity newUser = new UserEntity()
                {
                    UserName = model.UserName,
                };
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if(result.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(newUser, "anon");
                    if(result.Succeeded) await _signInManager.SignInAsync(newUser,false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return RedirectToAction("Index", "Home"); ;
        }

        [HttpPost]
        [Route("/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true , false);
                if (result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }


        [HttpGet]
        [Authorize(Roles =("admin,mentor"))]
        public async Task<IActionResult> Accounts(string username,int? roleId)
        {
            UserListFiltrViewModel model = new UserListFiltrViewModel();
            List<IdentityRole<int>> roles = _context.Roles.ToList();
            roles.Insert(0, new IdentityRole<int>() { Name = "Все", Id = 0 });
            model.RolesList = new SelectList(roles, "Id", "Name");
            foreach (var user in _context.Users.ToList())
            {
                UserViewModel userModel = new UserViewModel()
                {
                    User = user,
                    UserRoles = await _userManager.GetRolesAsync(user)
                };
                if (String.IsNullOrEmpty(username) && (roleId == null || roleId == 0)) model.Users.Add(userModel);
                else
                {
                    if (!String.IsNullOrEmpty(username))
                    {
                        if (user.UserName.Equals(username)) model.Users.Add(userModel);
                    }
                    if (roleId != null && roleId != 0)
                    {
                        IdentityRole<int> role = await _roleManager.FindByIdAsync(roleId.ToString());
                        if (role != null)
                        {
                            if (await _userManager.IsInRoleAsync(user, role.Name)) model.Users.Add(userModel);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [Route("Account/userinfo")]
        public async Task<IActionResult> UserInfo(string? idUser)
        {
            AccountSettingViewModel model = new AccountSettingViewModel();
            UserEntity user = new UserEntity();
            if(idUser!=null)
            {
                user = await _userManager.FindByIdAsync(idUser);
                model.edit.Name = user.Name;
                model.edit.Surname = user.Surname;
                model.edit.Email = user.Email;
                model.edit.Phone = user.PhoneNumber;
                model.edit.Patronym = user.Patronym;
                model.edit.ImageUrl = user.ImageUrl;
                model.edit.Id = int.Parse(idUser);
                model.changeRole.AllRoles = _roleManager.Roles.ToList();
                model.changeRole.UserRoles =await _userManager.GetRolesAsync(user);
                model.changeRole.Id = user.Id;
                model.changeRole.UserName= user.UserName;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountSettingViewModel model)
        {
            if(ModelState.IsValid)
            {
                UserEntity user = await _userManager.FindByIdAsync(model.edit.Id.ToString());
                if(user!=null)
                {
                    user.Email = model.edit.Email;
                    user.PhoneNumber = model.edit.Phone;
                    user.Name = model.edit.Name;
                    user.Surname = model.edit.Surname;
                    user.Patronym = model.edit.Patronym;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Accounts");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return RedirectToAction("Accounts");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            UserEntity user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Accounts");
        }


        [HttpGet]
        [Authorize(Roles =("admin"))]
        public IActionResult Create()
        {
            CreateUserViewModel model = new CreateUserViewModel();
            model.AllRoles = _roleManager.Roles.ToList();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = ("admin"))]
        public async Task<IActionResult> Create(CreateUserViewModel model,List<string> roles)
        {
            if(ModelState.IsValid)
            {
                UserEntity newuser = new UserEntity()
                {
                    Name=model.Name,
                    Surname=model.Surname,
                    Patronym=model.Patronym,
                    Email=model.Email,
                    PhoneNumber=model.Phone,
                    UserName=model.UserName
                };
                IdentityResult result = await _userManager.CreateAsync(newuser, model.Password);
                if(result.Succeeded)
                {
                    if (roles.Count != 0)
                    {
                        await _userManager.AddToRolesAsync(newuser, roles);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newuser, "anon");
                    }
                }
            }
            return RedirectToAction("Accounts");
        }

        [Authorize(Roles ="admin,mentor")]
        public async Task<IActionResult> Clients(string? name, string? surname, string? patron, string? phone, string? Email)
        {

            var clients = await _userManager.GetUsersInRoleAsync("employee");
            MentorsSearchClientsViewModel model = new MentorsSearchClientsViewModel();
            foreach (var user in clients)
            {
                int check = 0;
                if (!String.IsNullOrEmpty(name))
                {
                    check++;
                    if(user.Name==name) model.Users.Add(user);
                }
                if (!String.IsNullOrEmpty(surname))
                {
                    check++;
                    if (user.Name == surname) model.Users.Add(user);
                }
                if (!String.IsNullOrEmpty(patron))
                {
                    check++;
                    if (user.Name == patron) model.Users.Add(user);
                }
                if (!String.IsNullOrEmpty(phone))
                {
                    check++;
                    if (user.Name == phone) model.Users.Add(user);
                }
                if (!String.IsNullOrEmpty(Email))
                {
                    check++;
                    if (user.Name == Email) model.Users.Add(user);
                }
                if(!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(surname) && !String.IsNullOrEmpty(patron) && !String.IsNullOrEmpty(phone) && !String.IsNullOrEmpty(Email))
                {
                    check++;
                    if (user.Name == Email && user.Name == phone && user.Name == patron && user.Name == surname && user.Name == name) model.Users.Add(user);
                }
                if (check == 0) model.Users.Add(user);
            }
            return View(model);
        }


        [Authorize(Roles = "admin,mentor")]
        public async Task<IActionResult> Admins(string? name, string? surname, string? patron, string? phone, string? Email)
        {

            var clients = await _userManager.GetUsersInRoleAsync("admin");
            MentorsSearchClientsViewModel model = new MentorsSearchClientsViewModel();
            foreach (var user in clients)
            {
                int check = 0;
                if (!String.IsNullOrEmpty(name))
                {
                    check++;
                    if (user.Name == name) model.Users.Add(user);
                }
                if (!String.IsNullOrEmpty(surname))
                {
                    check++;
                    if (user.Name == surname) model.Users.Add(user);
                }
                if (!String.IsNullOrEmpty(patron))
                {
                    check++;
                    if (user.Name == patron) model.Users.Add(user);
                }
                if (!String.IsNullOrEmpty(phone))
                {
                    check++;
                    if (user.Name == phone) model.Users.Add(user);
                }
                if (!String.IsNullOrEmpty(Email))
                {
                    check++;
                    if (user.Name == Email) model.Users.Add(user);
                }
                if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(surname) && !String.IsNullOrEmpty(patron) && !String.IsNullOrEmpty(phone) && !String.IsNullOrEmpty(Email))
                {
                    check++;
                    if (user.Name == Email && user.Name == phone && user.Name == patron && user.Name == surname && user.Name == name) model.Users.Add(user);
                }
                if (check == 0) model.Users.Add(user);
            }
            return View(model);
        }
        [Authorize(Roles = "admin,mentor,employee")]
        public async Task<IActionResult> Mentors(string? name, string? surname, string? patron, string? phone, string? Email)
        {

            var clients = await _userManager.GetUsersInRoleAsync("mentor");
            MentorsSearchClientsViewModel model = new MentorsSearchClientsViewModel();
            foreach (var user in clients)
            {
                int check = 0;
                if (!String.IsNullOrEmpty(name))
                {
                    check++;
                    if (user.Name == name) model.Users.Add(user);
                }
                if (!String.IsNullOrEmpty(surname))
                {
                    check++;
                    if (user.Name == surname) model.Users.Add(user);
                }
                if (!String.IsNullOrEmpty(patron))
                {
                    check++;
                    if (user.Name == patron) model.Users.Add(user);
                }
                if (!String.IsNullOrEmpty(phone))
                {
                    check++;
                    if (user.Name == phone) model.Users.Add(user);
                }
                if (!String.IsNullOrEmpty(Email))
                {
                    check++;
                    if (user.Name == Email) model.Users.Add(user);
                }
                if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(surname) && !String.IsNullOrEmpty(patron) && !String.IsNullOrEmpty(phone) && !String.IsNullOrEmpty(Email))
                {
                    check++;
                    if (user.Name == Email && user.Name == phone && user.Name == patron && user.Name == surname && user.Name == name) model.Users.Add(user);
                }
                if (check == 0) model.Users.Add(user);
            }
            return View(model);
        }
    }
}
