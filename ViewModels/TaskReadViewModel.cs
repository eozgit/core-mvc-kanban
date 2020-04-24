using System;
using QuakeKanban.Models;

namespace QuakeKanban.ViewModels
{
    public class TaskReadViewModel
    {
        public Task Task { get; set; }
        public string Assignee { get; set; }
    }
}
