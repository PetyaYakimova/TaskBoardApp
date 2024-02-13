using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using TaskBoardApp.Data;
using TaskBoardApp.Models;

namespace TaskBoardApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly TaskBoardAppDbContext data;

        public HomeController(TaskBoardAppDbContext context)
        {
            data = context;
        }

        public async Task<IActionResult> Index()
        {
            var taskBoards = await data.Boards
                .Select(b => b.Name)
                .Distinct()
                .ToListAsync();

            var tasksCounts = new List<HomeBoardModel>();
            foreach (var boardName in taskBoards)
            {
                var tasksInBoard = await data.Tasks
                    .Where(t => t.Board.Name == boardName)
                    .AsNoTracking()
                    .ToListAsync();
                int count = tasksInBoard.Count();
                tasksCounts.Add(new HomeBoardModel()
                {
                    BoardName = boardName,
                    TasksCount = count
                });
            }

            var userTasksCount = -1;

            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userTasks = await data.Tasks
                    .Where(t => t.OwnerId == currentUserId)
                    .AsNoTracking()
                    .ToListAsync();
                userTasksCount = userTasks.Count();
            }

            var homeModel = new HomeViewModel()
            {
                AllTasksCount = data.Tasks.Count(),
                BoardsWithTasksCount = tasksCounts,
                UserTasksCount = userTasksCount
            };

            return View(homeModel);
        }
    }
}