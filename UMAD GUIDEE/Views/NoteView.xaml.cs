using ShareProject.Models.DTOs.OutputDTO;
using UMAD_GUIDEE.ViewModels;

namespace UMAD_GUIDEE.Views;

[QueryProperty("NoteToDisplay", "note")]
public partial class NoteView : ContentPage
{
	private readonly NoteViewModel _viewModel;
    private NoteOutputDTO _noteToDisplay;

    /// <summary>
    /// Constructor que inicializa la clase NoteView
    /// </summary>
    /// <param name="viewModel"></param>
    public NoteView(NoteViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
        BindingContext = _viewModel;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await _viewModel.LoadNoteAsync();
        });
    }

    public NoteOutputDTO NoteToDisplay
    {
        get => _noteToDisplay;
        set
        {
            if ( _noteToDisplay == value )
                return;

            _noteToDisplay = value;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _viewModel.LoadNoteAsync(_noteToDisplay);
            });
        }
    }
}