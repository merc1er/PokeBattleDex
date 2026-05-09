using BattleDex.Contracts.Services;

namespace BattleDex.Services;

public class CaughtPokemonService : ICaughtPokemonService
{
    private const string CaughtPokemonKey = "CaughtPokemon";

    private readonly ILocalSettingsService _localSettingsService;
    private readonly Task _initTask;
    private readonly SemaphoreSlim _writeGate = new(1, 1);

    private HashSet<int> _caughtIds = new();

    public CaughtPokemonService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
        _initTask = LoadAsync();
    }

    public Task EnsureLoadedAsync() => _initTask;

    public bool IsCaught(int id) => _caughtIds.Contains(id);

    public async Task SetCaughtAsync(int id, bool caught)
    {
        // Wait for the initial load before mutating, otherwise a write that races
        // ahead of the load would persist an empty set and wipe saved IDs.
        await _initTask;

        // Serialize mutation + save so rapid fire-and-forget toggles can't
        // interleave HashSet writes or race file saves in LocalSettingsService.
        await _writeGate.WaitAsync();
        try
        {
            var changed = caught ? _caughtIds.Add(id) : _caughtIds.Remove(id);
            if (changed)
            {
                await _localSettingsService.SaveSettingAsync(CaughtPokemonKey, _caughtIds.ToList());
            }
        }
        finally
        {
            _writeGate.Release();
        }
    }

    private async Task LoadAsync()
    {
        var saved = await _localSettingsService.ReadSettingAsync<List<int>>(CaughtPokemonKey);
        if (saved is not null)
        {
            _caughtIds = new HashSet<int>(saved);
        }
    }
}
