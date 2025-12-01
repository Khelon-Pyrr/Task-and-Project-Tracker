using System;

namespace Task___Project_Tracker.Models
{
    public class DevelopmentTask : TaskItem
    {
        public string CodeReviewer { get; set; }

        public DevelopmentTask(int id, string title, string description, DateTime dueDate, Priority priority, string assignee, string codeReviewer)
            : base(id, title, description, dueDate, priority, assignee)
        {
            CodeReviewer = codeReviewer;
        }

        public override string ToString()
        {
            return base.ToString() + (string.IsNullOrEmpty(CodeReviewer) ? "" : $" [Reviewer: {CodeReviewer}]");
        }
    }
}
