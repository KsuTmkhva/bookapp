using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace library {
    public class Book <T>{
        public string Title { get ; set; }

        public Book(string title) {
            Title = title;
        }

        public override string ToString() {

            return $"Title: {Title}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is Book<T> b)
            {
                return Title == b.Title;
            }
            
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title);
        }
    }

    public struct Story
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Author { get; set; }

        public Story(string title, int year, string author)
        {
            Title = title;
            Year = year;
            Author = author;
        }

        public override string ToString()
        {
            return $"{Title}, {Year}, {Author}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Story story && Title == story.Title && Year == story.Year && Author == story.Author;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Year, Author);
        }
    }

    public class CollectionOfStories<T> : Book<T>
    {
        public List<Story> Stories { get; set; } = new();
        public int NumberOfStories => Stories.Count;

        public CollectionOfStories(string title, List<Story> stories, int numberOfStories)
            : base(title)
        {
            Stories = stories;
        }
        

        public void AddStory(string title, int year, string author)
        {
            Stories.Add(new Story(title, year, author));
            Console.WriteLine($"Story successfully added!");
        }

        public void RemoveStory(string title)
        {
            var story = Stories.FirstOrDefault(s => s.Title == title);
            if (story.Title != null)
            {
                Stories.Remove(story);
            }
            else
            {
                Console.WriteLine("The story was not found");
            }
        }

        public void SortByTitle()
        {
            Stories.Sort((a, b) => a.Title.CompareTo(b.Title));
        }

        public void SortByYear()
        {
            Stories.Sort((a, b) => a.Year.CompareTo(b.Year));
        }

        public void SaveToFile(string fileName)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(fileName, json);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error saving to file: {e.Message}");
            }
        }

        public static CollectionOfStories<T> LoadFromFile(string fileName)
        {
            try
            {
                string json = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<CollectionOfStories<T>>(json);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading from file: {e.Message}");
                return null;
            }
        }

    public override string ToString()
        {
            string stories = string.Join("\n", Stories.Select(s => s.ToString()));
            return $"{base.ToString()}, NumberOfStories: {NumberOfStories} \n{stories}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is CollectionOfStories<T> c)
            {
                return base.Equals(c) && NumberOfStories == c.NumberOfStories;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), NumberOfStories);
        }
    }
}