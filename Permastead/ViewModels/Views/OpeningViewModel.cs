using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Services;

namespace Permastead.ViewModels.Views;

public partial class OpeningViewModel: ViewModelBase
{
    public QuoteViewModel QuoteViewModel { get; set; } = new QuoteViewModel();

    [ObservableProperty] 
    private ObservableCollection<string> _updates;
    
    public OpeningViewModel()
    {
        this.QuoteViewModel.Quote.Description = "This is a test quote";
        this.QuoteViewModel.Quote.AuthorName = "Anonymous";
        
        GetQuote();
        
        _updates = new ObservableCollection<string>(ScoreBoardService.CheckForNewToDos(ServiceMode.Local));

        var _upcomingTodos = Services.ToDoService.GetUpcomingToDos(ServiceMode.Local, 3);

        foreach (var t in _upcomingTodos)
        {
            _updates.Add(t.Description + " (" + t.DueDate.Date.ToShortDateString() + ", " + t.ToDoStatus.Description + ")");
        }
        
        if (_updates.Count == 0)
        {
            _updates.Add("Hello homesteader, you have no upcoming events.");
        }
    }

    public void GetQuote()
    {
        this.QuoteViewModel.Quote = Services.QuoteService.GetRandomQuote(AppSession.ServiceMode);
    }

}
