using UMAD_GUIDEE.ViewModels;

namespace UMAD_GUIDEE.Views;

public partial class NotesView : ContentPage
{
    /// <summary>
    /// Constructor for the NotesView.
    /// </summary>
    /// <param name="notesViewModel"></param>
    public NotesView(NotesViewModel notesViewModel)
	{
		InitializeComponent();
        BindingContext = notesViewModel;
        BttSecure(notesViewModel);
    }

    /// <summary>
    /// Sets the visibility of the button based on the user's role in this case Teacher.
    /// </summary>
    /// <param name="notesViewModel"></param>
    private async void BttSecure(NotesViewModel notesViewModel)
    {
        string? role = await SecureStorage.GetAsync("userRole");

        notesViewModel.IsVisible = role == "Teacher";
    }

}