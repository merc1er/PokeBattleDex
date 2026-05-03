using BattleDex.Contracts.Services;

namespace BattleDex.Services;

public class CaughtPokemonService : ICaughtPokemonService
{
    private const string CaughtPokemonKey = "CaughtPokemon";

    private readonly ILocalSettingsService _localSettingsService;
    private readonly Task _initTask;

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

        var changed = caught ? _caughtIds.Add(id) : _caughtIds.Remove(id);
        if (changed)
        {
            await _localSettingsService.SaveSettingAsync(CaughtPokemonKey, _caughtIds.ToList());
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
