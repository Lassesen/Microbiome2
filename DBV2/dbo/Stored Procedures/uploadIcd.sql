CREATE proc [dbo].[uploadIcd] @IndexXml xml,@TabularXml xml  as
-- First insert for reference or rollback
Insert into ICDSource (IndexXml,TabularXml) Values(@Indexxml, @TabularXml)
DELETE FROM [ICDAltName]
DELETE FROM ICDCode
--Select * from ICDSource

Declare @Latest DateTime
select @Latest=max(UploadDate) from ICDSource  
 INSERT INTO [dbo].[ICDCode]
           ([Code]
           ,[Description])
Select 
row.value('(name/text())[1]', 'varchar(50)') AS Name,
row.value('(desc/text())[1]', 'varchar(50)') AS description
 from ICDSource Cross Apply TabularXml.nodes('/ICD10CM.tabular/chapter/section/diag') as I(row)
 Where UploadDate=@Latest  AND row.value('(name/text())[1]', 'varchar(50)') is not null
UNION
Select 
row.value('(name/text())[1]', 'varchar(50)') AS Name,
row.value('(desc/text())[1]', 'varchar(50)') AS description
 from ICDSource Cross Apply TabularXml.nodes('/ICD10CM.tabular/chapter/section/diag/diag') as I(row)
 Where UploadDate=@Latest  AND row.value('(name/text())[1]', 'varchar(50)') is not null
UNION 
Select 
row.value('(name/text())[1]', 'varchar(50)') AS Name,
row.value('(desc/text())[1]', 'varchar(50)') AS description
 from ICDSource Cross Apply TabularXml.nodes('/ICD10CM.tabular/chapter/section/diag/diag/diag') as I(row)
 Where UploadDate=@Latest AND row.value('(name/text())[1]', 'varchar(50)') is not null
 UNION
 Select 
row.value('(name/text())[1]', 'varchar(50)') AS Name,
row.value('(desc/text())[1]', 'varchar(50)') AS description
 from ICDSource Cross Apply TabularXml.nodes('/ICD10CM.tabular/chapter/section/diag/diag/diag/diag') as I(row)
 Where UploadDate=@Latest AND row.value('(name/text())[1]', 'varchar(50)') is not null
 UNION
 Select 
row.value('(name/text())[1]', 'varchar(50)') AS Name,
row.value('(desc/text())[1]', 'varchar(50)') AS description
 from ICDSource Cross Apply TabularXml.nodes('/ICD10CM.tabular/chapter/section/diag/diag/diag/diag/diag') as I(row)
 Where UploadDate=@Latest AND row.value('(name/text())[1]', 'varchar(50)') is not null
 UNION
  Select 
row.value('(name/text())[1]', 'varchar(50)') AS Name,
row.value('(desc/text())[1]', 'varchar(50)') AS description
 from ICDSource Cross Apply TabularXml.nodes('/ICD10CM.tabular/chapter/section/diag/diag/diag/diag/diag/diag') as I(row)
 Where UploadDate=@Latest AND row.value('(name/text())[1]', 'varchar(50)') is not null


DECLARE @Pass1 AS TABLE 
  ( 
     code    VARCHAR(20), 
     details XML 
  ) 

INSERT INTO @Pass1 
            (code, 
             details) 
SELECT row.value('(name/text())[1]', 'varchar(50)') AS NAME, 
       row.query('.')                               AS description 
FROM   icdsource 
       CROSS apply tabularxml.nodes('/ICD10CM.tabular/chapter/section/diag') AS 
                   I(row) 
WHERE  uploaddate = @Latest 



INSERT INTO @Pass1 
            (code, 
             details) 
SELECT row.value('(name/text())[1]', 'varchar(50)') ,
       row.query('.')                                     AS description 
FROM   icdsource 
       CROSS apply tabularxml.nodes('/ICD10CM.tabular/chapter/section/diag/diag') AS 
                   I(row) 
WHERE  uploaddate = @Latest 
       AND row.value('(name/text())[1]', 'varchar(50)') IS NOT NULL 

INSERT INTO @Pass1 
            (code, 
             details) 
SELECT row.value('(name/text())[1]', 'varchar(50)') ,
       row.query('.')                                
FROM   icdsource 
       CROSS apply 
       tabularxml.nodes('/ICD10CM.tabular/chapter/section/diag/diag/diag') AS I( 
       row) 
WHERE  uploaddate = @Latest 
       AND row.value('(name/text())[1]', 'varchar(50)') IS NOT NULL 

INSERT INTO @Pass1 
            (code, 
             details) 
SELECT row.value('(name/text())[1]', 'varchar(50)')  ,
       row.query('.')                                 
FROM   icdsource 
       CROSS apply 
       tabularxml.nodes('/ICD10CM.tabular/chapter/section/diag/diag/diag/diag') 
       AS I(row) 
WHERE  uploaddate = @Latest 
       AND row.value('(name/text())[1]', 'varchar(50)') IS NOT NULL 

INSERT INTO @Pass1 
            (code, 
             details) 
SELECT row.value('(name/text())[1]', 'varchar(50)')  ,
       row.query('.')                                    
FROM   icdsource 
       CROSS apply 
       tabularxml.nodes('/ICD10CM.tabular/chapter/section/diag/diag/diag/diag/diag') 
       AS I( 
                        row) 
WHERE  uploaddate = @Latest 
       AND row.value('(name/text())[1]', 'varchar(50)') IS NOT NULL 

INSERT INTO @Pass1 
            (code, 
             details) 
SELECT row.value('(name/text())[1]', 'varchar(50)') , 
       row.query('.')                                   
FROM   icdsource 
       CROSS apply 
tabularxml.nodes('/ICD10CM.tabular/chapter/section/diag/diag/diag/diag/diag/diag') AS 
                 I(row) 
WHERE  uploaddate = @Latest 
       AND row.value('(name/text())[1]', 'varchar(50)') IS NOT NULL 
 
INSERT INTO [dbo].[ICDAltName]
           ([Code]
           ,[AltName])
  
SELECT distinct code, 
       row.value('(text())[1]', 'varchar(255)')Alternative 
FROM   @Pass1 
       CROSS apply details.nodes('/diag/inclusionTerm/note') AS J(row)