using System;
using System.Collections.Generic;
using QuakeKanban.Models;

namespace QuakeKanban.ViewModels
{
    public class TaskListViewModel
    {
        public List<Task> Tasks { get; set; }
        public List<string> Assignees { get; set; }
    }
}
