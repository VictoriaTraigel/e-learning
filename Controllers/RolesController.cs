using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LeshBrain.Models;
using LeshBrain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeshBrain.Controllers
{
    public class RolesController : Controller
    {
        RoleManager<IdentityRole<int>> _roleManager;
        UserManager<UserEntity> _userManager;
        public RolesController(RoleManager<IdentityRole<int>> roleManager,UserManager<UserEntity> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string? name)
        {
            List<IdentityRole<int>> list = new List<IdentityRole<int>>();
            if (!string.IsNullOrEmpty(name))
            {
                IdentityRole<int> role = await _roleManager.FindByNameAsync(name);
                list.Add(role);
            }
            else list = _roleManager.Roles.ToList();
            return View(list);
        }
        
        [HttpPost]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> UpdateRole(string? userid,List<string> roles)
        {
            UserEntity user = await _userManager.FindByIdAsync(userid);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);

                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();

                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);

                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("Accounts","Account");
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Create(string? nameRole)
        {
            if(!string.IsNullOrEmpty(nameRole))
            {
                IdentityRole<int> newRole = new IdentityRole<int>()
                {
                    Name = nameRole
                };
                IdentityResult result = await _roleManager.CreateAsync(newRole);
                if(result.Succeeded)
                {

                }
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string? id)
        {
            IdentityRole<int> deletedRole = await _roleManager.FindByIdAsync(id);
            if(deletedRole!=null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(deletedRole);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Edit(string? id)
        {
            IdentityRole<int> role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                EditRoleViewModel model = new EditRoleViewModel()
                {
                    Id = role.Id,
                    Name = role.Name
                };
                return View(model);
            }
            else return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            IdentityRole<int> role = await _roleManager.FindByIdAsync(model.Id.ToString());
            if(role!=null)
            {
                role.Name = model.Name;
                IdentityResult result = await _roleManager.UpdateAsync(role);
            }
            return RedirectToAction("Index");
        }
    }
}
