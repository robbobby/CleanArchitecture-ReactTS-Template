using Console.Application.Common.Interfaces;

namespace Console.Infrastructure.Services;

public class DateTimeService : IDateTime {
    public DateTime Now => DateTime.Now;
}
