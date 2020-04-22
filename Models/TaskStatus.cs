using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuakeKanban.Models
{
    public enum TaskStatus
    {
        Ready,
        InProgress,
        InQA,
        Done
    }
}
