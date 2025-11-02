using System.Net;
using System.Text.Json.Serialization;

namespace SchoolManagement.Domain.DTOs.Response.Common;

public class PaginatedResponseModel<TModel> : ResponseModel<IEnumerable<TModel>>
{
    /// <summary>
    /// Metadados de paginação
    /// </summary>
    [JsonPropertyOrder(5)]
    public PaginationMetadata? Metadata { get; set; }

    private PaginatedResponseModel(bool error, List<ErrorInfo> errorInfo)
        : base(error, errorInfo) { }

    private PaginatedResponseModel(
        bool error,
        IEnumerable<TModel> data,
        List<ErrorInfo>? errors,
        PaginationMetadata metadata,
        HttpStatusCode? statusCode = null)
    {
        Error = error;
        Data = data;
        ErrorInfo = errors ?? [];
        Metadata = metadata;
    }

    public static PaginatedResponseModel<TModel> Success(IEnumerable<TModel> data, PaginationMetadata metadata)
        => new(false, data, null, metadata);
    public new static PaginatedResponseModel<TModel> Failure(ErrorInfo error)
        => new(true, [error]);
}
