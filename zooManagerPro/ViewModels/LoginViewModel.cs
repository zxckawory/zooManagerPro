using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zooManagerPro.Context;
using zooManagerPro.Models;

namespace zooManagerPro.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial string Login { get; set; }
        [ObservableProperty]
        public partial string Password { get; set; }

        [RelayCommand]
        public async Task EntranceAsync()
        {

            await using ZooManagerProContext context = new();

            if (context.Employees.FirstOrDefault(x => x.Login == Login && x.Password == Password) is Employee employee)
            {
                await MainView.Current.ReplaceAsync(new HomeView() 
                {
                    DataContext = new HomeViewModel()
                });
            }
            else
            {
                MainView.Current.ShowNotification("Ошибка", "Неверный логин или пароль", Avalonia.Controls.Notifications.NotificationType.Error);
            }
        }
    }
}
