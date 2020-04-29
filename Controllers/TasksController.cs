using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuakeKanban.Data;
using QuakeKanban.ViewModels;

namespace QuakeKanban.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.Task.OrderBy(task => task.Id).ToListAsync();
            var vm = new TaskListViewModel
            {
                Tasks = tasks.Select(task => new TaskReadViewModel
                {
                    Task = task,
                    Assignee = GetAssignee(task.Assignee)
                }).ToList()
            };
            return View(vm);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            var vm = new TaskReadViewModel
            {
                Task = task,
                Assignee = GetAssignee(task.Assignee)
            };

            return View(vm);
        }

        // GET: Tasks/Create
        public IActionResult Create(int projectId)
        {
            var vm = new TaskWriteViewModel
            {
                Users = GetOptionsForAssignee(),
                ProjectId = projectId
            };
            return View(vm);
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int projectId, [Bind("Id,Summary,Description,Status,Assignee,StoryPoints")] QuakeKanban.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                var project = await _context.Project
                    .Include(project => project.Tasks)
                    .FirstOrDefaultAsync(project => project.Id == projectId);
                project.Tasks.Add(task);
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Board", new { Id = projectId });
            }
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            var vm = new TaskWriteViewModel
            {
                Task = task,
                Users = GetOptionsForAssignee(),
                ReturnUrl = returnUrl
            };
            return View(vm);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string returnUrl, [Bind("Id,Summary,Description,Status,Assignee,StoryPoints")] QuakeKanban.Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            var vm = new TaskReadViewModel
            {
                Task = task,
                Assignee = GetAssignee(task.Assignee),
                ReturnUrl = returnUrl
            };

            return View(vm);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string returnUrl)
        {
            var task = await _context.Task.FindAsync(id);
            _context.Task.Remove(task);
            await _context.SaveChangesAsync();
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        private bool TaskExists(int id)
        {
            return _context.Task.Any(e => e.Id == id);
        }

        private List<SelectListItem> GetOptionsForAssignee()
        {
            return _context.Users
                .OrderBy(user => user.Email)
                .ToList()
                .Select(user => new SelectListItem(GetAssignee(user.Email), user.Id))
                .ToList();
        }

        private string GetAssignee(string email)
        {
            var another = email != HttpContext.User.Identity.Name;
            var assignee = email.Split("@").First();
            if (another)
            {
                if (assignee.Length > 5)
                {
                    assignee = assignee.Substring(0, 5);
                }
                assignee = assignee + "**";
            }
            return assignee;
        }
    }
}
