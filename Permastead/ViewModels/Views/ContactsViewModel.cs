using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using System.Linq;
using Models;

namespace Permastead.ViewModels.Views;

public class ContactsViewModel : ViewModelBase
{
    private ObservableCollection<Person> _people;

    public HierarchicalTreeDataGridSource<Person> PersonSource { get; }
    
    public ContactsViewModel()
    {
        _people = new ObservableCollection<Person>(Services.PersonService.GetAllPeople(AppSession.ServiceMode));

        PersonSource = new HierarchicalTreeDataGridSource<Person>(_people)
        {
            Columns =
            {
                new TextColumn<Person, string>
                    ("Last Name", x => x.LastName),
                new TextColumn<Person, string>
                    ("First Name", x => x.FirstName),
            },
        };
        
        // PersonSource.Columns.Add(new TextColumn<Person, string>("Last Name", x => x.LastName));
        // PersonSource.Columns.Add(new TextColumn<Person, string>("First Name", x => x.FirstName));
        
        
    }
    
}