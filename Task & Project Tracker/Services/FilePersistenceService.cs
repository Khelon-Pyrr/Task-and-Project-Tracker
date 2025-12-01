using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Task___Project_Tracker.Services
{
    public class FilePersistenceService<T>
    {
        private readonly string _filePath;

        public FilePersistenceService(string filePath)
        {
            _filePath = filePath;
        }

        public void Save(List<T> items)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(items, options);
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        public List<T> Load()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            try
            {
                string jsonString = File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(jsonString)) return new List<T>();
                return JsonSerializer.Deserialize<List<T>>(jsonString) ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return new List<T>();
            }
        }
    }
}
