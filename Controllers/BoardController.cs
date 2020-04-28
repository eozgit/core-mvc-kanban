using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuakeKanban.Data;
using QuakeKanban.ViewModels;

namespace QuakeKanban.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction("Index", "Projects");
            }

            var project = await _context.Project
                .Include(project => project.Tasks)
                .FirstOrDefaultAsync(project => project.Id == id);

            var vm = new BoardViewModel
            {
                Project = project,
                Tasks = new List<GroupedTasks>()
            };

            var statusEnum = (QuakeKanban.Models.TaskStatus[])Enum.GetValues(typeof(QuakeKanban.Models.TaskStatus));
            foreach (QuakeKanban.Models.TaskStatus status in statusEnum)
            {
                var tasks = project.Tasks
                    .Where(task => task.Status == status)
                    .Select(task =>
                    {
                        string assignee = GetAssignee(task.Assignee);
                        return new TaskReadViewModel
                        {
                            Task = task,
                            Assignee = assignee
                        };
                    })
                    .ToList();

                vm.Tasks.Add(new GroupedTasks
                {
                    Status = SplitCamelCase(status.ToString()),
                    Tasks = tasks
                });
            }

            return View(vm);
        }

        private string GetAssignee(string userId)
        {
            var assignee = GetEmailByUserId(userId);
            var another = assignee != "Unassigned" && assignee != HttpContext.User.Identity.Name;
            assignee = assignee.Split("@").First();
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

        public async Task<IActionResult> MoveTaskBack(int id)
        {
            var task = await _context.Task.Include(task => task.Project).FirstOrDefaultAsync(task => task.Id == id);
            if (task.Status != QuakeKanban.Models.TaskStatus.Ready)
            {
                var i = (int)task.Status - 1;
                task.Status = (QuakeKanban.Models.TaskStatus)i;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Board", new { Id = task.Project.Id });
        }

        public async Task<IActionResult> AdvanceTask(int id)
        {
            var task = await _context.Task.Include(task => task.Project).FirstOrDefaultAsync(task => task.Id == id);
            if (task.Status != QuakeKanban.Models.TaskStatus.Done)
            {
                var i = (int)task.Status + 1;
                task.Status = (QuakeKanban.Models.TaskStatus)i;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Board", new { Id = task.Project.Id });
        }

        private string GetEmailByUserId(string Id)
        {
            return _context.Users.FirstOrDefault(user => user.Id == Id)?.Email ?? "Unassigned";
        }

        private static string SplitCamelCase(string str)
        {
            return Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }
    }
}