namespace SistemaVendasApi.Models;
public class VendasDetalhe
{
    public int ID {get;set;} = 0;
    public Vendas Venda {get;set;} = new Vendas();
    public Produtos Produto {get;set;} = new Produtos();
    public int Quantidade {get;set;} = 0;
    public decimal ValorUnitario {get;set;} = 0;
    public decimal ValorTotal {get;set;} = 0;
}