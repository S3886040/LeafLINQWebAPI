using LeafLINQWebAPI.Model;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using WebApi.Data;

namespace LeafLINQWebAPI.Services;
public class ExpiredSessionCleanupService : BackgroundService
{
    private readonly ILogger<ExpiredSessionCleanupService> _logger;
    private readonly IServiceProvider _services;

    // This is a clean up service which will remove old session data from the db. As multiple session entries can be added for each user.
    public ExpiredSessionCleanupService(
        ILogger<ExpiredSessionCleanupService> logger, IServiceProvider services)
    {
        _logger = logger;
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                
                using var scope = _services.CreateScope();
                LeafLINQContext context = scope.ServiceProvider.GetRequiredService<LeafLINQContext>();

                var sessions = context.Session.ToList();
                foreach (var session in sessions)
                {
                    if (session.RefreshTokenExpiration <= DateTime.UtcNow)
                    {
                        context.Session.Remove(session);
                        await context.SaveChangesAsync();
                        _logger.LogInformation($"Removed session {session.SessionId} from existence for expiriration reasons.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the expired session cleanup task.");
            }

            // Schedule the task to run again after a specified delay
            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }
}

