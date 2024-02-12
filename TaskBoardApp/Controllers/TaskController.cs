using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
