CREATE TABLE dbo.Vendas(ID Integer Identity(1,1) NOT NULL
                       ,DH_Inclusao Datetime NOT NULL
                       ,VL_Total_Produtos Numeric(18,2) NOT NULL
                       ,QT_Total_Produtos Integer NOT NULL
                       ,CONSTRAINT PK_Vendas PRIMARY KEY (ID)
)