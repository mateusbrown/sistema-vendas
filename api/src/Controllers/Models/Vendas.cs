using SistemaVendasApi.Core.Validate;
using SistemaVendasApi.Data.Models;

namespace SistemaVendasApi.Controllers.Models;

public class Venda
{
    
    public static VendaResponse ConvertResponse(SistemaVendasApi.Models.Vendas model)
    {
        var detalhes = new List<VendaDetalheResponse>();
        foreach(var d in model.Detalhes)
        {
            detalhes.Add(VendaDetalhe.ConvertResponse(d));
        }
        return new VendaResponse()
        {
            ID = model.ID,
            DataInclusao = model.DataInclusao,
            ValorTotal = model.ValorTotal,
            QuantidadeTotal = model.QuantidadeTotal,
            Detalhes = detalhes ?? new List<VendaDetalheResponse>()
        };
    }

    public static SistemaVendasApi.Models.Vendas ConvertModel(VendaRequest request)
    {
        return new SistemaVendasApi.Models.Vendas()
        {
            ID = request.ID ?? 0,
            DataInclusao = request.DataInclusao ?? DateTime.MinValue,
            ValorTotal = request.ValorTotal ?? 0,
            QuantidadeTotal = request.QuantidadeTotal ?? 0,
            Detalhes = new List<SistemaVendasApi.Models.VendasDetalhe>()
        };
    }
}

public class VendaRequest
{
    public int ?ID {get;set;}
    public DateTime ?DataInclusao {get;set;}
    public decimal ?ValorTotal {get;set;}
    public int ?QuantidadeTotal {get;set;}
}

public class VendaResponse
{
    
    public int ?ID {get;set;}
    public DateTime ?DataInclusao {get;set;}
    public decimal ?ValorTotal {get;set;}
    public int ?QuantidadeTotal {get;set;}
    public List<VendaDetalheResponse> ?Detalhes {get;set;}
}