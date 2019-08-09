CREATE TABLE [dbo].[CategoryReference] (
    [CategoryId]   INT           IDENTITY (1, 1) NOT NULL,
    [CategoryName] VARCHAR (255) NOT NULL,
    [ICDCode]      VARCHAR (16)  NULL,
    CONSTRAINT [PK_CategoryReference] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
);





