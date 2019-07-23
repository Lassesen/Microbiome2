CREATE TABLE [dbo].[ICDSource] (
    [UploadDate] DATETIME CONSTRAINT [DF_ICDSource_UploadDate] DEFAULT (getutcdate()) NOT NULL,
    [IndexXml]   XML      NOT NULL,
    [TabularXml] XML      NOT NULL,
    CONSTRAINT [PK_ICDSource] PRIMARY KEY CLUSTERED ([UploadDate] ASC)
);

