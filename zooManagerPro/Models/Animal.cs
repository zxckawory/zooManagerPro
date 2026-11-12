using Avalonia.Controls.Notifications;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Ursa.Controls;
using zooManagerPro.Context;
using zooManagerPro.ViewModels;
using zooManagerPro.Views;

namespace zooManagerPro.Models;

public partial class Animal
{
    public int Id { get; set; }

    public string AnimalName { get; set; } = null!;

    public int SpeciesId { get; set; }

    public int GenderId { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public int DietaryRegimenId { get; set; }

    public int HealthStatusId { get; set; }

    public DateOnly LastDayOfVeterinaryExamination { get; set; }

    public int EnclosureId { get; set; }

    public string Photo { get; set; } = null!;

    public virtual ICollection<AnimalsFeeding> AnimalsFeedings { get; set; } = new List<AnimalsFeeding>();

    public virtual DietaryRegiman DietaryRegimen { get; set; } = null!;

    public virtual Enclosure Enclosure { get; set; } = null!;

    public virtual Gender Gender { get; set; } = null!;

    public virtual HealthStatus HealthStatus { get; set; } = null!;

    public virtual Species Species { get; set; } = null!;

    public virtual ICollection<Feature> Features { get; set; } = new List<Feature>();

    public int Age
    {
        get
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            int age = today.Year - DateOfBirth.Year;

            if (DateOfBirth > today.AddYears(-age))
                age--;

            return age;
        }
    }

    [NotMapped]
    public Bitmap? PhotoBitmap
    {
        get
        {
            if (string.IsNullOrEmpty(Photo))
            {
                return new Bitmap("Images/zaglushka.jpg");
            }
            try
            {
                return new Bitmap($"Images/{Photo}");
            }
            catch
            {
                return new Bitmap("Images/zaglushka.jpg");
            }
        }
    }

    [RelayCommand]
    public async Task UpdateAsync(Animal animal)
    {
        await MainView.Current.PushAsync(new AnimalView()
        {
            DataContext = new AnimalViewModel(animal)
        });
    }
    [RelayCommand]
    public async Task DeleteAsync(Animal animal)
    {
        await using ZooManagerProContext context = new();
        var a = await OverlayMessageBox.ShowAsync("Удалить животное?", "Удаление", null, MessageBoxIcon.Warning, MessageBoxButton.YesNo);
        if (a != MessageBoxResult.Yes)
        {
            return;
        }
        else
        {
            var selectedAnimal = context.Animals.First(x => x.Id == animal.Id);
            if (selectedAnimal != null)
            {
                context.Animals.Remove(selectedAnimal);
                await context.SaveChangesAsync();
            }
            await HomeView.Current.Load();

            MainView.Current.ShowNotification("Успешно", "Удаление прошло успешно", NotificationType.Success);
        }
    }

    public string AnimalTypeEnclosure
    {
        get
        {
            return $"{AnimalName}, {Species.SpeciesName}, {Enclosure.TypeOfEnclosure.TypeOfEnclosureName}";
        }
    }
}
