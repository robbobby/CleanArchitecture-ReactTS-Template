using Console.Application.Common.Interfaces;
using Console.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace Console.Infrastructure; 

public class DontCare : IDontCare {
    private IApplicationDbContext _dbContext;
    private ILogger<DontCare> _logger;

    public DontCare(IApplicationDbContext dbContext, ILogger<DontCare> logger) {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task CheckDb() {
        System.Console.WriteLine(await _dbContext.CanConnect());
    }
}

public interface IDontCare {
    Task CheckDb();
} 