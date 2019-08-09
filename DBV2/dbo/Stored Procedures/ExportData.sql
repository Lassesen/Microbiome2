CREATE Proc [dbo].[ExportData]  @SourceName varchar(100)='Mysystem'
AS
Declare @ExportDate DateTime=GetUtcDate()
Declare @OwnerGuid uniqueIdentifier = NewId() 
Declare @OwnerId int
Declare @ImportDate dateTime
Declare @Owner [dbo].[tbi_Table0] 
Insert into @Owner
	Select @OwnerGuid, @SourceName,@SourceName,@ExportDate,-1  

Declare @Labs [dbo].[tbi_Table1] 
Insert into @Labs 
	Select NewId() LabGuid,LabName,LabId from Labs
Declare @LabTests [dbo].[tbi_Table2] 
Insert into @LabTests 
	Select NewId() LabTestGuid,LabGuid,LabTestName,LabTestId,L.labid from LabTests L
		JOIN @Labs T ON L.LabId=T.labid
Declare @LabResults [dbo].[tbi_Table3] 
Insert into @LabResults 
	Select SyncGuid LabResultGuid,@ExportDate,@OwnerGuid, LabTestGuid,'Export',LabResultsId,OwnerId,L.labtestid   from LabResults L
	JOIN @LabTests T on L.LabTestId=T.labtestid
Declare @LabResultTaxon [dbo].[tbi_Table4] 
Insert into @LabResultTaxon 
	Select LabResultGuid,taxon,BaseOneMillion,L.labresultsid from LabResultTaxon L
		JOIN @LabResults T ON L.LabResultsId=T.labresultid
Declare @Category [dbo].[tbi_Table5] 
Insert into @Category 
	Select NewId() CategoryGuid, CategoryName,CategoryId from CategoryReference

Declare @LabResultReport [dbo].[tbi_Table9] 
Insert into @LabResultReport 
	Select LabResultGuId,SyncGuid ReportGuid, labresultid,L.ReportId  from LabResultReport L
	Join @LabResults T ON T.labresultid=L.LabResultsId
	JOIN OwnerReport O ON O.ReportId=L.ReportId
Declare @ReportCategory [dbo].[tbi_Table6] 
	Insert into @ReportCategory 
		Select ReportGuid,CategoryGuid,L.ReportId,L.CategoryId from ReportCategory L
		JOIN @LabResultReport T On T.reportid=L.ReportId
		JOIN @Category C on L.CategoryId=C.categoryid
Declare @Continuous [dbo].[tbi_Table7] 
	Insert into @Continuous 
		Select NewId(),ContinuousName,ContinuousUnits,ContinuousId from ContinuousReference L
Declare @ReportContinuous [dbo].[tbi_Table8] 
Insert into @ReportContinuous Select ReportGuid,ContinuousGuID,Reading,L.reportid,L.continuousid from ReportContinuous L
	JOIN @LabResultReport T On T.reportid=L.ReportId
	JOIN @Continuous C ON L.ContinuousId=L.continuousid

Select OwnerGuid,Name,Email,CreatedDate from @Owner
Select LabGuid,LabName From @Labs
Select LabTestGuid,LabGuid,LabTestName From @LabTests
Select LabResultGuid,SampleDate,OwnerGuid,LabTestGuid,OtherNotes From @LabResults 
Select LabResultGuid,Taxon,BaseOneMillion From @LabResultTaxon
Select CategoryGuid,CategoryName  from @Category
Select ReportGuid,CategoryGuid  from @ReportCategory
Select ContinuousGuid,ContinuousName,ContinuousUnits    from @Continuous
Select ReportGuid,ContinuousGuid,Reading  from @ReportContinuous
Select LabResultGuid,ReportGuid FROM @LabResultReport