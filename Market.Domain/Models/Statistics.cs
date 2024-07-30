namespace Market.Domain.Models;

public class Statistics
{
    public int Orders { get; set; }
    public Total Total { get; set; }
    public int Profit { get; set; } 
    public decimal Costs { get; set; }
    public decimal Debts { get; set; }
}
