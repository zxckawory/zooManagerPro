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
    public partial class AnimalFeedingViewModel : ObservableObject
    {
        ZooManagerProContext context = new();
        public ObservableCollection<Animal> Animals { get; set; } = null!;
        public ObservableCollection<TypeOfFeeding> TypeOfFeedings { get; set; } = null!;
        [ObservableProperty]
        private DateOnly _selectedDateOfFeeding = DateOnly.FromDateTime(DateTime.Today);
        [ObservableProperty]
        public ObservableCollection<AnimalsFeeding> _feedingAnimals = new();
        private List<AnimalsFeeding> AnimalsFeedingNotSorted = new();
        [ObservableProperty]
        private Animal _selectedAnimals = null!;
        [ObservableProperty]
        private TypeOfFeeding _selectedFeeding = null!;
        private bool _isInitializing = false;

        partial void OnSelectedAnimalsChanged(Animal value)
        {
            if (!_isInitializing) Sort();
        }
        partial void OnSelectedFeedingChanged(TypeOfFeeding value)
        {
            if (!_isInitializing) Sort();
        }
        partial void OnSelectedDateOfFeedingChanging(DateOnly value)
        {
            if (!_isInitializing) Sort();
        }

        [RelayCommand]
        public async Task CrudAnimalFeedingPageAsync()
        {
            await MainView.Current.PushAsync(new CrudAnimalFeedingView()
            {
                DataContext = new CrudAnimalFeedingViewModel()
            });
        }

        public AnimalFeedingViewModel()
        {
            _ = Load();
        }

        public async Task Load()
        {
            _isInitializing = true;
            Animals = new(context.Animals);
            TypeOfFeedings = new(context.TypeOfFeedings);

            List<AnimalsFeeding> animalsFeedings = new(context.AnimalsFeedings.Include(x => x.Employee).Include(x => x.TypeOfFeeding).Include(x => x.UnitOfMass));
            AnimalsFeedingNotSorted = animalsFeedings;
            Animals.Insert(0, new Animal
            {
                Id = 0,
                AnimalName = "Животное"
            });

            TypeOfFeedings.Insert(0, new TypeOfFeeding
            {
                Id = 0,
                TypeOfFeedingName = "Вид корма"
            });

            SelectedAnimals ??= Animals[0];
            SelectedFeeding ??= TypeOfFeedings[0];

            _isInitializing = false;
            Sort();
        }

        private void Sort()
        {
            List<AnimalsFeeding> animalsFeedings = AnimalsFeedingNotSorted;

            if (SelectedAnimals != null && SelectedAnimals.Id != 0)
            {
                animalsFeedings = animalsFeedings.Where(x => x.AnimalId == SelectedAnimals.Id).ToList();
            }

            if (SelectedFeeding != null && SelectedFeeding.Id != 0)
            {
                animalsFeedings = animalsFeedings.Where(x => x.TypeOfFeedingId == SelectedFeeding.Id).ToList();
            }

            if (SelectedDateOfFeeding != DateOnly.FromDateTime(DateTime.Today))
            {
                animalsFeedings = animalsFeedings.Where(x => DateOnly.FromDateTime(x.FeedingDateTime.Date) == SelectedDateOfFeeding).ToList();
            }

            if (FeedingAnimals == null)
            {
                FeedingAnimals = new ObservableCollection<AnimalsFeeding>(animalsFeedings);
            }
            else
            {
                FeedingAnimals.Clear();
                foreach (var a in animalsFeedings)
                    FeedingAnimals.Add(a);
            }
        }
    }
}
