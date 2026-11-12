using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using zooManagerPro.ViewModels;

namespace zooManagerPro;

public partial class HomeView : ContentPage
{
    public static HomeView Current = null!;

    public HomeView()
    {
        InitializeComponent();
        Current = this;
    }

    public async Task Load()
    {
        if (DataContext is HomeViewModel vm)
        {
            _ = vm.Load();
        }
    }
}