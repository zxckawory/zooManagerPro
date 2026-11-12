using Avalonia.Controls.Notifications;
using Avalonia.Media.Imaging;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ursa.Controls;
using zooManagerPro.Context;
using zooManagerPro.ViewModels;
using zooManagerPro.Views;
using static System.Net.Mime.MediaTypeNames;

namespace zooManagerPro.Models;

public partial class Enclosure
{
    public int Id { get; set; }

    public int TypeOfEnclosureId { get; set; }

    public int Capacity { get; set; }

    public int MaxCapacity { get; set; }

    public int EnclosureStatusId { get; set; }

    public decimal Square { get; set; }

    public int ClimateZoneId { get; set; }

    public DateOnly LastDateOfTechnicalInsperation { get; set; }

    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();

    public virtual ClimateZone ClimateZone { get; set; } = null!;

    public virtual EnclosureStatus EnclosureStatus { get; set; } = null!;

    public virtual TypeOfEnclosure TypeOfEnclosure { get; set; } = null!;

    public string EnclosureAndType
    {
        get
        {
            return "№ " + Id + " - " + TypeOfEnclosure.TypeOfEnclosureName;
        }
    }
    [RelayCommand]
    public async Task UpdateEnclosureAsync(Enclosure enclosure)
    {
        await MainView.Current.ReplaceAsync(new CrudEnclosureView()
        {
            DataContext = new CrudEnclosureViewModel(enclosure)
        });
    }
    [RelayCommand]
    public async Task DeleteEnclosureAsync(Enclosure enclosure)
    {
        await using ZooManagerProContext context = new();
        var a = await OverlayMessageBox.ShowAsync("Удалить вольер?", "Удаление", null, MessageBoxIcon.Warning, MessageBoxButton.YesNo);
        if (a != MessageBoxResult.Yes)
        {
            return;
        }
        else
        {
            var selectedEnclosure = context.Enclosures.First(x => x.Id == enclosure.Id);
            if (selectedEnclosure != null)
            {
                context.Enclosures.Remove(selectedEnclosure);
                await context.SaveChangesAsync();
            }
            await EnclosuresView.Current.Load();

            MainView.Current.ShowNotification("Успешно", "Удаление прошло успешно", NotificationType.Success);
        }
    }

    public Bitmap Image
    {
        get
        {
            try
            {
                using var generator = new QRCodeGenerator();
                using var data = generator.CreateQrCode(Id.ToString(), QRCodeGenerator.ECCLevel.Q);
                var png = new PngByteQRCode(data);
                byte[] bytes = png.GetGraphic(10);

                using var ms = new MemoryStream(bytes);
                return new Bitmap(ms);

            }
            catch
            {
                return new Bitmap("Images/zaglushka.jpg");
            }
        }
    }
}
