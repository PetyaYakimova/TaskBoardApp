using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Security.Claims;
using TaskBoardApp.Data;
using TaskBoardApp.Models;

namespace TaskBoardApp.Controllers
{
	[Authorize]
	public class TaskController : Controller
	{
		private readonly TaskBoardAppDbContext data;

		public TaskController(TaskBoardAppDbContext context)
		{
			this.data = context;
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var model = new TaskFormViewModel();
			model.Boards = await GetBoards();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(TaskFormViewModel model)
		{
			if (!(await GetBoards()).Any(b => b.Id == model.BoardId))
			{
				ModelState.AddModelError(nameof(model.BoardId), "Board does not exist");
			}

			if (!ModelState.IsValid)
			{
				model.Boards = await GetBoards();

				return View(model);
			}

			var entity = new TaskBoardApp.Data.Task()
			{
				BoardId = model.BoardId,
				CreatedOn = DateTime.Now,
				Description = model.Description,
				Title = model.Title,
				OwnerId = GetUserId()
			};

			await data.AddAsync(entity);
			await data.SaveChangesAsync();

			return RedirectToAction("Index", "Board");
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var task = await data.Tasks
				.Where(t => t.Id == id)
				.Select(t => new TaskDetailsViewModel()
				{
					Board = t.Board.Name,
					Title = t.Title,
					Description = t.Description,
					Id = t.Id,
					CreatedOn = t.CreatedOn != null ? t.CreatedOn.Value.ToString("dd.MM.yyy HH:mm") : "",
					Owner = t.Owner.UserName
				}).FirstOrDefaultAsync();

			return View(task);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var task = await data.Tasks
				.FindAsync(id);

			if (task == null)
			{
				return BadRequest();
			}

			if (task.OwnerId != GetUserId())
			{
				return Unauthorized();
			}

			var model = new TaskFormViewModel()
			{
				BoardId = task.BoardId,
				Description = task.Description,
				Title = task.Title,
				Id = task.Id,
				Boards = await GetBoards()
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(TaskFormViewModel model, int id)
		{
			var task = await data.Tasks
				.FindAsync(id);

			if (task == null)
			{
				return BadRequest();
			}

			if (task.OwnerId != GetUserId())
			{
				return Unauthorized();
			}

			if (!(await GetBoards()).Any(b => b.Id == model.BoardId))
			{
				ModelState.AddModelError(nameof(model.BoardId), "Board does not exist");
			}

			if (!ModelState.IsValid)
			{
				model.Boards = await GetBoards();
				return View(model);
			}

			task.Title = model.Title;
			task.Description = model.Description;
			task.BoardId = model.BoardId;

			await data.SaveChangesAsync();
			return RedirectToAction("Index", "Board");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
            var task = await data.Tasks
                .FindAsync(id);

            if (task == null)
            {
                return BadRequest();
            }

            if (task.OwnerId != GetUserId())
            {
                return Unauthorized();
            }

            var model = new TaskViewModel()
            {
                Description = task.Description,
                Title = task.Title,
                Id = task.Id
            };

            return View(model);
        }

		[HttpPost]
		public async Task<IActionResult> Delete(TaskViewModel model)
		{
            var task = await data.Tasks
                .FindAsync(model.Id);

            if (task == null)
            {
                return BadRequest();
            }

            if (task.OwnerId != GetUserId())
            {
                return Unauthorized();
            }

			data.Tasks.Remove(task);
			await data.SaveChangesAsync();
			return RedirectToAction("Index", "Board");
        }

		private string GetUserId()
		{
			return User.FindFirstValue(ClaimTypes.NameIdentifier);
		}

		private async Task<IEnumerable<TaskBoardViewModel>> GetBoards()
		{
			return await data.Boards
				.Select(x => new TaskBoardViewModel
				{
					Id = x.Id,
					Name = x.Name
				})
				.ToListAsync();
		}
	}
}
