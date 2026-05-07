using Microsoft.Extensions.DependencyInjection;

namespace FitGames.app
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        // Expose the application's service provider so views can resolve services from parameterless constructors
        public static IServiceProvider Services { get; private set; } = default!;

        public App(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Services = serviceProvider;
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_serviceProvider.GetRequiredService<AppShell>());
        }
    }
}