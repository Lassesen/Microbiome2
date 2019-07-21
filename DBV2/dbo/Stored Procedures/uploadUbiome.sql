CREATE proc uploadUbiome  @Data [dbo].[dt_LabResultTaxon] ReadOnly, 
		@SampleId int=null,
		@sampleDate DateTime=null ,
		@ownerEmail nvarchar(255)='NoOne@Somewhere.edu'		
		as

If @sampleDate is null
	Set @sampleDate =GETUTCDATE() 

Declare @LabId int
Select @LabId=LabId from Labs where LabName='Ubiome'If NOT Exists(Select 1 from Labs where LabName='Ubiome')
If  @LabId IS NULL
BEGIN
	Insert into [labs] (LabName) Values('Ubiome')
	SET @LabId=@@IDENTITY
END

Declare @LabTestId int
Select @LabTestId=LabTestId from LabTests where LabId=@LabId and LabTestName='Explorer'
If @LabTestId IS NULL
BEGIN
	Insert into [LabTests] (LabId,LabTestName) Values(@LabId,'Explorer')
	SET @LabTestId =@@IDENTITY
END

Declare @OwnerId int 
Select @OwnerId=OwnerId from  Owner where Email=@ownerEmail
If @OwnerId IS NULL
BEGIN
	Insert into [Owner] ([Name],Email) Values(@ownerEmail,@ownerEmail)
	SET @OwnerId  = @@IDENTITY
END
 
Declare @LabResultId INT
-- We are not checking if this is an update, we deem all to be new inserts
Insert into LabResults (SampleDate,OwnerId,LabTestId,OtherNotes)
	Values(@sampleDate,@OwnerId,@LabTestId, 'Ubiome Sequence No:'+ Cast(@SampleId as varchar(11)))
Set @LabResultId=@@IDENTITY

INSERT INTO [dbo].[LabResultTaxon]
           ([LabResultsId]
           ,[Taxon]
           ,[BaseOneMillion]
           ,[Count]
           ,[Count_Norm])
Select @LabResultId
               ,D.[Taxon]
           ,[BaseOneMillion]
           ,[Count]
           ,[Count_Norm] From @Data D
		    JOIN TaxonHierarchy H (NOLOCK) ON D.Taxon=H.Taxon
-- Return no matches
Select @SampleId As SampleId, D.*
        From @Data D
		LEFT JOIN TaxonHierarchy H (NOLOCK) ON D.Taxon=H.Taxon Where H.Taxon is Null