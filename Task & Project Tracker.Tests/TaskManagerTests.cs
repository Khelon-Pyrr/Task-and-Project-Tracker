using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Task___Project_Tracker.Models;
using Task___Project_Tracker.Services;

namespace Task___Project_Tracker.Tests
{
    [TestClass]
    public class TaskManagerTests
    {
        private string _testDataFile;
        private string _testLogFile;
        private TaskManager _taskManager;

        [TestInitialize]
        public void Setup()
        {
            // Use unique file names for each test to avoid conflicts
            _testDataFile = $"test_tasks_{Guid.NewGuid()}.json";
            _testLogFile = $"test_log_{Guid.NewGuid()}.txt";

            _taskManager = new TaskManager(_testDataFile, _testLogFile);
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                if (File.Exists(_testDataFile)) File.Delete(_testDataFile);
                if (File.Exists(_testLogFile)) File.Delete(_testLogFile);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        [TestMethod]
        public void AddTask_ShouldIncreaseTaskCount()
        {
            // Arrange
            var task = new DevelopmentTask(1, "Test Task", "Desc", DateTime.Now, Priority.Medium, "User", "Reviewer");

            // Act
            _taskManager.AddTask(task);
            var addedTask = _taskManager.GetTaskById(1);

            // Assert
            Assert.AreEqual(1, _taskManager.GetAllTasks().Count);
            Assert.IsNotNull(addedTask);
            Assert.AreEqual("Test Task", addedTask.Title);
        }

        [TestMethod]
        public void DeleteTask_ShouldRemoveTask()
        {
            // Arrange
            var task = new DevelopmentTask(1, "Test Task", "Desc", DateTime.Now, Priority.Medium, "User", "Reviewer");
            _taskManager.AddTask(task);

            // Act
            _taskManager.DeleteTask(1);

            // Assert
            Assert.AreEqual(0, _taskManager.GetAllTasks().Count);
        }

        [TestMethod]
        public void UpdateTaskStatus_ShouldChangeStatus()
        {
            // Arrange
            var task = new DevelopmentTask(1, "Test Task", "Desc", DateTime.Now, Priority.Medium, "User", "Reviewer");
            _taskManager.AddTask(task);

            // Act
            _taskManager.UpdateTaskStatus(1, Models.TaskStatus.InProgress);

            // Assert
            var updatedTask = _taskManager.GetTaskById(1);
            Assert.AreEqual(Models.TaskStatus.InProgress, updatedTask.Status);
        }

        [TestMethod]
        public void GetTaskById_ShouldReturnCorrectTask()
        {
            // Arrange
            _taskManager.AddTask(new DevelopmentTask(1, "Task 1", "Desc", DateTime.Now, Priority.Low, "User", ""));
            _taskManager.AddTask(new DevelopmentTask(3, "Task 3", "Desc", DateTime.Now, Priority.High, "User", ""));
            _taskManager.AddTask(new DevelopmentTask(5, "Task 5", "Desc", DateTime.Now, Priority.Medium, "User", ""));

            // Act
            var result = _taskManager.GetTaskById(3);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Task 3", result.Title);
        }

        [TestMethod]
        public void SortByPriority_ShouldSortTasksCorrectly()
        {
            // Arrange
            // Priority: Low=0, Medium=1, High=2
            _taskManager.AddTask(new DevelopmentTask(1, "Low", "Desc", DateTime.Now, Priority.Low, "User", ""));
            _taskManager.AddTask(new DevelopmentTask(2, "High", "Desc", DateTime.Now, Priority.High, "User", ""));
            _taskManager.AddTask(new DevelopmentTask(3, "Medium", "Desc", DateTime.Now, Priority.Medium, "User", ""));

            // Act
            var tasks = _taskManager.SortByPriority();

            // Assert
            Assert.AreEqual(Priority.Low, tasks[0].Priority);
            Assert.AreEqual(Priority.Medium, tasks[1].Priority);
            Assert.AreEqual(Priority.High, tasks[2].Priority);
        }
    }
}
