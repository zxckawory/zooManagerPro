using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using zooManagerPro.ViewModels;

namespace zooManagerPro;

public partial class MainView : NavigationPage
{
    public static MainView Current { get; private set; } = null!;

    public MainView()
    {
        InitializeComponent();

        Current = this;

        Loaded += async (_, _) =>
        {
            await PushAsync(new LoginView()
            {
                DataContext = new LoginViewModel()
            });
        };
    }

    private WindowNotificationManager notificationManager = null!;

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        notificationManager = new WindowNotificationManager(TopLevel.GetTopLevel(this));
    }

    public void ShowNotification(string title, string message, NotificationType notificationType )
    {
        notificationManager.Show(new Notification
        {
            Message = message,
            Expiration = TimeSpan.FromSeconds(3),
            Type = notificationType,
            Title = title,
        });
    }
}