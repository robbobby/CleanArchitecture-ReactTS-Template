using System.Text.Json;

namespace Console.WebUI.Models;

public class ExceptionDetails {
    public readonly string Message;
    public readonly int StatusCode;

    public ExceptionDetails(int statusCode, string message) {
        StatusCode = statusCode;
        Message = message ?? "No error message found in exception.";
    }

    public override string ToString() {
        return JsonSerializer.Serialize(this);
    }
}
