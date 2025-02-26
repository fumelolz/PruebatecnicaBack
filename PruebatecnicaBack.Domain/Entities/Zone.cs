namespace PruebatecnicaBack.Domain.Entities;

public class Zone
{
    public int ZoneId { get; set; }
    public string Name { get; set; }
    public int Anio { get; set; }
    public string Participant {  get; set; }
    public string SubAccount { get; set; }
    public decimal CapacidadDemandada { get; set; }
    public decimal RequisitoAnualDePotencia { get; set; }
    public decimal ValorDelRequisitoAnualEficiente { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
