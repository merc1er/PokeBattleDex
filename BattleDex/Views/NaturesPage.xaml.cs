using System.Globalization;
using System.Text;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using BattleDex.Core.Models;
using BattleDex.Helpers;
using BattleDex.ViewModels;

using Windows.UI;

namespace BattleDex.Views;

public sealed partial class NaturesPage : Page
{
    private static readonly Color HeaderBg    = Color.FromArgb(255,  35,  35,  35);
    private static readonly Color EvenRowBg   = Color.FromArgb(255,  42,  42,  42);
    private static readonly Color OddRowBg    = Color.FromArgb(255,  52,  52,  52);
    private static readonly Color IncreasedBg = Color.FromArgb(255,  60, 140,  60);
    private static readonly Color DecreasedBg = Color.FromArgb(255, 180,  70,  70);
    private static readonly Color NeutralFg   = Color.FromArgb(120, 180, 180, 180);

    private static (string header, double width)[] GetColumns() =>
    [
        ("Natures_ColEnglish".GetLocalized(),       108),
        ("Natures_ColFrench".GetLocalized(),        108),
        ("Natures_ColGerman".GetLocalized(),        108),
        ("Natures_ColJapanese".GetLocalized(),       90),
        ("Natures_ColIncreasedStat".GetLocalized(),  68),
        ("Natures_ColDecreasedStat".GetLocalized(),  68),
    ];

    public NaturesViewModel ViewModel { get; }

    public NaturesPage()
    {
        ViewModel = App.GetService<NaturesViewModel>();
        InitializeComponent();
        Loaded += (_, _) => BuildTable(filter: string.Empty);
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        BuildTable(SearchBox.Text);
    }

    private void BuildTable(string filter)
    {
        TableGrid.Children.Clear();
        TableGrid.RowDefinitions.Clear();
        TableGrid.ColumnDefinitions.Clear();

        var columns = GetColumns();
        foreach (var (_, width) in columns)
            TableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width) });

        // Header row
        TableGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(36) });
        for (var col = 0; col < columns.Length; col++)
            AddCell(0, col, columns[col].header, HeaderBg, Colors.White, isBold: true, fontSize: 12);

        // Data rows
        var normalizedFilter = Normalize(filter);
        var natures = string.IsNullOrWhiteSpace(normalizedFilter)
            ? Nature.All
            : Nature.All.Where(n =>
                Normalize(n.English).Contains(normalizedFilter, StringComparison.OrdinalIgnoreCase) ||
                Normalize(n.French).Contains(normalizedFilter, StringComparison.OrdinalIgnoreCase) ||
                Normalize(n.German).Contains(normalizedFilter, StringComparison.OrdinalIgnoreCase) ||
                n.Japanese.Contains(filter.Trim(), StringComparison.OrdinalIgnoreCase))
            .ToList();

        for (var i = 0; i < natures.Count; i++)
        {
            var nature = natures[i];
            TableGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(34) });

            var row = i + 1;
            var rowBg = i % 2 == 0 ? EvenRowBg : OddRowBg;

            AddCell(row, 0, nature.English,  rowBg, Colors.White);
            AddCell(row, 1, nature.French,   rowBg, Colors.White);
            AddCell(row, 2, nature.German,   rowBg, Colors.White);
            AddCell(row, 3, nature.Japanese, rowBg, Colors.White);
            AddStatCell(row, 4, nature.IncreasedStat, increase: true,  rowBg);
            AddStatCell(row, 5, nature.DecreasedStat, increase: false, rowBg);
        }
    }

    private static string Normalize(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;
        var decomposed = text.Trim().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(decomposed.Length);
        foreach (var c in decomposed)
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    private void AddCell(int row, int col, string text, Color bg, Color fg, bool isBold = false, double fontSize = 13)
    {
        var tb = new TextBlock
        {
            Text = text,
            FontSize = fontSize,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = new SolidColorBrush(fg),
            Padding = new Thickness(10, 0, 10, 0),
        };
        if (isBold)
            tb.FontWeight = Microsoft.UI.Text.FontWeights.SemiBold;

        var border = new Border
        {
            Background = new SolidColorBrush(bg),
            Child = tb,
            BorderBrush = new SolidColorBrush(Color.FromArgb(25, 128, 128, 128)),
            BorderThickness = new Thickness(0, 0, 0.5, 0.5),
        };

        Grid.SetRow(border, row);
        Grid.SetColumn(border, col);
        TableGrid.Children.Add(border);
    }

    private void AddStatCell(int row, int col, string? stat, bool increase, Color rowBg)
    {
        Color bg;
        Color fg;
        string text;

        if (stat is null)
        {
            bg = rowBg;
            fg = NeutralFg;
            text = "—";
        }
        else if (increase)
        {
            bg = IncreasedBg;
            fg = Colors.White;
            text = "+" + stat;
        }
        else
        {
            bg = DecreasedBg;
            fg = Colors.White;
            text = "−" + stat;
        }

        var tb = new TextBlock
        {
            Text = text,
            FontSize = 12,
            FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = new SolidColorBrush(fg),
        };

        var border = new Border
        {
            Background = new SolidColorBrush(bg),
            Child = tb,
            BorderBrush = new SolidColorBrush(Color.FromArgb(25, 128, 128, 128)),
            BorderThickness = new Thickness(0, 0, 0.5, 0.5),
        };

        Grid.SetRow(border, row);
        Grid.SetColumn(border, col);
        TableGrid.Children.Add(border);
    }
}
