using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace DocumentService.Services.Hubs
{
    public class DocumentHub : Hub
    {
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            Log.Information(Context.ConnectionId + "joined group" + groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}

