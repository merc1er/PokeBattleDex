using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

using BattleDex.Contracts.Services;
using BattleDex.Helpers;
using BattleDex.ViewModels;

using Windows.Graphics;
using Windows.System;

namespace BattleDex.Views;

public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel
    {
        get;
    }

    public ShellPage(ShellViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationService.Navigated += OnNavigated;

        // Set the title bar icon
        var iconPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Square44x44Logo.targetsize-24_altform-unplated.png");
        AppTitleBarIcon.Source = new BitmapImage(new Uri(iconPath));

        // A custom title bar is required for full window theme and Mica support.
        // https://docs.microsoft.com/windows/apps/develop/title-bar?tabs=winui3#full-customization
        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        SetMenuBarPassthrough();
        TitleBarMenuBar.SizeChanged += (_, _) => SetMenuBarPassthrough();

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));

        // Move focus away from the MenuBar so "Help" isn't highlighted on startup
        NavigationFrame.Focus(FocusState.Programmatic);
    }

    private void SetMenuBarPassthrough()
    {
        var scale = XamlRoot.RasterizationScale;
        var transform = TitleBarMenuBar.TransformToVisual(null);
        var position = transform.TransformPoint(new Windows.Foundation.Point(0, 0));

        var region = new RectInt32
        {
            X = (int)(position.X * scale),
            Y = (int)(position.Y * scale),
            Width = (int)(TitleBarMenuBar.ActualWidth * scale),
            Height = (int)(TitleBarMenuBar.ActualHeight * scale),
        };

        var nonClientSource = InputNonClientPointerSource.GetForWindowId(App.MainWindow.AppWindow.Id);
        nonClientSource.SetRegionRects(NonClientRegionKind.Passthrough, [region]);
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        App.AppTitlebar = AppTitleBarText as UIElement;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        ViewModel.NavigationService.Navigated -= OnNavigated;
    }

    private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            ViewModel.NavigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
            return;
        }

        if (args.InvokedItemContainer is NavigationViewItem item && item.Tag is string tag)
        {
            ViewModel.NavigationService.NavigateTo(tag);
        }
    }

    private void OnNavigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        var pageService = App.GetService<IPageService>();

        if (pageService.GetPageType(typeof(SettingsViewModel).FullName!) == e.SourcePageType)
        {
            NavigationViewControl.SelectedItem = NavigationViewControl.SettingsItem;
            return;
        }

        foreach (var menuItem in NavigationViewControl.MenuItems.OfType<NavigationViewItem>())
        {
            if (menuItem.Tag is string tag && pageService.GetPageType(tag) == e.SourcePageType)
            {
                NavigationViewControl.SelectedItem = menuItem;
                return;
            }
        }
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }
}
