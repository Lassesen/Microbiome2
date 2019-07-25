CREATE TABLE [dbo].[Citation] (
    [CitationId]          INT             IDENTITY (1, 1) NOT NULL,
    [Title]               NVARCHAR (255)  NOT NULL,
    [CitationJson]        NVARCHAR (MAX)  NULL,
    [RawText]             NVARCHAR (MAX)  NULL,
    [RawDocument]         VARBINARY (MAX) NULL,
    [RawDocumentMimeType] VARCHAR (60)    NULL,
    [Summary]             NVARCHAR (MAX)  NULL,
    [PmId]                INT             NULL,
    [PmcId]               VARCHAR (14)    NULL,
    [Doi]                 VARCHAR (60)    NULL,
    [Pii]                 VARCHAR (50)    NULL,
    [Url]                 VARCHAR (255)   NULL,
    CONSTRAINT [PK_Citation] PRIMARY KEY CLUSTERED ([CitationId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Citation]
    ON [dbo].[Citation]([Title] ASC);

