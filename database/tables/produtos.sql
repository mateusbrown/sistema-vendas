CREATE TABLE dbo.Produtos(ID Integer Identity(1,1) NOT NULL
                         ,CD_Produto Char(10) NOT NULL
                         ,DS_Produto Varchar(50) NOT NULL
                         ,VL_Produto Numeric(18,2) NOT NULL
                         ,CONSTRAINT PK_Produtos PRIMARY KEY (ID)
)