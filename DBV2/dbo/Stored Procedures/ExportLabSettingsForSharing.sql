Create Proc ExportLabSettingsForSharing @LabName varchar(50)='GI-Map'
AS
SELECT * from  LabTests T (NOLOCK) 
			JOIN LabTestStandards S (NOLOCK)
			ON T.LabTestId=S.LabTestId
			LEFT Join LabTestStdLevel L (NOLOCK)
			ON L.LabTestStandardsId = S.LabTestStandardsId
			LEFT JOIN LabTestLevel M (NOLOCK)
			ON L.LabTestLevelId=M.LabTestLevelId
			WHERE LabTestName=@LabName
			FOR XML AUTO