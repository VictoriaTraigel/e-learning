using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeshBrain.Models;
using LeshBrain.ViewModels;

namespace LeshBrain.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        ContextDB _context;
        RoleManager<IdentityRole<int>> _roleManager;
        UserManager<UserEntity> _userManager;
        public AdminController(ContextDB context,RoleManager<IdentityRole<int>> roleManager,UserManager<UserEntity> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            IndexAdminViewModel model = new IndexAdminViewModel();
            model.Tests = _context.Tests.ToList();
            foreach (var user in _context.Users.ToList())
            {
                UserViewModel userModel = new UserViewModel()
                {
                    User = user,
                    UserRoles = await _userManager.GetRolesAsync(user)
                };
                if (userModel.UserRoles.Contains("employee")) model.AmountClients++;
                if (userModel.UserRoles.Contains("anon")) model.AmountAnons++;
                model.Users.Add(userModel);
            }

            return View(model);
        }

        [NonAction]
        public async Task<IndexAdminViewModel> InitializeModel()
        {
            IndexAdminViewModel model = new IndexAdminViewModel();
            model.Tests = _context.Tests.ToList();
            foreach(var user in _context.Users)
            {
                UserViewModel userModel = new UserViewModel()
                {
                    User = user,
                    UserRoles =await _userManager.GetRolesAsync(user)
                };
                if (userModel.UserRoles.Contains("employee")) model.AmountClients++;
                if (userModel.UserRoles.Contains("anon")) model.AmountAnons++;
                model.Users.Add(userModel);
            }
            return model;
        }
    }
}
