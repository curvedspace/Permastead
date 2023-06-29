using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;

namespace Permastead.ViewModels.Views;

public partial class HelpViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _request = string.Empty;
    
    [ObservableProperty]
    private string _response = string.Empty;

    //private Services.GaiaService _gaia;

    [ObservableProperty]
    private ObservableCollection<RequestResponse> _RequestResponses = new ObservableCollection<RequestResponse>();


    public HelpViewModel()
    {
        //_gaia = new Services.GaiaService();

        //Response = _gaia.GetResponse("Hello");

    }  

    [RelayCommand]
    private void SendRequest()
    {
        //Response = _gaia.GetResponse(Request);

        RequestResponses.Add(new RequestResponse() { Request = _request, Response = _response });

        Request = string.Empty;
    }
    
    bool CanSendRequest(object parameter)
    {
        return Response != null;
    }
    
}

public class RequestResponse
{

    public string Request { get; set; }
    public string Response { get; set; }

    public string FullText => DateTime.Now.ToShortTimeString() + ": " + Request + '\n' + "     " + Response;

}