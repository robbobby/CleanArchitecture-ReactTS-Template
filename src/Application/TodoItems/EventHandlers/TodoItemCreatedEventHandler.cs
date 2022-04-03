using Console.Application.Common.Models;
using Console.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Console.Application.TodoItems.EventHandlers;

public class TodoItemCreatedEventHandler : INotificationHandler<DomainEventNotification<TodoItemCreatedEvent>> {
    private readonly ILogger<TodoItemCreatedEventHandler> _logger;

    public TodoItemCreatedEventHandler(ILogger<TodoItemCreatedEventHandler> logger) {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<TodoItemCreatedEvent> notification, CancellationToken cancellationToken) {
        TodoItemCreatedEvent domainEvent = notification.DomainEvent;

        _logger.LogInformation("Console Domain Event: {DomainEvent}", domainEvent.GetType().Name);

        return Task.CompletedTask;
    }
}
