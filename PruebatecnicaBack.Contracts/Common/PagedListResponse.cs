namespace PruebatecnicaBack.Contracts.Common
{
    public record PagedListResponse<T>(List<T> Items,
                                       int Page,
                                       int PageSize,
                                       int TotalCount,
                                       int TotalPages,
                                       bool HasNextPage,
                                       bool HasPreviousPage);
}
