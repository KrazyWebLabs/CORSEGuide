namespace UMAD_GUIDEE.Pages;
using UMAD_GUIDEE.Models;
using System.Threading.Tasks;

public partial class HomePage : ContentPage
{
    private readonly List<Announcements> _publications = [
        new(){Title="Dr. Juan Pérez", Description="¡No te pierdas la conferencia sobre inteligencia artificial este viernes en el auditorio principal!"},
        new(){Title="Mtra. Laura González", Description="Curso de redacción académica disponible para todos los estudiantes. Inscripciones abiertas."},
        new(){Title="Ing. Roberto Castillo", Description="Taller de desarrollo web con React y Astro. Cupo limitado, regístrate en servicios escolares."},
        new(){Title="Dra. Sofía Ramírez", Description="Convocatoria abierta para servicio social en el laboratorio de biotecnología. Consulta los requisitos."},
        new(){Title="Lic. Daniel Ortega", Description="Se buscan voluntarios para el programa de tutorías. Gana experiencia y horas de servicio."},
        new(){Title="Mtro. Carlos Fernández", Description="Nueva beca de movilidad estudiantil para intercambio internacional. Infórmate en la oficina de becas."}
        ];
    private List<Announcements> _filteredpublications;
    public HomePage()
    {
        InitializeComponent();
        _filteredpublications = new(_publications);
        listAnnouncement.ItemsSource = _publications;
    }

    private async void OnContactSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Announcements selectedAnnouncement)
        {
            await Navigation.PushAsync(new AnnouncementsDetails(selectedAnnouncement));
            listAnnouncement.SelectedItem = null;
        }
    }
}
