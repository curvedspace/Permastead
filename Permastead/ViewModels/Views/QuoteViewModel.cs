using Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Permastead.ViewModels.Views
{
    public partial class QuoteViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Quote _quote = new Quote();
        
        public string QuoteText => _quote.ToString();
    }
}
