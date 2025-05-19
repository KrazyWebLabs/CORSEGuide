using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UMAD_GUIDEE.Services;

namespace UMAD_GUIDEE.ViewModels
{
    public partial class LogInViewModel : ObservableObject
    {
        private readonly HttpService _httpService;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private bool _isRunning;

        public LogInViewModel(HttpService httpService)
        {
            _httpService = httpService;
        }

        /// <summary>
        /// Command to log in the user
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task Login()
        {
            IsRunning = true;

            if ( Email == null || Password == null )
            {
                await Application.Current.Windows[0].Page.DisplayAlert("Error", "Please enter your email and password.", "OK");
                return;
            }

            bool result = await _httpService.InitializeClient(Email, Password);

            if ( result )
            {
                Application.Current.Windows[0].Page = new AppShell();
                return;
            }

            await Application.Current.Windows[0].Page.DisplayAlert("Error", "Invalid email or password.", "OK");

            IsRunning = false;
        }
    }
}
