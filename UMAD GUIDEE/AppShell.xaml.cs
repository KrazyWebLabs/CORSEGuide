using UMAD_GUIDEE.ViewModels;
using UMAD_GUIDEE.Views;

namespace UMAD_GUIDEE
{
    public partial class AppShell : Shell
    {
        /// <summary>
        /// Constructor del AppShell encargado de recoger las rutas de las vistas y de inicializar el menu
        /// </summary>
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NoteView), typeof(NoteView));
            SetupMenuBasedOnRole();
        }

        private async void SetupMenuBasedOnRole()
        {
            string? role = await SecureStorage.GetAsync("userRole");

            //NoteOption.IsVisible = role == "Teacher";
        }

        /// <summary>
        /// Metodo en donde se libera el SecureStorage y se redirige a la vista de login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogOut(object sender, EventArgs e)
        {
            // Logout
            SecureStorage.Remove("userName");
            SecureStorage.Remove("userRole");
            SecureStorage.Remove("accessToken");

            NavigationPage logInView = new(App.ServiceProvider.GetRequiredService<LogInView>());


            Application.Current.Windows[0].Page = logInView;
        }
    }
}
