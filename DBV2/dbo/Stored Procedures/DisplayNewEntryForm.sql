CREATE Proc DisplayNewEntryForm @LabName varchar(50)='GI-Map'
AS
SELECT Taxon,DisplayName,DisplayOrder from  LabTests T (NOLOCK) 
			JOIN LabTestStandards S (NOLOCK)
			ON T.LabTestId=S.LabTestId Where LabTestName=@LabName