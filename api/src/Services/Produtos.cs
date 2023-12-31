using SistemaVendasApi.Data;
using SistemaVendasApi.Validation;

namespace SistemaVendasApi.Services;

public class Produtos(SVContext Context)
{
    private readonly Data.Repository.Produtos repo = new Data.Repository.Produtos(Context);

    private Models.Produtos FillModel(Data.Models.Produtos model)
    {
        return new Models.Produtos()
        {
            ID = model.ID,
            Codigo = model.CD_Produto,
            Descricao = model.DS_Produto,
            Valor = model.VL_Produto
        };
    }

    private Data.Models.Produtos FillDataModel(Models.Produtos model)
    {
        return new Data.Models.Produtos()
        {
            ID = model.ID,
            CD_Produto = model.Codigo,
            DS_Produto = model.Descricao,
            VL_Produto = model.Valor
        };
    }

    private ValidateProcess ValidateData(Models.Produtos model, ActionType action)
    {
        ValidateProcess validation = new ValidateProcess();

        if (model.Codigo.Equals(""))
        {
            validation.Add(new ModelValid()
            {
                Type = ValidType.Warning,
                Message = "Código não informado"
            });
        }

        if (model.Descricao.Equals(""))
        {
            validation.Add(new ModelValid()
            {
                Type = ValidType.Warning,
                Message = "Descrição não informado"
            });
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

            var produtoCodigo = GetProduto(model.Codigo);
            if (produtoCodigo != null)
            {
                validation.Add(new ModelValid()
                {
                    Type = ValidType.Error,
                    Message = $"Código {model.Codigo} já utilizado no produto {produtoCodigo.ID}"
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

            var produtoID = GetProduto(model.ID);
            if (produtoID == null)
            {
                validation.Add(new ModelValid()
                {
                    Type = ValidType.Error,
                    Message = $"Registro {model.ID} não encontrado"
                });
            }
            else
            {
                var produtoCodigo = GetProduto(model.Codigo);

                if (produtoCodigo != null)
                {
                    if (!produtoCodigo.ID.Equals(produtoID.ID))
                    {
                        validation.Add(new ModelValid()
                        {
                            Type = ValidType.Error,
                            Message = $"Código {model.Codigo} já utilizado no produto {produtoCodigo.ID}"
                        });
                    }
                }
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

    public List<Models.Produtos> GetProdutos()
    {
        var lst = repo.GetProdutos();
        var produtos = new List<Models.Produtos>();

        foreach (Data.Models.Produtos i in lst)
        {
            produtos.Add(FillModel(i));
        }

        return produtos;
    }

    public Models.Produtos ?GetProduto(int ID)
    {
        var produto = repo.GetProduto(ID);
        if (produto == null) return null;
        else return FillModel(produto);
        
    }

    public Models.Produtos ?GetProduto(string Codigo)
    {
        var produto = repo.GetProduto(Codigo);
        if (produto == null) return null;
        else return FillModel(produto);
    }

    public Models.Produtos Add (Models.Produtos produto)
    {
        var valid = ValidateData(produto, ActionType.Insert);
        if (valid.HasError) 
        {
            throw new Exception(valid.GetErrorMessages());
        }
        else if (valid.HasWarning)
        {
            throw new Exception(valid.GetWarningMessages());
        }

        produto = FillModel(repo.Add(FillDataModel(produto)));

        return produto;
    }

    public Models.Produtos Update(Models.Produtos produto)
    {
        var valid = ValidateData(produto, ActionType.Update);
        if (valid.HasError) 
        {
            throw new Exception(valid.GetErrorMessages());
        }
        else if (valid.HasWarning)
        {
            throw new Exception(valid.GetWarningMessages());
        }

        if (!repo.Update(FillDataModel(produto)))
        {
            throw new Exception("Erro ao atualizar o produto");
        }
        return produto;
    }

    public bool Delete(Models.Produtos produto)
    {
        var valid = ValidateData(produto, ActionType.Delete);
        if (valid.HasError) 
        {
            throw new Exception(valid.GetErrorMessages());
        }
        else if (valid.HasWarning)
        {
            throw new Exception(valid.GetWarningMessages());
        }

        return repo.Delete(FillDataModel(produto));
    }
}