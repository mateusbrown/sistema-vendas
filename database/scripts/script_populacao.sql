IF NOT EXISTS(SELECT 1 FROM dbo.Produtos)
BEGIN
  ;WITH CTA
   AS(SELECT T.Descricao
            ,ROWID = ROW_NUMBER() OVER(ORDER BY NEWID())
      FROM (VALUES ('Arroz'),('Feijão'),('Óleo'),('Azeite de oliva'),('Vinagre'),('Açúcar'),('Milho para pipoca'),('Farinha de trigo'),('Fermento em pó'),('Aveia'),('Cereais'),('Amido de milho'),('Farinha de mandioca'),('Extrato de tomate'),('Macarrão'),('Queijo ralado'),('Enlatados'),('Conservas'),('Bolachas'),('Petiscos'),('Pães'),('Maionese'),('Ketchup'),('Mostarda'),('Frios'),('Manteiga'),('Requeijão'),('Geleias ou doces pastosos'),('Mel'),('Sal'),('Temperos secos'),('Especiarias'),('Café'),('Chás'),('Sucos'),('Iogurtes'),('Leite'),('Achocolatado'),('Água mineral'),('Refrigerantes'),('Bebidas alcoólicas de sua preferência'),('Ovos'),('Verduras'),('Legumes'),('Vegetais variados'),('Frutas da estação'),('Cebola'),('Alho'),('Ervas e temperos frescos'),('Bifes'),('Carne moída'),('Carne de frango'),('Filés de peixes'),('Bacon'),('Hambúrgueres'),('Linguiças'),('Salsichas'),('Shampoo'),('Condicionador'),('Sabonetes'),('Sabonete líquido'),('Cotonetes'),('Algodão'),('Papel higiênico'),('Pasta de dente'),('Escova de dente'),('Fio dental'),('Antisséptico bucal'),('Porta escova de dentes'),('Saboneteira'),('Esponja para banho'),('Desodorante'),('Curativos'),('Detergente'),('Desengordurante'),('Esponja para louça'),('Palha de aço'),('Escova de limpeza'),('Sabão em barra'),('Balde e bacia'),('Rodo, vassoura, pá'),('Panos de limpeza e flanelas'),('Sabão em pó ou líquido para roupas'),('Amaciante'),('Água sanitária'),('Cesto para roupas'),('Lixeira grande e pequena'),('Lixeira para banheiro'),('Escova sanitária'),('Sacos de lixo'),('Desinfetante'),('Limpa vidros'),('Limpa piso'),('Limpador multiuso'),('Álcool'),('Lustrador de móveis'),('Guardanapos de papel'),('Papel toalha'),('Papel alumínio'),('Sacos plásticos para alimentos'),('Papel filme'),('Filtro para café'),('Varal para roupas'),('Pregadores'),('Lâmpadas'),('Fósforos'),('Velas'),('Pilhas'),('Inseticida')) AS T(Descricao)),
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