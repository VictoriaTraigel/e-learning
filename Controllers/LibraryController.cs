using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeshBrain.Models;
using LeshBrain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace LeshBrain.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ContextDB _context;

        private readonly IWebHostEnvironment _appEnvironment;
        public LibraryController(ContextDB context, IWebHostEnvironment app)
        {
            _context = context;
            _appEnvironment = app;
        }

        [HttpGet]
        public IActionResult Index(string? name,int? idcategory)
        {
            bool check = false;
            LibraryListViewModel model = new LibraryListViewModel();
            var books = _context.Books.Include(p => p.Category);
            List<Category> categories = _context.Categories.ToList();
            categories.Insert(0, new Category() { Name = "Все", Id = 0,Description="" });
            model.ListCategory = new SelectList(categories, "Id", "Name");
            if (!String.IsNullOrEmpty(name))
            {
                model.Books = books.Where(p => p.Name == name);
                check = true;
            }
            if (idcategory != 0 && idcategory != null)
            {
                model.Books = books.Where(p => p.CategoryId == idcategory);
                check = true;
            }
            if(!check)
            {
                model.Books = books;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Info(string idBook)
        {
            InfoBooksViewModel model = new InfoBooksViewModel();
            int id = int.Parse(idBook);

            var books = _context.Books.Include(p => p.Category).Where(p => p.Id == id);

            Book book = books.First();
            if(book!=null)
            {
                model.Book = book;
                IEnumerable<Category> categories = _context.Categories.ToList().Where(p=>p.Name!=book.Category.Name);
                model.ListCategory = new SelectList(categories, "Id", "Name");
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles ="mentor,admin,employee")]
        public async Task<IActionResult> Edit(InfoBooksViewModel model, IFormFile newFile, string categoryID)
        {
            Book update = _context.Books.Find(model.Book.Id);
            update.Name = model.Book.Name;
            update.Description = model.Book.Description;
            if (categoryID!="0")
            {
                int idCategory = int.Parse(categoryID);
                update.CategoryId = idCategory;
            }
            if(newFile != null)
            {
                string path = "/Content/Library/" + newFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await newFile.CopyToAsync(fileStream);
                }
                update.ResourceURL = path;
            }
            _context.Books.Update(update);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "mentor,admin")]
        public async Task<IActionResult> Add()
        {
            InfoBooksViewModel model = new InfoBooksViewModel();
            IEnumerable<Category> categories = _context.Categories.ToList();
            model.ListCategory = new SelectList(categories, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "mentor,admin")]
        public async Task<IActionResult> Add(InfoBooksViewModel model, string categoryID, IFormFile newFile)
        {
            Book book = new Book()
            {
                Name = model.Book.Name,
                Description = model.Book.Description
            };
            int idCategory = int.Parse(categoryID);
            book.CategoryId = idCategory;
            if (newFile != null)
            {
                string path = "/Content/Library/" + newFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await newFile.CopyToAsync(fileStream);
                }
                book.ResourceURL = path;
                _context.Books.Add(book);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "mentor,admin")]
        public IActionResult Delete(int id)
        {
            Book book = _context.Books.Find(id);
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
