using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Permastead.ViewModels.Views;
using Serilog;
using Services;

namespace Permastead.ViewModels.Dialogs;

public partial class QuoteWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private Quote _quote;
    
    private HomeViewModel _controlViewModel { get; set;  } = new HomeViewModel();

    public QuoteWindowViewModel(Quote quote, HomeViewModel controlViewModel) : this(controlViewModel)
    {
        _quote = quote;
    }

    public QuoteWindowViewModel(HomeViewModel controlViewModel)
    {
        Quote = new Quote();
        _controlViewModel = controlViewModel;
    }
    
    
    public void SaveRecord()
    {
        
        if (_quote != null && _quote.Id == 0 && !string.IsNullOrEmpty(_quote.Description) && !string.IsNullOrEmpty(_quote.AuthorName))
        {

            _quote.CreationDate = DateTime.Now;
            
            var rtnValue = QuoteService.AddQuote(AppSession.ServiceMode, _quote);
            Log.Logger.Information("Quoute: " + _quote.Description + " saved: " + rtnValue);

        }
        
        //need a way to send a refresh message back to the tree browser...
        if (_controlViewModel != null) _controlViewModel.RefreshData();

    }
}