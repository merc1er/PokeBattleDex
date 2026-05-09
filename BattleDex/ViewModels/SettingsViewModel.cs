using CommunityToolkit.Mvvm.ComponentModel;

using BattleDex.Contracts.Services;
using BattleDex.Contracts.ViewModels;

namespace BattleDex.ViewModels;

public partial class SettingsViewModel : ObservableRecipient, INavigationAware
{
    private readonly IAppSettingsService _appSettingsService;
    private bool _settingsLoaded;

    [ObservableProperty]
    public partial bool ShowCaughtColumn { get; set; } = true;

    public SettingsViewModel(IAppSettingsService appSettingsService)
    {
        _appSettingsService = appSettingsService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        await _appSettingsService.EnsureLoadedAsync();
        ShowCaughtColumn = _appSettingsService.ShowCaughtColumn;
        _settingsLoaded = true;
    }

    public void OnNavigatedFrom()
    {
    }

    partial void OnShowCaughtColumnChanged(bool value)
    {
        if (_settingsLoaded)
        {
            _ = _appSettingsService.SetShowCaughtColumnAsync(value);
        }
    }
}
