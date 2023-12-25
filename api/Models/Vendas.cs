namespace SistemaVendasApi.Models;
public class Vendas
{
    public int ID {get;set;} = 0;
    public DateTime DataInclusao {get;set;} = DateTime.Now;
    public decimal ValorTotal {get;set;} = 0;
    public int QuantidadeTotal {get;set;} = 0;
}