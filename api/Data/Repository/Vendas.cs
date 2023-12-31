namespace SistemaVendasApi.Data.Repository;
public class Vendas(SVContext Context)
{
    private readonly SVContext context = Context;
    public List<Models.Vendas> GetVendas() => context.Vendas.ToList();
    public Models.Vendas ?GetVenda(int ID) => context.Vendas.SingleOrDefault((r) => r.ID.Equals(ID));
    public Models.Vendas Add(Models.Vendas model)
    {
        context.Vendas.Add(model);
        context.SaveChanges();
        return model;
    }
    public bool Update(Models.Vendas model)
    {
        bool ret = false;
        var modelContext = GetVenda(model.ID);
        if (modelContext != null)
        {
            modelContext.QT_Total_Produtos = model.QT_Total_Produtos;
            modelContext.VL_Total_Produtos = model.VL_Total_Produtos;
            context.Vendas.Update(modelContext);
            
            ret = context.SaveChanges() > 0;
        }
        return ret;
    }
    public bool Delete(Models.Vendas model)
    {
        bool ret = false;
        var modelContext = GetVenda(model.ID);
        if (modelContext != null)
        {
            context.Vendas.Remove(modelContext);
            ret = context.SaveChanges() > 0;
        }
        return ret;
    }
}