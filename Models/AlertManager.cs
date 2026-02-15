using System.Collections.Concurrent;

namespace Models;

public class AlertManager
{
    
    public ConcurrentDictionary<Guid, AlertItem> Alerts = new ConcurrentDictionary<Guid, AlertItem>();
    
    public int Count => Alerts.Count;

    public bool AddAlert(AlertItem alert)
    {
        return Alerts.TryAdd(alert.Id, alert);
    }
    
    public bool AddAlert(string code, string description, string comment)
    {
        var alert = new AlertItem()
        {
            Code = code, Description = description, Comment = comment
        };
        
        return Alerts.TryAdd(alert.Id, alert);
    }

    public void ClearAlerts(string code, string description, string comment)
    {
        foreach (var alert in Alerts.Values)
        {
            if (alert.Code == code && alert.Description == description && alert.Comment == comment)
            {
                Alerts.TryRemove(alert.Id, out _);
            }
        }
    }
    
    public bool AlertExists(AlertItem alert)
    {
        foreach (var a in Alerts.Values)
        {
            if (a.Code == alert.Code && a.Description == alert.Description && a.Comment == alert.Comment)
            {
                return true;
            }
        }
        
        return false;
    }

    public bool AddAlertIfNotFound(AlertItem alert)
    {
        if (!AlertExists(alert))
        {
            return AddAlert(alert);
        }
        else
        {
            return false;
        }
    }
    
}