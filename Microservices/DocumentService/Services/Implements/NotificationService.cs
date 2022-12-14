using System.Threading;
using System.Threading.Tasks;
using DocumentService.Services.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DocumentService.Services.Implements
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<DocumentHub> _hubContext;

        public NotificationService(IHubContext<DocumentHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task TriggerUpdateDocument(long documentId, CancellationToken cancellationToken)
        {
            await _hubContext.Clients
                .Groups(documentId.ToString())
                .SendAsync("ReceiveMessage", "Document has been updated", cancellationToken);
        }

        public async Task TriggerDeleteDocument(long documentId, CancellationToken cancellationToken)
        {
            await _hubContext.Clients
              .Groups(documentId.ToString())
              .SendAsync("ReceiveMessage", "Document has been deleted", cancellationToken);
        }
    }
}

