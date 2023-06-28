
namespace Services
{
    /// <summary>
    /// Indicates if the service is off-grid (local) or on-grid (server).
    /// Local services utilize the sqlite database while server is postegres.
    /// </summary>
    public enum ServiceMode
    {
        Local = 0,
        Server
    }
}
