namespace SchoolManagement.Domain.DTOs.Response.Common;

public class ResponseModel<TModel>
{
    public ResponseModel() { }

    public ResponseModel(TModel data)
    {
        Data = data;
        Error = false;
    }

    public ResponseModel(bool error)
    {
        Error = error;
    }

    public ResponseModel(TModel data, bool error, List<ErrorInfo> errorInfo)
    {
        Data = data;
        Error = error;
        ErrorInfo = errorInfo;
    }

    public ResponseModel(bool error, List<ErrorInfo> errorInfo)
    {
        Data = default;
        Error = error;
        ErrorInfo = errorInfo;
    }

    /// <summary>
    /// Os dados retornados na resposta.
    /// </summary>
    public TModel? Data { get; set; }

    /// <summary>
    /// Indica se ocorreu um erro na operação.
    /// </summary>
    public bool Error { get; set; }

    /// <summary>
    /// Informações adicionais sobre o erro, se houver.
    /// </summary>
    public List<ErrorInfo> ErrorInfo { get; set; } = [];

    public int GetStatusCode()
    {
        return (int)ErrorInfo.First().StatusCode;
    }

    public static ResponseModel<TModel> Success(TModel value) => new(value);

    public static ResponseModel<TModel> Failure(ErrorInfo error)
        => new(true, [error]);

    public void AddError(ErrorInfo error)
    {
        ErrorInfo.Add(error);
        Error = true;
    }
}
