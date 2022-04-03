using Console.Application.TodoLists.Queries.ExportTodos;

namespace Console.Application.Common.Interfaces;

public interface ICsvFileBuilder {
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
