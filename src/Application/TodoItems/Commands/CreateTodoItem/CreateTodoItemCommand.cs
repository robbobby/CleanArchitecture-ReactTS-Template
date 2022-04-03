using Console.Application.Common.Interfaces;
using Console.Domain.Entities;
using Console.Domain.Events;
using MediatR;

namespace Console.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommand : IRequest<int> {
    public int ListId { get; set; }

    public string? Title { get; set; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int> {
    private readonly IApplicationDbContext _context;

    public CreateTodoItemCommandHandler(IApplicationDbContext context) {
        _context = context;
    }

    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken) {
        TodoItem entity = new TodoItem {
            ListId = request.ListId,
            Title = request.Title,
            Done = false
        };

        entity.DomainEvents.Add(new TodoItemCreatedEvent(entity));

        _context.TodoItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
