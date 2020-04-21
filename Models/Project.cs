using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuakeKanban.Models
{
    public class Project
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        [Column(TypeName = "varchar(60)")]
        public string Name { get; set; }

        [StringLength(300)]
        [Column(TypeName = "varchar(300)")]
        public string Description { get; set; }
    }
}
