namespace Market.Api.Models;

public class Response
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = default!;
    public object? Data { get; set; }

    public Response(int statusCode, string message, object? data = null)
        => (StatusCode, Message, Data) = (statusCode, message, data);
}
