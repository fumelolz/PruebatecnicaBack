using Microsoft.AspNetCore.SignalR;

namespace PruebatecnicaBack.Infrastructure.Persistence.SignalR;

public class NotificationHub : Hub
{
    private static readonly Dictionary<int, string> ClientConnections = new();
    public Task RegisterClient(int userId)
    {
        ClientConnections[userId] = Context.ConnectionId;
        Console.WriteLine($"Cliente registrado con id: {userId} con conexión {Context.ConnectionId}");

        return Task.CompletedTask;
    }

    public static string? GetConnectionId(int userId)
    {
        return ClientConnections.ContainsKey(userId) ? ClientConnections[userId] : null;
    }
}
