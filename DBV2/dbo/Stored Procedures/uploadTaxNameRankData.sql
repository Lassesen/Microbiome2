CREATE proc [dbo].[uploadTaxNameRankData]  @Data [dbo].[dt_TaxNameRankUpload]  ReadOnly, 
		@LabName varchar(50)='unknown',
		@LabTestName varchar(50)='unknown',
		@sampleDate DateTime=null ,
		@sampleId nvarchar(255)=null,
		@ownerEmail nvarchar(255)='NoOne@Somewhere.edu'		
		as
SELECT * Into XenoGene FROM @Data
SELECT *  FROM @Data