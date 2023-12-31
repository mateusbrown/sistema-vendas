namespace SistemaVendasApi.Models;
public class VendasDetalhe
{
    public int ID {get;set;} = 0;
    public Produtos Produto {get;set;} = new Produtos();
    public int Quantidade {get;set;} = 0;
    public decimal ValorUnitario {get;set;} = 0;
    public decimal ValorTotal {get;set;} = 0;
}
public class Vendas
{
    public int ID {get;set;} = 0;
    public DateTime DataInclusao {get;set;} = DateTime.Now;
    public decimal ValorTotal {get;set;} = 0;
    public int QuantidadeTotal {get;set;} = 0;
    public List<VendasDetalhe> Detalhes {get;set;} = new List<VendasDetalhe>();
}