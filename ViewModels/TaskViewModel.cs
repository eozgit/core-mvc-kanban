using System;
using QuakeKanban.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuakeKanban.ViewModels
{
    public class TaskViewModel
    {
        public Task Task { get; set; }
        public List<SelectListItem> Users { get; set; }
    }
}
