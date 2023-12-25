using SistemaVendasApi.Data;
using System.Linq;

namespace SistemaVendasApi.Data.Repository;
public class Produtos(SVContext Context)
{
    private readonly SVContext context = Context;

    public List<Models.Produtos> GetProdutos() => context.Produtos.ToList();
    public Models.Produtos ?GetProduto(int ID) => context.Produtos.SingleOrDefault((p) => p.ID.Equals(ID));
    public Models.Produtos ?GetProduto(string CD_Produto) => context.Produtos.SingleOrDefault((p) => p.CD_Produto.Equals(CD_Produto));
    public Models.Produtos Add(Models.Produtos model)
    {
        context.Produtos.Add(model);
        context.SaveChanges();
        return model;
    }
    public Models.Produtos Update(Models.Produtos model)
    {
        var modelContext = GetProduto(model.ID);
        if (modelContext != null)
        {
            modelContext.CD_Produto = model.CD_Produto;
            modelContext.DS_Produto = model.DS_Produto;
            modelContext.VL_Produto = model.VL_Produto;
            context.Produtos.Update(modelContext);
            context.SaveChanges();
        }
        else
        {
            throw new Exception("Produto não encontrado");
        }
        return model;
    }
    public bool Delete(Models.Produtos model)
    {
        var modelContext = GetProduto(model.ID);
        if (modelContext != null)
        {
            context.Produtos.Remove(modelContext);
            context.SaveChanges();
        }
        return GetProduto(model.ID) == null;
    }
}