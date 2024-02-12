using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TaskBoardApp.Data;

namespace TaskBoardApp.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.Task.TitleMaxLength)]
        [MinLength(DataConstants.Task.TitleMinLength)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(DataConstants.Task.DescriptionMaxLength)]
        [MinLength(DataConstants.Task.DescriptionMinLength)]
        public string Description { get; set; } = string.Empty;

        public DateTime? CreatedOn { get; set; }

        [Required]
        public string Owner { get; set; } = string.Empty;
    }
}
