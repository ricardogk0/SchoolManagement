using System.Net;
using System.Text;

namespace SchoolManagement.Domain.DTOs.Response.Common;

public class ErrorInfo
{
    public ErrorInfo()
    {
        ErrorId = Guid.NewGuid();
    }

    public ErrorInfo(HttpStatusCode statusCode, string message)
    {
        ErrorId = Guid.NewGuid();
        StatusCode = statusCode;
        Message = message;
    }

    public ErrorInfo(HttpStatusCode statusCode, string message, string details)
    {
        ErrorId = Guid.NewGuid();
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }

    /// <summary>
    /// O identificador único do erro.
    /// </summary>
    public Guid ErrorId { get; set; }

    /// <summary>
    /// O código de status HTTP associado ao erro.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;

    /// <summary>
    /// Uma mensagem descritiva do erro.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Detalhes adicionais sobre o erro, se houver.
    /// </summary>
    public string Details { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder();
        return builder.Append("ErrorId: ").Append(ErrorId).AppendLine()
            .Append("StatusCode: ").Append(StatusCode).AppendLine()
            .Append("Message: ").Append(Message).AppendLine()
            .Append("Details: ").Append(Details).AppendLine().ToString();
    }
}
