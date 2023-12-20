using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Permastead.ViewModels.Views;

public partial class GaiaViewModel : ViewModelBase
{
    [ObservableProperty]
    private long _responseCount;
    
    [ObservableProperty]
    private string _request = string.Empty;
    
    [ObservableProperty]
    private string _response = string.Empty;

    private Services.GaiaService _gaia;

    [ObservableProperty]
    private ObservableCollection<RequestResponse> _RequestResponses = new ObservableCollection<RequestResponse>();


    public GaiaViewModel()
    {
        _gaia = new Services.GaiaService();

        Response = _gaia.GetResponse("Hello");

    }  

    [RelayCommand]
    private void SendRequest()
    {
        Response = _gaia.GetResponse(Request);
        ResponseCount++;
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