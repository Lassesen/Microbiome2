create proc updateCustomLabTestStatics @LabTestId int,@ColumnName varchar(50), @Data [dbo].[dt_LabTestCustomStatistics]  READONLY AS
Declare @StatisticsId int
Select  @StatisticsId =StatisticsId  From dbo.[Statistics] where StatisticsName=@ColumnName
IF @StatisticsId is null
BEGIN
	Insert Into [Statistics] (StatisticsName) Values(@ColumnName)
	Set @StatisticsId =@@IDENTITY
END
Update S
	SET [StatisticsValue]=[Value],[LastUpdated]=GETUTCDATE()
FROM [LabTestStatistics] S
JOIN @Data D
ON S.TaxonId=D.Taxon
And StatisticsId=@StatisticsId
AND LabTestId=@LabTestId
 	 
INSERT INTO [dbo].[LabTestStatistics]
           ([LabTestId]
           ,[TaxonId]
           ,[StatisticsId]
           ,[StatisticsValue]
           ,[LastUpdated])
     Select @LabTestId 
           ,Taxon
           ,@StatisticsId 
           , Value 
           ,GetUtcDate() FROM @Data D
		   LEFT JOIN [LabTestStatistics] S
		   ON D.Taxon =S.TaxonId
		   AND  @LabTestId=LabTestId 
		   AND StatisticsId=@StatisticsId
		   WHERE S.TaxonId is Null