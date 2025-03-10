namespace UMAD_GUIDEE.Pages;
using UMAD_GUIDEE.Models;
using System.Threading.Tasks;

public partial class HomePage : ContentPage
{
    private readonly List<Announcements> _publications = [
        new(){Title="Dr. Juan P�rez", Description="�No te pierdas la conferencia sobre inteligencia artificial este viernes en el auditorio principal!"},
        new(){Title="Mtra. Laura Gonz�lez", Description="Curso de redacci�n acad�mica disponible para todos los estudiantes. Inscripciones abiertas."},
        new(){Title="Ing. Roberto Castillo", Description="Taller de desarrollo web con React y Astro. Cupo limitado, reg�strate en servicios escolares."},
        new(){Title="Dra. Sof�a Ram�rez", Description="Convocatoria abierta para servicio social en el laboratorio de biotecnolog�a. Consulta los requisitos."},
        new(){Title="Lic. Daniel Ortega", Description="Se buscan voluntarios para el programa de tutor�as. Gana experiencia y horas de servicio."},
        new(){Title="Mtro. Carlos Fern�ndez", Description="Nueva beca de movilidad estudiantil para intercambio internacional. Inf�rmate en la oficina de becas."}
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
