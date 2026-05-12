using CommunityToolkit.Mvvm.ComponentModel;

using BattleDex.Contracts.ViewModels;

namespace BattleDex.ViewModels;

public partial class NaturesViewModel : ObservableRecipient, INavigationAware
{
    public void OnNavigatedTo(object parameter) { }
    public void OnNavigatedFrom() { }
}
