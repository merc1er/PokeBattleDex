using BattleDex.Contracts.Services;

namespace BattleDex.Services;

public class AppSettingsService : IAppSettingsService
{
    private const string ShowCaughtColumnKey = "ShowCaughtColumn";

    private readonly ILocalSettingsService _localSettingsService;
    private readonly SemaphoreSlim _initGate = new(1, 1);

    private bool _showCaughtColumn = true;
    private bool _loaded;

    public event EventHandler? SettingsChanged;

    public AppSettingsService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public bool ShowCaughtColumn => _showCaughtColumn;

    public async Task EnsureLoadedAsync()
    {
        if (_loaded)
        {
            return;
        }

        await _initGate.WaitAsync();
        try
        {
            if (_loaded)
            {
                return;
            }

            var saved = await _localSettingsService.ReadSettingAsync<bool?>(ShowCaughtColumnKey);
            if (saved.HasValue)
            {
                _showCaughtColumn = saved.Value;
            }

            _loaded = true;
        }
        finally
        {
            _initGate.Release();
        }
    }

    public async Task SetShowCaughtColumnAsync(bool value)
    {
        if (_showCaughtColumn == value)
        {
            return;
        }

        _showCaughtColumn = value;
        await _localSettingsService.SaveSettingAsync(ShowCaughtColumnKey, value);
        SettingsChanged?.Invoke(this, EventArgs.Empty);
    }
}
