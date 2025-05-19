using UMAD_GUIDEE.Views;

namespace UMAD_GUIDEE
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Constructor de la Applicacion recibiento el servicio de inyeccion de dependencias.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// Metodo que se encarga de redirigir a la vista de inicio de sesion.
        /// </summary>
        /// <param name="activationState"></param>
        /// <returns></returns>
        protected override Window CreateWindow(IActivationState? activationState)
        {
            NavigationPage logInView = new(ServiceProvider.GetRequiredService<LogInView>());

            return new Window(logInView);
        }
    }
}