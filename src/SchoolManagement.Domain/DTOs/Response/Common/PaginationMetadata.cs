namespace SchoolManagement.Domain.DTOs.Response.Common;

/// <summary>
/// Classe que representa os metadados de paginação.
/// </summary>
public class PaginationMetadata
{
    /// <summary>
    /// Índice da página atual.
    /// </summary>
    public int? PageIndex { get; set; }

    /// <summary>
    /// Tamanho da página.
    /// </summary>
    public int? PageSize { get; set; }

    /// <summary>
    /// Contagem total de itens.
    /// </summary>
    public int? TotalCount { get; set; }

    /// <summary>
    /// Número total de páginas.
    /// </summary>
    public int? TotalPages { get; set; }

    /// <summary>
    /// Indica se há uma próxima página.
    /// </summary>
    public bool HasNextPage => PageIndex < TotalPages;

    /// <summary>
    /// Indica se há uma página anterior.
    /// </summary>
    public bool HasPreviousPage => PageIndex > 1;
}
