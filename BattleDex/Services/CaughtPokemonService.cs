using BattleDex.Contracts.Services;

namespace BattleDex.Services;

public class CaughtPokemonService : ICaughtPokemonService
{
    private const string CaughtPokemonKey = "CaughtPokemon";

    private readonly ILocalSettingsService _localSettingsService;
    private readonly SemaphoreSlim _initGate = new(1, 1);

    private HashSet<int> _caughtIds = new();
    private bool _loaded;

    public CaughtPokemonService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

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

            var saved = await _localSettingsService.ReadSettingAsync<List<int>>(CaughtPokemonKey);
            if (saved is not null)
            {
                _caughtIds = new HashSet<int>(saved);
            }

            _loaded = true;
        }
        finally
        {
            _initGate.Release();
        }
    }

    public bool IsCaught(int id) => _caughtIds.Contains(id);

    public async Task SetCaughtAsync(int id, bool caught)
    {
        var changed = caught ? _caughtIds.Add(id) : _caughtIds.Remove(id);
        if (changed)
        {
            await _localSettingsService.SaveSettingAsync(CaughtPokemonKey, _caughtIds.ToList());
        }
    }
}
