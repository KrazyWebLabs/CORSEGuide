using UMAD_GUIDEE.ViewModels;

namespace UMAD_GUIDEE.Views;

public partial class LogInView : ContentPage
{
	public LogInView(LogInViewModel logInViewModel)
	{
		InitializeComponent();
		BindingContext = logInViewModel;
    }
}