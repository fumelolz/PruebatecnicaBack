
namespace PruebatecnicaBack.Application.Common.Responses
{
    public record ResponseResult<TData>(string Status, int StatusCode, string Message, DateTime Timestamp, TData Data);
}
