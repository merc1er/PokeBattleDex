using CommunityToolkit.WinUI.UI;
using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using BattleDex.ViewModels;

namespace BattleDex.Views;

public sealed partial class ListDetailsPage : Page
{
    public ListDetailsViewModel ViewModel
    {
        get;
    }

    public ListDetailsPage()
    {
        ViewModel = App.GetService<ListDetailsViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }

    private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            ViewModel.SearchText = sender.Text;
        }
    }

    private void SearchAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        SearchBox.Focus(Microsoft.UI.Xaml.FocusState.Keyboard);
        var textBox = SearchBox.FindDescendant<TextBox>();
        textBox?.SelectAll();
        args.Handled = true;
    }

    private void ListDetailsViewControl_GotFocus(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        // Don't steal focus from a control the user clicked — only redirect
        // when focus moved programmatically (e.g. ListView re-selecting after a filter change).
        if (e.OriginalSource is Control { FocusState: Microsoft.UI.Xaml.FocusState.Pointer })
        {
            return;
        }
        if (!string.IsNullOrEmpty(ViewModel.SearchText))
        {
            SearchBox.Focus(Microsoft.UI.Xaml.FocusState.Programmatic);
        }
    }
}
