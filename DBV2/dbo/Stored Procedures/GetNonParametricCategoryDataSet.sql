CREATE Proc [dbo].[GetNonParametricCategoryDataSet] @LabTestId int=1,@CategoryId int=1, @QuantileRoot varchar(10)='Q4_', @MinCount float=12
AS
SELECT [TaxonId] as [Taxon]
      ,[StatisticsValue]
      ,[StatisticsName]
	  ,[Count]
  FROM  [LabTestStatistics] L (NOLOCK) 
  JOIN  [Statistics] S (NOLOCK) 
  ON L.StatisticsId=S.StatisticsId
  JOIN [LabTestCoreStatistics] C (NOLOCK)
  ON C.LabTestId=L.LabTestId AND C.Taxon=TaxonId
  WHERE C.[LabTestId]=@LabTestId AND C.[Count] >=@MinCount AND StatisticsName like @QuantileRoot+'%'
  ORDER BY TaxonId, StatisticsName

  SELECT  [Taxon]
      ,[BaseOneMillion] Value
  FROM  [ViewNonParametric]
  Where [Count] >=@MinCount And [CategoryId]=@CategoryId And [LabTestId]=@LabTestId
  Order By Taxon

    SELECT  N.[Taxon],TaxonName
    FROM  [TaxonNames] N (NOLOCK)
	 JOIN  [LabTestCoreStatistics] S (NOLOCK) 
  ON N.Taxon=S.Taxon
  Where [Count] >=@MinCount ANd LabTestId=@LabTestId
  Order By Taxon