using System.Collections.ObjectModel;
using ToDoList;

namespace ToDoList;

public partial class MainPage : ContentPage
{
    private ObservableCollection<ToDoClass> todoList;
    private int nextId = 1;
    private ToDoClass selectedToDo = null;

    public MainPage()
    {
        InitializeComponent();
        todoList = new ObservableCollection<ToDoClass>();
        todoLV.ItemsSource = todoList;
    }

    // Add a new to-do item
    private void AddToDoItem(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(titleEntry.Text))
        {
            DisplayAlert("Validation", "Please enter a task title.", "OK");
            return;
        }

        var newToDo = new ToDoClass
        {
            id = nextId++,
            title = titleEntry.Text.Trim(),
            detail = detailsEditor.Text ?? ""
        };

        todoList.Add(newToDo);
        ClearInputs();
        DisplayAlert("Success", "Task added successfully!", "OK");
    }

    // Edit an existing to-do item
    private void EditToDoItem(object sender, EventArgs e)
    {
        if (selectedToDo == null)
        {
            DisplayAlert("Error", "No task selected.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(titleEntry.Text))
        {
            DisplayAlert("Validation", "Please enter a task title.", "OK");
            return;
        }

        selectedToDo.title = titleEntry.Text.Trim();
        selectedToDo.detail = detailsEditor.Text ?? "";

        todoLV.ItemsSource = null;
        todoLV.ItemsSource = todoList;

        ClearInputs();
        ShowAddButton();
        DisplayAlert("Success", "Task updated successfully!", "OK");
    }

    // Delete a to-do item
    private async void DeleteToDoItem(object sender, EventArgs e)
    {
        if (sender is Button btn && int.TryParse(btn.ClassId.ToString(), out int id))
        {
            var todoToDelete = todoList.FirstOrDefault(t => t.id == id);
            if (todoToDelete != null)
            {
                bool confirm = await DisplayAlert("Delete Task", 
                    "Are you sure you want to delete this task?", "Yes", "No");

                if (confirm)
                {
                    todoList.Remove(todoToDelete);
                    if (selectedToDo?.id == id)
                    {
                        ClearInputs();
                        ShowAddButton();
                    }
                    DisplayAlert("Deleted", "Task deleted successfully!", "OK");
                }
            }
        }
    }

    // Handle when a to-do item is selected
    private void TodoLV_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is ToDoClass selected)
        {
            selectedToDo = selected;
            titleEntry.Text = selected.title;
            detailsEditor.Text = selected.detail;
            ShowEditButton();
        }
    }

    // Handle when a to-do item is tapped (same as selected)
    private void todoLV_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is ToDoClass selected)
        {
            selectedToDo = selected;
            titleEntry.Text = selected.title;
            detailsEditor.Text = selected.detail;
            ShowEditButton();
        }
    }

    // Cancel editing
    private void CancelEdit(object sender, EventArgs e)
    {
        ClearInputs();
        ShowAddButton();
    }

    // Clear input fields
    private void ClearInputs()
    {
        titleEntry.Text = "";
        detailsEditor.Text = "";
        selectedToDo = null;
        todoLV.SelectedItem = null;
    }

    // Show Add button and hide Edit/Cancel buttons
    private void ShowAddButton()
    {
        addBtn.IsVisible = true;
        editBtn.IsVisible = false;
        cancelBtn.IsVisible = false;
    }

    // Show Edit and Cancel buttons and hide Add button
    private void ShowEditButton()
    {
        addBtn.IsVisible = false;
        editBtn.IsVisible = true;
        cancelBtn.IsVisible = true;
    }
}