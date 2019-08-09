CREATE proc [dbo].[updateCoreLabTestStatics] @LabTestId int, @Data [dbo].[dt_LabTestCoreStatistics]  READONLY AS
 
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
 

INSERT INTO [dbo].[LabTestCoreStatistics]
           ([LabTestId]
           ,[Taxon]
           ,[Mean]
           ,[Median]
           ,[Mode]
           ,[Minimum]
           ,[Maximum]
           ,[StandardDeviation]
           ,[Variance]
           ,[Skewness]
           ,[Kurtosis]
           ,[Count]
           ,[LastUpdated])
SELECT @LabTestId
           ,D.[Taxon]
           ,D.[Mean]
           ,D.[Median]
           ,D.[Mode]
           ,D.[Minimum]
           ,D.[Maximum]
           ,D.[StandardDeviation]
           ,D.[Variance]
           ,D.[Skewness]
           ,D.[Kurtosis]
           ,D.[Count]
           ,GetUtcDate()
FROM  [dbo].[LabTestCoreStatistics] U RIGHT JOIN @Data D
ON U.Taxon=D.Taxon AND LabTestId=@LabTestId
WHERE U.Taxon is null