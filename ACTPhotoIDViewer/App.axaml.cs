using ACTPhotoIDViewer.ViewModels;
using ACTPhotoIDViewer.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace ACTPhotoIDViewer
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {

                // Register all the services needed for the application to run
                var collection = new ServiceCollection();
                collection.AddCommonServices();

                // Creates a ServiceProvider containing services from the provided IServiceCollection
                var services = collection.BuildServiceProvider();

                var vm = services.GetRequiredService<MainWindowViewModel>();



                desktop.MainWindow = new MainWindow
                {
                    DataContext = vm
                };
            }
            
              
           

            base.OnFrameworkInitializationCompleted();
        }

    }
}