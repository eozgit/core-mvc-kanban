using System;
using System.Collections.Generic;
using QuakeKanban.Models;

namespace QuakeKanban.ViewModels
{
    public class BoardViewModel
    {
        public Project Project { get; set; }
        public List<GroupedTasks> Tasks { get; set; }
    }

    public class GroupedTasks
    {
        public string Status { get; set; }
        public List<TaskReadViewModel> Tasks { get; set; }
    }
}
