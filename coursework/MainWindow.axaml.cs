using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using library;

namespace AvaloniaApplication2
{
    public partial class MainWindow : Window
    {
        private CollectionOfStories<Story> collection;
        public ObservableCollection<Story> Stories { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            
            Stories = new ObservableCollection<Story>(); 
            collection = new CollectionOfStories<Story>("Stories", new List<Story>(), 0);
            UpdateStories();
        }
        
        private void UpdateCollectionInfo_Click(object sender, RoutedEventArgs e)
        {
            Panel.IsVisible = true;
            
            string title = CollectionTitleInput.Text;
            if (!string.IsNullOrWhiteSpace(title))
            {
                collection.Title = title;
                NotificationText.Text = "Collection title updated successfully!";
                TitleInput.Clear();
                UpdateStories();
            }
            else
            {
                NotificationText.Text = "Title is required!";
            }
        }
        
        private void AddStory_Click(object sender, RoutedEventArgs e)
        {
            InputPanel.IsVisible = true;
            
            string title = TitleInput.Text;
            string author = AuthorInput.Text;
            string yearText = YearInput.Text;
            
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(author))
            {
                NotificationText.Text = "Enter the title, year and author of the story to add.";
                return;
            }

            if (!int.TryParse(yearText, out int year))
            {
                NotificationText.Text = "Please enter a valid integer.";
                return;
            }
            
            if (string.IsNullOrWhiteSpace(title))
            {
                NotificationText.Text = "Title cannot be empty.";
                return;
            }
            
            if (string.IsNullOrWhiteSpace(author))
            {
                NotificationText.Text = "Author cannot be empty.";
                return;
            }
            
            collection.AddStory(title, year, author);
            UpdateStories();
            TitleInput.Clear();
            YearInput.Clear();
            AuthorInput.Clear();
            NotificationText.Text = "Story added successfully!";
        }

        private void RemoveStory_Click(object sender, RoutedEventArgs e)
        {
            InputPanel.IsVisible = true;
            
            string title = TitleInput.Text;
            if (!string.IsNullOrWhiteSpace(title))
            {
                collection.RemoveStory(title);
                UpdateStories();
                TitleInput.Clear();
                YearInput.Clear();
                AuthorInput.Clear();
                NotificationText.Text = "Story removed successfully!";
            }
            else
            {
                NotificationText.Text = "Enter the title, year and author of the story to remove.";
            }
        }

        private void SortByTitle_Click(object sender, RoutedEventArgs e)
        {
            collection.SortByTitle();
            UpdateStories();
        }

        private void SortByYear_Click(object sender, RoutedEventArgs e)
        {
            collection.SortByYear();
            UpdateStories();
        }

        private void UpdateStories()
        {
            StoriesTextBox.Text = $"{collection.Title}, Number of Stories: {collection.NumberOfStories}" +
                                  $"\n{string.Join("\n", collection.Stories)}";
        }


        private async void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                DefaultExtension = "json",
                Filters = new List<FileDialogFilter>
                {
                    new() { Name = "JSON Files", Extensions = { "json" } }
                }
            };
            
            string? filePath = await saveDialog.ShowAsync(this);

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                collection.SaveToFile(filePath); 
                NotificationText.Text = "Stories saved.";
            }
            else
            {
                NotificationText.Text = "Save operation canceled.";
            }
        }

        private async void LoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                AllowMultiple = false,
                Filters = new List<FileDialogFilter> 
                {
                    new() { Name = "JSON Files", Extensions = { "json" } }
                }
            };
            
            string[]? filePaths = await openDialog.ShowAsync(this);

            if (filePaths != null && filePaths.Length > 0)
            {
                string filePath = filePaths[0];
                var loadedCollection = CollectionOfStories<Story>.LoadFromFile(filePath);

                if (loadedCollection != null)
                {
                    collection = loadedCollection;
                    CollectionTitleInput.Text = collection.Title;
                    UpdateStories();
                    NotificationText.Text = "Stories loaded.";
                }
                else
                {
                    NotificationText.Text = "Failed to load stories from the file.";
                }
            }
            else
            {
                NotificationText.Text = "Load operation canceled.";
            }
        }
        }
    }