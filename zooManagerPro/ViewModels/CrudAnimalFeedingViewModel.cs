using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zooManagerPro.Context;
using zooManagerPro.Models;
using zooManagerPro.Views;

namespace zooManagerPro.ViewModels
{
    public partial class CrudAnimalFeedingViewModel : ObservableObject
    {
        ZooManagerProContext context = new();
        [ObservableProperty]
        public partial int FoodAmount { get; set; } = 0;
        [ObservableProperty]
        public partial string FeedingDes { get; set; } = null!;

        public ObservableCollection<Animal> Animals { get; set; } = null!;
        public ObservableCollection<TypeOfFeeding> TypeOfFeedings { get; set; } = null!;
        public ObservableCollection<UnitOfMass> UnitOfMasses { get; set; } = null!;
        public ObservableCollection<Employee> Employees { get; set; } = null!;
        [ObservableProperty]
        private DateTime _selectedFeedingDateTime = DateTime.Today;

        [ObservableProperty]
        private Animal _selectedAnimal = null!;
        [ObservableProperty]
        private TypeOfFeeding _selectedTypeOfFeeding = null!;
        [ObservableProperty]
        private UnitOfMass _selectedUnitOfMass = null!;
        [ObservableProperty]
        private Employee _selectedEmploee = null!;
        private AnimalsFeeding animalsFeeding { get; set; } = new();

        public CrudAnimalFeedingViewModel()
        {
            _ = Load();
        }

        private async Task Load()
        {
            Animals = new(context.Animals.Include(x => x.Species).Include(x => x.Enclosure).ThenInclude(x => x.TypeOfEnclosure));
            TypeOfFeedings = new(context.TypeOfFeedings);
            UnitOfMasses = new(context.UnitOfMasses);
            Employees = new(context.Employees);
        }

        [RelayCommand]
        public async Task SaveAnimalFeedingAsync()
        {
            if (string.IsNullOrEmpty(FeedingDes) || FoodAmount <= 0 || SelectedAnimal == null || SelectedTypeOfFeeding == null || SelectedUnitOfMass == null || SelectedEmploee == null)
            {
                MainView.Current.ShowNotification("Ошибка", "Поля не заполнены", NotificationType.Error);
                return;
            }
            try
            {
                animalsFeeding.AnimalId = SelectedAnimal.Id;
                animalsFeeding.FeedingDateTime = SelectedFeedingDateTime;
                animalsFeeding.TypeOfFeedingId = SelectedTypeOfFeeding.Id;
                animalsFeeding.FeedAmount = FoodAmount;
                animalsFeeding.UnitOfMassId = SelectedUnitOfMass.Id;
                animalsFeeding.EmployeeId = SelectedEmploee.Id;
                animalsFeeding.FeedingDescription = FeedingDes;

                context.AnimalsFeedings.Add(animalsFeeding);
                await context.SaveChangesAsync();
                await MainView.Current.ReplaceAsync(new AnimalFeedingView
                {
                    DataContext = new AnimalFeedingViewModel()
                });
            }
            catch
            {
                MainView.Current.ShowNotification("Ошибка", "Поля неверно заполнены", NotificationType.Error);
            }
        }
    }
}
