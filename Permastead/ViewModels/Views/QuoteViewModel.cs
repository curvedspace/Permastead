using Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
