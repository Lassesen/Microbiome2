CREATE  proc getTaxonWithValues @LabTestId int=51 AS
SELECT Taxon,BaseOneMillion Value from LabResultTaxon T (NOLOCK)
JOIN LabResults R (NOLOCK) ON T.LabResultsId=R.LabResultsId
Where LabTestId=@LabTestId
Order by Taxon, BaseOneMillion