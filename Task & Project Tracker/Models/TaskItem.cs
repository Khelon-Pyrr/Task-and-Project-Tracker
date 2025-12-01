using System;
using System.Text.Json.Serialization;

namespace Task___Project_Tracker.Models
{
    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Done
    }

    public enum Priority
    {
        Low,
        Medium,
        High
    }

    [JsonDerivedType(typeof(DevelopmentTask), typeDiscriminator: "DevelopmentTask")]
    public abstract class TaskItem : IComparable<TaskItem>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public string Assignee { get; set; }

        public TaskItem(int id, string title, string description, DateTime dueDate, Priority priority, string assignee)
        {
            Id = id;
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Status = TaskStatus.ToDo;
            Assignee = assignee;
        }

        public override string ToString()
        {
            return $"[{Id}] {Title} - {Status} (Due: {DueDate.ToShortDateString()}, Pri: {Priority})";
        }

        public int CompareTo(TaskItem other)
        {
            if (other == null) return 1;
            return this.DueDate.CompareTo(other.DueDate);
        }
    }
}
