CREATE Proc [dbo].[ImportData] @Table0 [dbo].[tbi_Table0] READONLY
,@Table1 [dbo].[tbi_Table1] READONLY
,@Table2 [dbo].[tbi_Table2] READONLY
,@Table3 [dbo].[tbi_Table3] READONLY
,@Table4 [dbo].[tbi_Table4] READONLY
,@Table5 [dbo].[tbi_Table5] READONLY
,@Table6 [dbo].[tbi_Table6] READONLY
,@Table7 [dbo].[tbi_Table7] READONLY
,@Table8 [tbi_Table8] READONLY
,@Table9 [tbi_Table9] READONLY
AS
Declare @OwnerId int
Declare @ImportDate dateTime
Declare @Owner [dbo].[tbi_Table0] 
Insert into @Owner Select * from @Table0
Declare @Labs [dbo].[tbi_Table1] 
Insert into @Labs Select * from @Table1
Declare @LabTests [dbo].[tbi_Table2] 
Insert into @LabTests Select * from @Table2
Declare @LabResults [dbo].[tbi_Table3] 
Insert into @LabResults Select * from @Table3
Declare @LabResultTaxon [dbo].[tbi_Table4] 
Insert into @LabResultTaxon Select * from @Table4
Declare @Category [dbo].[tbi_Table5] 
Insert into @Category Select * from @Table5
Declare @ReportCategory [dbo].[tbi_Table6] 
Insert into @ReportCategory Select * from @Table6
Declare @Continuous [dbo].[tbi_Table7] 
Insert into @Continuous Select * from @Table7
Declare @ReportContinuous [dbo].[tbi_Table8] 
Insert into @ReportContinuous Select * from @Table8

Declare @LabResultReport [dbo].[tbi_Table9] 
Insert into @LabResultReport Select * from @Table9
 

INSERT INTO [dbo].[Owner]
           ([Name]
           ,[Email]
           ,[CreatedDate])
     Select [Name]
           ,[Email]
           ,[CreatedDate] FROM @Owner Where [name] NOT IN (SELECT [Name] FROM [Owner])
Select @OwnerId=OwnerId,@ImportDate=[CreatedDate] From Owner WHere Name in (Select Name from @Owner)

Update Tmp
	Set OwnerId=O.OwnerId
From @Owner Tmp JOIN Owner O ON Tmp.Name=O.Name
 
INSERT INTO [dbo].[Labs]
           ([LabName])
   Select [LabName] From @Labs where LabName not in (Select LabName from Labs)
Update Tmp
	Set LabId=O.LabId
From @Labs Tmp JOIN Labs O ON Tmp.LabName=O.LabName
-----------------------------
INSERT INTO [dbo].[CategoryReference]
           ([CategoryName])
   Select [CategoryName] From @Category where CategoryName not in (Select CategoryName from CategoryReference)
Update Tmp
	Set CategoryId=O.CategoryId
From @Category Tmp JOIN CategoryReference O ON Tmp.CategoryName=O.CategoryName
--------------------------------
         INSERT INTO [dbo].[ContinuousReference]
           ([ContinuousName],[ContinuousUnits])
   Select [ContinuousName],[ContinuousUnits] From @Continuous where ContinuousName not in (Select ContinuousName from ContinuousReference)
Update Tmp
	Set ContinuousId=O.ContinuousId
From @Continuous Tmp JOIN ContinuousReference O ON Tmp.ContinuousName=O.ContinuousName
--------------------------
INSERT INTO [dbo].[LabTests]
           ([LabId]
           ,[LabTestName])
Select L.[LabId]
           ,[LabTestName] From @LabTests T JOIN @Labs L ON L.LabGuid=T.LabGuid
		   Where [LabTestName] NOT IN (Select [LabTestName] from LabTests)
UPDATE R
	SET LabTestId=T.LabTestId
	FROM @LabTests R JOIN LabTests T on R.LabTestName=T.LabTestName 
--------------------
Update R
	Set LabtestId= T.LabTestId
	From @Labresults R 
	Join @LabTests T 
	ON R.LabTestGuid =T.LabTestGuid
 
INSERT INTO [dbo].[LabResults]
           ([SampleDate]
           ,[OwnerId]
           ,[LabTestId]
           ,[SyncGuid])
   Select [SampleDate]
           ,@OwnerID
           ,[LabTestId]
           ,LabResultGuid 
		   FROM @LabResults Where LabResultGuid NOT IN (Select SyncGuid From LabResults)


INSERT INTO [dbo].[OwnerReport]
           ([OwnerId]
           ,[AsOfDate]
            
           ,[SyncGuid])
Select @OwnerId, @ImportDate           
           ,[ReportGuid]
		   FROM @LabResultReport Where [ReportGuid] NOT IN (Select SyncGuid From OwnerReport)
Update R
	SET ReportId=O.ReportId
	From @LabResultReport R JOIN OwnerReport O ON SyncGuid=[ReportGuid]
Update R
	SET LabresultId=O.LabResultsId
	From @LabResultReport R JOIN [LabResults] O ON SyncGuid=[LabResultGuid]
INSERT INTO [dbo].[LabResultReport]
           ([LabResultsId]
           ,[ReportId])
Select [LabResultId]
           ,[ReportId]
         From @LabResultReport Where  [LabResultId]*10000 +[ReportId] NOT IN (Select 10000* [LabResultsId] +[ReportId] FROM [LabResultReport])

Update R
	SET ReportId=O.ReportId
	From @ReportCategory R JOIN OwnerReport O ON O.SyncGuid=[ReportGuid]
Update R
	SET reportid=O.ReportId
	From @ReportContinuous R JOIN OwnerReport O ON O.SyncGuid=[ReportGuid]

Update R
	SET ContinuousId=O.ContinuousId
	From @ReportContinuous R JOIN @Continuous O ON R.ContinuousGuID=O.ContinuousGuID
Update R
	SET CategoryId=O.CategoryId
	From @ReportCategory R JOIN @Category O ON R.CategoryGuID=O.CategoryGuID

Delete From ReportCategory
	Where ReportId in (Select ReportId from @ReportCategory)
Delete From ReportContinuous
	Where ReportId in (Select ReportId from @ReportContinuous)
INSERT INTO [dbo].[ReportCategory]
           ([ReportId]
           ,[CategoryId])
Select [ReportId]
           ,[CategoryId] From @ReportCategory
INSERT INTO [dbo].[ReportContinuous]
           ([ReportId]
           ,[ContinuousId]
           ,[Reading])
SELECT [ReportId]
           ,[ContinuousId]
           ,[Reading]
		   From @ReportContinuous