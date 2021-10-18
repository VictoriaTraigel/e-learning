using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using LeshBrain.Models;
using LeshBrain.ViewModels;

namespace LeshBrain.Controllers
{
    public class MentorController : Controller
    {
        private readonly ContextDB _context;
        private readonly UserManager<UserEntity> _userManager;
        public MentorController(ContextDB context,UserManager<UserEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
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
                if (userModel.UserRoles.Contains("employee"))
                {
                    model.AmountClients++;
                    model.Users.Add(userModel);
                }
                if (userModel.UserRoles.Contains("anon")) model.AmountAnons++;
            }
            return View(model);
        }
    }
}
