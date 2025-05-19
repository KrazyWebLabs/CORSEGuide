using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShareProject.Models;
using ShareProject.Models.DTOs.InputDTO;
using ShareProject.Models.DTOs.OutputDTO;
using System.Collections.ObjectModel;
using UMAD_GUIDEE.Services;

namespace UMAD_GUIDEE.ViewModels;

public partial class NoteViewModel : ObservableObject
{
    private readonly HttpService _httpService;

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _description;

    [ObservableProperty]
    private ObservableCollection<Category> _categories = [];

    [ObservableProperty]
    private Category? _selectedCategory;


    public NoteViewModel(HttpService httpService)
    {
        _httpService = httpService;
    }

    /// <summary>
    /// LoadNoteAsync method is used to load the note data into the view model.
    /// </summary>
    /// <param name="noteDto"></param>
    /// <returns></returns>
    public async Task LoadNoteAsync(NoteOutputDTO? noteDto = null)
    {
        if ( Categories.Count == 0 )
        {
            await LoadDropdownData();
        }

        if ( noteDto == null )
            return;

        Id = noteDto.Id;
        Title = noteDto.Title;
        Description = noteDto.Description;

        //SelectedWorker = Workers.FirstOrDefault(w => w.Id == noteDto.WorkerId);
        SelectedCategory = Categories.FirstOrDefault(c => c.Id == noteDto.CategoryId);
    }

    /// <summary>
    /// LoadDropdownData method is used to load the dropdown data into the view model.
    /// </summary>
    /// <returns></returns>
    private async Task LoadDropdownData()
    {
        //var workersList = await _httpService.GetAllWorkers();
        var categoriesList = await _httpService.GetAllCategories();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Categories = [ .. categoriesList ];
        });
    }

    /// <summary>
    /// SaveData method is used to save the note data into the database.
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    public async Task SaveData()
    {
        if ( Id.Equals(0) )
            await InsertNote();
        else
            await UpdateNote();
    }

    /// <summary>
    /// InsertNote method is used to insert a new note into the database.
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    public async Task InsertNote()
    {
        if ( string.IsNullOrEmpty(Title) ||  SelectedCategory == null )
        {
            await Shell.Current.DisplayAlert("Error", "Please fill in all fields.", "OK");
            return;
        }

        var newNote = new NoteInputDTO
        {
            Title = Title,
            Description = Description,
            UserEmail = await SecureStorage.GetAsync("email"),
            //UserId = Worker.
            //UserId = SelectedWorker.Id,
            CategoryId = SelectedCategory.Id
        };

        await _httpService.CreateNote(newNote);
        await Shell.Current.DisplayAlert("Success", "Note created successfully.", "OK");
        WeakReferenceMessenger.Default.Send(new RefreshMessage(true));
        await Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// UpdateNote method is used to update an existing note in the database.
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    public async Task UpdateNote()
    {
        if ( string.IsNullOrEmpty(Title) || SelectedCategory == null )
        {
            await Shell.Current.DisplayAlert("Error", "Please fill in all fields.", "OK");
            return;
        }
        var updatedNote = new NoteInputDTO
        {
            Id = Id,
            Title = Title,
            Description = Description,
            UserEmail = await SecureStorage.GetAsync("email"),
            //UserId = SelectedWorker.Id,
            CategoryId = SelectedCategory.Id
        };
        await _httpService.UpdateNote(updatedNote);
        await Shell.Current.DisplayAlert("Success", "Note updated successfully.", "OK");
        WeakReferenceMessenger.Default.Send(new RefreshMessage(true));
        await Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// DeletePart method is used to delete a note from the database.
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    public async Task DeletePart()
    {
        if ( Id.Equals(0) )
            return;

        bool action = await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete this note?", "Yes", "No");

        if ( !action )
            return;

        var noteToDelete = new NoteInputDTO
        {
            Id = Id,
            Title = Title,
            Description = Description,
            UserEmail = await SecureStorage.GetAsync("email"),
            //UserId = SelectedWorker.Id,
            CategoryId = SelectedCategory.Id
        };

        await _httpService.DeleteNote(noteToDelete);
        WeakReferenceMessenger.Default.Send(new RefreshMessage(true));
        await Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// DoneEditing method is used to navigate back to the previous page.
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    public async Task DoneEditing()
    {
        await Shell.Current.GoToAsync("..");
    }
    
}
