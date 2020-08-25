using System.Threading;
using System.Threading.Tasks;

namespace EngineerTest.Services
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}