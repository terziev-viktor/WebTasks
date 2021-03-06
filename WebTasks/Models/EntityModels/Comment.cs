﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTasks.Models.EntityModels
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [StringLength(500, ErrorMessage = "The comment can not be empty.", MinimumLength = 1)]
        public string Content { get; set; }

        [Required]
        public virtual ApplicationUser Author { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; }

        [ForeignKey("Project")]
        public int? Project_Id { get; set; }

        public virtual Project Project { get; set; }

        [ForeignKey("DailyTask")]
        public int? DailyTaskId { get; set; }

        public virtual DailyTask DailyTask { get; set; }
    }
}