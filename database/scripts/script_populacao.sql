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