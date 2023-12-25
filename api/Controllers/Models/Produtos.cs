using SistemaVendasApi.Core.Validate;

namespace SistemaVendasApi.Controllers.Models;

public class Produto
{
    public int ?ID {get;set;}
    public string ?Codigo {get;set;}
    public string ?Descricao {get;set;}
    public decimal ?Valor {get;set;}
    public void Fill(SistemaVendasApi.Models.Produtos model)
    {
        this.ID = model.ID;
        this.Codigo = model.Codigo;
        this.Descricao = model.Descricao;
        this.Valor = model.Valor;
    }
    
    public void Fill(ProdutoRequest model)
    {
        this.ID = model.ID;
        this.Codigo = model.Codigo;
        this.Descricao = model.Descricao;
        this.Valor = model.Valor;
    }

    public SistemaVendasApi.Models.Produtos ToModel()
    {
        return new SistemaVendasApi.Models.Produtos()
        {
            ID = this.ID ?? 0,
            Codigo = this.Codigo ?? "",
            Descricao = this.Descricao ?? "",
            Valor = this.Valor ?? 0
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

public class ProdutoResponse
{
    public object ?Data {get;set;}
    public Validation ?Validation {get;set;}
}