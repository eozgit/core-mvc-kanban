using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuakeKanban.Models
{
    public class Task
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        [Column(TypeName = "varchar(60)")]
        public string Summary { get; set; }

        [StringLength(300)]
        [Column(TypeName = "varchar(300)")]
        public string Description { get; set; }
        
        [Column(TypeName = "int")]
        public TaskStatus Status { get; set; }
        
        [Column(TypeName = "varchar(36)")]
        public string Assignee { get; set; }
        
        public int StoryPoints { get; set; }

        public Project Project { get; set; }
    }
}
