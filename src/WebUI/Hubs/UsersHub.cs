using Microsoft.AspNetCore.SignalR;

namespace Console.WebUI.Hubs;

public class UsersHub : Hub<IUsersHub> {
    public UsersHub() {
    }
    public async Task UserLogin() {
        await Clients.All.UserLogin();
    }

    public async Task UserLogout() {
        await Clients.All.UserLogout();
    }

    public async Task CloseAllConnections(string reason) {
        await Clients.All.CloseAllConnections(reason);
    }
}
