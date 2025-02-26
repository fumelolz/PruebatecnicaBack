namespace PruebatecnicaBack.Application.Common.Persistence
{
    public interface IAuditable
    {
        DateTime CreationDate { get; }

        DateTime? UpdatedDate { get; }
    }
}
