using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zooManagerPro.Context;
using zooManagerPro.Models;

namespace zooManagerPro.ViewModels
{
    public partial class AnimalViewModel : ObservableObject
    {
        ZooManagerProContext context = new();
        [ObservableProperty]
        public partial string AnimalName { get; set; } = null!;
        [ObservableProperty]
        private Species _selectedSpecies = null!;
        [ObservableProperty]
        private Gender _selectedGender = null!;
        [ObservableProperty]
        private DietaryRegiman _selectedDietaryRegiman = null!;
        [ObservableProperty]
        private HealthStatus _selectedHealthStatus = null!;
        [ObservableProperty]
        private Enclosure _selectedEnclosure = null!;
        [ObservableProperty]
        private DateOnly _selectedDateOfBirth = DateOnly.FromDateTime(DateTime.Today);
        [ObservableProperty]
        private DateOnly _selectedDateOfLastVeterinaryExamination = DateOnly.FromDateTime(DateTime.Today);

        public ObservableCollection<Species> Species { get; set; } = null!;
        public ObservableCollection<Gender> Genders { get; set; } = null!;
        public ObservableCollection<DietaryRegiman> DietaryRegimans { get; set; } = null!;
        public ObservableCollection<HealthStatus> HealthStatuses { get; set; } = null!;
        public ObservableCollection<Enclosure> Enclosures { get; set; } = null!;

        private Animal animal { get; set; } = new();
        private string newImageName = null!;

        [ObservableProperty]
        private Bitmap _image = new Bitmap("Images/zaglushka.jpg");

        private bool IsEdit = false;

        [RelayCommand]
        public async Task AddPhotoAsync()
        {
            var storage = TopLevel.GetTopLevel(MainView.Current)!.StorageProvider;

            var files = (await storage.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
            {
                AllowMultiple = false,
                FileTypeFilter = [FilePickerFileTypes.ImageAll],
                Title = "Выберите картинку"
            }));

            if (files.Count == 0)
                return;

            var file = files[0];

            var filePath = file.Path.AbsolutePath;

            newImageName = $"{Guid.NewGuid()}{Path.GetExtension(filePath)}";

            var newFilePath = Path.Combine("Images", newImageName);

            File.Copy(filePath, newFilePath);

            Image = new Bitmap(newFilePath);
        }
        [RelayCommand]
        public async Task RemovePhotoAsync()
        {
            Image = new Bitmap("Images/zaglushka.jpg");
        }
        [RelayCommand]
        public async Task SaveAnimal()
        {
            if(string.IsNullOrEmpty(AnimalName) || SelectedSpecies == null || SelectedGender == null || SelectedDietaryRegiman == null || SelectedHealthStatus == null || SelectedEnclosure == null)
            {
                MainView.Current.ShowNotification("Ошибка", "Поля не заполнены", NotificationType.Error);
                return;
            }
            try
            {
                animal.AnimalName = AnimalName;
                animal.SpeciesId = SelectedSpecies.Id;
                animal.GenderId = SelectedGender.Id;
                animal.DietaryRegimenId = SelectedDietaryRegiman.Id;
                animal.HealthStatusId = SelectedHealthStatus.Id;
                animal.EnclosureId = SelectedEnclosure.Id;
                animal.DateOfBirth = SelectedDateOfBirth;
                animal.LastDayOfVeterinaryExamination = SelectedDateOfLastVeterinaryExamination;
                animal.Photo = newImageName;
            }
            catch
            {
                MainView.Current.ShowNotification("Ошибка", "Поля неверно заполнены", NotificationType.Error);
            }

            if(IsEdit == true)
            {
                animal.DietaryRegimen = null!;
                animal.Species = null!;
                animal.Gender = null!;
                animal.HealthStatus = null!;
                animal.Enclosure = null!;

                context.Animals.Update(animal);
                await context.SaveChangesAsync();
                await MainView.Current.ReplaceAsync(new HomeView
                {
                    DataContext = new HomeViewModel()
                });
            }
            else
            {
                animal.DietaryRegimen = null!;
                animal.Species = null!;
                animal.Gender = null!;
                animal.HealthStatus = null!;
                animal.Enclosure = null!;

                context.Animals.Add(animal);
                await context.SaveChangesAsync();
                await MainView.Current.ReplaceAsync(new HomeView
                {
                    DataContext = new HomeViewModel()
                });
            }
        }

        public AnimalViewModel()
        {
            _ = Load();
        }

        public AnimalViewModel(Animal animal1)
        {
            _ = Load();
            animal = animal1;

            IsEdit = true;
            AnimalName = animal.AnimalName;
            SelectedSpecies = context.Species.FirstOrDefault(s => s.Id == animal.SpeciesId)!;
            SelectedGender = context.Genders.FirstOrDefault(g => g.Id == animal.GenderId)!;
            SelectedDietaryRegiman = context.DietaryRegimen.FirstOrDefault(d => d.Id == animal.DietaryRegimenId)!;
            SelectedHealthStatus = context.HealthStatuses.FirstOrDefault(h => h.Id == animal.HealthStatusId)!;
            SelectedEnclosure = context.Enclosures.FirstOrDefault(e => e.Id == animal.EnclosureId)!;
            SelectedDateOfBirth = animal.DateOfBirth;
            SelectedDateOfLastVeterinaryExamination = animal.LastDayOfVeterinaryExamination;
            newImageName = animal.Photo;
            Image = new Bitmap($"Images/{animal.Photo}");
        }

        private async Task Load()
        {
            Species = new(context.Species);
            Genders = new(context.Genders);
            DietaryRegimans = new(context.DietaryRegimen);
            HealthStatuses = new(context.HealthStatuses);
            Enclosures = new(context.Enclosures.Where(x => x.EnclosureStatusId == 1).Include(x => x.TypeOfEnclosure));
        }
    }
}

