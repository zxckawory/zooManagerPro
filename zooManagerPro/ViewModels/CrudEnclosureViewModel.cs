using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zooManagerPro.Context;
using zooManagerPro.Models;

namespace zooManagerPro.ViewModels
{
    public partial class CrudEnclosureViewModel : ObservableObject
    {
        ZooManagerProContext context = new();
        [ObservableProperty]
        public partial int EnclosureCapacity { get; set; } = 0;
        [ObservableProperty]
        public partial int EnclosureMaxCapacity { get; set; } = 0;
        [ObservableProperty]
        public partial decimal EnclosureSquare { get; set; } = 0;
        [ObservableProperty]
        private DateOnly _selectedDateOfTI = DateOnly.FromDateTime(DateTime.Today);
        [ObservableProperty]
        private TypeOfEnclosure _selectedTypeOfEnclosure = null!;
        [ObservableProperty]
        private EnclosureStatus _selectedStatusEnclosure = null!;
        [ObservableProperty]
        private ClimateZone _selectedClimateZone = null!;
        public ObservableCollection<TypeOfEnclosure> TypeOfEnclosures { get; set; } = null!;
        public ObservableCollection<EnclosureStatus> EnclosureStatuses { get; set; } = null!;
        public ObservableCollection<ClimateZone> ClimateZones { get; set; } = null!;
        private Enclosure enclosure { get; set; } = new();
        private bool IsEdit = false;

        public CrudEnclosureViewModel()
        {
            _ = Load();
        }

        public CrudEnclosureViewModel(Enclosure enclosure1)
        {
            _ = Load();
            enclosure = enclosure1;
            IsEdit = true;
            EnclosureCapacity = enclosure.Capacity;
            EnclosureMaxCapacity = enclosure.MaxCapacity;
            EnclosureSquare = enclosure.Square;
            SelectedDateOfTI = enclosure.LastDateOfTechnicalInsperation;
            SelectedTypeOfEnclosure = context.TypeOfEnclosures.FirstOrDefault(x => x.Id == enclosure.TypeOfEnclosureId)!;
            SelectedStatusEnclosure = context.EnclosureStatuses.FirstOrDefault(x => x.Id == enclosure.EnclosureStatusId)!;
            SelectedClimateZone = context.ClimateZones.FirstOrDefault(x => x.Id == enclosure.ClimateZoneId)!;
        }

        private async Task Load()
        {
            TypeOfEnclosures = new(context.TypeOfEnclosures);
            EnclosureStatuses = new(context.EnclosureStatuses);
            ClimateZones = new(context.ClimateZones);
        }
        [RelayCommand]
        public async Task SaveEnclosureAsync()
        {
            if (/*int.TryParse(EnclosureCapacity.ToString(), out var _)
                || int.TryParse(EnclosureMaxCapacity.ToString(), out var _)
                || decimal.TryParse(EnclosureSquare.ToString(), out var _)
                || */SelectedClimateZone == null
                || SelectedStatusEnclosure == null 
                || SelectedTypeOfEnclosure == null)
            {
                MainView.Current.ShowNotification("Ошибка", "Поля не заполнены", NotificationType.Error);
                return;
            }
            try
            {
                enclosure.Capacity = EnclosureCapacity;
                enclosure.MaxCapacity = EnclosureMaxCapacity;
                enclosure.Square = EnclosureSquare;
                enclosure.LastDateOfTechnicalInsperation = SelectedDateOfTI;
                enclosure.TypeOfEnclosureId = SelectedTypeOfEnclosure.Id;
                enclosure.EnclosureStatusId = SelectedStatusEnclosure.Id;
                enclosure.ClimateZoneId = SelectedClimateZone.Id;
            }
            catch
            {
                MainView.Current.ShowNotification("Ошибка", "Поля неверно заполнены", NotificationType.Error);
            }

            if (IsEdit == true)
            {
                enclosure.ClimateZone = null!;
                enclosure.EnclosureStatus = null!;
                enclosure.TypeOfEnclosure = null!;

                context.Enclosures.Update(enclosure);
                await context.SaveChangesAsync();
                await MainView.Current.ReplaceAsync(new EnclosuresView
                {
                    DataContext = new EnclosuresViewModel()
                });
            }
            else
            {
                enclosure.ClimateZone = null!;
                enclosure.EnclosureStatus = null!;
                enclosure.TypeOfEnclosure = null!;

                context.Enclosures.Add(enclosure);
                await context.SaveChangesAsync();
                await MainView.Current.ReplaceAsync(new EnclosuresView 
                {
                    DataContext = new EnclosuresViewModel()
                });
            }
        }
    }
}
