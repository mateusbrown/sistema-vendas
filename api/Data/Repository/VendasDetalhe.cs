namespace SistemaVendasApi.Data.Repository;
public class VendasDetalhe(SVContext Context)
{
    private readonly SVContext context = Context;
    public Models.VendasDetalhe ?GetDetalhe(int ID) => context.VendasDetalhe.SingleOrDefault((p) => p.ID.Equals(ID));
    public List<Models.VendasDetalhe> GetVendaDetalhes(int ID_Venda) => context.VendasDetalhe.Where(r => r.ID_Venda.Equals(ID_Venda)).ToList();
    
    public Models.VendasDetalhe Add(Models.VendasDetalhe model)
    {
        context.VendasDetalhe.Add(model);
        context.SaveChanges();
        return model;
    }
    public bool Update(Models.VendasDetalhe model)
    {
        bool ret = false;
        var modelContext = GetDetalhe(model.ID);
        if (modelContext != null)
        {
            modelContext.QT_Produto = model.QT_Produto;
            modelContext.VL_Unitario_Produto = model.VL_Unitario_Produto;
            modelContext.VL_Produto_Total = model.VL_Produto_Total;
            context.VendasDetalhe.Update(modelContext);
            ret = context.SaveChanges() > 0;
        }
        else
        {
            throw new Exception("Detalhe não encontrado");
        }
        return ret;
    }
    public bool Delete(Models.VendasDetalhe model)
    {
        bool ret = false;
        var modelContext = GetDetalhe(model.ID);
        if (modelContext != null)
        {
            context.VendasDetalhe.Remove(modelContext);
            ret = context.SaveChanges() > 0;
        }
        return ret;
    }
}