using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using BattleDex.Core.Models;
using BattleDex.ViewModels;

namespace BattleDex.Views;

public sealed partial class ListDetailsDetailControl : UserControl
{
    private readonly ListDetailsViewModel _viewModel;

    public PokemonSpecies? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as PokemonSpecies;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public TypeMatchup? CurrentMatchup =>
        ListDetailsMenuItem is { } item
            ? TypeEffectiveness.GetDefensiveMatchup(item.Types, _viewModel.SelectedGeneration)
            : null;

    public Visibility HasImmunities =>
        CurrentMatchup?.Immunities.Count > 0
            ? Visibility.Visible
            : Visibility.Collapsed;

    public bool IsShiny
    {
        get => (bool)GetValue(IsShinyProperty);
        set => SetValue(IsShinyProperty, value);
    }

    public string DisplayedSpriteUri =>
        ListDetailsMenuItem is { } item
            ? (IsShiny ? item.ShinySpriteUri : item.SpriteUri)
            : string.Empty;

    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(PokemonSpecies), typeof(ListDetailsDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public static readonly DependencyProperty IsShinyProperty = DependencyProperty.Register("IsShiny", typeof(bool), typeof(ListDetailsDetailControl), new PropertyMetadata(false, OnIsShinyPropertyChanged));

    public ListDetailsDetailControl()
    {
        _viewModel = App.GetService<ListDetailsViewModel>();
        InitializeComponent();
        Loaded += ListDetailsDetailControl_Loaded;
        Unloaded += ListDetailsDetailControl_Unloaded;
    }

    private void ListDetailsDetailControl_Loaded(object sender, RoutedEventArgs e)
    {
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        UpdateMatchupBindings();
    }

    private void ListDetailsDetailControl_Unloaded(object sender, RoutedEventArgs e)
    {
        _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
    }
    private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ListDetailsViewModel.SelectedGeneration))
        {
            UpdateMatchupBindings();
        }
    }

    private void UpdateMatchupBindings()
    {
        var matchup = CurrentMatchup;
        WeaknessesControl.ItemsSource = matchup?.Weaknesses;
        ResistancesControl.ItemsSource = matchup?.Resistances;
        ImmunitiesControl.ItemsSource = matchup?.Immunities;
        var immuneVis = matchup?.Immunities.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        ImmunitiesHeader.Visibility = immuneVis;
        ImmunitiesControl.Visibility = immuneVis;
        EvYieldText.Text = ListDetailsMenuItem?.GetEvYieldDisplay(_viewModel.SelectedGeneration) ?? string.Empty;
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ListDetailsDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
            control.IsShiny = false;
            control.Bindings.Update();
            control.UpdateMatchupBindings();
        }
    }

    private static void OnIsShinyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ListDetailsDetailControl control)
        {
            control.Bindings.Update();
        }
    }
}
