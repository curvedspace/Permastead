
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Services;

namespace Permastead.ViewModels.Views;

public partial class GaiaViewModel : ViewModelBase
{
    [ObservableProperty]
    private long _responseCount;
    
    [ObservableProperty]
    private string _request = string.Empty;
    
    [ObservableProperty]
    private string _response = string.Empty;

    private GaiaService _gaia;

    [ObservableProperty] 
    private ObservableCollection<RequestResponse> _requestResponses;
    

    public GaiaViewModel()
    {
        _gaia = AppSession.Instance.GaiaService;
        _requestResponses = _gaia.RequestResponses;
        ResponseCount = _gaia.RequestResponses.Count;
    }  
    
    
    [RelayCommand]
    private void SendRequest()
    {
        
        Response = _gaia.GetResponse(Request);
        ResponseCount++;

        if (string.IsNullOrEmpty(Response))
        {
            Response = "Sorry, not sure how to answer.";
            _gaia.AddRequestResponse(new RequestResponse() { Request = _request, Response = _response });
        }
        else
        {
            _gaia.AddRequestResponse(new RequestResponse() { Request = _request, Response = _response });
        }

        Request = string.Empty;
        ResponseCount = _gaia.RequestResponses.Count;

    }

    
    bool CanSendRequest(object parameter)
    {
        return Response != null;
    }
    
    
}
