namespace PruebatecnicaBack.Contracts.Common
{
    public record Response<TData> (string Status, int StatusCode, string Message, DateTime Timestamp, TData Data);
}
