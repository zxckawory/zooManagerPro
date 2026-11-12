using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace zooManagerPro.ViewModels
{
    public partial class EnclosuresView : ContentPage
    {
        public static EnclosuresView Current = null!;
        public EnclosuresView()
        {
            InitializeComponent();
            Current = this;
        }

        public async Task Load()
        {
            if (DataContext is EnclosuresViewModel evm)
            {
                _ = evm.Load();
            }
        }
    }
}