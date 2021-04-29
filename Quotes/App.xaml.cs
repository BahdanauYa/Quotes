using Prism.Ioc;
using System.Windows;
using Quotes.Views;

namespace Quotes
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
