using Microsoft.Maui.Controls;
using UMAD_GUIDEE.Models;

namespace UMAD_GUIDEE.Pages
{
    public partial class CreateAnnouncementPage : ContentPage
    {
        public CreateAnnouncementPage()
        {
            InitializeComponent();
        }

        private async void OnCreateAnnouncementClicked(object sender, EventArgs e)
        {
            string title = TitleEntry.Text;
            string description = DescriptionEditor.Text;

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
            {
                await DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
                return;
            }

            var newAnnouncement = new Announcements { Title = title, Description = description };

            // Aquí puedes agregar la lógica para guardar la nueva publicación
            // Por ejemplo, agregarla a la lista de publicaciones en HomePage
            MessagingCenter.Send(this, "AddAnnouncement", newAnnouncement);

            await Navigation.PopAsync(); // Regresar a la página anterior
        }
    }
}