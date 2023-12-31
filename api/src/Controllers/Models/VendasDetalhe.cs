namespace SistemaVendasApi.Controllers.Models;

public class VendaDetalhe
{
    public static VendaDetalheResponse ConvertResponse(SistemaVendasApi.Models.VendasDetalhe detalhe)
    {
        return new VendaDetalheResponse()
        {
            ID = detalhe.ID,
            ID_Produto = detalhe.Produto.ID,
            CD_Produto = detalhe.Produto.Codigo,
            DS_Produto = detalhe.Produto.Descricao,
            Quantidade = detalhe.Quantidade,
            ValorUnitario = detalhe.ValorUnitario,
            ValorTotal = detalhe.ValorTotal
        };
    }

    public static SistemaVendasApi.Models.VendasDetalhe ConvertModel(VendaDetalheRequest request)
    {
        return new SistemaVendasApi.Models.VendasDetalhe()
        {
            ID = request.ID ?? 0,
            Produto =  new SistemaVendasApi.Models.Produtos()
            {
                ID = request.Produto ?? 0
            },
            Quantidade = request.Quantidade ?? 0,
            ValorUnitario = request.ValorUnitario ?? 0,
            ValorTotal = request.ValorTotal ?? 0
        };
    }
}

public class VendaDetalheRequest
{
    public int ?ID {get;set;}
    public int ?Produto {get;set;}
    public int ?Quantidade {get;set;}
    public decimal ?ValorUnitario {get;set;}
    public decimal ?ValorTotal {get;set;}
}

public class VendaDetalheResponse
{
    public int ?ID {get;set;}
    public int ?ID_Produto {get;set;}
    public string ?CD_Produto {get;set;}
    public string ?DS_Produto {get;set;}
    public int ?Quantidade {get;set;}
    public decimal ?ValorUnitario {get;set;}
    public decimal ?ValorTotal {get;set;}
}