namespace BattleDex.Contracts.Services;

public interface ICaughtPokemonService
{
    Task EnsureLoadedAsync();

    bool IsCaught(int id);

    Task SetCaughtAsync(int id, bool caught);
}
