create proc updateCoreLabTestStatics @LabTestId int, @Data [dbo].[dt_LabTestCoreStatistics]  READONLY AS
 
UPDATE U   SET 
      [Mean] = D.Mean
      ,[Median] = D.Median
      ,[Mode] = D.Mode
      ,[Minimum] = D.Minimum
      ,[Maximum] = D.Maximum
      ,[StandardDeviation] = D.StandardDeviation
      ,[Variance] = D.Variance
      ,[Skewness] = D.Skewness
      ,[Kurtosis] = D.Kurtosis
FROM  [dbo].[LabTestCoreStatistics] U JOIN @Data D
ON U.Taxon=D.Taxon AND LabTestId=@LabTestId