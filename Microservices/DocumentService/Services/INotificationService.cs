using System.Threading;
using System.Threading.Tasks;

namespace DocumentService.Services
{
    public interface INotificationService
    {
        Task TriggerUpdateDocument(long documentId, CancellationToken cancellationToken);
        Task TriggerDeleteDocument(long documentId, CancellationToken cancellationToken);
    }
}

