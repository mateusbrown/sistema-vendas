namespace SistemaVendasApi.Controllers.Models;

public class Produto
{
    public static ProdutoResponse ConvertResponse(SistemaVendasApi.Models.Produtos produto)
    {
        return new ProdutoResponse()
        {
            ID = produto.ID,
            Codigo = produto.Codigo,
            Descricao = produto.Descricao,
            Valor = produto.Valor
        };
    }
    
    public static SistemaVendasApi.Models.Produtos ConvertModel(ProdutoRequest request)
    {
        return new SistemaVendasApi.Models.Produtos()
        {
            ID = request.ID ?? 0,
            Codigo = request.Codigo ?? "",
            Descricao = request.Descricao ?? "",
            Valor = request.Valor ?? 0
        };
    }    
}

public class ProdutoRequest
{
    public int ?ID {get;set;}
    public string ?Codigo {get;set;}
    public string ?Descricao {get;set;}
    public decimal ?Valor {get;set;}
}

public class ProdutoResponse : ProdutoRequest { }