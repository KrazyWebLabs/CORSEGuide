using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShareProject.Models.DTOs.OutputDTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMAD_GUIDEE.Services;
using UMAD_GUIDEE.Views;

namespace UMAD_GUIDEE.ViewModels
{
    public partial class NotesViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<NoteOutputDTO> _notes;

        [ObservableProperty]
        private bool _isRefreshing = false;

        [ObservableProperty]
        private bool _isBusy = false;

        [ObservableProperty]
        private bool _isVisible = false;

        [ObservableProperty]
        private NoteOutputDTO? _selectedNote;

        private readonly HttpService _httpService;

        public NotesViewModel(HttpService httpService)
        {
            _httpService = httpService;
            _notes = [];

            WeakReferenceMessenger.Default.Register<RefreshMessage>(this, async (r, m) =>
            {
                await LoadData();
            });

            Task.Run(LoadData);
        }

        /// <summary>
        /// Método que se ejecuta cuando se selecciona una nota
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task NoteSelected()
        {
            if ( SelectedNote == null )
                return;

            var navigationParameter = new Dictionary<string, object>()
            {
                { "Note", SelectedNote }
            };

            // await Shell.Current.GoToAsync(nameof(NoteView), navigationParameter);
            await Shell.Current.GoToAsync($"{nameof(NoteView)}?NoteId={SelectedNote.Id}");

            MainThread.BeginInvokeOnMainThread(() =>
            {
                SelectedNote = null;
            });
        }

        /// <summary>
        /// Método que carga los datos de las notas
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task LoadData()
        {
            if ( IsBusy )
                return;

            try
            {
                IsRefreshing = true;
                IsBusy = true;

                var notesCollection = await _httpService.GetNotes();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Notes.Clear();

                    foreach ( NoteOutputDTO note in notesCollection )
                    {
                        Notes.Add(note);
                    }
                });
            }
            catch ( Exception ex )
            {
                Console.WriteLine($"Error loading notes: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        /// <summary>
        /// Método que se ejecuta cuando se selecciona el botón de añadir nueva nota
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public static async Task AddNewNote()
        {
            await Shell.Current.GoToAsync(nameof(NoteView));

        }
    }
}
