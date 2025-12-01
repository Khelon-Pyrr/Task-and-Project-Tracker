using System;
using System.Collections.Generic;
using System.IO;
using Task___Project_Tracker.Models;
using Task___Project_Tracker.Services;

namespace Task___Project_Tracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dataFile = "tasks.json";
            string logFile = "log.txt";
            string reportFile = "report.txt";

            TaskManager taskManager = new TaskManager(dataFile, logFile);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==========================================");
                Console.WriteLine("   Task & Project Tracking System");
                Console.WriteLine("==========================================");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. List All Tasks");
                Console.WriteLine("3. Update Task Status");
                Console.WriteLine("4. Delete Task");
                Console.WriteLine("5. Search Task by ID");
                Console.WriteLine("6. Search Tasks by Assignee (Linear Search)");
                Console.WriteLine("7. Sort Tasks by Priority (Manual Sort)");
                Console.WriteLine("8. Sort Tasks by Due Date (Built-in Sort)");
                Console.WriteLine("9. Export Report");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTask(taskManager);
                        break;
                    case "2":
                        ListTasks(taskManager.GetAllTasks());
                        break;
                    case "3":
                        UpdateTaskStatus(taskManager);
                        break;
                    case "4":
                        DeleteTask(taskManager);
                        break;
                    case "5":
                        SearchById(taskManager);
                        break;
                    case "6":
                        SearchByAssignee(taskManager);
                        break;
                    case "7":
                        ListTasks(taskManager.SortByPriority());
                        break;
                    case "8":
                        ListTasks(taskManager.SortByDueDate());
                        break;
                    case "9":
                        taskManager.ExportReport(reportFile);
                        Console.WriteLine($"Report exported to {Path.GetFullPath(reportFile)}");
                        Pause();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        Pause();
                        break;
                }
            }
        }

        static void AddTask(TaskManager manager)
        {
            Console.WriteLine("\n--- Add New Task ---");

            int id = ReadInt("Enter ID (Integer): ");

            // Check for duplicate ID
            if (manager.GetTaskById(id) != null)
            {
                Console.WriteLine($"Error: Task with ID {id} already exists.");
                Pause();
                return;
            }

            string title = ReadString("Enter Title: ", "Title cannot be empty.");

            Console.Write("Enter Description: ");
            string desc = Console.ReadLine(); // Description can be empty

            DateTime dueDate = ReadDate("Enter Due Date (yyyy-mm-dd): ");

            Priority priority = ReadPriority();

            string assignee = ReadString("Enter Assignee: ", "Assignee cannot be empty.");

            bool isDev = ReadBool("Is this a Development Task? (y/n): ");

            TaskItem newTask;
            if (isDev)
            {
                Console.Write("Enter Code Reviewer: ");
                string reviewer = Console.ReadLine();
                newTask = new DevelopmentTask(id, title, desc, dueDate, priority, assignee, reviewer);
            }
            else
            {
                newTask = new DevelopmentTask(id, title, desc, dueDate, priority, assignee, "");
            }

            manager.AddTask(newTask);
            Console.WriteLine("Task added successfully.");
            Pause();
        }

        static void ListTasks(List<TaskItem> tasks)
        {
            Console.WriteLine("\n--- Task List ---");
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
            }
            else
            {
                foreach (var task in tasks)
                {
                    Console.WriteLine(task);
                }
            }
            Pause();
        }

        static void UpdateTaskStatus(TaskManager manager)
        {
            int id = ReadInt("\nEnter Task ID to update: ");

            // Verify task exists first
            if (manager.GetTaskById(id) == null)
            {
                Console.WriteLine("Task not found.");
                Pause();
                return;
            }

            Models.TaskStatus status = ReadTaskStatus();
            manager.UpdateTaskStatus(id, status);
            Console.WriteLine("Status updated.");
            Pause();
        }

        static void DeleteTask(TaskManager manager)
        {
            int id = ReadInt("\nEnter Task ID to delete: ");
            manager.DeleteTask(id);
            Pause();
        }

        static void SearchById(TaskManager manager)
        {
            int id = ReadInt("\nEnter Task ID to search: ");
            var task = manager.GetTaskById(id);
            if (task != null)
            {
                Console.WriteLine("Task Found:");
                Console.WriteLine(task);
            }
            else
            {
                Console.WriteLine("Task not found.");
            }
            Pause();
        }

        static void SearchByAssignee(TaskManager manager)
        {
            Console.Write("\nEnter Assignee Name: ");
            string assignee = Console.ReadLine();
            var results = manager.SearchByAssignee(assignee);
            ListTasks(results);
        }

        static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // --- Helper Methods for Robust Input ---

        static int ReadInt(string prompt)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out result))
                {
                    return result;
                }
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }
        }

        static string ReadString(string prompt, string errorPrompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                Console.WriteLine(errorPrompt);
            }
        }

        static DateTime ReadDate(string prompt)
        {
            DateTime result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (DateTime.TryParse(input, out result))
                {
                    return result;
                }
                Console.WriteLine("Invalid date format. Please use yyyy-mm-dd.");
            }
        }

        static Priority ReadPriority()
        {
            while (true)
            {
                Console.Write("Select Priority (0: Low, 1: Medium, 2: High): ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int val) && Enum.IsDefined(typeof(Priority), val))
                {
                    return (Priority)val;
                }
                Console.WriteLine("\nInvalid selection. Please enter 0, 1, or 2.");
            }
        }

        static Models.TaskStatus ReadTaskStatus()
        {
            while (true)
            {
                Console.Write("Select New Status (0: ToDo, 1: InProgress, 2: Done): ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int val) && Enum.IsDefined(typeof(Models.TaskStatus), val))
                {
                    return (Models.TaskStatus)val;
                }
                Console.WriteLine("\nInvalid selection. Please enter 0, 1, or 2.");
            }
        }

        static bool ReadBool(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim().ToLower();
                if (input == "y" || input == "yes") return true;
                if (input == "n" || input == "no") return false;
                Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
            }
        }
    }
}
