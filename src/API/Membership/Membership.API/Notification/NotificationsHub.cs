namespace Membership.API.Notification;

public class NotificationsHub : Hub
{

    const string USER_NAME = "HEALTH_DATA";
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId,USER_NAME);
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, USER_NAME);
        await base.OnDisconnectedAsync(exception);
    }
}

