using LeshBrain.Models;
using LeshBrain.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeshBrain.Controllers
{
    public class ClientController : Controller
    {
        ContextDB _context;
        RoleManager<IdentityRole<int>> _roleManager;
        UserManager<UserEntity> _userManager;
        public ClientController(ContextDB context, RoleManager<IdentityRole<int>> roleManager, UserManager<UserEntity> userManager)
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
    }
}
