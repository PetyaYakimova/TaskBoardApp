using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaskBoardApp.Data
{
    [Comment("Board")]
    public class Board
    {
        [Key]
        [Comment("Board identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.Board.NameMaxLength)]
        [Comment("Board name")]
        public string Name { get; set; } = string.Empty;

        public virtual IEnumerable<Task> Tasks { get; set; } = new List<Task>();
    }
}