using Microsoft.Maui.Controls;
using UMAD_GUIDEE.Models;
using System.Collections.Generic;

namespace UMAD_GUIDEE.Pages
{
    public partial class HomePage : ContentPage
    {
        private readonly List<Announcements> _publications = new List<Announcements>
        {
            new Announcements { Title = "Dr. Juan Pérez", Description = "¡No te pierdas la conferencia sobre inteligencia artificial este viernes en el auditorio principal!" },
            new Announcements { Title = "Mtra. Laura González", Description = "Curso de redacción académica disponible para todos los estudiantes. Inscripciones abiertas." },
            new Announcements { Title = "Ing. Roberto Castillo", Description = "Taller de desarrollo web con React y Astro. Cupo limitado, regístrate en servicios escolares." },
            new Announcements { Title = "Dra. Sofía Ramírez", Description = "Convocatoria abierta para servicio social en el laboratorio de biotecnología. Consulta los requisitos." },
            new Announcements { Title = "Lic. Daniel Ortega", Description = "Se buscan voluntarios para el programa de tutorías. Gana experiencia y horas de servicio." },
            new Announcements { Title = "Mtro. Carlos Fernández", Description = "Nueva beca de movilidad estudiantil para intercambio internacional. Infórmate en la oficina de becas." }
        };

        private List<Announcements> _filteredpublications;

        public HomePage()
        {
            InitializeComponent();
            _filteredpublications = new List<Announcements>(_publications);
            listAnnouncement.ItemsSource = _publications;

            // Suscribirse al mensaje para agregar nuevas publicaciones
            MessagingCenter.Subscribe<CreateAnnouncementPage, Announcements>(this, "AddAnnouncement", (sender, newAnnouncement) =>
            {
                _publications.Add(newAnnouncement);
                listAnnouncement.ItemsSource = null;
                listAnnouncement.ItemsSource = _publications;
            });
        }

        private async void OnContactSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Announcements selectedAnnouncement)
            {
                await Navigation.PushAsync(new AnnouncementsDetails(selectedAnnouncement));
                listAnnouncement.SelectedItem = null;
            }
        }

        private async void OnFloatingButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateAnnouncementPage());
        }
    }
}