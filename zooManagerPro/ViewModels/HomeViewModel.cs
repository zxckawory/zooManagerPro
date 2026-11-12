using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Ursa.Controls;
using zooManagerPro.Context;
using zooManagerPro.Models;
using zooManagerPro.Views;

namespace zooManagerPro.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        public ObservableCollection<Species> Species { get; set; } = null!;

        private List<Animal> AnimalsNotSorted = new();

        private bool _isInitializing = false;

        public ObservableCollection<HealthStatus> HealthStatuses { get; set; } = null!;

        [ObservableProperty]
        public ObservableCollection<Animal> _animals = new();

        [ObservableProperty]
        private int _animalsCount = 0;

        [ObservableProperty]
        private int _speciesCount = 0;

        [ObservableProperty]
        private int _freeEnclosure = 0;

        [ObservableProperty]
        private int _lastDateVeterinar = 0;

        [ObservableProperty]
        private Species _selectedSpecies = null!;

        partial void OnSelectedSpeciesChanged(Species value)
        {
            if (!_isInitializing) Sort();
        }
        [ObservableProperty]
        private HealthStatus _selectedHealthStatus = null!;

        partial void OnSelectedHealthStatusChanged(HealthStatus value)
        {
            if (!_isInitializing) Sort();
        }

        [ObservableProperty]
        private string _searchText = null!;

        partial void OnSearchTextChanged(string value)
        {
            if (!_isInitializing) Sort();
        }

        ZooManagerProContext context = new();

        [RelayCommand]
        public async Task AddAsync()
        {
            await MainView.Current.PushAsync(new AnimalView
            {
                DataContext = new AnimalViewModel()
            });
        }

        [RelayCommand]
        public async Task EnclosurePageAsync()
        {
            await MainView.Current.PushAsync(new EnclosuresView()
            {
                DataContext = new EnclosuresViewModel()
            });
        }

        [RelayCommand]
        public async Task AnimalFeedingPageAsync()
        {
            await MainView.Current.PushAsync(new AnimalFeedingView()
            {
                DataContext = new AnimalFeedingViewModel()
            });
        }

        public HomeViewModel()
        {
            _ = Load();
        }

        public async Task Load()
        {
            _isInitializing = true;

            Species = new(context.Species);
            HealthStatuses = new(context.HealthStatuses);

            List<Animal> animals = new(context.Animals
                .Include(x => x.Species)
                .Include(x => x.DietaryRegimen)
                .Include(x => x.Gender)
                .Include(x => x.HealthStatus)
                .Include(x => x.Enclosure).ThenInclude(x => x.TypeOfEnclosure));
            AnimalsNotSorted = animals;

            AnimalsCount = context.Animals.Count();
            SpeciesCount = context.Species.Count();
            FreeEnclosure = context.Enclosures.Where(x => x.EnclosureStatusId == 1).Count();
            var today = DateOnly.FromDateTime(DateTime.Today);

            LastDateVeterinar = Animals.Count(a =>
                (today.DayNumber - a.LastDayOfVeterinaryExamination.DayNumber) >= 7);

            Species.Insert(0, new Species
            {
                Id = 0,
                SpeciesName = "Вид"
            });

            HealthStatuses.Insert(0, new HealthStatus
            {
                Id = 0,
                HealthStatusName = "Статус здоровья"
            });

            SelectedSpecies ??= Species[0];
            SelectedHealthStatus ??= HealthStatuses[0];

            _isInitializing = false;
            Sort();
        }

        private void Sort()
        {
            List<Animal> animals = AnimalsNotSorted;

            if (SelectedSpecies != null && SelectedSpecies.Id != 0)
            {
                animals = animals.Where(x => x.SpeciesId == SelectedSpecies.Id).ToList();
            }

            if (SelectedHealthStatus != null && SelectedHealthStatus.Id != 0)
            {
                animals = animals.Where(x => x.HealthStatusId == SelectedHealthStatus.Id).ToList();
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var search = SearchText.ToLower();
                animals = animals.Where(x =>
                    (x.AnimalName ?? string.Empty).ToLower().Contains(search)
                    || (x.Species?.SpeciesName ?? string.Empty).ToLower().Contains(search)
                    || (x.Enclosure?.TypeOfEnclosure?.TypeOfEnclosureName ?? string.Empty).ToLower().Contains(search)
                    || (x.HealthStatus?.HealthStatusName ?? string.Empty).ToLower().Contains(search)
                ).ToList();
            }

            if (Animals == null)
            {
                Animals = new ObservableCollection<Animal>(animals);
            }
            else
            {
                Animals.Clear();
                foreach (var a in animals)
                    Animals.Add(a);
            }
        }
    }
}
