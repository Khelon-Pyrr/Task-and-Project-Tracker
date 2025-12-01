using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Task___Project_Tracker.Models;

namespace Task___Project_Tracker.Services
{
    public class TaskManager
    {
        private Dictionary<int, TaskItem> _tasks;
        private readonly FilePersistenceService<TaskItem> _persistenceService;
        private readonly LoggerService _loggerService;

        public TaskManager(string dataFilePath, string logFilePath)
        {
            _persistenceService = new FilePersistenceService<TaskItem>(dataFilePath);
            _loggerService = new LoggerService(logFilePath);
            // Load the list and convert it to a dictionary for fast lookups
            _tasks = _persistenceService.Load().ToDictionary(t => t.Id);
        }

        public void AddTask(TaskItem task)
        {
            _tasks.Add(task.Id, task);
            _persistenceService.Save(_tasks.Values.ToList());
            _loggerService.Log($"Task created: {task.Id} - {task.Title}");
        }

        public void UpdateTaskStatus(int taskId, Models.TaskStatus newStatus)
        {
            if (_tasks.TryGetValue(taskId, out var task))
            {
                var oldStatus = task.Status;
                task.Status = newStatus;
                _persistenceService.Save(_tasks.Values.ToList());
                _loggerService.Log($"Task {taskId} status updated from {oldStatus} to {newStatus}");
            }
            else
            {
                Console.WriteLine("Task not found.");
            }
        }

        public void DeleteTask(int taskId)
        {
            if (_tasks.Remove(taskId))
            {
                _persistenceService.Save(_tasks.Values.ToList());
                _loggerService.Log($"Task deleted: {taskId}");
            }
            else
            {
                Console.WriteLine("Task not found.");
            }
        }

        public List<TaskItem> GetAllTasks()
        {
            return _tasks.Values.ToList();
        }

        // Search by Assignee now iterates over the dictionary's values
        public List<TaskItem> SearchByAssignee(string assignee)
        {
            List<TaskItem> results = new List<TaskItem>();
            foreach (var task in _tasks.Values)
            {
                if (task.Assignee.Equals(assignee, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(task);
                }
            }
            return results;
        }

        // REPLACED: BinarySearchById is now a highly efficient dictionary lookup
        public TaskItem GetTaskById(int id)
        {
            _tasks.TryGetValue(id, out var task);
            return task;
        }

        // Manual Bubble Sort by Priority
        public List<TaskItem> SortByPriority()
        {
            var taskList = _tasks.Values.ToList();
            int n = taskList.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (taskList[j].Priority > taskList[j + 1].Priority)
                    {
                        // Swap
                        var temp = taskList[j];
                        taskList[j] = taskList[j + 1];
                        taskList[j + 1] = temp;
                    }
                }
            }
            Console.WriteLine("Tasks sorted by Priority (Bubble Sort).");
            return taskList;
        }

        // Built-in Sort by DueDate
        public List<TaskItem> SortByDueDate()
        {
            var taskList = _tasks.Values.ToList();
            taskList.Sort(); // Uses IComparable implementation in TaskItem
            Console.WriteLine("Tasks sorted by Due Date (Built-in Sort).");
            return taskList;
        }

        public void ExportReport(string reportFilePath)
        {
            var overdue = _tasks.Values.Where(t => t.DueDate < DateTime.Now && t.Status != Models.TaskStatus.Done).ToList();
            var upcoming = _tasks.Values.Where(t => t.DueDate >= DateTime.Now && t.Status != Models.TaskStatus.Done).ToList();

            using (StreamWriter writer = new StreamWriter(reportFilePath))
            {
                writer.WriteLine("Task Report");
                writer.WriteLine($"Generated on: {DateTime.Now}");
                writer.WriteLine("--------------------------------------------------");

                writer.WriteLine("OVERDUE TASKS:");
                if (overdue.Count == 0) writer.WriteLine("None.");
                foreach (var t in overdue) writer.WriteLine(t);

                writer.WriteLine("--------------------------------------------------");

                writer.WriteLine("UPCOMING TASKS:");
                if (upcoming.Count == 0) writer.WriteLine("None.");
                foreach (var t in upcoming) writer.WriteLine(t);
            }
            _loggerService.Log("Report exported.");
        }
    }
}
