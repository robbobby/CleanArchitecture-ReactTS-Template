namespace Console.WebUI.Hubs
{
    public interface IUsersHub
    {
        Task UserLogin();
        Task UserLogout();
        Task CloseAllConnections(string reason);
    }
}