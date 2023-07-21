using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ToDoListApp
{
    class Program
    {
        struct TaskItem
        {
            public string Description;
            public bool IsComplete;
        }

        static List<TaskItem> tasks = new List<TaskItem>();

        static void DisplayTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
            }
            else
            {
                Console.WriteLine("Current tasks:");
                for (int i = 0; i < tasks.Count; i++)
                {
                    string status = tasks[i].IsComplete ? "Complete" : "Incomplete";

                    // Set the text color based on task completion status
                    Console.ForegroundColor = tasks[i].IsComplete ? ConsoleColor.Green : ConsoleColor.Red;

                    Console.WriteLine($"{i + 1}. [{status}] {tasks[i].Description}");

                    // Reset the text color to the default color
                    Console.ResetColor();
                }
            }
        }

        static void AddTask(string description)
        {
            // Validate if the task description is not empty or contains only whitespaces
            if (!string.IsNullOrWhiteSpace(description))
            {
                tasks.Add(new TaskItem { Description = description, IsComplete = false });
                Console.WriteLine($"Task '{description}' added successfully!");
            }
            else
            {
                Console.WriteLine("Task description cannot be empty. Please enter a valid description.");
            }
        }

        static void MarkComplete(int index)
        {
            // Validate if the task index is within the valid range
            if (index >= 1 && index <= tasks.Count)
            {
                // Show all tasks before marking a task as complete or incomplete
                DisplayTasks();

                TaskItem task = tasks[index - 1];
                Console.Write($"Do you want to mark task {index} as complete (C) or incomplete (I)? ");
                string choice = Console.ReadLine().Trim().ToLower();

                if (choice == "C" || choice == "complete" || choice =="c")
                {
                    task.IsComplete = true;
                    tasks[index - 1] = task;
                    Console.WriteLine($"Task {index} marked as complete.");
                }
                else if (choice == "I" || choice == "incomplete" || choice == "i")
                {
                    MarkIncomplete(index);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter 'C' for complete or 'I' or 'incomplete' for incomplete.");
                }
            }
            else
            {
                Console.WriteLine("Invalid task index. Please enter a valid task index.");
            }
        }

        static void MarkIncomplete(int index)
        {
            // Validate if the task index is within the valid range
            if (index >= 1 && index <= tasks.Count)
            {
                TaskItem task = tasks[index - 1];
                task.IsComplete = false;
                tasks[index - 1] = task;

                // Show all tasks after marking a task as incomplete
                DisplayTasks();

                Console.WriteLine($"Task {index} marked as incomplete.");
            }
            else
            {
                Console.WriteLine("Invalid task index. Please enter a valid task index.");
            }
        }

        static void RemoveTask(int index)
        {
            // Validate if the task index is within the valid range
            if (index >= 1 && index <= tasks.Count)
            {
                // Show all tasks before confirming task removal
                DisplayTasks();

                TaskItem task = tasks[index - 1];

                // Confirm with the user before removing the task
                while (true)
                {
                    Console.WriteLine($"Are you sure you want to remove task {index}? (Y/N)");
                    string confirmation = Console.ReadLine().Trim().ToLower();

                    if (confirmation == "Y" || confirmation == "yes" || confirmation == "y" || confirmation == "YES")
                    {
                        tasks.RemoveAt(index - 1);
                        Console.WriteLine($"Task {index} removed successfully.");

                        // Show all tasks after removing a task
                        DisplayTasks();
                        break;
                    }
                    else if (confirmation == "N" || confirmation == "no" || confirmation == "n" || confirmation == "NO")
                    {
                        Console.WriteLine("Task removal canceled.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid task index. Please enter a valid task index.");
            }
        }

        static void ClearAllTasks()
        {
            if (tasks.Count > 0)
            {
                // Confirm with the user before clearing all tasks
                Console.WriteLine("Are you sure you want to clear all tasks? This action cannot be undone. (Y/N)");
                string confirmation = Console.ReadLine().Trim().ToLower();

                if (confirmation == "y" || confirmation == "yes" || confirmation == "Y" || confirmation =="YES")
                {
                    tasks.Clear();
                    Console.WriteLine("All tasks cleared successfully.");
                }
                else if(confirmation == "N" || confirmation == "no" || confirmation == "n" || confirmation == "NO")
                {
                    Console.WriteLine("Clear all tasks canceled.");
                }
            }
            else
            {
                Console.WriteLine("No tasks found. There are no tasks to clear.");
            }
        }

        static void LoadTasks()
        {
            // Check if the tasks file exists
            if (File.Exists("tasks.txt"))
            {
                string[] lines = File.ReadAllLines("tasks.txt");

                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 2)
                    {
                        string description = parts[0];
                        bool isComplete = bool.Parse(parts[1]);
                        tasks.Add(new TaskItem { Description = description, IsComplete = isComplete });
                    }
                }

                Console.WriteLine("Tasks loaded successfully.");
            }
        }

        static void SaveTasks()
        {
            List<string> lines = new List<string>();

            foreach (TaskItem task in tasks)
            {
                string line = $"{task.Description};{task.IsComplete}";
                lines.Add(line);
            }

            File.WriteAllLines("tasks.txt", lines);
            Console.WriteLine("Tasks saved successfully.");
        }

        static bool ValidateTaskIndex(string input, out int index)
        {
            if (int.TryParse(input, out index))
            {
                if (index >= 1 && index <= tasks.Count)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Invalid task index. Please enter a valid task index.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid task index.");
            }

            return false;
        }

        static void EditTaskDescription(int index)
        {
            // Validate if the task index is within the valid range
            if (index >= 1 && index <= tasks.Count)
            {
                TaskItem task = tasks[index - 1];
                Console.Write($"Enter new description for task {index}: ");
                string newDescription = Console.ReadLine().Trim();
                if (!string.IsNullOrWhiteSpace(newDescription))
                {
                    task.Description = newDescription;
                    tasks[index - 1] = task;
                    Console.WriteLine($"Task {index} description updated successfully.");
                }
                else
                {
                    Console.WriteLine("Task description cannot be empty. Task update canceled.");
                }
            }
            else
            {
                Console.WriteLine("Invalid task index. Task update canceled.");
            }
        }

        static void Main(string[] args)
        {
            // Load tasks from the file at the beginning
            LoadTasks();

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Display tasks");
                Console.WriteLine("2. Add task");
                Console.WriteLine("3. Mark task as complete");
                Console.WriteLine("4. Mark task as incomplete");
                Console.WriteLine("5. Remove task");
                Console.WriteLine("6. Edit task description");
                Console.WriteLine("7. Clear all tasks");
                Console.WriteLine("8. Exit");
                Console.Write("Enter your choice: ");
                string input = Console.ReadLine().Trim().ToLower();

                switch (input)
                {
                    case "1":
                        DisplayTasks();
                        break;
                    case "2":
                        Console.Write("Enter task description: ");
                        string description = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(description))
                        {
                            AddTask(description);
                        }
                        else
                        {
                            Console.WriteLine("Task description cannot be empty. Please enter a valid description.");
                        }
                        break;
                    case "3":
                        Console.Write("Enter task index to mark as complete: ");
                        if (ValidateTaskIndex(Console.ReadLine(), out int completeIndex))
                        {
                            MarkComplete(completeIndex);
                        }
                        break;
                    case "4":
                        Console.Write("Enter task index to mark as incomplete: ");
                        if (ValidateTaskIndex(Console.ReadLine(), out int incompleteIndex))
                        {
                            MarkIncomplete(incompleteIndex);
                        }
                        break;
                    case "5":
                        Console.Write("Enter task index to remove: ");
                        if (ValidateTaskIndex(Console.ReadLine(), out int removeIndex))
                        {
                            RemoveTask(removeIndex);
                        }
                        break;
                    case "6":
                        Console.Write("Enter task index to edit description: ");
                        if (ValidateTaskIndex(Console.ReadLine(), out int editIndex))
                        {
                            EditTaskDescription(editIndex);
                        }
                        break;
                    case "7":
                        // Option to clear all tasks
                        ClearAllTasks();
                        break;
                    case "8":
                        Console.WriteLine("Exiting the application.");
                        SaveTasks(); // Save tasks to the file before exiting the application
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option (1-8).");
                        break;
                }
            }
        }
    }
}