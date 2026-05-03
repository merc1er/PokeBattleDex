namespace BattleDex.Contracts.Services;

public interface IAppSettingsService
{
    Task EnsureLoadedAsync();

    bool ShowCaughtColumn { get; }

    Task SetShowCaughtColumnAsync(bool value);

    event EventHandler? SettingsChanged;
}
