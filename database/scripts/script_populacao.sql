IF NOT EXISTS(SELECT 1 FROM dbo.Produtos)
BEGIN
  ;WITH CTA
   AS(SELECT T.Descricao
            ,ROWID = ROW_NUMBER() OVER(ORDER BY NEWID())
      FROM (VALUES ('Arroz'),('Feij�o'),('�leo'),('Azeite de oliva'),('Vinagre'),('A��car'),('Milho para pipoca'),('Farinha de trigo'),('Fermento em p�'),('Aveia'),('Cereais'),('Amido de milho'),('Farinha de mandioca'),('Extrato de tomate'),('Macarr�o'),('Queijo ralado'),('Enlatados'),('Conservas'),('Bolachas'),('Petiscos'),('P�es'),('Maionese'),('Ketchup'),('Mostarda'),('Frios'),('Manteiga'),('Requeij�o'),('Geleias ou doces pastosos'),('Mel'),('Sal'),('Temperos secos'),('Especiarias'),('Caf�'),('Ch�s'),('Sucos'),('Iogurtes'),('Leite'),('Achocolatado'),('�gua mineral'),('Refrigerantes'),('Bebidas alco�licas de sua prefer�ncia'),('Ovos'),('Verduras'),('Legumes'),('Vegetais variados'),('Frutas da esta��o'),('Cebola'),('Alho'),('Ervas e temperos frescos'),('Bifes'),('Carne mo�da'),('Carne de frango'),('Fil�s de peixes'),('Bacon'),('Hamb�rgueres'),('Lingui�as'),('Salsichas'),('Shampoo'),('Condicionador'),('Sabonetes'),('Sabonete l�quido'),('Cotonetes'),('Algod�o'),('Papel higi�nico'),('Pasta de dente'),('Escova de dente'),('Fio dental'),('Antiss�ptico bucal'),('Porta escova de dentes'),('Saboneteira'),('Esponja para banho'),('Desodorante'),('Curativos'),('Detergente'),('Desengordurante'),('Esponja para lou�a'),('Palha de a�o'),('Escova de limpeza'),('Sab�o em barra'),('Balde e bacia'),('Rodo, vassoura, p�'),('Panos de limpeza e flanelas'),('Sab�o em p� ou l�quido para roupas'),('Amaciante'),('�gua sanit�ria'),('Cesto para roupas'),('Lixeira grande e pequena'),('Lixeira para banheiro'),('Escova sanit�ria'),('Sacos de lixo'),('Desinfetante'),('Limpa vidros'),('Limpa piso'),('Limpador multiuso'),('�lcool'),('Lustrador de m�veis'),('Guardanapos de papel'),('Papel toalha'),('Papel alum�nio'),('Sacos pl�sticos para alimentos'),('Papel filme'),('Filtro para caf�'),('Varal para roupas'),('Pregadores'),('L�mpadas'),('F�sforos'),('Velas'),('Pilhas'),('Inseticida')) AS T(Descricao)),
    CTB
  AS (SELECT ROWID
            ,CD = CONCAT('CD',RIGHT(CONCAT(REPLICATE('0',8),ROWID),8))
            ,Descricao
      FROM CTA),
    CTC
  AS (SELECT CTB.ROWID
            ,CTB.CD
            ,CTB.Descricao
            ,VL = tbAux.VL
      FROM CTB
           CROSS APPLY (SELECT VL = CONVERT(NUMERIC(18,2), (T.VL * (RAND() * CTB.ROWID)))
                        FROM (VALUES((RAND() * 10))) AS T(VL)) AS tbAux)
  INSERT INTO dbo.Produtos(CD_Produto, DS_Produto, VL_Produto)
  SELECT CD, Descricao, VL
  FROM CTC
END
GO
IF NOT EXISTS(SELECT 1 FROM dbo.Vendas)
BEGIN
  DECLARE @DH_Inicial_Venda Datetime = '20231201 00:00:00.000'
         ,@DH_Final_Venda Datetime = '20231225 23:59:59.997'
         ,@QT_Vendas_Dia Integer = 1000
         ,@DT_Base Date
         ,@Fator_Multiplicador Integer
         ,@NO_Fator1 Integer
         ,@NO_Fator2 Integer;
  
  SET @DT_Base = CONVERT(DATE,@DH_Inicial_Venda);
  WHILE (@DT_Base <= CONVERT(DATE,@DH_Final_Venda))
  BEGIN
    SET @Fator_Multiplicador = CONVERT(Int,(86399997 / @QT_Vendas_Dia));

    SET @NO_Fator1 = CONVERT(int,(RAND() * 1000));
    SET @NO_Fator2 = CONVERT(int,(RAND() * 100000));
    
    ;WITH tbVendasDia
    AS (SELECT ROWID = 1
              ,DH = DATEADD(ms, T.Fator, CONVERT(DATETIME,@DT_Base))
       FROM (SELECT Fator = CONVERT(int,(@NO_Fator2 * (@Fator_Multiplicador / @NO_Fator1)))) AS T
       UNION ALL
       SELECT ROWID = tbVendasDia.ROWID + 1
             ,DH = CASE WHEN tbAux.DH < @DH_Final_Venda THEN tbAux.DH
                        ELSE @DH_Final_Venda
                   END
       FROM tbVendasDia
            CROSS APPLY (SELECT DH = DATEADD(ms, T.Fator, tbVendasDia.DH)
                         FROM (VALUES(1 + CONVERT(int,(@NO_Fator2 * (@Fator_Multiplicador / (@NO_Fator1 * tbVendasDia.ROWID)))))) AS T(Fator)) AS tbAux
       WHERE tbVendasDia.ROWID < @QT_Vendas_Dia)
    INSERT INTO dbo.Vendas(DH_Inclusao, VL_Total_Produtos, QT_Total_Produtos)
    SELECT dh,0,0
    FROM tbVendasDia
    OPTION(MAXRECURSION 0)
    
    SET @DT_Base = DATEADD(DD,1,@DT_Base);
  END
END
GO
IF EXISTS(SELECT 1 FROM dbo.Vendas WHERE NOT EXISTS(SELECT 1 FROM dbo.Vendas_Detalhe AS D WHERE D.ID_Venda = Vendas.ID))
BEGIN
  DECLARE @ID_Venda Integer
         ,@QT_Produtos Integer;

  SET @ID_Venda = 0;
  SELECT TOP 1 @ID_Venda = V.ID
  FROM dbo.Vendas AS V
  WHERE NOT EXISTS(SELECT 1 FROM dbo.Vendas_Detalhe AS D WHERE D.ID_Venda = V.ID)
    AND V.ID > @ID_Venda
  ORDER BY V.ID;

  WHILE @@ROWCOUNT > 0
  BEGIN
    SET @QT_Produtos = CONVERT(int,(RAND() * 10));

    IF (@QT_Produtos = 0) SET @QT_Produtos = 1;
    
    ;WITH tbProdutos
     AS (SELECT TOP (@QT_Produtos)
                ID
               ,VL_Produto
               ,QT = CONVERT(INT, (RAND() * ID))
         FROM dbo.Produtos
         ORDER BY NEWID())
    INSERT INTO dbo.Vendas_Detalhe(ID_Venda, ID_Produto, QT_Produto, VL_Unitario_Produto, VL_Produto_Total)
    SELECT @ID_Venda
          ,ID
          ,QT
          ,VL_Produto
          ,CONVERT(NUMERIC(18,2),(VL_Produto * QT))
    FROM tbProdutos;

    ;WITH CTA
     AS (SELECT VL = SUM(VL_Produto_Total)
               ,QT = SUM(QT_Produto)
         FROM dbo.Vendas_Detalhe
         WHERE ID_Venda = @ID_Venda)
    UPDATE dbo.Vendas
    SET VL_Total_Produtos = CTA.VL
       ,QT_Total_Produtos = CTA.QT
    FROM dbo.Vendas
         CROSS APPLY CTA
    WHERE ID = @ID_Venda;

    SELECT TOP 1 @ID_Venda = V.ID
    FROM dbo.Vendas AS V
    WHERE NOT EXISTS(SELECT 1 FROM dbo.Vendas_Detalhe AS D WHERE D.ID_Venda = V.ID)
      AND V.ID > @ID_Venda
    ORDER BY V.ID;
  END
END
