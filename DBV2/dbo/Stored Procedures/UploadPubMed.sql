CREATE Proc UploadPubMed
@Pmid int,
@Title Nvarchar(255)
           ,@CitationJson   nvarchar(max)=null
		   ,@RawText nvarchar(max)=null
           ,@Summary nvarchar(max)=null
            ,@PmcId  varchar(14)=null
           ,@Doi varchar(60)=null
           ,@Pii varchar(50) =null
AS
If NOT EXISTS(Select 1 From Citation where Pmid=@Pmid OR @Title=Title)
BEGIN
INSERT INTO [dbo].[Citation]
           ([Title]
           ,[CitationJson]
           ,[RawText]
            ,[Summary]
           ,[PmId]
           ,[PmcId]
           ,[Doi]
           ,[Pii] )
     VALUES
           (@Title
           ,@CitationJson
           ,@RawText
           ,@Summary 
           ,@PmId 
           ,@PmcId 
           ,@Doi
           ,@Pii
           )
		   SELECT @@IDENTITY
End
ELSE
BEGIN
UPDATE [dbo].[Citation]
   SET [Title] = @Title
      ,[CitationJson] = @CitationJson
      ,[RawText] = @RawText
      ,[Summary] = @Summary
      ,[PmcId] = @PmcId
      ,[Doi] = @Doi
      ,[Pii] =  @Pii 
 
 WHERE @Pmid=PmId OR @Title=Title
 Select pmid From Citation where Pmid=@Pmid OR @Title=Title
END