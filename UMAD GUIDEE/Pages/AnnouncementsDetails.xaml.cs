namespace UMAD_GUIDEE.Pages;
using UMAD_GUIDEE.Models;

public partial class AnnouncementsDetails : ContentPage
{
	public AnnouncementsDetails(Announcements announcements)
	{
		InitializeComponent();

        titleLabel.Text = $"Title: {announcements.Title}";
        descriptionLabel.Text = $"Description: {announcements.Description}";
    }
}