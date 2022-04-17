using Console.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Console.Application.Common.Interfaces;

public interface IApplicationDbContext {
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<bool> CanConnect();
}
