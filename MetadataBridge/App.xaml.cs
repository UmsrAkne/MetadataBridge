using System.Windows;
using Prism.DryIoc;
using Prism.Ioc;
using MetadataBridge.Views;

namespace MetadataBridge;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
    }

    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }
}