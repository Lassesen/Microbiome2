CREATE TABLE [dbo].[Owner] (
    [OwnerId]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Email]       NVARCHAR (255) NOT NULL,
    [CreatedDate] DATE           CONSTRAINT [DF_Owner_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Owner] PRIMARY KEY CLUSTERED ([OwnerId] ASC)
);



