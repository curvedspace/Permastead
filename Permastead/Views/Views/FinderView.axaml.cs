using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class FinderView : UserControl
{
    public FinderView()
    {
        InitializeComponent();
    }
    
    private void SearchBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        // if the key is return, do a search and filter the grid data
        if (e.Key == Key.Return)
        {
            var textValue = SearchBox.Text;
            var vm = (FinderViewModel)this.DataContext;

            if (vm != null)
            {
                vm.RefreshDataOnly(textValue);
            }
        }

        if (e.Key == Key.Escape)
        {
            var vm = (FinderViewModel)this.DataContext;

            if (vm != null)
            {
                vm.SearchText = "";
                vm.RefreshDataOnly(vm.SearchText);
            }
        }
    }

    private void SearchResultsGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        
    }
}