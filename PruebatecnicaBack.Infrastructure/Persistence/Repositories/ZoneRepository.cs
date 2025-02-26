using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.Bulk;
using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;
using System.Linq.Expressions;

namespace PruebatecnicaBack.Infrastructure.Persistence.Repositories;

public class ZoneRepository : IZoneRepository
{
    private readonly ApplicationDbContext _context;

    public ZoneRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<Zone>> GetAllAsync(
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? year,
        decimal? minCapacity,
        decimal? maxCapacity,
        int page,
        int pageSize)
    {
        IQueryable<Zone> zonesQuery = _context.Zones;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            zonesQuery = zonesQuery.Where(p =>
            p.Anio.ToString().Contains(searchTerm) ||
            p.Name.ToLower().Contains(searchTerm.ToLower()) ||
            p.Participant.ToLower().Contains(searchTerm.ToLower()) ||
            p.SubAccount.ToLower().Contains(searchTerm.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(minCapacity.ToString()))
        {
            zonesQuery = zonesQuery.Where(z => z.CapacidadDemandada >= minCapacity.Value);
        }

        if (!string.IsNullOrWhiteSpace(maxCapacity.HasValue.ToString()))
        {
            zonesQuery = zonesQuery.Where(z => z.CapacidadDemandada <= maxCapacity.Value);
        }

        if (!string.IsNullOrWhiteSpace(year.ToString()))
        {
            zonesQuery = zonesQuery.Where(z => z.Anio == year.Value);
        }

        if (!string.IsNullOrWhiteSpace(sortColumn))
        {
            Expression<Func<Zone, object>> keySelector = sortColumn.ToLower() switch
            {
                "name" => zone => zone.Name,
                "anio" => zone => zone.Anio,
                "participant" => zone => zone.Participant,
                "subAccount" => zone => zone.SubAccount,
                "capacidadDemandada" => zone => zone.CapacidadDemandada,
                "requisitoAnualDePotencia" => zone => zone.RequisitoAnualDePotencia,
                "valorDelRequisitoAnualEficiente" => zone => zone.ValorDelRequisitoAnualEficiente,
                "updatedDate" => zone => zone.UpdatedDate,
                "creationDate" => zone => zone.CreationDate,
                _ => zone => zone.ZoneId
            };

            if (sortOrder.ToLower() == "desc")
            {
                zonesQuery = zonesQuery.OrderByDescending(keySelector);
            }
            else
            {
                zonesQuery = zonesQuery.OrderBy(keySelector);
            }
        }

        var zonequery = zonesQuery.AsNoTracking();

        var zones = await PagedList<Zone>.CreateAsync(zonequery, page, pageSize);

        return zones;
    }

    public async Task<Zone?> GetByIdAsync(int id)
    {
        return await _context.Zones.FindAsync(id);
    }

    public async Task AddAsync(Zone zone)
    {
        await _context.Zones.AddAsync(zone);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Zone zone)
    {
        _context.Zones.Update(zone);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var zone = await _context.Zones.FindAsync(id);
        if (zone != null)
        {
            _context.Zones.Remove(zone);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteByYearAsync(int year)
    {
        var zones = await _context.Zones.Where(z => z.Anio == year).ToListAsync();
        _context.Zones.RemoveRange(zones);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByYearAsync(int year)
    {
        return await _context.Zones.AnyAsync(z => z.Anio == year);
    }

    public async Task BulkInsertAsync(List<Zone> zones)
    {
        var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        using (var writer = ((NpgsqlConnection)connection).BeginBinaryImport(
            "COPY \"Zones\" (\"Name\", \"Anio\", \"Participant\", \"SubAccount\", \"CapacidadDemandada\", \"RequisitoAnualDePotencia\", \"ValorDelRequisitoAnualEficiente\", \"CreationDate\", \"UpdatedDate\") FROM STDIN (FORMAT BINARY)"))
        {
            foreach (var zone in zones)
            {
                writer.StartRow();
                writer.Write(zone.Name);
                writer.Write(zone.Anio);
                writer.Write(zone.Participant);
                writer.Write(zone.SubAccount);
                writer.Write(zone.CapacidadDemandada);
                writer.Write(zone.RequisitoAnualDePotencia);
                writer.Write(zone.ValorDelRequisitoAnualEficiente);
                writer.Write(DateTime.Now);
                writer.Write(DateTime.Now);
            }

            await writer.CompleteAsync();
        }

        await connection.CloseAsync();
    }
}
