using ACTPhotoIDViewer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTPhotoIDViewer
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            //collection.AddSingleton<IRepository, Repository>();
            //collection.AddTransient<BusinessService>();
            //collection.AddTransient<MainViewModel>();
            collection.AddTransient<MainWindowViewModel>();
        }
    }
}

