CREATE proc uploadLabDef  @Data [dbo].[dt_LabDef]  ReadOnly, 
		@LabName varchar(50)='unknown',
		@LabTestName varchar(50)='unknown'
		as
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
-- FK Cleanup needed.
Delete from LabTestStdLevel where LabTestStandardsId in (SELECT LabTestStandardsId from [LabTestStandards] Where LabTestId=@LabTestId)
Delete from [dbo].[LabTestStandards] Where LabTestId=@LabTestId
INSERT INTO [dbo].[LabTestStandards]
           ([LabTestId]
           ,[Taxon]
           ,[DisplayOrder])
Select @LabTestId 
           ,D.Taxon 
           ,DisplayOrder FROM @Data D JOIN TaxonHierarchy H (NOLOCK)
		   ON D.taxon=H.Taxon
-- Update with probable names
Update S
	Set DisplayName= TaxonName
FROM LabTestStandards S
JOIN (
	Select Taxon, TaxonName FROM  TaxonNames N (NOLOCK)
Where Len(TaxonName) <= (
	Select Min(Len(TaxonName)) FROM  TaxonNames (NOLOCK) Where Taxon=N.Taxon group by taxon)) N2
ON S.Taxon=N2.Taxon  
Where DisplayName is null
-- Orphans
Select @LabTestId 
           ,D.Taxon 
           ,DisplayOrder FROM @Data D LEFT JOIN TaxonHierarchy H (NOLOCK)
		   ON D.taxon=H.Taxon WHERE H.Taxon is null