﻿using System.Globalization;
using Console.Application.Common.Interfaces;
using Console.Application.TodoLists.Queries.ExportTodos;
using Console.Infrastructure.Files.Maps;
using CsvHelper;

namespace Console.Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder {
    public byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records) {
        using MemoryStream memoryStream = new MemoryStream();
        using (StreamWriter streamWriter = new StreamWriter(memoryStream)) {
            using CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Configuration.RegisterClassMap<TodoItemRecordMap>();
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }
}
