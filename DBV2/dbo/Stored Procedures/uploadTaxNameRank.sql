CREATE proc uploadTaxNameRank  @Data [dbo].[dt_TaxNameRankUpload]  ReadOnly, 
		@LabName varchar(50)='unknown',
		@LabTestName varchar(50)='unknown',
		@sampleDate DateTime=null ,
		@sampleId nvarchar(255)=null,
		@ownerEmail nvarchar(255)='NoOne@Somewhere.edu'		
		as

If @sampleDate is null
	Set @sampleDate =GETUTCDATE() 

Declare @LabId int
Select @LabId=LabId from Labs where LabName=@LabName
If  @LabId IS NULL
BEGIN
	Insert into [labs] (LabName) Values(@LabName)
	SET @LabId=@@IDENTITY
END

Declare @LabTestId int
Select @LabTestId=LabTestId from LabTests where LabId=@LabId and LabTestName=@LabTestName
If @LabTestId IS NULL
BEGIN
	Insert into [LabTests] (LabId,LabTestName) Values(@LabId,@LabTestName)
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
Insert into LabResults (SampleDate,OwnerId,LabTestId, OtherNotes)
	Values(@sampleDate,@OwnerId,@LabTestId,@sampleId)
Set @LabResultId=@@IDENTITY
-- Now we start the parsing of names...
Declare @Resolved As Table (
	Taxon int NULL,
	taxon_rank varchar(50),
	taxon_name varchar(255),
	BaseOneMillion float
)
Insert into @Resolved(taxon_rank,taxon_name,BaseOneMillion)
SELECT tax_rank,tax_name,BaseOneMillion
	FROM @Data

Update D
	Set Taxon=N.Taxon
	From @Resolved D JOIN TaxonNames N (NOLOCK)
	ON D.taxon_name=N.TaxonName
	JOIN TaxonHierarchy H (NOLOCK)
	ON H.[Rank]=D.Taxon_Rank AND H.Taxon=N.Taxon
-- Sometime enclosed with [ ]
Update D
	Set Taxon=N.Taxon
	From @Resolved D JOIN TaxonNames N (NOLOCK)
	ON D.taxon_name='['+N.TaxonName+']'
	JOIN TaxonHierarchy H (NOLOCK)
	ON H.[Rank]=D.Taxon_Rank AND H.Taxon=N.Taxon
	where D.taxon is null
-- relaxed matching
Update D
	Set Taxon=N.Taxon
	From @Resolved D JOIN TaxonNames N (NOLOCK)
	ON D.Taxon_Name=N.TaxonName
	where D.Taxon is null
/* Optional kludge - not the best solution	 
Update D
	Set Taxon=N.Taxon
	From @Resolved D JOIN TaxonNames N (NOLOCK)
	ON D.Tax_Name Like N.TaxonName+'%'
	where D.Taxon is null
	Order by Len(N.taxonName)
	*/
-- Uncomment for triaging issues	Select * from @Resolved Order by taxon
INSERT INTO [dbo].[LabResultTaxon]
           ([LabResultsId]
           ,[Taxon]
           ,[BaseOneMillion]
           )
Select @LabResultId
           ,[Taxon]
           ,Max([BaseOneMillion]) --We get some dup taxon 
 From @Resolved D Where Taxon is not null
 group by [Taxon]
		     
-- Return no matches
Select  D.*
        From @Resolved D where taxon is null