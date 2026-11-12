using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using zooManagerPro.Context;
using zooManagerPro.Models;
using zooManagerPro.Views;

namespace zooManagerPro.ViewModels
{
    public partial class EnclosuresViewModel : ObservableObject
    {
        ZooManagerProContext context = new();
        [ObservableProperty]
        public ObservableCollection<Enclosure> _enclosures = new();
        [RelayCommand]
        public async Task AddAsync()
        {
            await MainView.Current.PushAsync(new CrudEnclosureView
            {
                DataContext = new CrudEnclosureViewModel()
            });
        }

        public EnclosuresViewModel()
        {
            _ = Load();
        }

        public async Task Load()
        {
            Enclosures = new(context.Enclosures.Include(x => x.EnclosureStatus).Include(x => x.TypeOfEnclosure).Include(x => x.ClimateZone));
        } 
    }
}
