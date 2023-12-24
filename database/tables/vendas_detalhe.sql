CREATE TABLE dbo.Vendas_Detalhe(ID Integer Identity(1,1) NOT NULL
                               ,ID_Venda Integer NOT NULL
                               ,ID_Produto Integer NOT NULL
                               ,QT_Produto Integer NOT NULL
                               ,VL_Unitario_Produto Numeric(18,2) NOT NULL
                               ,VL_Produto_Total Numeric(18,2) NOT NULL
                               ,CONSTRAINT PK_Vendas_Detalhe PRIMARY KEY (ID)
                               ,CONSTRAINT FK_Venda FOREIGN KEY (ID_Venda) REFERENCES dbo.Vendas
                               ,CONSTRAINT FK_Produto FOREIGN KEY (ID_Produto) REFERENCES dbo.Produtos
)