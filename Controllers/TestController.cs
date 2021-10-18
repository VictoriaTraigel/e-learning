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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LeshBrain.Controllers
{
    public class TestController : Controller
    {
        private readonly ContextDB _context;
        public TestController(ContextDB context)
        {
            _context = context;
        }


        [Authorize(Roles =("admin,mentor,employee"))]
        public IActionResult Index(string? name,int? id)
        {
            bool check = false;
            TestListViewModel model = new TestListViewModel();
            var test = _context.Tests.ToList();
            List<Category> categories = _context.Categories.ToList();
            categories.Insert(0, new Category() { Name = "Все", Id = 0, Description = "" });
            model.ListCategory = new SelectList(categories, "Id", "Name");
            if (!String.IsNullOrEmpty(name))
            {
                model.Test = test.Where(p => p.Name == name);
                check = true;
            }
            if (id != 0 && id != null)
            {
                model.Test = test.Where(p => p.CategoryId == id);
                check = true;
            }
            if (!check)
            {
                model.Test = test;
            }
            return View(model);
        }

        [Authorize(Roles = ("admin,mentor,employee"))]
        public IActionResult Info(int id)
        {
            InfoTestViewModel model = new InfoTestViewModel();
            IQueryable<Test> tests = _context.Tests.Include(p=>p.Category).Where(p=>p.Id==id);
            Test test = tests.First();
            model.Test = test;

            IEnumerable<Category> categories = _context.Categories.ToList().Where(p=>p.Id!=test.CategoryId);
            model.ListCategory = new SelectList(categories, "Id", "Name");

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = ("admin,mentor"))]
        public IActionResult Delete(int id)
        {
            Test test = _context.Tests.Find(id);
            if(test!=null)
            {
                _context.Tests.Remove(test);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = ("admin,mentor"))]
        public IActionResult Add()
        {
            InfoTestViewModel model = new InfoTestViewModel();
            model.Test = new Test();

            IEnumerable<Category> categories = _context.Categories.ToList();
            model.ListCategory = new SelectList(categories, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = ("admin,mentor"))]
        public IActionResult Add(InfoTestViewModel model,int categoryID)
        {
            if(ModelState.IsValid)
            {
                Test test = new Test()
                {
                    Name=model.Test.Name,
                    Description=model.Test.Description,
                    AmountQuestion=model.Test.AmountQuestion,
                    CategoryId= categoryID,
                    TopicId=1
                };
                _context.Tests.Add(test);
                _context.SaveChanges();
                return RedirectToAction("FillContext",new { idTest = test.Id });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = ("admin,mentor"))]
        public IActionResult FillContext(int idTest,int page=1)
        {
            Test test = _context.Tests.Find(idTest);
            if (test != null)
            {
                TestPassViewModel model = new TestPassViewModel();
                model.Test = test;
                model.Questions.TestId = test.Id;
                model.Passer.PageNumber = page;
                model.Passer.TotalPages = test.AmountQuestion;
                return View(model);
            }
            else return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = ("admin,mentor"))]
        public IActionResult FillContext(TestPassViewModel model,List<string> answers,List<string> isTrue)
        {
            model.Passer.PageNumber++;
            Question question = new Question();
            question.TestId = model.Test.Id;
            question.Content = model.Questions.Content;
            _context.Questions.Add(question);
            _context.SaveChanges();

            int amountTrueAnswers = isTrue.Count();
            for(int i=0,tr=0;i<answers.Count;i++)
            {
                Answer answer = new Answer();
                for(int j=0;j<isTrue.Count;j++)
                {
                    if(i==int.Parse(isTrue[j])) answer.RightAnswer = true;
                }
                answer.Content = answers[i];
                answer.QuestionId = question.Id;
                _context.Add(answer);
                tr++;
            }
            _context.SaveChanges();
            if(model.Passer.PageNumber > model.Test.AmountQuestion)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("FillContext", new { idTest = model.Test.Id, page = model.Passer.PageNumber });
        }

        [HttpGet]
        [Authorize(Roles = ("admin,mentor,employee"))]
        public IActionResult CompleteTest(int id,int page=0)
        {
            var tests = _context.Tests.Include(p => p.Questions).ThenInclude(a => a.Answers).Where(p=>p.Id==id);
            Test mainTest = tests.First();
            if(mainTest!=null)
            {
                TestComplPassViewModel model = new TestComplPassViewModel();
                model.Test = mainTest;
                model.Passer.PageNumber = page;
                model.Passer.TotalPages = mainTest.AmountQuestion;
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = ("admin,mentor,employee"))]
        public IActionResult CompleteTest(TestComplPassViewModel model, List<string> isTrue)
        {
            var tests = _context.Tests.Include(p => p.Questions).ThenInclude(a => a.Answers).Where(p => p.Id == model.Test.Id);
            Test mainTest = tests.First();
            model.Test = mainTest;
            if (mainTest != null)
            {

                int numQ = model.Passer.PageNumber;
                model.Passer.PageNumber++;
                List<Answer> answers = mainTest.Questions[numQ].Answers;
                int maxresult = 0;
                for(int i=0;i<answers.Count;i++)
                {
                    if (answers[i].RightAnswer) maxresult++;
                }
                int currentresult = maxresult;
                for(int i=0;i<isTrue.Count;i++)
                {
                    int num = int.Parse(isTrue[i]);
                    if (!answers[num].RightAnswer) currentresult--;
                }
                if(currentresult == maxresult)
                {
                    model.CurrentResult++;
                }

                if (model.Passer.PageNumber >= model.Test.AmountQuestion)
                {
                    return RedirectToAction("ResultCompl", new { result = model.CurrentResult , max = model.Test.AmountQuestion });
                }
                return RedirectToAction("CompleteTest", new { id = model.Test.Id, page = model.Passer.PageNumber });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ResultCompl(int result,int max)
        {
            TestComplPassViewModel modelview = new TestComplPassViewModel()
            {
                CurrentResult = result,
                MaxResult = max
            };
            return View(modelview);
        }
    }
}
