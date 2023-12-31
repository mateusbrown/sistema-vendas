using SistemaVendasApi.Data;
using SistemaVendasApi.Validation;

namespace SistemaVendasApi.Services;

public class Vendas(SVContext Context)
{
    private readonly Data.Repository.Vendas repo = new Data.Repository.Vendas(Context);
    private readonly Data.Repository.VendasDetalhe repoDetalhes = new Data.Repository.VendasDetalhe(Context);
    private readonly Produtos produtosCore = new Produtos(Context);

    private Models.Vendas FillModel(Data.Models.Vendas model, List<Data.Models.VendasDetalhe> ?detalhes)
    {
        return new Models.Vendas()
        {
            ID = model.ID,
            DataInclusao = model.DH_Inclusao,
            ValorTotal = model.VL_Total_Produtos,
            QuantidadeTotal = model.QT_Total_Produtos,
            Detalhes = FillModel(detalhes ?? new List<Data.Models.VendasDetalhe>())
        };
    }

    private Data.Models.Vendas FillDataModel(Models.Vendas model)
    {
        return new Data.Models.Vendas()
        {
            ID = model.ID,
            DH_Inclusao = model.DataInclusao,
            VL_Total_Produtos = model.ValorTotal,
            QT_Total_Produtos = model.QuantidadeTotal
        };
    }

    private Models.VendasDetalhe FillModel(Data.Models.VendasDetalhe model)
    {
        var produto = produtosCore.GetProduto(model.ID_Produto) ?? new Models.Produtos();
        return new Models.VendasDetalhe()
        {
            ID = model.ID,
            Produto = produto,
            Quantidade = model.QT_Produto,
            ValorUnitario = model.VL_Unitario_Produto,
            ValorTotal = model.VL_Produto_Total
        };
    }

    private List<Models.VendasDetalhe> FillModel(List<Data.Models.VendasDetalhe> model)
    {
        var lst = new List<Models.VendasDetalhe>();

        foreach (var i in model)
        {
            lst.Add(FillModel(i));
        }
        return lst;
    }

    private Data.Models.VendasDetalhe FillDataModel(int ID_Venda, Models.VendasDetalhe model)
    {
        return new Data.Models.VendasDetalhe()
        {
            ID = model.ID,
            ID_Produto = model.Produto.ID,
            ID_Venda = ID_Venda,
            QT_Produto = model.Quantidade,
            VL_Unitario_Produto = model.ValorUnitario,
            VL_Produto_Total = model.ValorTotal
        };
    }

    private ValidateProcess ValidateData(Models.Vendas model, ActionType action)
    {
        ValidateProcess validation = new ValidateProcess();
        if (action.Equals(ActionType.Insert))
        {
            if (!model.ID.Equals(0))
            {
                validation.Add(new ModelValid()
                {
                    Type = ValidType.Warning,
                    Message = "ID não pode ser informado na inclusão do registro"
                });
            }
        }
        
        if (action.Equals(ActionType.Update))
        {
             if (model.ID.Equals(0))
            {
                validation.Add(new ModelValid()
                {
                    Type = ValidType.Warning,
                    Message = "ID não informado"
                });
            }
        }

        if (action.Equals(ActionType.Delete))
        {
             if (model.ID.Equals(0))
            {
                validation.Add(new ModelValid()
                {
                    Type = ValidType.Warning,
                    Message = "ID não informado"
                });
            }
        }

        return validation;
    }

    private ValidateProcess ValidateData(Models.VendasDetalhe model, ActionType action)
    {
        ValidateProcess validation = new ValidateProcess();

        if (model.Quantidade.Equals(0))
        {
            validation.Add(new ModelValid()
            {
                Type = ValidType.Warning,
                Message = "Quantidade não informado"
            });
        }

        if (model.ValorUnitario.Equals(0))
        {
            validation.Add(new ModelValid()
            {
                Type = ValidType.Warning,
                Message = "Valor Unitário não informado"
            });
        }

        if (model.ValorTotal.Equals(0))
        {
            validation.Add(new ModelValid()
            {
                Type = ValidType.Warning,
                Message = "Valor Total não informado"
            });
        }
        else
        {
            var total = model.Quantidade * model.ValorUnitario;
            if (!model.ValorTotal.Equals(total))
            {
                validation.Add(new ModelValid()
                {
                    Type = ValidType.Warning,
                    Message = "Valor Total não é compatível com os valores unitário e quantidade"
                });
            }   
        }

        if (action.Equals(ActionType.Insert))
        {
            if (!model.ID.Equals(0))
            {
                validation.Add(new ModelValid()
                {
                    Type = ValidType.Warning,
                    Message = "ID não pode ser informado na inclusão do registro"
                });
            }
        }

        if (action.Equals(ActionType.Update))
        {
             if (model.ID.Equals(0))
            {
                validation.Add(new ModelValid()
                {
                    Type = ValidType.Warning,
                    Message = "ID não informado"
                });
            }
        }


        if (action.Equals(ActionType.Delete))
        {
             if (model.ID.Equals(0))
            {
                validation.Add(new ModelValid()
                {
                    Type = ValidType.Warning,
                    Message = "ID não informado"
                });
            }
        }

        return validation;
    }

    private void AtualizarTotaisVenda(Models.Vendas venda)
    {
        int quantidade = 0;
        decimal valor = 0;
        List<Models.VendasDetalhe> detalhes = GetDetalhes(venda.ID);
        foreach (var d in detalhes)
        {
            quantidade += d.Quantidade;
            valor += d.ValorTotal;
        }

        venda.QuantidadeTotal = quantidade;
        venda.ValorTotal = valor;

        UpdateVenda(venda);
    }

    public List<Models.Vendas> GetVendas()
    {
        var vendas = new List<Models.Vendas>();
        var vendasLst = repo.GetVendas();
        foreach (var v in vendasLst)
        {
            var detalhesLst = repoDetalhes.GetVendaDetalhes(v.ID);
            var venda = FillModel(v,detalhesLst);
            vendas.Add(venda);
        }

        return vendas;
    }

    public Models.Vendas ?GetVenda(int ID)
    {
        var venda = repo.GetVenda(ID);
        if (venda == null) return null;
        else return FillModel(venda,repoDetalhes.GetVendaDetalhes(venda.ID));
    }

    public List<Models.VendasDetalhe> GetDetalhes(int ID_Venda)
    {
        var lst = repoDetalhes.GetVendaDetalhes(ID_Venda);
        var detalhes = new List<Models.VendasDetalhe>();

        foreach (Data.Models.VendasDetalhe i in lst)
        {
            detalhes.Add(FillModel(i));
        }

        return detalhes;
    }
    

    public Models.VendasDetalhe ?GetDetalhe(int ID)
    {
        var detalhe = repoDetalhes.GetDetalhe(ID);
        if (detalhe == null) return null;
        else return FillModel(detalhe);
    }

    public Models.Vendas AddVenda(Models.Vendas venda)
    {
        var valid = ValidateData(venda, ActionType.Insert);
        if (valid.HasError) 
        {
            throw new Exception(valid.GetErrorMessages());
        }
        else if (valid.HasWarning)
        {
            throw new Exception(valid.GetWarningMessages());
        }
        venda.DataInclusao = DateTime.Now;
        venda.QuantidadeTotal = 0;
        venda.ValorTotal = 0;
        venda.ID = repo.Add(FillDataModel(venda)).ID;

        return venda;
    }

    public Models.Vendas UpdateVenda(Models.Vendas venda)
    {
         var valid = ValidateData(venda, ActionType.Update);
        if (valid.HasError)
        {
            throw new Exception(valid.GetErrorMessages());
        }
        else if (valid.HasWarning)
        {
            throw new Exception(valid.GetWarningMessages());
        }

        if (!repo.Update(FillDataModel(venda)))
        {
            throw new Exception("Erro ao atualização a Venda");
        }
        
        return venda;
    }

    public bool DeleteVenda(Models.Vendas venda)
    {
         var valid = ValidateData(venda, ActionType.Delete);
        if (valid.HasError)
        {
            throw new Exception(valid.GetErrorMessages());
        }
        else if (valid.HasWarning)
        {
            throw new Exception(valid.GetWarningMessages());
        }

        return repo.Delete(FillDataModel(venda));
    }

    public Models.VendasDetalhe AddDetalhe(Models.Vendas venda, Models.VendasDetalhe detalhe)
    {
         var valid = ValidateData(detalhe, ActionType.Insert);
        if (valid.HasError) 
        {
            throw new Exception(valid.GetErrorMessages());
        }
        else if (valid.HasWarning)
        {
            throw new Exception(valid.GetWarningMessages());
        }

        detalhe.ValorTotal = detalhe.ValorUnitario * detalhe.Quantidade;
        detalhe = FillModel(repoDetalhes.Add(FillDataModel(venda.ID, detalhe)));
        AtualizarTotaisVenda(venda);
        return detalhe;
    }

    public Models.VendasDetalhe UpdateDetalhe(Models.Vendas venda, Models.VendasDetalhe detalhe)
    {
         var valid = ValidateData(detalhe, ActionType.Update);
        if (valid.HasError)
        {
            throw new Exception(valid.GetErrorMessages());
        }
        else if (valid.HasWarning)
        {
            throw new Exception(valid.GetWarningMessages());
        }
        detalhe.ValorTotal = detalhe.ValorUnitario * detalhe.Quantidade;
        
        if (repoDetalhes.Update(FillDataModel(venda.ID, detalhe)))
        {
            AtualizarTotaisVenda(venda);
        }
        else
        {
            throw new Exception("Erro ao atualização o detalhe da venda");
        }

        return detalhe;
    }

    public bool DeleteDetalhe(Models.Vendas venda, Models.VendasDetalhe detalhe)
    {
        var valid = ValidateData(detalhe, ActionType.Delete);
        if (valid.HasError)
        {
            throw new Exception(valid.GetErrorMessages());
        }
        else if (valid.HasWarning)
        {
            throw new Exception(valid.GetWarningMessages());
        }

        var ret = repoDetalhes.Delete(FillDataModel(venda.ID, detalhe));

        if (ret)
        {
            AtualizarTotaisVenda(venda);
        }
        return ret;
    }
}