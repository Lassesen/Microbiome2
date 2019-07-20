CREATE proc Uploadncbi  @nData dbo.dt_TaxonNames READONLY, @hdata dt_TaxonHierarchy READONLY
 AS
INSERT INTO [dbo].[TaxonHierarchy]
           ([Taxon]
           ,[ParentTaxon]
           ,[Rank])
Select  [Taxon]
           ,[ParentTaxon]
           ,[Rank] FROM @hdata 
		   Where Taxon not in (Select Taxon from [TaxonHierarchy])
		   Order by Taxon
  
INSERT INTO [dbo].[TaxonNames]
           ([TaxonName]
           ,[Taxon])
 Select DISTINCT [TaxonName]
           ,[Taxon]
		   FROM @nData Where TaxonName not in (Select TaxonName from [TaxonNames])