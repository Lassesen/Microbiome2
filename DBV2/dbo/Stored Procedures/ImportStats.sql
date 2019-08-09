CREATE Proc ImportStats as
Select 'LabResultTaxon' as TableName, Count(1) as Count from LabResultTaxon
UNION
Select 'LabResults' as TableName, Count(1) as Count from LabResults
UNION
Select 'LabTests' as TableName, Count(1) as Count from LabTests
UNION
Select 'Labs' as TableName, Count(1) as Count from Labs
UNION
Select 'OwnerReport' as TableName, Count(1) as Count from OwnerReport
UNION
Select 'ReportCategory' as TableName, Count(1) as Count from ReportCategory
UNION
Select 'ReportContinuous' as TableName, Count(1) as Count from ReportContinuous
UNION
Select 'LabTestCoreStatistics' as TableName, Count(1) as Count from LabTestCoreStatistics
UNION
Select 'LabTestStatistics' as TableName, Count(1) as Count from LabTestStatistics